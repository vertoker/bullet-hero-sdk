using System;
using System.Collections.Generic;
using BH.SDK.Models.Data;
using BH.SDK.Models.Primitives;
using BH.SDK.Utils;

namespace BH.SDK.Interop.AfterBeat
{
    // Afterbeat names a shape with a (main, option) pair; this format names it with a ShapeId. Most
    // pairs land on a built-in preset, because the two libraries grew from the same ancestor: the
    // _T* presets ARE outlines (T is how thin) and the _F* presets ARE fractions (F2 is a half, F4
    // a quarter, F8 an eighth), which covers Afterbeat's outline and half/quarter/eighth families
    // almost exactly.
    //
    // Seven pairs have no preset, and every one of them is a COMBINATION the built-in library never
    // made - an outline of a fraction (half/quarter/eighth circle outline, half hexagon outline),
    // plus the two arrows. They are synthesized instead of being approximated onto something else,
    // and land in the level's own Resources.CompositeShapes: a level carrying its own geometry
    // works in every build, where a 79th preset would have to exist in all of them. That is also
    // why the two arrows were removed from the preset library rather than kept for this.
    //
    // Every synthesized shape is addressed by a Guid DERIVED from its (main, option) pair, so a
    // level using half-circle outlines four hundred times gets one shape, and re-importing the same
    // level gets the same id.

    /// <summary> Afterbeat's (shape, option) pairs, mapped onto <see cref="ShapeId"/>. </summary>
    public static class ABShapeMap
    {
        /// <summary> How many sides a synthesized round shape gets. Matches the built-in circle
        /// presets closely enough that a half-circle outline sits beside a full one without
        /// reading as a different shape. </summary>
        public const int RoundSides = 24;

        /// <summary> Ring width of a synthesized outline, as a fraction of the radius. Chosen to
        /// match the _T4 presets, which is what Afterbeat calls a plain "Outline". </summary>
        public const float OutlineThickness = 0.25f;

        /// <summary> Ring width of a synthesized "Outline Thin". </summary>
        public const float ThinOutlineThickness = 0.08f;

        /// <summary> What the caller must build instead of resolving a preset. </summary>
        private enum Synth
        {
            None = 0,
            CircleOutlineHalf,
            CircleOutlineQuarter,
            CircleOutlineEighth,
            HexagonOutlineHalf,
            HexagonOutlineThinHalf,
            ArrowFull,
            ArrowHead,
        }

        private readonly struct Entry
        {
            public readonly ShapeId Preset;
            public readonly Synth Synth;
            public readonly string Name;

            public Entry(ShapeId preset, string name)
            {
                Preset = preset;
                Synth = Synth.None;
                Name = name;
            }
            public Entry(Synth synth, string name)
            {
                Preset = ShapeId.Null;
                Synth = synth;
                Name = name;
            }
        }

        private static readonly Dictionary<(int, int), Entry> Table = new()
        {
            { (0, 0), new Entry(ShapeId.Square, "Square") },
            { (0, 1), new Entry(ShapeId.Square_T4, "Square Outline") },
            { (0, 2), new Entry(ShapeId.Square_T16, "Square Outline Thin") },

            { (1, 0), new Entry(ShapeId.Circle, "Circle") },
            { (1, 1), new Entry(ShapeId.Circle_T4, "Circle Outline") },
            { (1, 2), new Entry(ShapeId.Circle_F2, "Half Circle") },
            { (1, 3), new Entry(Synth.CircleOutlineHalf, "Half Circle Outline") },
            { (1, 4), new Entry(ShapeId.Circle_T16, "Circle Outline Thin") },
            { (1, 5), new Entry(ShapeId.Circle_F4, "Quarter Circle") },
            { (1, 6), new Entry(Synth.CircleOutlineQuarter, "Quarter Circle Outline") },
            { (1, 7), new Entry(ShapeId.Circle_F8, "Eighth Circle") },
            { (1, 8), new Entry(Synth.CircleOutlineEighth, "Eighth Circle Outline") },

            { (2, 0), new Entry(ShapeId.Triangle, "Triangle") },
            { (2, 1), new Entry(ShapeId.Triangle_T4, "Triangle Outline") },
            { (2, 2), new Entry(ShapeId.RightTriangle, "Right Triangle") },
            { (2, 3), new Entry(ShapeId.RightTriangle_T4, "Right Triangle Outline") },

            { (3, 0), new Entry(Synth.ArrowFull, "Full Arrow") },
            { (3, 1), new Entry(Synth.ArrowHead, "Top Arrow") },

            { (5, 0), new Entry(ShapeId.Hexagon, "Hexagon") },
            { (5, 1), new Entry(ShapeId.Hexagon_T4, "Hexagon Outline") },
            { (5, 2), new Entry(ShapeId.Hexagon_T16, "Hexagon Outline Thin") },
            { (5, 3), new Entry(ShapeId.Hexagon_F2, "Half Hexagon") },
            { (5, 4), new Entry(Synth.HexagonOutlineHalf, "Half Hexagon Outline") },
            { (5, 5), new Entry(Synth.HexagonOutlineThinHalf, "Half Hexagon Outline Thin") },
        };

        private static readonly Dictionary<Guid, (int Shape, int Option)> ReverseTable = BuildReverse();

        private static Dictionary<Guid, (int, int)> BuildReverse()
        {
            var reverse = new Dictionary<Guid, (int, int)>();
            foreach (var pair in Table)
            {
                var entry = pair.Value;
                if (entry.Synth != Synth.None) continue;
                reverse[entry.Preset.value] = pair.Key;
            }
            return reverse;
        }

        /// <summary> True when this main shape means "this is a text object", which has no shape at
        /// all in either format. </summary>
        public static bool IsText(int shape) => shape == (int)ABShape.Text;

        /// <summary>
        /// Resolves a (shape, option) pair, synthesizing into <paramref name="levelShapes"/> when the
        /// built-in library has no equivalent. Returns Null for text and for an unreadable pair.
        /// </summary>
        public static ShapeId Import(int shape, int option,
            IDictionary<ShapeId, CompositeShape> levelShapes,
            InteropReport report = null, string path = null, Models.VgdObject source = null)
        {
            if (IsText(shape)) return ShapeId.Null;

            if (!Table.TryGetValue((shape, option), out var entry))
            {
                // A pair with no preset is where the editor's CUSTOM POLYGON lives: every shape
                // family's option list ends with one, and an object using it carries its parameters
                // in csp. So an unknown pair is only really unknown when it has none.
                var custom = ImportCustom(source, levelShapes, report, path);
                if (custom.HasValue) return custom.Value;

                report?.Approximated("shape_unknown",
                    $"Shape ({shape}, {option}) is not one this converter knows; those objects use a Square.",
                    path);
                return ShapeId.Square;
            }

            if (entry.Synth == Synth.None) return entry.Preset;

            var shapeId = ABIdMap.ToShapeId($"{shape}:{option}");
            if (levelShapes == null) return shapeId;
            if (levelShapes.ContainsKey(shapeId)) return shapeId;

            var built = Build(entry.Synth, shapeId, entry.Name);
            if (built == null)
            {
                report?.Approximated("shape_synth_failed",
                    $"Shape '{entry.Name}' could not be built; those objects use a Square.", path);
                return ShapeId.Square;
            }

            levelShapes.Add(shapeId, built);
            report?.Info("shape_synthesized",
                "Shapes with no built-in equivalent (outlined fractions, arrows) were added to the level's own shape resources.",
                path);
            return shapeId;
        }

        // The custom polygon: five numbers (sides, roundness, thickness, slices, inverted) that the
        // format's own description never mentions and that a real level uses by the thousand. All
        // five were recovered by counting the keys real documents carry that no model here reads,
        // and the first four map onto geometry this library already generates:
        //
        //   sides + slices  -> how much of a full turn the shape covers (slices / sides), i.e. a
        //                      wedge, which is what makes a "half circle" out of a 32-sided one.
        //   thickness       -> 1 is filled, anything less is a ring of that width.
        //
        // Roundness and inverted have no equivalent and are reported rather than approximated:
        // rounding a polygon's corners is a different generator, and an inverted shape is a hole in
        // one, which this format's geometry (a triangle soup with no winding rule) cannot express.
        //
        // The id is derived from the PARAMETERS rather than from the (shape, option) pair, because
        // one pair now stands for every custom polygon in the level - deriving from the pair would
        // give a level's fifty different custom shapes one id and one geometry.
        private static ShapeId? ImportCustom(Models.VgdObject source,
            IDictionary<ShapeId, CompositeShape> levelShapes, InteropReport report, string path)
        {
            if (source?.CustomShape == null || source.CustomShape.Count == 0) return null;

            var sides = (int)Math.Round(source.GetCustomShape(Models.VgdObject.CustomShapeIndex.Sides));
            var roundness = source.GetCustomShape(Models.VgdObject.CustomShapeIndex.Roundness);
            var thickness = source.GetCustomShape(Models.VgdObject.CustomShapeIndex.Thickness, 1f);
            var slices = (int)Math.Round(source.GetCustomShape(Models.VgdObject.CustomShapeIndex.Slices));
            var inverted = source.GetCustomShape(Models.VgdObject.CustomShapeIndex.Inverted) > 0.5f;

            sides = Math.Clamp(sides, ShapeSynthUtils.MinSides, ShapeSynthUtils.MaxRingSides);
            slices = Math.Clamp(slices <= 0 ? sides : slices, 1, sides);
            thickness = Math.Clamp(thickness, 0f, 1f);

            var turns = slices >= sides ? 1f : slices / (float)sides;
            var filled = thickness >= 1f;

            var shapeId = ABIdMap.ToShapeId(
                $"custom:{sides}:{thickness:0.####}:{slices}");

            if (roundness > 0f)
                report?.Approximated("shape_roundness",
                    "Afterbeat can round a custom polygon's corners; this format's geometry has no equivalent, so those shapes import with sharp ones.",
                    path);

            if (inverted)
                report?.Approximated("shape_inverted",
                    "Afterbeat can invert a custom polygon into a hole; this format's geometry cannot express one, so those shapes import solid.",
                    path);

            if (levelShapes == null) return shapeId;
            if (levelShapes.ContainsKey(shapeId)) return shapeId;

            var name = $"Custom {sides}-gon" + (filled ? string.Empty : " Outline")
                       + (turns < 1f ? $" {slices}/{sides}" : string.Empty);

            var built = filled
                ? (turns >= 1f
                    ? ShapeSynthUtils.Polygon(shapeId, name, sides)
                    : ShapeSynthUtils.Wedge(shapeId, name, sides, turns))
                : (turns >= 1f
                    ? ShapeSynthUtils.Ring(shapeId, name, sides, thickness)
                    : ShapeSynthUtils.RingWedge(shapeId, name, sides, thickness, turns));

            if (built == null)
            {
                report?.Approximated("shape_synth_failed",
                    $"Shape '{name}' could not be built; those objects use a Square.", path);
                return ShapeId.Square;
            }

            levelShapes.Add(shapeId, built);
            report?.Info("shape_custom_polygon",
                "Custom polygons were rebuilt as level-authored geometry - this format has no such shape family, so each distinct one became a shape resource.",
                path);
            return shapeId;
        }

        private static CompositeShape Build(Synth synth, ShapeId shapeId, string name) => synth switch
        {
            Synth.CircleOutlineHalf => ShapeSynthUtils.RingWedge(shapeId, name, RoundSides, OutlineThickness, 0.5f),
            Synth.CircleOutlineQuarter => ShapeSynthUtils.RingWedge(shapeId, name, RoundSides, OutlineThickness, 0.25f),
            Synth.CircleOutlineEighth => ShapeSynthUtils.RingWedge(shapeId, name, RoundSides, OutlineThickness, 0.125f),
            Synth.HexagonOutlineHalf => ShapeSynthUtils.RingWedge(shapeId, name, 6, OutlineThickness, 0.5f),
            Synth.HexagonOutlineThinHalf => ShapeSynthUtils.RingWedge(shapeId, name, 6, ThinOutlineThickness, 0.5f),
            Synth.ArrowFull => ShapeSynthUtils.Arrow(shapeId, name),
            Synth.ArrowHead => ShapeSynthUtils.ArrowHead(shapeId, name),
            _ => null,
        };

        /// <summary>
        /// Writes a shape back as a (shape, option) pair. Anything not a built-in preset - a
        /// level-authored shape, or one this converter synthesized on the way in - has no name in
        /// the target format and becomes a Square.
        /// </summary>
        public static (int Shape, int Option) Export(ShapeId shapeId,
            InteropReport report = null, string path = null)
        {
            if (ReverseTable.TryGetValue(shapeId.value, out var pair)) return pair;

            report?.Approximated("shape_not_representable",
                "Afterbeat has no name for level-authored geometry; those objects export as a Square.",
                path);
            return (0, 0);
        }
    }
}
