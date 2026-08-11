using System;
using System.Runtime.CompilerServices;
using BH.SDK.Models.Interfaces.Primitives;
// ReSharper disable InconsistentNaming

namespace BH.SDK.Models.Primitives.Resources
{
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
        
        public static readonly TextureResourceId Square =        new(1);
        public static readonly TextureResourceId Circle =        new(2);
        public static readonly TextureResourceId Triangle =      new(3);
        public static readonly TextureResourceId RightTriangle = new(4);
        public static readonly TextureResourceId Pentagon =      new(5);
        public static readonly TextureResourceId Hexagon =       new(6);
        public static readonly TextureResourceId Heptagon =      new(7);
        public static readonly TextureResourceId Octagon =       new(8);
        public static readonly TextureResourceId Nonagon =       new(9);
        public static readonly TextureResourceId Decagon =       new(10);
        
        public static readonly TextureResourceId Circle_F2 =     new(11);
        public static readonly TextureResourceId Circle_F4 =     new(12);
        public static readonly TextureResourceId Circle_F8 =     new(13);
        public static readonly TextureResourceId Pentagon_F2 =   new(14);
        public static readonly TextureResourceId Pentagon_F4 =   new(15);
        public static readonly TextureResourceId Pentagon_F4_2 = new(16);
        public static readonly TextureResourceId Hexagon_F2 =    new(17);
        public static readonly TextureResourceId Hexagon_F4 =    new(18);
        public static readonly TextureResourceId Heptagon_F2 =   new(19);
        public static readonly TextureResourceId Heptagon_F4 =   new(20);
        public static readonly TextureResourceId Heptagon_F4_2 = new(21);
        public static readonly TextureResourceId Octagon_F2 =    new(22);
        public static readonly TextureResourceId Octagon_F4 =    new(23);
        public static readonly TextureResourceId Nonagon_F2 =    new(24);
        public static readonly TextureResourceId Nonagon_F4 =    new(25);
        public static readonly TextureResourceId Nonagon_F4_2 =  new(26);
        public static readonly TextureResourceId Decagon_F2 =    new(27);
        public static readonly TextureResourceId Decagon_F4 =    new(28);
        
        public static readonly TextureResourceId Square_T2 =  new(29);
        public static readonly TextureResourceId Square_T4 =  new(30);
        public static readonly TextureResourceId Square_T8 =  new(31);
        public static readonly TextureResourceId Square_T16 = new(32);
        public static readonly TextureResourceId Square_T32 = new(33);
        
        public static readonly TextureResourceId Circle_T2 =  new(34);
        public static readonly TextureResourceId Circle_T4 =  new(35);
        public static readonly TextureResourceId Circle_T8 =  new(36);
        public static readonly TextureResourceId Circle_T16 = new(37);
        public static readonly TextureResourceId Circle_T32 = new(38);
        
        public static readonly TextureResourceId Triangle_T2 =  new(39);
        public static readonly TextureResourceId Triangle_T4 =  new(40);
        public static readonly TextureResourceId Triangle_T8 =  new(41);
        public static readonly TextureResourceId Triangle_T16 = new(42);
        public static readonly TextureResourceId Triangle_T32 = new(43);
        
        public static readonly TextureResourceId RightTriangle_T2 =  new(44);
        public static readonly TextureResourceId RightTriangle_T4 =  new(45);
        public static readonly TextureResourceId RightTriangle_T8 =  new(46);
        public static readonly TextureResourceId RightTriangle_T16 = new(47);
        public static readonly TextureResourceId RightTriangle_T32 = new(48);
        
        public static readonly TextureResourceId Pentagon_T2 =  new(49);
        public static readonly TextureResourceId Pentagon_T4 =  new(50);
        public static readonly TextureResourceId Pentagon_T8 =  new(51);
        public static readonly TextureResourceId Pentagon_T16 = new(52);
        public static readonly TextureResourceId Pentagon_T32 = new(53);
        
        public static readonly TextureResourceId Hexagon_T2 =  new(54);
        public static readonly TextureResourceId Hexagon_T4 =  new(55);
        public static readonly TextureResourceId Hexagon_T8 =  new(56);
        public static readonly TextureResourceId Hexagon_T16 = new(57);
        public static readonly TextureResourceId Hexagon_T32 = new(58);
        
        public static readonly TextureResourceId Heptagon_T2 =  new(59);
        public static readonly TextureResourceId Heptagon_T4 =  new(60);
        public static readonly TextureResourceId Heptagon_T8 =  new(61);
        public static readonly TextureResourceId Heptagon_T16 = new(62);
        public static readonly TextureResourceId Heptagon_T32 = new(63);
        
        public static readonly TextureResourceId Octagon_T2 =  new(64);
        public static readonly TextureResourceId Octagon_T4 =  new(65);
        public static readonly TextureResourceId Octagon_T8 =  new(66);
        public static readonly TextureResourceId Octagon_T16 = new(67);
        public static readonly TextureResourceId Octagon_T32 = new(68);
        
        public static readonly TextureResourceId Nonagon_T2 =  new(69);
        public static readonly TextureResourceId Nonagon_T4 =  new(70);
        public static readonly TextureResourceId Nonagon_T8 =  new(71);
        public static readonly TextureResourceId Nonagon_T16 = new(72);
        public static readonly TextureResourceId Nonagon_T32 = new(73);
        
        public static readonly TextureResourceId Decagon_T2 =  new(74);
        public static readonly TextureResourceId Decagon_T4 =  new(75);
        public static readonly TextureResourceId Decagon_T8 =  new(76);
        public static readonly TextureResourceId Decagon_T16 = new(77);
        public static readonly TextureResourceId Decagon_T32 = new(78);
        
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