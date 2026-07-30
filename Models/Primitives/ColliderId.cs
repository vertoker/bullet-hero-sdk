using System;
using System.Runtime.CompilerServices;
using BH.SDK.Models.Interfaces.Primitives;

namespace BH.SDK.Models.Primitives
{
    public struct ColliderId : IEquatable<ColliderId>, IPrimitiveGuid
    {
        public Guid value;
        Guid IPrimitiveGuid.Value => value;

        public ColliderId(Guid value)
        {
            this.value = value;
        }
        public ColliderId(string str)
        {
            value = new Guid(str);
        }
        public void Reset()
        {
            value = NullValue;
        }

        // Collider ids are a stable identifier for a CompositeCollider/CompositeColliderShapeScriptable
        // entry, same role ThemeId/EffectId play for Theme/Effect - a TextureObject references a
        // shared, reusable collider shape by id instead of embedding its own copy. Unlike the
        // previous int-based id, there is no game-defined/user-defined range split - a Guid has no
        // meaningful "positive/negative" ordering to split on (see ThemeId/EffectId/PrefabId/LevelId
        // for the same reasoning); "game-defined" vs "user-defined" is now determined by which
        // collection an id is found in (GameResources.CompositeShapes vs Level.Resources.CompositeShapes),
        // not by the id's own value. Guid.Empty is the only reserved/Null value.

        public static readonly Guid NullValue = Guid.Empty;

        public static readonly ColliderId Null = new(NullValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsEnabled() => value != NullValue;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEnabled(Guid value) => value != NullValue;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColliderId NewId() => new(Guid.NewGuid());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColliderId NewGuid() => new(Guid.NewGuid());


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ColliderId a, ColliderId b) => a.value == b.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ColliderId a, ColliderId b) => a.value != b.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ColliderId other) => value == other.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is ColliderId other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => value.GetHashCode();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => $"{nameof(ColliderId)}={value}";
    }
}
