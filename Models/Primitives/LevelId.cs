using System;
using System.Runtime.CompilerServices;
using BH.SDK.Models.Interfaces.Primitives;

namespace BH.SDK.Models.Primitives
{
    /// <summary>
    /// Identity of a whole level, carried by LevelMeta rather than Level itself. Survives renaming
    /// and moving the level folder, which is what makes scores/progress attachable to it.
    /// </summary>
    public struct LevelId : IEquatable<LevelId>, IPrimitiveGuid
    {
        /// <summary> The raw Guid. </summary>
        public Guid value;
        Guid IPrimitiveGuid.Value => value;

        public LevelId(Guid value)
        {
            this.value = value;
        }
        public LevelId(string str)
        {
            value = new Guid(str);
        }
        public void Reset()
        {
            value = Guid.Empty;
        }

        // Level ids are a stable identifier for a Level (replaces LevelMeta.LevelGuid). Unlike the
        // int-based ids (AudioId, TypedResourceId, ...) there is no game-defined/user-defined range
        // split - levels are always standalone authored data, and a Guid has no meaningful
        // "positive/negative" ordering to split on anyway. Guid.Empty is the only reserved/Null value.

        public static readonly Guid NullValue = Guid.Empty;

        public static readonly LevelId Null = new(NullValue);

        // default, not NullValue: reading a static field would drag this type's initializer into
        // any Burst job that asks - see ShapeId.IsEnabled for the failure that caused.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsEnabled() => value != Guid.Empty;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEnabled(Guid value) => value != Guid.Empty;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LevelId NewId() => new(Guid.NewGuid());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LevelId NewGuid() => new(Guid.NewGuid());


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(LevelId a, LevelId b) => a.value == b.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(LevelId a, LevelId b) => a.value != b.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(LevelId other) => value == other.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is LevelId other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => value.GetHashCode();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => $"{nameof(LevelId)}={value}";
    }
}
