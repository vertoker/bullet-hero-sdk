using System;
using System.Runtime.CompilerServices;
using BH.SDK.Models.Interfaces.Primitives;

namespace BH.SDK.Models.Primitives
{
    [Serializable]
    public struct PrefabId : IEquatable<PrefabId>, IPrimitiveGuid
    {
        public Guid value;
        Guid IPrimitiveGuid.Value => value;

        public PrefabId(Guid value)
        {
            this.value = value;
        }
        public PrefabId(string str)
        {
            value = new Guid(str);
        }

        // Prefab ids are a stable identifier for a Prefab entry (Level.Resources.Prefabs) - what
        // PrefabObject.PrefabGuid references back to. Unlike the int-based ids (ColliderId,
        // ThemeId, ...) there is no game-defined/user-defined range split - prefabs are always
        // per-level authored data, and a Guid has no meaningful "positive/negative" ordering to
        // split on anyway. Guid.Empty is the only reserved/Null value.

        public static readonly PrefabId Null = new(Guid.Empty);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsEnabled() => value != Guid.Empty;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEnabled(Guid value) => value != Guid.Empty;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PrefabId NewId() => new(Guid.NewGuid());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PrefabId NewGuid() => new(Guid.NewGuid());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(PrefabId a, PrefabId b) => a.value == b.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(PrefabId a, PrefabId b) => a.value != b.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(PrefabId other) => value == other.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is PrefabId other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => value.GetHashCode();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => $"{nameof(PrefabId)}={value}";
    }
}
