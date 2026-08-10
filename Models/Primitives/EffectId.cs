using System;
using System.Runtime.CompilerServices;
using BH.SDK.Models.Interfaces.Primitives;

namespace BH.SDK.Models.Primitives
{
    /// <summary>
    /// Identity of an EffectData preset in Level.Resources.Effects, referenced by EffectObject.
    /// The indirection is the point: one edited preset changes every placement at once.
    /// </summary>
    public struct EffectId : IEquatable<EffectId>, IPrimitiveGuid
    {
        /// <summary> The raw Guid. </summary>
        public Guid value;
        Guid IPrimitiveGuid.Value => value;

        public EffectId(Guid value)
        {
            this.value = value;
        }
        public EffectId(string str)
        {
            value = new Guid(str);
        }
        public void Reset()
        {
            value = Guid.Empty;
        }

        // Effect ids are a stable identifier for an EffectData entry (Level.Resources.Effects),
        // same role ThemeId plays for Theme - an EffectObject references a shared, reusable
        // EffectData preset instead of embedding its own copy, so editing one EffectData updates
        // every EffectObject pointing at it. Unlike the previous int-based id, there is no
        // game-defined/user-defined range split - a Guid has no meaningful "positive/negative"
        // ordering to split on (see PrefabId/LevelId for the same reasoning). Guid.Empty is the
        // only reserved/Null value.

        public static readonly Guid NullValue = Guid.Empty;

        public static readonly EffectId Null = new(NullValue);

        // default, not NullValue: reading a static field would drag this type's initializer into
        // any Burst job that asks - see ColliderId.IsEnabled for the failure that caused.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsEnabled() => value != Guid.Empty;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEnabled(Guid value) => value != Guid.Empty;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EffectId NewId() => new(Guid.NewGuid());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EffectId NewGuid() => new(Guid.NewGuid());


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(EffectId a, EffectId b) => a.value == b.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(EffectId a, EffectId b) => a.value != b.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(EffectId other) => value == other.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is EffectId other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => value.GetHashCode();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => $"{nameof(EffectId)}={value}";
    }
}
