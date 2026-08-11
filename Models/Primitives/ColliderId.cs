using System;
using System.Runtime.CompilerServices;
using BH.SDK.Models.Interfaces.Primitives;
using BH.SDK.Utils;
// ReSharper disable InconsistentNaming

namespace BH.SDK.Models.Primitives
{
    /// <summary>
    /// Identity of a reusable collision shape (CompositeCollider). The only Guid id here with a
    /// large set of well-known constants below - the built-in shape library every ShapeObject
    /// picks from, generated from small ints so they stay readable in a saved file.
    /// </summary>
    public struct ColliderId : IEquatable<ColliderId>, IPrimitiveGuid
    {
        /// <summary> The raw Guid; game-defined vs. user-defined is decided by which collection it
        /// is found in, not by the value. </summary>
        public Guid value;
        Guid IPrimitiveGuid.Value => value;

        public ColliderId(Guid value)
        {
            this.value = value;
        }
        public ColliderId(string str)
        {
            value = new Guid(str);
        }
        public void Reset()
        {
            value = Guid.Empty;
        }

        // Collider ids are a stable identifier for a CompositeCollider/CompositeColliderShapeScriptable
        // entry, same role ThemeId/EffectId play for Theme/Effect - a ShapeObject references a
        // shared, reusable collider shape by id instead of embedding its own copy. Unlike the
        // previous int-based id, there is no game-defined/user-defined range split - a Guid has no
        // meaningful "positive/negative" ordering to split on (see ThemeId/EffectId/PrefabId/LevelId
        // for the same reasoning); "game-defined" vs "user-defined" is now determined by which
        // collection an id is found in (GameResources.CompositeShapes vs Level.Resources.CompositeShapes),
        // not by the id's own value. Guid.Empty is the only reserved/Null value.

        public static readonly Guid NullValue = Guid.Empty;

        public static readonly ColliderId Null = new(NullValue);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsEnabled() => value != Guid.Empty;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEnabled(Guid value) => value != Guid.Empty;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColliderId NewId() => new(Guid.NewGuid());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColliderId NewGuid() => new(Guid.NewGuid());
        
        public static readonly ColliderId Square =        new(GuidHelper.FromIntAtEnd(1));
        public static readonly ColliderId Circle =        new(GuidHelper.FromIntAtEnd(2));
        public static readonly ColliderId Triangle =      new(GuidHelper.FromIntAtEnd(3));
        public static readonly ColliderId RightTriangle = new(GuidHelper.FromIntAtEnd(4));
        public static readonly ColliderId Pentagon =      new(GuidHelper.FromIntAtEnd(5));
        public static readonly ColliderId Hexagon =       new(GuidHelper.FromIntAtEnd(6));
        public static readonly ColliderId Heptagon =      new(GuidHelper.FromIntAtEnd(7));
        public static readonly ColliderId Octagon =       new(GuidHelper.FromIntAtEnd(8));
        public static readonly ColliderId Nonagon =       new(GuidHelper.FromIntAtEnd(9));
        public static readonly ColliderId Decagon =       new(GuidHelper.FromIntAtEnd(10));
        
        public static readonly ColliderId Circle_F2 =     new(GuidHelper.FromIntAtEnd(11));
        public static readonly ColliderId Circle_F4 =     new(GuidHelper.FromIntAtEnd(12));
        public static readonly ColliderId Circle_F8 =     new(GuidHelper.FromIntAtEnd(13));
        public static readonly ColliderId Pentagon_F2 =   new(GuidHelper.FromIntAtEnd(14));
        public static readonly ColliderId Pentagon_F4 =   new(GuidHelper.FromIntAtEnd(15));
        public static readonly ColliderId Pentagon_F4_2 = new(GuidHelper.FromIntAtEnd(16));
        public static readonly ColliderId Hexagon_F2 =    new(GuidHelper.FromIntAtEnd(17));
        public static readonly ColliderId Hexagon_F4 =    new(GuidHelper.FromIntAtEnd(18));
        public static readonly ColliderId Heptagon_F2 =   new(GuidHelper.FromIntAtEnd(19));
        public static readonly ColliderId Heptagon_F4 =   new(GuidHelper.FromIntAtEnd(20));
        public static readonly ColliderId Heptagon_F4_2 = new(GuidHelper.FromIntAtEnd(21));
        public static readonly ColliderId Octagon_F2 =    new(GuidHelper.FromIntAtEnd(22));
        public static readonly ColliderId Octagon_F4 =    new(GuidHelper.FromIntAtEnd(23));
        public static readonly ColliderId Nonagon_F2 =    new(GuidHelper.FromIntAtEnd(24));
        public static readonly ColliderId Nonagon_F4 =    new(GuidHelper.FromIntAtEnd(25));
        public static readonly ColliderId Nonagon_F4_2 =  new(GuidHelper.FromIntAtEnd(26));
        public static readonly ColliderId Decagon_F2 =    new(GuidHelper.FromIntAtEnd(27));
        public static readonly ColliderId Decagon_F4 =    new(GuidHelper.FromIntAtEnd(28));
        
        public static readonly ColliderId Square_T2 =  new(GuidHelper.FromIntAtEnd(29));
        public static readonly ColliderId Square_T4 =  new(GuidHelper.FromIntAtEnd(30));
        public static readonly ColliderId Square_T8 =  new(GuidHelper.FromIntAtEnd(31));
        public static readonly ColliderId Square_T16 = new(GuidHelper.FromIntAtEnd(32));
        public static readonly ColliderId Square_T32 = new(GuidHelper.FromIntAtEnd(33));
        
        public static readonly ColliderId Circle_T2 =  new(GuidHelper.FromIntAtEnd(34));
        public static readonly ColliderId Circle_T4 =  new(GuidHelper.FromIntAtEnd(35));
        public static readonly ColliderId Circle_T8 =  new(GuidHelper.FromIntAtEnd(36));
        public static readonly ColliderId Circle_T16 = new(GuidHelper.FromIntAtEnd(37));
        public static readonly ColliderId Circle_T32 = new(GuidHelper.FromIntAtEnd(38));
        
        public static readonly ColliderId Triangle_T2 =  new(GuidHelper.FromIntAtEnd(39));
        public static readonly ColliderId Triangle_T4 =  new(GuidHelper.FromIntAtEnd(40));
        public static readonly ColliderId Triangle_T8 =  new(GuidHelper.FromIntAtEnd(41));
        public static readonly ColliderId Triangle_T16 = new(GuidHelper.FromIntAtEnd(42));
        public static readonly ColliderId Triangle_T32 = new(GuidHelper.FromIntAtEnd(43));
        
        public static readonly ColliderId RightTriangle_T2 =  new(GuidHelper.FromIntAtEnd(44));
        public static readonly ColliderId RightTriangle_T4 =  new(GuidHelper.FromIntAtEnd(45));
        public static readonly ColliderId RightTriangle_T8 =  new(GuidHelper.FromIntAtEnd(46));
        public static readonly ColliderId RightTriangle_T16 = new(GuidHelper.FromIntAtEnd(47));
        public static readonly ColliderId RightTriangle_T32 = new(GuidHelper.FromIntAtEnd(48));
        
        public static readonly ColliderId Pentagon_T2 =  new(GuidHelper.FromIntAtEnd(49));
        public static readonly ColliderId Pentagon_T4 =  new(GuidHelper.FromIntAtEnd(50));
        public static readonly ColliderId Pentagon_T8 =  new(GuidHelper.FromIntAtEnd(51));
        public static readonly ColliderId Pentagon_T16 = new(GuidHelper.FromIntAtEnd(52));
        public static readonly ColliderId Pentagon_T32 = new(GuidHelper.FromIntAtEnd(53));
        
        public static readonly ColliderId Hexagon_T2 =  new(GuidHelper.FromIntAtEnd(54));
        public static readonly ColliderId Hexagon_T4 =  new(GuidHelper.FromIntAtEnd(55));
        public static readonly ColliderId Hexagon_T8 =  new(GuidHelper.FromIntAtEnd(56));
        public static readonly ColliderId Hexagon_T16 = new(GuidHelper.FromIntAtEnd(57));
        public static readonly ColliderId Hexagon_T32 = new(GuidHelper.FromIntAtEnd(58));
        
        public static readonly ColliderId Heptagon_T2 =  new(GuidHelper.FromIntAtEnd(59));
        public static readonly ColliderId Heptagon_T4 =  new(GuidHelper.FromIntAtEnd(60));
        public static readonly ColliderId Heptagon_T8 =  new(GuidHelper.FromIntAtEnd(61));
        public static readonly ColliderId Heptagon_T16 = new(GuidHelper.FromIntAtEnd(62));
        public static readonly ColliderId Heptagon_T32 = new(GuidHelper.FromIntAtEnd(63));
        
        public static readonly ColliderId Octagon_T2 =  new(GuidHelper.FromIntAtEnd(64));
        public static readonly ColliderId Octagon_T4 =  new(GuidHelper.FromIntAtEnd(65));
        public static readonly ColliderId Octagon_T8 =  new(GuidHelper.FromIntAtEnd(66));
        public static readonly ColliderId Octagon_T16 = new(GuidHelper.FromIntAtEnd(67));
        public static readonly ColliderId Octagon_T32 = new(GuidHelper.FromIntAtEnd(68));
        
        public static readonly ColliderId Nonagon_T2 =  new(GuidHelper.FromIntAtEnd(69));
        public static readonly ColliderId Nonagon_T4 =  new(GuidHelper.FromIntAtEnd(70));
        public static readonly ColliderId Nonagon_T8 =  new(GuidHelper.FromIntAtEnd(71));
        public static readonly ColliderId Nonagon_T16 = new(GuidHelper.FromIntAtEnd(72));
        public static readonly ColliderId Nonagon_T32 = new(GuidHelper.FromIntAtEnd(73));
        
        public static readonly ColliderId Decagon_T2 =  new(GuidHelper.FromIntAtEnd(74));
        public static readonly ColliderId Decagon_T4 =  new(GuidHelper.FromIntAtEnd(75));
        public static readonly ColliderId Decagon_T8 =  new(GuidHelper.FromIntAtEnd(76));
        public static readonly ColliderId Decagon_T16 = new(GuidHelper.FromIntAtEnd(77));
        public static readonly ColliderId Decagon_T32 = new(GuidHelper.FromIntAtEnd(78));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ColliderId a, ColliderId b) => a.value == b.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ColliderId a, ColliderId b) => a.value != b.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ColliderId other) => value == other.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is ColliderId other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => value.GetHashCode();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => $"{nameof(ColliderId)}={value}";
    }
}
