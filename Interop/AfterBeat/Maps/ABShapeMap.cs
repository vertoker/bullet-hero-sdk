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
    // THE TABLE BELOW IS THE GAME'S OWN, not a transcription of a description of it. Its
    // DataManager.GameObjectShapes is an Inspector-authored list of categories, each holding
    // options that carry their own ValueIndex - and the list ORDER is not that index, which is why
    // a Circle Outline Thin is option 4 sitting third in the list. Both were read out of
    // Afterbeat_Data/level2, so the pairs below are the game's, index by index.
    //
    // Two things that reading it settled, each of which had an object landing on a Square before:
    // Triangle has SIX presets, not four (Triangle Bottom and its outline), and Misc's third entry
    // is a PA Logo that the game's own custom-polygon index makes unreachable - see Import.
    //
    // WHICH _T* PRESET AN OUTLINE IS was measured the same way, off the meshes rather than off the
    // naming. Every outline in both libraries is the outer loop with an inner loop scaled about the
    // centroid, so one number describes each - inner/outer, higher being thinner - and the pick is
    // simply the nearest rung of this format's ladder (_T2 = 0.50, _T4 = 0.75, _T8 = 0.875,
    // _T16 = 0.9375; RightTriangle_T* is not a uniform family and runs 0.51 / 0.67 / 0.71 / 0.74):
    //
    //   Square Outline           0.750  -> _T4    exact
    //   Circle Outline           0.750  -> _T4    exact
    //   Hexagon Outline          0.700  -> _T4    nearest
    //   Square Outline Thin      0.900  -> _T8    nearest, and _T16 was two rungs off
    //   Circle Outline Thin      0.900  -> _T8    likewise
    //   Hexagon Outline Thin     0.910  -> _T16   nearest
    //   Triangle Outline         0.6075 -> _T2    nearest, and _T4 was on the wrong side of it
    //   Right Triangle Outline   0.6284 -> RightTriangle_T4 (0.671), by far the nearest
    //
    // The two that are still visibly approximate are the triangles, whose real width falls between
    // two rungs; closing that needs a ring generated at an arbitrary width from a preset's own
    // outline, which this library cannot do - it knows a preset by id and never sees its geometry.
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
        // THIRTY-TWO IS NOT A TASTE, it is what both libraries are. Every circle in the source
        // game's own mesh set steps by exactly 11.25 degrees - its full circle carries 32 rim
        // points, its half 17, its quarter 9, its eighth 5 - and this format's own Circle presets
        // carry the same 32. The 24 that used to be here matched neither: a synthesized half-circle
        // outline arrived at 12 segments where the preset half circle beside it had 16, and a
        // synthesized eighth got THREE, which is a visibly faceted 45-degree arc rather than a
        // curve. At 32 the counts land on the source meshes exactly, point for point.

        /// <summary> How many sides a synthesized round shape gets over a full turn - the source
        /// game's own resolution, and this format's preset library's. </summary>
        public const int RoundSides = 32;

        // AFTERBEAT'S OUTLINES ARE NOT ONE WIDTH, and the three below were measured off its own
        // meshes rather than picked to look right. Every one of them is the shape's outer loop with
        // an inner loop scaled about the centroid, so the width is fully described by that scale:
        // its circle and square outlines inset by a quarter (inner/outer = 0.75), its HEXAGON ones
        // by three tenths (0.70), and every "Outline Thin" by about a tenth (0.90-0.91). Building
        // the hexagon wedges at the circle's own quarter drew them visibly thinner than the full
        // hexagon outline they sit beside.

        /// <summary> Ring width of a synthesized circle outline, as a fraction of the radius. </summary>
        public const float OutlineThickness = 0.25f;

        /// <summary> The same for a hexagon, which Afterbeat draws thicker. </summary>
        public const float HexagonOutlineThickness = 0.30f;

        /// <summary> Ring width of a synthesized "Outline Thin". </summary>
        public const float ThinOutlineThickness = 0.09f;

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

        // Two of Afterbeat's shapes are another one MOVED rather than another one drawn, and the
        // move is the whole difference: its "Triangle Bottom" is byte-for-byte the same isoceles
        // triangle as its "Triangle" (same width, same height, same winding), translated up by a
        // third of its height so that its base lies on y = 0 instead of its centroid. So the pair
        // is one geometry with two reference points - a triangle pivoted at its middle and one
        // pivoted at its base - and reproducing it means moving the PIVOT, not building a mesh.
        //
        // Which is also the only mapping that keeps rotation right: over there the object turns
        // about its transform, and a Triangle Bottom's transform is at its base, so it swings
        // rather than spins. A pivot is exactly that here.

        /// <summary> How far up its own box a shape sits relative to the reference point, in the
        /// same units this format's normalized geometry uses. Zero for every ordinary shape. </summary>
        private readonly struct Entry
        {
            public readonly ShapeId Preset;
            public readonly Synth Synth;
            public readonly string Name;
            public readonly float PivotOffsetY;

            public Entry(ShapeId preset, string name, float pivotOffsetY = 0f)
            {
                Preset = preset;
                Synth = Synth.None;
                Name = name;
                PivotOffsetY = pivotOffsetY;
            }
            public Entry(Synth synth, string name)
            {
                Preset = ShapeId.Null;
                Synth = synth;
                Name = name;
                PivotOffsetY = 0f;
            }
        }

        #region Custom polygon numbers

        /// <summary> The source editor's own slider bounds for a custom polygon's side count. </summary>
        public const int MinCustomSides = 3;
        public const int MaxCustomSides = 32;

        /// <summary> What the source game reads for a custom polygon that wrote no side count. </summary>
        public const float DefaultCustomSides = 3f;

        /// <summary> And for one that wrote no roundness - so an ordinary custom polygon over there
        /// is HALF rounded, not sharp. </summary>
        public const float DefaultCustomRoundness = 0.5f;

        /// <summary> Above this many sides the source game rounds nothing at all. </summary>
        public const int MaxRoundedSides = 12;

        /// <summary> The authored 0-1 roundness is remapped onto 0..this, which narrows as the
        /// polygon gains sides - a triangle rounds by up to half its radius, a twelve-sided shape by
        /// a quarter. </summary>
        public const float MaxRoundnessAtMinSides = 0.5f;
        public const float MaxRoundnessAtMaxSides = 0.25f;

        /// <summary> Circumradius of a custom polygon, which the source game varies by side count so
        /// the shape meets the box rather than being inscribed in it. </summary>
        public static float GetCustomRadius(int sides) => sides switch
        {
            3 => 0.575f,
            4 => 0.7071f,
            _ => ShapeSynthUtils.Radius,
        };

        /// <summary> Whether a custom polygon's first corner sits half a step round, which is what
        /// makes four sides a square rather than a diamond. </summary>
        public static bool UsesHalfStepPhase(int sides) => sides == 4 || sides % 2 == 1;

        // A THREE-SIDED CUSTOM POLYGON DOES NOT FIT, and that is this format's rule rather than a
        // rounding: the source game draws it at a circumradius of 0.575, which puts its apex at
        // 0.575 while ValueRules.MaxShapePoint stops at 0.5. Geometry is clamped into that box on
        // the way in, so building it at its real size would flatten the apex - the one corner the
        // shape is recognised by - instead of shrinking it.
        //
        // So the SHAPE is built to fit and the OBJECT is grown back by the same factor. That lands
        // it at the size the source level draws, exactly, and it has to be the object's Size rather
        // than its Scale: a Scale would reach the object's children, and none of them got smaller.
        //
        // Four sides needs none of this. Its 0.7071 puts the corners on the box corners, which is
        // where an axis-aligned unit square's corners belong, so it fits at full size.

        /// <summary> How much bigger an object has to be drawn than the shape built for it, because
        /// the shape had to be shrunk to fit the box. 1 whenever it did not. </summary>
        public static float GetCustomSizeCompensation(int sides)
        {
            var requested = GetCustomRadius(sides);
            var fitted = ShapeSynthUtils.FitRadius(requested, sides, UsesHalfStepPhase(sides));
            return fitted > 0f ? requested / fitted : 1f;
        }

        /// <summary> The same, for one source object - 1 for anything that is not a custom
        /// polygon. </summary>
        public static float GetCustomSizeCompensation(Models.VgdObject source)
        {
            if (source?.CustomShape == null || source.CustomShape.Count == 0) return 1f;
            if (source.ShapeOption != ABShapeOptions.GetCustomOption(source.Shape)) return 1f;

            var sides = (int)Math.Round(source.GetCustomShape(
                Models.VgdObject.CustomShapeIndex.Sides, DefaultCustomSides));
            return GetCustomSizeCompensation(Math.Clamp(sides, MinCustomSides, MaxCustomSides));
        }

        /// <summary> The authored roundness as the fillet fraction the geometry is built with. </summary>
        public static float ResolveCustomRoundness(float roundness, int sides)
        {
            if (sides > MaxRoundedSides) return 0f;

            var t = Math.Clamp((sides - MinCustomSides) / 9f, 0f, 1f);
            var ceiling = MaxRoundnessAtMinSides
                          + (MaxRoundnessAtMaxSides - MaxRoundnessAtMinSides) * t;
            return ceiling * Math.Clamp(roundness, 0f, 1f);
        }

        #endregion

        /// <summary> Distance from an isoceles triangle's centroid to its base, as a fraction of
        /// the box - measured off both libraries' own geometry, which agree: the source game's
        /// triangle mesh spans y in [-0.2875, 0.575] and this format's Triangle preset spans
        /// [-0.288675, 0.57735]. </summary>
        public const float TriangleCentroidOffset = 0.288675f;

        private static readonly Dictionary<(int, int), Entry> Table = new()
        {
            { (0, 0), new Entry(ShapeId.Square, "Square") },
            { (0, 1), new Entry(ShapeId.Square_T4, "Square Outline") },
            { (0, 2), new Entry(ShapeId.Square_T8, "Square Outline Thin") },

            { (1, 0), new Entry(ShapeId.Circle, "Circle") },
            { (1, 1), new Entry(ShapeId.Circle_T4, "Circle Outline") },
            { (1, 2), new Entry(ShapeId.Circle_F2, "Half Circle") },
            { (1, 3), new Entry(Synth.CircleOutlineHalf, "Half Circle Outline") },
            { (1, 4), new Entry(ShapeId.Circle_T8, "Circle Outline Thin") },
            { (1, 5), new Entry(ShapeId.Circle_F4, "Quarter Circle") },
            { (1, 6), new Entry(Synth.CircleOutlineQuarter, "Quarter Circle Outline") },
            { (1, 7), new Entry(ShapeId.Circle_F8, "Eighth Circle") },
            { (1, 8), new Entry(Synth.CircleOutlineEighth, "Eighth Circle Outline") },

            // Triangle outlines are the one family whose width is nowhere near a quarter: Afterbeat
            // insets them by 0.39, i.e. nearly a half, so its triangle outline is a chunky ring
            // rather than a hairline. _T2 (0.50) is the nearest preset and _T4 (0.25) was the
            // furthest thing from it in the wrong direction.
            { (2, 0), new Entry(ShapeId.Triangle, "Triangle") },
            { (2, 1), new Entry(ShapeId.Triangle_T2, "Triangle Outline") },
            { (2, 2), new Entry(ShapeId.RightTriangle, "Right Triangle") },
            { (2, 3), new Entry(ShapeId.RightTriangle_T4, "Right Triangle Outline") },
            { (2, 4), new Entry(ShapeId.Triangle, "Triangle Bottom", TriangleCentroidOffset) },
            { (2, 5), new Entry(ShapeId.Triangle_T2, "Triangle Bottom Outline", TriangleCentroidOffset) },

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

        // A preset that two source options both resolve to (a triangle and the same triangle
        // pivoted at its base) must export as the CENTRED one, since a ShapeId carries no pivot to
        // tell them apart. Skipping the offset entries rather than letting the last one win is what
        // makes that a decision instead of a dictionary-ordering accident.
        private static Dictionary<Guid, (int, int)> BuildReverse()
        {
            var reverse = new Dictionary<Guid, (int, int)>();
            foreach (var pair in Table)
            {
                var entry = pair.Value;
                if (entry.Synth != Synth.None) continue;
                if (entry.PivotOffsetY != 0f) continue;
                reverse[entry.Preset.value] = pair.Key;
            }
            return reverse;
        }

        /// <summary> True when this main shape means "this is a text object", which has no shape at
        /// all in either format. Both of the family's options are text - the second is a newer text
        /// renderer over there, not a different kind of object. </summary>
        public static bool IsText(int shape) => shape == (int)ABShape.Text;

        /// <summary> How far a (shape, option) pair's geometry sits above its own reference point,
        /// which this format expresses as a pivot. Zero for all but the two Triangle Bottoms. </summary>
        public static float GetPivotOffsetY(int shape, int option)
            => Table.TryGetValue((shape, option), out var entry) ? entry.PivotOffsetY : 0f;

        /// <summary>
        /// Resolves a (shape, option) pair, synthesizing into <paramref name="levelShapes"/> when the
        /// built-in library has no equivalent. Returns Null for text and for an unreadable pair.
        /// </summary>
        public static ShapeId Import(int shape, int option,
            IDictionary<ShapeId, CompositeShape> levelShapes,
            InteropReport report = null, string path = null, Models.VgdObject source = null)
        {
            if (IsText(shape)) return ShapeId.Null;

            // THE CUSTOM POLYGON WINS OVER THE TABLE, and there is one pair where that matters.
            // Every family's option list ends with a custom polygon, at the index
            // ABShapeOptions names; the source game decides purely on that index
            // (BeatmapObject.IsCustom) and never looks at its own shape table afterwards. For five
            // of the six families the two agree, because the custom index sits one past the last
            // preset. For Misc they do NOT: its list holds three presets (Full Arrow, Top Arrow,
            // PA Logo) while its custom index is 2, so the game's own table entry for PA Logo is
            // unreachable content and (3, 2) is a custom polygon. Deferring to the index rather
            // than to the table is what reproduces that - including if a later build fixes it,
            // since an object with no csp still falls through to the table below.
            if (option == ABShapeOptions.GetCustomOption(shape))
            {
                var declaredCustom = ImportCustom(source, levelShapes, report, path);
                if (declaredCustom.HasValue) return declaredCustom.Value;
            }

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

        // The custom polygon: five numbers (sides, roundness, thickness, slices, inverted). The
        // format's own description never mentions them and the five names were originally GUESSED
        // from the keys real documents carry - they turn out to be right, and the sliders that set
        // them are in the source editor (EditorElement_ShapeSettings: sides 3-32, roundness 0-1,
        // thickness 0-1, slices 1-sides). What was NOT right was treating them as decoration.
        //
        //   sides + slices  -> how much of a full turn the shape covers (slices / sides), i.e. a
        //                      wedge, which is what makes a "half circle" out of a 32-sided one.
        //   thickness       -> 1 is filled, anything less is a ring of that width.
        //   roundness       -> a QUADRATIC BEZIER FILLET across every corner, five points each, and
        //                      the single biggest thing this conversion used to throw away. It is
        //                      not a subtle bevel: a rounded six-sided shape reads as a squircle
        //                      and the same shape built sharp reads as a hexagon, which is exactly
        //                      the "looks low-poly" a converted level had. Its default is 0.5, so
        //                      the ORDINARY custom polygon over there is a rounded one.
        //
        // Two numbers behind it are the game's own and not obvious. The authored 0-1 roundness is
        // REMAPPED before it is used - onto 0..Lerp(0.5, 0.25, (sides - 3) / 9) - and above twelve
        // sides it is forced to zero, because a shape that round already has no corners to speak of.
        // And the radius is not one number either: a triangle is drawn at 0.575, a square at 0.7071
        // and everything else at 0.5, which is what puts a square's corners on the box instead of
        // inscribing it. Both are reproduced here.
        //
        // Inverted still has no equivalent - it is a hole in a shape, and this format's geometry (a
        // triangle soup with no winding rule) cannot express one.
        //
        // The id is derived from the PARAMETERS rather than from the (shape, option) pair, because
        // one pair now stands for every custom polygon in the level - deriving from the pair would
        // give a level's fifty different custom shapes one id and one geometry.
        private static ShapeId? ImportCustom(Models.VgdObject source,
            IDictionary<ShapeId, CompositeShape> levelShapes, InteropReport report, string path)
        {
            if (source?.CustomShape == null || source.CustomShape.Count == 0) return null;

            var sides = (int)Math.Round(source.GetCustomShape(
                Models.VgdObject.CustomShapeIndex.Sides, DefaultCustomSides));
            var roundness = source.GetCustomShape(
                Models.VgdObject.CustomShapeIndex.Roundness, DefaultCustomRoundness);
            var thickness = source.GetCustomShape(Models.VgdObject.CustomShapeIndex.Thickness, 1f);
            var slices = (int)Math.Round(source.GetCustomShape(
                Models.VgdObject.CustomShapeIndex.Slices, -1f));
            var inverted = source.GetCustomShape(Models.VgdObject.CustomShapeIndex.Inverted) > 0.5f;

            sides = Math.Clamp(sides, MinCustomSides, MaxCustomSides);
            slices = Math.Clamp(slices <= 0 ? sides : slices, 1, sides);
            thickness = Math.Clamp(thickness, 0f, 1f);
            roundness = ResolveCustomRoundness(roundness, sides);

            var turns = slices >= sides ? 1f : slices / (float)sides;
            var filled = thickness >= 1f;

            var shapeId = ABIdMap.ToShapeId(
                $"custom:{sides}:{roundness:0.####}:{thickness:0.####}:{slices}");

            if (inverted)
                report?.Approximated("shape_inverted",
                    "Afterbeat can invert a custom polygon into a hole; this format's geometry cannot express one, so those shapes import solid.",
                    path);

            if (levelShapes == null) return shapeId;
            if (levelShapes.ContainsKey(shapeId)) return shapeId;

            var name = $"Custom {sides}-gon" + (filled ? string.Empty : " Outline")
                       + (turns < 1f ? $" {slices}/{sides}" : string.Empty)
                       + (roundness > 0f ? " Rounded" : string.Empty);

            var built = ShapeSynthUtils.RoundedShape(shapeId, name, sides, roundness, thickness, turns,
                GetCustomRadius(sides), UsesHalfStepPhase(sides));

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
            Synth.HexagonOutlineHalf => ShapeSynthUtils.RingWedge(shapeId, name, 6, HexagonOutlineThickness, 0.5f),
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
