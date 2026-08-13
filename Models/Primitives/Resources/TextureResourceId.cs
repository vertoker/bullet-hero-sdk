using System;
using System.Runtime.CompilerServices;
using BH.SDK.Models.Interfaces.Primitives;
// ReSharper disable InconsistentNaming

namespace BH.SDK.Models.Primitives.Resources
{
    // The 78 named preset ids that used to live here (Square, Circle, Circle_T32, ...) are GONE, and
    // with them the whole game-defined tier of this family in practice: those were the built-in
    // shapes painted into one atlas, and a shape is real geometry now (ShapeId), so keeping a second
    // way to say "a circle" meant two sources of truth for the same 80 forms. The RANGE stays -
    // IsGameDefined and MinGameDefinedValue are still the format's contract, and nothing stops a
    // future build from shipping images again - it is only that the game currently ships none.

    /// <summary>
    /// Points at an image - a TextureResource of Level.Resources.Textures or a game-shipped one.
    /// Referenced by ShapeObject and by EffectObjectCore (particles are textured the same way).
    /// </summary>
    [Serializable]
    public struct TextureResourceId : IEquatable<TextureResourceId>, IPrimitiveInt
    {
        /// <summary> The raw number, sharing TypedResourceId's sign convention. </summary>
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