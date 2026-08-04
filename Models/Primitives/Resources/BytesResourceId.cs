using System;
using System.Runtime.CompilerServices;
using BH.SDK.Models.Interfaces.Primitives;

namespace BH.SDK.Models.Primitives.Resources
{
    /// <summary>
    /// Points at an opaque binary blob (BytesResource) - the fallback category for data the format
    /// has no dedicated type for. Nothing in Models references it directly today; it exists so a
    /// level can carry arbitrary payloads without a format change.
    /// </summary>
    [Serializable]
    public struct BytesResourceId : IEquatable<BytesResourceId>, IPrimitiveInt
    {
        /// <summary> The raw number, sharing TypedResourceId's sign convention. </summary>
        public int value;
        int IPrimitiveInt.Value => value;

        public BytesResourceId(int value)
        {
            this.value = value;
        }
        public BytesResourceId(TypedResourceId typedResourceId)
        {
            value = typedResourceId.value;
        }
        public void Reset()
        {
            value = NullValue;
        }
        
        // Same range semantics as TypedResourceId, narrowed to bytes resources

        public const int NullValue = 0;
        public const int MinGameDefinedValue = 1;
        public const int MaxUserDefinedValue = -1;

        public static readonly BytesResourceId Null = new(NullValue);
        public static readonly BytesResourceId MinGameDefined = new(MinGameDefinedValue);
        public static readonly BytesResourceId MaxUserDefined = new(MaxUserDefinedValue);
        
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
        public static bool operator ==(BytesResourceId a, BytesResourceId b) => a.value == b.value;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(BytesResourceId a, BytesResourceId b) => a.value != b.value;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(BytesResourceId other) => value == other.value;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is BytesResourceId other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => $"{nameof(BytesResourceId)}={value}";
    }
}