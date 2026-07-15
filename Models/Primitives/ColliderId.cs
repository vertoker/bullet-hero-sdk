using System;
using System.Runtime.CompilerServices;
using BH.SDK.Models.Interfaces.Primitives;

namespace BH.SDK.Models.Primitives
{
    [Serializable]
    public struct ColliderId : IEquatable<ColliderId>, IPrimitiveInt
    {
        public int value;
        int IPrimitiveInt.Value => value;

        public ColliderId(int value)
        {
            this.value = value;
        }

        // Collider ids are not a separate top-level resource list, they are embedded directly
        // on TextureObject/CompositeCollider. What each number means:
        // 0 => Null / not-set
        // (1 - int.MaxValue) => game-defined colliders, built into the game (Square, Circle, ...)
        // (int.MinValue - -1) => user-defined colliders, unique per level (custom CompositeCollider shapes)

        public const int NullValue = 0;
        public const int MinGameDefinedValue = 1;
        public const int MaxUserDefinedValue = -1;
        
        public static readonly ColliderId Null = new(NullValue);
        public static readonly ColliderId MinGameDefined = new(MinGameDefinedValue);
        public static readonly ColliderId MaxUserDefined = new(MaxUserDefinedValue);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsEnabled() => value != NullValue;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsGameDefined() => value >= MinGameDefinedValue;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsUserDefined() => value <= MaxUserDefinedValue;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEnabled(int value) => value != NullValue;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsGameDefined(int value) => value >= MinGameDefinedValue;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsUserDefined(int value) => value <= MaxUserDefinedValue;
        
        // built-in game colliders, ids are stable and guaranteed to never change
        public static readonly ColliderId Square = new(1);
        public static readonly ColliderId Circle = new(2);
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ColliderId a, ColliderId b) => a.value == b.value;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ColliderId a, ColliderId b) => a.value != b.value;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ColliderId other) => value == other.value;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is ColliderId other && Equals(other);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => $"{nameof(ColliderId)}={value}";
    }
}