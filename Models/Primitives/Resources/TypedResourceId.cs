using System;
using System.Runtime.CompilerServices;
using BH.SDK.Models.Interfaces.Primitives;

namespace BH.SDK.Models.Primitives.Resources
{
    [Serializable]
    public struct TypedResourceId : IEquatable<TypedResourceId>, IPrimitiveInt
    {
        public int value;
        int IPrimitiveInt.Value => value;

        public TypedResourceId(int value)
        {
            this.value = value;
        }
        public TypedResourceId(BytesResourceId bytesResourceId)
        {
            value = bytesResourceId.value;
        }
        public TypedResourceId(TextResourceId textResourceId)
        {
            value = textResourceId.value;
        }
        public TypedResourceId(TextureResourceId textureResourceId)
        {
            value = textureResourceId.value;
        }
        public TypedResourceId(AudioResourceId audioResourceId)
        {
            value = audioResourceId.value;
        }
        public TypedResourceId(FontResourceId fontResourceId)
        {
            value = fontResourceId.value;
        }
        
        // ------------------------------------------------------------------------------------------------------------
        // TypedResourceId
        // ------------------------------------------------------------------------------------------------------------

        // TypedResourceId (AudioResourceId, TextureResourceId, ...) is a stable identifier for saving,
        // unique per resource type (audio, textures, ...). What each number means:
        // 0 => Null / not-set, reserved value, never a valid resource id
        // (1 - int.MaxValue) => game-space resources, defaults for whole game, stored in game, each resource has permanent id
        // (int.MinValue - -1) => user-space resources, unique for each level, stored externally: level folder, url...
        //
        // Each per-type struct below (AudioResourceId, TextureResourceId, ...) shares this exact
        // range semantics, just narrowed to its own resource type.
        
        public const int NullValue = 0;
        public const int MinGameDefinedValue = 1;
        public const int MaxUserDefinedValue = -1;
        
        public static readonly TypedResourceId Null = new(NullValue);
        public static readonly TypedResourceId MinGameDefined = new(MinGameDefinedValue);
        public static readonly TypedResourceId MaxUserDefined = new(MaxUserDefinedValue);
        
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
        public static bool operator ==(TypedResourceId a, TypedResourceId b) => a.value == b.value;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(TypedResourceId a, TypedResourceId b) => a.value != b.value;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(TypedResourceId other) => value == other.value;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is TypedResourceId other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => $"{nameof(TypedResourceId)}={value}";
    }
}