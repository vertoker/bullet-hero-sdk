using System;
using System.Runtime.CompilerServices;
using BH.SDK.Models.Interfaces.Primitives;

namespace BH.SDK.Models.Primitives.Resources
{
    [Serializable]
    public struct AudioResourceId : IEquatable<AudioResourceId>, IPrimitiveInt
    {
        public int value;
        int IPrimitiveInt.Value => value;

        public AudioResourceId(int value)
        {
            this.value = value;
        }
        public AudioResourceId(TypedResourceId typedResourceId)
        {
            value = typedResourceId.value;
        }
        public void Reset()
        {
            value = NullValue;
        }
        
        // Same range semantics as TypedResourceId, narrowed to audio resources

        public const int NullValue = 0;
        public const int MinGameDefinedValue = 1;
        public const int MaxUserDefinedValue = -1;

        public static readonly AudioResourceId Null = new(NullValue);
        public static readonly AudioResourceId MinGameDefined = new(MinGameDefinedValue);
        public static readonly AudioResourceId MaxUserDefined = new(MaxUserDefinedValue);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValid() => value != NullValue;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsGameDefined() => value >= MinGameDefinedValue;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsUserDefined() => value <= MaxUserDefinedValue;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValid(int value) => value != NullValue;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsGameDefined(int value) => value >= MinGameDefinedValue;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsUserDefined(int value) => value <= MaxUserDefinedValue;
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(AudioResourceId a, AudioResourceId b) => a.value == b.value;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(AudioResourceId a, AudioResourceId b) => a.value != b.value;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(AudioResourceId other) => value == other.value;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is AudioResourceId other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => $"{nameof(AudioResourceId)}={value}";
    }
}