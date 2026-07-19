using System;
using System.Runtime.CompilerServices;
using BH.SDK.Models.Interfaces.Primitives;

namespace BH.SDK.Models.Primitives
{
    [Serializable]
    public struct ThemeId : IEquatable<ThemeId>, IPrimitiveInt
    {
        public int value;
        int IPrimitiveInt.Value => value;

        public ThemeId(int value)
        {
            this.value = value;
        }

        // Theme ids are a stable identifier for a Theme entry (Level.Resources.Themes), replacing
        // positional ThemeIndex - a theme's position in the list can change (reorder/delete),
        // an id never does. What each number means:
        // 0 => Null / not-set
        // (1 - int.MaxValue) => game-defined themes, built into the game (default presets)
        // (int.MinValue - -1) => user-defined themes, unique per level

        public const int NullValue = 0;
        public const int MinGameDefinedValue = 1;
        public const int MaxUserDefinedValue = -1;

        public static readonly ThemeId Null = new(NullValue);
        public static readonly ThemeId MinGameDefined = new(MinGameDefinedValue);
        public static readonly ThemeId MaxUserDefined = new(MaxUserDefinedValue);

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


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ThemeId a, ThemeId b) => a.value == b.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ThemeId a, ThemeId b) => a.value != b.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ThemeId other) => value == other.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is ThemeId other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => $"{nameof(ThemeId)}={value}";
    }
}
