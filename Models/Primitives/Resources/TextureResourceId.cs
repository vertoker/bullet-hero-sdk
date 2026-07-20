using System;
using System.Runtime.CompilerServices;
using BH.SDK.Models.Interfaces.Primitives;

namespace BH.SDK.Models.Primitives.Resources
{
    [Serializable]
    public struct TextureResourceId : IEquatable<TextureResourceId>, IPrimitiveInt
    {
        public int value;
        int IPrimitiveInt.Value => value;

        public TextureResourceId(int value)
        {
            this.value = value;
        }
        public TextureResourceId(TypedResourceId typedResourceId)
        {
            value = typedResourceId.value;
        }
        public void Reset()
        {
            value = NullValue;
        }
        
        // Same range semantics as TypedResourceId, narrowed to texture resources

        public const int NullValue = 0;
        public const int MinGameDefinedValue = 1;
        public const int MaxUserDefinedValue = -1;

        public static readonly TextureResourceId Null = new(NullValue);
        public static readonly TextureResourceId MinGameDefined = new(MinGameDefinedValue);
        public static readonly TextureResourceId MaxUserDefined = new(MaxUserDefinedValue);
        
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
        
        public static readonly TextureResourceId Square = new(1);
        public static readonly TextureResourceId Circle = new(2);
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(TextureResourceId a, TextureResourceId b) => a.value == b.value;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(TextureResourceId a, TextureResourceId b) => a.value != b.value;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(TextureResourceId other) => value == other.value;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is TextureResourceId other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => $"{nameof(TextureResourceId)}={value}";
    }
}