using System;
using System.Runtime.CompilerServices;
using BH.SDK.Models.Interfaces.Primitives;
using BH.SDK.Rules;

namespace BH.SDK.Models.Primitives
{
    [Serializable]
    public struct AudioId : IEquatable<AudioId>, IPrimitiveInt
    {
        public int value;
        int IPrimitiveInt.Value => value;

        public AudioId(int value)
        {
            this.value = value;
        }

        // Same idea as ObjectId, but scoped to a single level's audio tracks and much simpler:
        // unlike ColliderId/TypedResourceId, there is no negative (game-defined) range here -
        // negative values are banned for consistency, see LevelTrack.AudioId
        // 0 => Null / not-set
        // (1 - int.MaxValue) => valid, level-local audio track id

        public const int NullValue = 0;
        public const int MinValue = 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValid() => value >= MinValue;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValid(int value) => value >= MinValue;

        public static readonly AudioId Null = new(NullValue);
        public static readonly AudioId Min = new(MinValue);

        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(AudioId a, AudioId b) => a.value == b.value;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(AudioId a, AudioId b) => a.value != b.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(AudioId other) => value == other.value;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is AudioId other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => $"{nameof(AudioId)}={value}";
    }
}