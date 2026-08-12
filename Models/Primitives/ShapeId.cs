using System;
using System.Runtime.CompilerServices;
using BH.SDK.Models.Interfaces.Primitives;
using BH.SDK.Utils;
// ReSharper disable InconsistentNaming

namespace BH.SDK.Models.Primitives
{
    /// <summary>
    /// Identity of a reusable shape (CompositeShape). The only Guid id here with a large set of
    /// well-known constants below - the built-in shape library, generated from small ints so they
    /// stay readable in a saved file.
    /// </summary>
    public struct ShapeId : IEquatable<ShapeId>, IPrimitiveGuid
    {
        /// <summary> The raw Guid; game-defined vs. user-defined is decided by which collection it
        /// is found in, not by the value. </summary>
        public Guid value;
        Guid IPrimitiveGuid.Value => value;

        public ShapeId(Guid value)
        {
            this.value = value;
        }
        public ShapeId(string str)
        {
            value = new Guid(str);
        }
        public void Reset()
        {
            value = Guid.Empty;
        }

        // One id type serves BOTH of ShapeObject's shape fields - ShapeId (what is drawn) and
        // ColliderId (what is hit) - because a shape and a hitbox are the same thing described the
        // same way: triangles inside [-0.5, 0.5]. Nothing distinguishes "a shape you can render"
        // from "a shape you can collide against", so a second id type would only be able to
        // disagree with this one.
        //
        // Shape ids are a stable identifier for a CompositeShape/CompositeShapeScriptable entry,
        // same role ThemeId/EffectId play for Theme/Effect - a ShapeObject references a shared,
        // reusable shape by id instead of embedding its own copy. There is no game-defined/
        // user-defined range split - a Guid has no meaningful "positive/negative" ordering to split
        // on (see ThemeId/EffectId/PrefabId/LevelId for the same reasoning); "game-defined" vs
        // "user-defined" is determined by which collection an id is found in
        // (GameResources.CompositeShapes vs Level.Resources.CompositeShapes), not by the id's own
        // value. Guid.Empty is the only reserved/Null value.

        public static readonly Guid NullValue = Guid.Empty;

        public static readonly ShapeId Null = new(NullValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsEnabled() => value != Guid.Empty;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEnabled(Guid value) => value != Guid.Empty;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ShapeId NewId() => new(Guid.NewGuid());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ShapeId NewGuid() => new(Guid.NewGuid());

        public static readonly ShapeId Square =        new(GuidHelper.FromIntAtEnd(1));
        public static readonly ShapeId Circle =        new(GuidHelper.FromIntAtEnd(2));
        public static readonly ShapeId Triangle =      new(GuidHelper.FromIntAtEnd(3));
        public static readonly ShapeId RightTriangle = new(GuidHelper.FromIntAtEnd(4));
        public static readonly ShapeId Pentagon =      new(GuidHelper.FromIntAtEnd(5));
        public static readonly ShapeId Hexagon =       new(GuidHelper.FromIntAtEnd(6));
        public static readonly ShapeId Heptagon =      new(GuidHelper.FromIntAtEnd(7));
        public static readonly ShapeId Octagon =       new(GuidHelper.FromIntAtEnd(8));
        public static readonly ShapeId Nonagon =       new(GuidHelper.FromIntAtEnd(9));
        public static readonly ShapeId Decagon =       new(GuidHelper.FromIntAtEnd(10));

        public static readonly ShapeId Circle_F2 =     new(GuidHelper.FromIntAtEnd(11));
        public static readonly ShapeId Circle_F4 =     new(GuidHelper.FromIntAtEnd(12));
        public static readonly ShapeId Circle_F8 =     new(GuidHelper.FromIntAtEnd(13));
        public static readonly ShapeId Pentagon_F2 =   new(GuidHelper.FromIntAtEnd(14));
        public static readonly ShapeId Pentagon_F4 =   new(GuidHelper.FromIntAtEnd(15));
        public static readonly ShapeId Pentagon_F4_2 = new(GuidHelper.FromIntAtEnd(16));
        public static readonly ShapeId Hexagon_F2 =    new(GuidHelper.FromIntAtEnd(17));
        public static readonly ShapeId Hexagon_F4 =    new(GuidHelper.FromIntAtEnd(18));
        public static readonly ShapeId Heptagon_F2 =   new(GuidHelper.FromIntAtEnd(19));
        public static readonly ShapeId Heptagon_F4 =   new(GuidHelper.FromIntAtEnd(20));
        public static readonly ShapeId Heptagon_F4_2 = new(GuidHelper.FromIntAtEnd(21));
        public static readonly ShapeId Octagon_F2 =    new(GuidHelper.FromIntAtEnd(22));
        public static readonly ShapeId Octagon_F4 =    new(GuidHelper.FromIntAtEnd(23));
        public static readonly ShapeId Nonagon_F2 =    new(GuidHelper.FromIntAtEnd(24));
        public static readonly ShapeId Nonagon_F4 =    new(GuidHelper.FromIntAtEnd(25));
        public static readonly ShapeId Nonagon_F4_2 =  new(GuidHelper.FromIntAtEnd(26));
        public static readonly ShapeId Decagon_F2 =    new(GuidHelper.FromIntAtEnd(27));
        public static readonly ShapeId Decagon_F4 =    new(GuidHelper.FromIntAtEnd(28));

        public static readonly ShapeId Square_T2 =  new(GuidHelper.FromIntAtEnd(29));
        public static readonly ShapeId Square_T4 =  new(GuidHelper.FromIntAtEnd(30));
        public static readonly ShapeId Square_T8 =  new(GuidHelper.FromIntAtEnd(31));
        public static readonly ShapeId Square_T16 = new(GuidHelper.FromIntAtEnd(32));
        public static readonly ShapeId Square_T32 = new(GuidHelper.FromIntAtEnd(33));

        public static readonly ShapeId Circle_T2 =  new(GuidHelper.FromIntAtEnd(34));
        public static readonly ShapeId Circle_T4 =  new(GuidHelper.FromIntAtEnd(35));
        public static readonly ShapeId Circle_T8 =  new(GuidHelper.FromIntAtEnd(36));
        public static readonly ShapeId Circle_T16 = new(GuidHelper.FromIntAtEnd(37));
        public static readonly ShapeId Circle_T32 = new(GuidHelper.FromIntAtEnd(38));

        public static readonly ShapeId Triangle_T2 =  new(GuidHelper.FromIntAtEnd(39));
        public static readonly ShapeId Triangle_T4 =  new(GuidHelper.FromIntAtEnd(40));
        public static readonly ShapeId Triangle_T8 =  new(GuidHelper.FromIntAtEnd(41));
        public static readonly ShapeId Triangle_T16 = new(GuidHelper.FromIntAtEnd(42));
        public static readonly ShapeId Triangle_T32 = new(GuidHelper.FromIntAtEnd(43));

        public static readonly ShapeId RightTriangle_T2 =  new(GuidHelper.FromIntAtEnd(44));
        public static readonly ShapeId RightTriangle_T4 =  new(GuidHelper.FromIntAtEnd(45));
        public static readonly ShapeId RightTriangle_T8 =  new(GuidHelper.FromIntAtEnd(46));
        public static readonly ShapeId RightTriangle_T16 = new(GuidHelper.FromIntAtEnd(47));
        public static readonly ShapeId RightTriangle_T32 = new(GuidHelper.FromIntAtEnd(48));

        public static readonly ShapeId Pentagon_T2 =  new(GuidHelper.FromIntAtEnd(49));
        public static readonly ShapeId Pentagon_T4 =  new(GuidHelper.FromIntAtEnd(50));
        public static readonly ShapeId Pentagon_T8 =  new(GuidHelper.FromIntAtEnd(51));
        public static readonly ShapeId Pentagon_T16 = new(GuidHelper.FromIntAtEnd(52));
        public static readonly ShapeId Pentagon_T32 = new(GuidHelper.FromIntAtEnd(53));

        public static readonly ShapeId Hexagon_T2 =  new(GuidHelper.FromIntAtEnd(54));
        public static readonly ShapeId Hexagon_T4 =  new(GuidHelper.FromIntAtEnd(55));
        public static readonly ShapeId Hexagon_T8 =  new(GuidHelper.FromIntAtEnd(56));
        public static readonly ShapeId Hexagon_T16 = new(GuidHelper.FromIntAtEnd(57));
        public static readonly ShapeId Hexagon_T32 = new(GuidHelper.FromIntAtEnd(58));

        public static readonly ShapeId Heptagon_T2 =  new(GuidHelper.FromIntAtEnd(59));
        public static readonly ShapeId Heptagon_T4 =  new(GuidHelper.FromIntAtEnd(60));
        public static readonly ShapeId Heptagon_T8 =  new(GuidHelper.FromIntAtEnd(61));
        public static readonly ShapeId Heptagon_T16 = new(GuidHelper.FromIntAtEnd(62));
        public static readonly ShapeId Heptagon_T32 = new(GuidHelper.FromIntAtEnd(63));

        public static readonly ShapeId Octagon_T2 =  new(GuidHelper.FromIntAtEnd(64));
        public static readonly ShapeId Octagon_T4 =  new(GuidHelper.FromIntAtEnd(65));
        public static readonly ShapeId Octagon_T8 =  new(GuidHelper.FromIntAtEnd(66));
        public static readonly ShapeId Octagon_T16 = new(GuidHelper.FromIntAtEnd(67));
        public static readonly ShapeId Octagon_T32 = new(GuidHelper.FromIntAtEnd(68));

        public static readonly ShapeId Nonagon_T2 =  new(GuidHelper.FromIntAtEnd(69));
        public static readonly ShapeId Nonagon_T4 =  new(GuidHelper.FromIntAtEnd(70));
        public static readonly ShapeId Nonagon_T8 =  new(GuidHelper.FromIntAtEnd(71));
        public static readonly ShapeId Nonagon_T16 = new(GuidHelper.FromIntAtEnd(72));
        public static readonly ShapeId Nonagon_T32 = new(GuidHelper.FromIntAtEnd(73));

        public static readonly ShapeId Decagon_T2 =  new(GuidHelper.FromIntAtEnd(74));
        public static readonly ShapeId Decagon_T4 =  new(GuidHelper.FromIntAtEnd(75));
        public static readonly ShapeId Decagon_T8 =  new(GuidHelper.FromIntAtEnd(76));
        public static readonly ShapeId Decagon_T16 = new(GuidHelper.FromIntAtEnd(77));
        public static readonly ShapeId Decagon_T32 = new(GuidHelper.FromIntAtEnd(78));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ShapeId a, ShapeId b) => a.value == b.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ShapeId a, ShapeId b) => a.value != b.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ShapeId other) => value == other.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is ShapeId other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => value.GetHashCode();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => $"{nameof(ShapeId)}={value}";
    }
}
