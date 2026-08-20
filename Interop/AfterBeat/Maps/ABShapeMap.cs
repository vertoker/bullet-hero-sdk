using System;
using System.Collections.Generic;
using BH.SDK.Models.Data;
using BH.SDK.Models.Primitives;
using BH.SDK.Services.Shapes;
using BH.SDK.Utils;

namespace BH.SDK.Interop.AfterBeat
{
    // Afterbeat names a shape with a (main, option) pair; this format names it with a ShapeId. EVERY
    // preset pair now lands on a built-in shape - none is synthesized any more - because the two
    // libraries describe the same parameter space and this one finally covers all of it. What used
    // to need building at import time (a half circle OUTLINE, a quarter circle outline, an eighth
    // circle outline, a half hexagon outline and its thin twin) are ordinary catalogue entries:
    // Circle.S2_T4, Circle.S4_T4, Circle.S8_T4, Hexagon.S2_T4, Hexagon.S2_T16.
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
    // WHICH _T* PRESET AN OUTLINE IS was measured off the meshes rather than off the naming. Every
    // outline in both libraries is the outer loop with an inner loop scaled about the centroid, so
    // one number describes each - inner/outer, higher being thinner - and the pick is the nearest
    // rung of this format's ladder (T2 = 0.50, T4 = 0.75, T8 = 0.875, T16 = 0.9375, T32 = 0.96875):
    //
    //   Square Outline           0.750  -> T4    exact
    //   Circle Outline           0.750  -> T4    exact
    //   Hexagon Outline          0.700  -> T4    nearest
    //   Square Outline Thin      0.900  -> T8    nearest, and T16 was two rungs off
    //   Circle Outline Thin      0.900  -> T8    likewise
    //   Hexagon Outline Thin     0.910  -> T16   nearest
    //   Triangle Outline         0.6075 -> T2    nearest, and T4 was on the wrong side of it
    //   Right Triangle Outline   0.6284 -> RightTriangle.T4 (0.75), the nearest rung
    //
    // Only the two ARROWS have no equivalent left, and they never will have one: an arrow is not a
    // point in the sides/sector/thickness space this library is built from. They are synthesized
    // into the level's own Resources.CompositeShapes, where a level carrying its own geometry works
    // in every build - which is also why they are not preset 498 and 499.

    /// <summary> Afterbeat's (shape, option) pairs, mapped onto <see cref="ShapeId"/>. </summary>
    public static class ABShapeMap
    {
        /// <summary> What the caller must build instead of resolving a built-in shape. </summary>
        private enum Synth
        {
            None = 0,
            ArrowFull,
            ArrowHead,
        }

        // TWO OF AFTERBEAT'S SHAPES ARE ANOTHER ONE MOVED rather than another one drawn: its
        // "Triangle Bottom" is byte for byte the same isoceles triangle as its "Triangle",
        // translated up by a third of its height so its base lies on y = 0 instead of its centroid.
        // One geometry, two reference points - and reproducing it means moving the PIVOT, not
        // building a mesh. Which is also the only mapping that keeps rotation right: over there the
        // object turns about its transform, and a Triangle Bottom's transform is at its base, so it
        // swings rather than spins.
        //
        // THE PLAIN TRIANGLE NOW NEEDS AN OFFSET TOO, and that is new. Afterbeat's triangle mesh is
        // centred on its CENTROID; every shape in this library is centred on its bounding box, and
        // for a triangle those are 0.144 apart. Both entries below therefore carry an offset, and
        // an imported triangle that carried none would sit visibly low.

        /// <summary> How far the reference point sits below the shape's own box centre - the pivot
        /// this format has to write to reproduce where the source object's transform was. </summary>
        private readonly struct Entry
        {
            public readonly ShapeId Preset;
            public readonly Synth Synth;
            public readonly string Name;
            public readonly float PivotOffsetY;

            /// <summary> Whether this pair is the one an export writes for that shape. Two pairs
            /// resolve to the same triangle and only the centred one may be written back, since a
            /// ShapeId carries no pivot to tell them apart. </summary>
            public readonly bool Canonical;

            public Entry(ShapeId preset, string name, float pivotOffsetY = 0f, bool canonical = true)
            {
                Preset = preset;
                Synth = Synth.None;
                Name = name;
                PivotOffsetY = pivotOffsetY;
                Canonical = canonical;
            }
            public Entry(Synth synth, string name)
            {
                Preset = ShapeId.Null;
                Synth = synth;
                Name = name;
                PivotOffsetY = 0f;
                Canonical = false;
            }
        }

        #region Triangle reference points

        /// <summary> Distance from an AABB-centred equilateral triangle's own centre down to its
        /// centroid, as a fraction of the box - where Afterbeat puts a Triangle's transform. </summary>
        public const float TriangleCentroidOffset = 0.14433757f;

        /// <summary> The same, down to its base - where Afterbeat puts a Triangle Bottom's. </summary>
        public const float TriangleBaseOffset = 0.4330127f;

        #endregion

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
        /// makes four sides a square rather than a diamond. Answers for ShapeSynthUtils' angular
        /// convention, which measures from straight DOWN - do not hand it to ShapeLoopUtils. </summary>
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
        // A custom polygon that resolved to a BUILT-IN shape needs the same correction for a
        // different reason: this library fits a form by its bounding box while Afterbeat draws one
        // at a fixed circumradius, so its pentagon is 5% smaller than ours and its hexagon exactly
        // the same size. Same arithmetic, different pair of radii.

        /// <summary> How much bigger an object has to be drawn than the shape built for it. </summary>
        public static float GetCustomSizeCompensation(int sides)
        {
            var requested = GetCustomRadius(sides);
            var fitted = ShapeSynthUtils.FitRadius(requested, sides, UsesHalfStepPhase(sides));
            return fitted > 0f ? requested / fitted : 1f;
        }

        /// <summary> The same for a custom polygon that landed on a built-in form. </summary>
        public static float GetCatalogSizeCompensation(ShapeForm form)
        {
            var sides = form.Sides;
            var fitted = ShapeLoopUtils.FitRadius(sides, form.UsesHalfStepPhase);
            return fitted > 0f ? GetCustomRadius(sides) / fitted : 1f;
        }

        /// <summary> The same, for one source object - 1 for anything that is not a custom
        /// polygon. </summary>
        public static float GetCustomSizeCompensation(Models.VgdObject source)
        {
            if (!TryReadCustom(source, out var custom)) return 1f;

            return TryResolveCatalog(custom, out var parameters)
                ? GetCatalogSizeCompensation(parameters.Form)
                : GetCustomSizeCompensation(custom.Sides);
        }

        /// <summary> The authored 0-1 roundness as the fillet fraction the geometry is built with. </summary>
        public static float ResolveCustomRoundness(float roundness, int sides)
        {
            if (sides > MaxRoundedSides) return 0f;

            var t = Math.Clamp((sides - MinCustomSides) / 9f, 0f, 1f);
            var ceiling = MaxRoundnessAtMinSides
                          + (MaxRoundnessAtMaxSides - MaxRoundnessAtMinSides) * t;
            return ceiling * Math.Clamp(roundness, 0f, 1f);
        }

        #endregion

        private static readonly Dictionary<(int, int), Entry> Table = new()
        {
            { (0, 0), new Entry(ShapeId.Square.Fill, "Square") },
            { (0, 1), new Entry(ShapeId.Square.T4, "Square Outline") },
            { (0, 2), new Entry(ShapeId.Square.T8, "Square Outline Thin") },

            { (1, 0), new Entry(ShapeId.Circle.Fill, "Circle") },
            { (1, 1), new Entry(ShapeId.Circle.T4, "Circle Outline") },
            { (1, 2), new Entry(ShapeId.Circle.S2, "Half Circle") },
            { (1, 3), new Entry(ShapeId.Circle.S2_T4, "Half Circle Outline") },
            { (1, 4), new Entry(ShapeId.Circle.T8, "Circle Outline Thin") },
            { (1, 5), new Entry(ShapeId.Circle.S4, "Quarter Circle") },
            { (1, 6), new Entry(ShapeId.Circle.S4_T4, "Quarter Circle Outline") },
            { (1, 7), new Entry(ShapeId.Circle.S8, "Eighth Circle") },
            { (1, 8), new Entry(ShapeId.Circle.S8_T4, "Eighth Circle Outline") },

            // Triangle outlines are the one family whose width is nowhere near a quarter: Afterbeat
            // insets them by 0.39, i.e. nearly a half, so its triangle outline is a chunky ring
            // rather than a hairline. T2 (0.50) is the nearest rung and T4 (0.25) was the furthest
            // thing from it in the wrong direction.
            { (2, 0), new Entry(ShapeId.Triangle.Fill, "Triangle", TriangleCentroidOffset) },
            { (2, 1), new Entry(ShapeId.Triangle.T2, "Triangle Outline", TriangleCentroidOffset) },
            { (2, 2), new Entry(ShapeId.RightTriangle.Fill, "Right Triangle") },
            { (2, 3), new Entry(ShapeId.RightTriangle.T4, "Right Triangle Outline") },
            { (2, 4), new Entry(ShapeId.Triangle.Fill, "Triangle Bottom", TriangleBaseOffset, canonical: false) },
            { (2, 5), new Entry(ShapeId.Triangle.T2, "Triangle Bottom Outline", TriangleBaseOffset, canonical: false) },

            { (3, 0), new Entry(Synth.ArrowFull, "Full Arrow") },
            { (3, 1), new Entry(Synth.ArrowHead, "Top Arrow") },

            { (5, 0), new Entry(ShapeId.Hexagon.Fill, "Hexagon") },
            { (5, 1), new Entry(ShapeId.Hexagon.T4, "Hexagon Outline") },
            { (5, 2), new Entry(ShapeId.Hexagon.T16, "Hexagon Outline Thin") },
            { (5, 3), new Entry(ShapeId.Hexagon.S2, "Half Hexagon") },
            { (5, 4), new Entry(ShapeId.Hexagon.S2_T4, "Half Hexagon Outline") },
            { (5, 5), new Entry(ShapeId.Hexagon.S2_T16, "Half Hexagon Outline Thin") },
        };

        private static readonly Dictionary<Guid, (int Shape, int Option)> ReverseTable = BuildReverse();

        private static Dictionary<Guid, (int, int)> BuildReverse()
        {
            var reverse = new Dictionary<Guid, (int, int)>();
            foreach (var pair in Table)
            {
                var entry = pair.Value;
                if (entry.Synth != Synth.None) continue;
                if (!entry.Canonical) continue;
                reverse[entry.Preset.value] = pair.Key;
            }
            return reverse;
        }

        /// <summary> True when this main shape means "this is a text object", which has no shape at
        /// all in either format. Both of the family's options are text - the second is a newer text
        /// renderer over there, not a different kind of object. </summary>
        public static bool IsText(int shape) => shape == (int)ABShape.Text;

        /// <summary> How far a (shape, option) pair's reference point sits below its own box centre,
        /// which this format expresses as a pivot. Zero for everything but the triangles. </summary>
        public static float GetPivotOffsetY(int shape, int option)
            => Table.TryGetValue((shape, option), out var entry) ? entry.PivotOffsetY : 0f;

        #region Import

        /// <summary>
        /// Resolves a (shape, option) pair, synthesizing into <paramref name="levelShapes"/> only for
        /// the two arrows and for custom polygons the built-in library cannot name. Returns Null for
        /// text and for an unreadable pair.
        /// </summary>
        public static ShapeId Import(int shape, int option,
            IDictionary<ShapeId, CompositeShape> levelShapes,
            InteropReport report = null, string path = null, Models.VgdObject source = null)
        {
            if (IsText(shape)) return ShapeId.Null;

            // THE CUSTOM POLYGON WINS OVER THE TABLE, and there is one pair where that matters.
            // Every family's option list ends with a custom polygon, at the index ABShapeOptions
            // names; the source game decides purely on that index (BeatmapObject.IsCustom) and never
            // looks at its own shape table afterwards. For five of the six families the two agree,
            // because the custom index sits one past the last preset. For Misc they do NOT: its list
            // holds three presets (Full Arrow, Top Arrow, PA Logo) while its custom index is 2, so
            // the game's own table entry for PA Logo is unreachable content and (3, 2) is a custom
            // polygon. Deferring to the index rather than to the table is what reproduces that.
            if (option == ABShapeOptions.GetCustomOption(shape))
            {
                var declaredCustom = ImportCustom(source, levelShapes, report, path);
                if (declaredCustom.HasValue) return declaredCustom.Value;
            }

            if (!Table.TryGetValue((shape, option), out var entry))
            {
                var custom = ImportCustom(source, levelShapes, report, path);
                if (custom.HasValue) return custom.Value;

                report?.Approximated("shape_unknown",
                    $"Shape ({shape}, {option}) is not one this converter knows; those objects use a Square.",
                    path);
                return ShapeId.Square.Fill;
            }

            if (entry.Synth == Synth.None) return entry.Preset;

            var shapeId = ABIdMap.ToShapeId($"{shape}:{option}");
            if (levelShapes == null) return shapeId;
            if (levelShapes.ContainsKey(shapeId)) return shapeId;

            var built = entry.Synth == Synth.ArrowFull
                ? ShapeSynthUtils.Arrow(shapeId, entry.Name)
                : ShapeSynthUtils.ArrowHead(shapeId, entry.Name);

            if (built == null)
            {
                report?.Approximated("shape_synth_failed",
                    $"Shape '{entry.Name}' could not be built; those objects use a Square.", path);
                return ShapeId.Square.Fill;
            }

            levelShapes.Add(shapeId, built);
            report?.Info("shape_synthesized",
                "Afterbeat's two arrows are the only shapes with no equivalent here, so they were added to the level's own shape resources.",
                path);
            return shapeId;
        }

        // The custom polygon: five numbers (sides, roundness, thickness, slices, inverted). The
        // format's own description never mentions them and the five names were originally GUESSED
        // from the keys real documents carry - they turn out to be right, and the sliders that set
        // them are in the source editor (EditorElement_ShapeSettings: sides 3-32, roundness 0-1,
        // thickness 0-1, slices 1-sides).
        //
        //   sides + slices  -> how much of a full turn the shape covers (slices / sides), i.e. a
        //                      wedge, which is what makes a "half circle" out of a 32-sided one.
        //   thickness       -> 1 is filled, anything less is a ring of that width.
        //   inverted        -> a hole in the shape, which this format now expresses directly.
        //   roundness       -> a QUADRATIC BEZIER FILLET across every corner. Its default is 0.5, so
        //                      the ORDINARY custom polygon over there is a rounded one - and it is
        //                      the ONE axis this library has no rung for, which is why a rounded
        //                      custom polygon is still the only kind that has to be built.
        private readonly struct CustomShape
        {
            public readonly int Sides;
            public readonly float Roundness;
            public readonly float Thickness;
            public readonly int Slices;
            public readonly bool Inverted;

            public CustomShape(int sides, float roundness, float thickness, int slices, bool inverted)
            {
                Sides = sides;
                Roundness = roundness;
                Thickness = thickness;
                Slices = slices;
                Inverted = inverted;
            }

            public float Turns => Slices >= Sides ? 1f : Slices / (float)Sides;
        }

        private static bool TryReadCustom(Models.VgdObject source, out CustomShape custom)
        {
            custom = default;
            if (source?.CustomShape == null || source.CustomShape.Count == 0) return false;

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

            custom = new CustomShape(sides, ResolveCustomRoundness(roundness, sides), thickness,
                slices, inverted);
            return true;
        }

        // A custom polygon lands on a built-in shape whenever every one of its five numbers sits on
        // a rung this library has. That is the whole point of the exercise: a level using fifty
        // custom polygons used to write fifty shape resources, and now writes none unless it rounded
        // its corners or picked a thickness between rungs.
        private static bool TryResolveCatalog(CustomShape custom, out ShapeParameters parameters)
        {
            parameters = default;
            if (custom.Roundness > 0f) return false;
            if (!TryResolveForm(custom.Sides, out var form)) return false;
            if (!TryResolveThickness(custom.Thickness, out var thickness)) return false;
            if (!TryResolveSlice(form, custom.Turns, out var slice)) return false;

            parameters = new ShapeParameters(form, slice, ShapeSliceVariant.Primary,
                thickness, custom.Inverted);

            return !ShapeCatalogService.IsDegenerate(parameters)
                   && ShapeCatalogService.TryDecode(ShapeCatalogService.ToShapeId(parameters), out _);
        }

        private static bool TryResolveForm(int sides, out ShapeForm form)
        {
            form = default;
            foreach (var candidate in ShapeCatalogService.EnumerateForms())
            {
                if (candidate.IsRightTriangle) continue; // never what a regular custom polygon means
                if (candidate.Sides != sides) continue;

                form = candidate;
                return true;
            }
            return false;
        }

        private static bool TryResolveThickness(float thickness, out ShapeThickness rung)
        {
            rung = ShapeThickness.Filled;
            if (thickness >= 1f) return true;

            foreach (var candidate in ShapeCatalogService.Thicknesses)
            {
                if (candidate == ShapeThickness.Filled) continue;

                var width = new ShapeParameters(ShapeForm.Circle, thickness: candidate)
                    .ThicknessFraction;
                if (Math.Abs(width - thickness) > 1e-3f) continue;

                rung = candidate;
                return true;
            }
            return false;
        }

        private static bool TryResolveSlice(ShapeForm form, float turns, out ShapeSlice slice)
        {
            slice = ShapeSlice.Full;
            if (turns >= 1f) return true;
            if (ShapeCatalogService.HasNoSectors(form)) return false;

            foreach (var (candidate, variant) in ShapeCatalogService.EnumerateSectors(form))
            {
                if (variant != ShapeSliceVariant.Primary) continue;
                if (candidate == ShapeSlice.Full) continue;

                var fraction = new ShapeParameters(form, candidate).Turns;
                if (Math.Abs(fraction - turns) > 1e-4f) continue;

                slice = candidate;
                return true;
            }
            return false;
        }

        private static ShapeId? ImportCustom(Models.VgdObject source,
            IDictionary<ShapeId, CompositeShape> levelShapes, InteropReport report, string path)
        {
            if (!TryReadCustom(source, out var custom)) return null;

            if (TryResolveCatalog(custom, out var parameters))
                return ShapeCatalogService.ToShapeId(parameters);

            if (custom.Inverted)
                report?.Approximated("shape_inverted",
                    "A custom polygon that is both rounded and inverted has to be built here, and this format's geometry cannot express the hole; those shapes import solid.",
                    path);

            // The id is derived from the PARAMETERS rather than from the (shape, option) pair,
            // because one pair stands for every custom polygon in the level - deriving from the pair
            // would give a level's fifty different custom shapes one id and one geometry.
            var shapeId = ABIdMap.ToShapeId(
                $"custom:{custom.Sides}:{custom.Roundness:0.####}:{custom.Thickness:0.####}:{custom.Slices}");

            if (levelShapes == null) return shapeId;
            if (levelShapes.ContainsKey(shapeId)) return shapeId;

            var filled = custom.Thickness >= 1f;
            var name = $"Custom {custom.Sides}-gon" + (filled ? string.Empty : " Outline")
                       + (custom.Turns < 1f ? $" {custom.Slices}/{custom.Sides}" : string.Empty)
                       + (custom.Roundness > 0f ? " Rounded" : string.Empty);

            var built = ShapeSynthUtils.RoundedShape(shapeId, name, custom.Sides, custom.Roundness,
                custom.Thickness, custom.Turns, GetCustomRadius(custom.Sides),
                UsesHalfStepPhase(custom.Sides));

            if (built == null)
            {
                report?.Approximated("shape_synth_failed",
                    $"Shape '{name}' could not be built; those objects use a Square.", path);
                return ShapeId.Square.Fill;
            }

            levelShapes.Add(shapeId, built);
            report?.Info("shape_custom_polygon",
                "Custom polygons whose corners are rounded have no built-in equivalent here, so each distinct one became a shape resource.",
                path);
            return shapeId;
        }

        #endregion

        #region Export

        /// <summary>
        /// Writes a shape back as a (shape, option) pair. Anything with no preset pair of its own is
        /// written as a CUSTOM POLYGON where it can be - see <see cref="TryExportCustom"/> - so this
        /// overload is the lossy one and only a caller that cannot write csp should use it.
        /// </summary>
        /// <summary> Whether this shape has a preset pair of its own, i.e. whether
        /// <see cref="Export"/> answers exactly rather than approximately. </summary>
        public static bool ReverseTableHas(ShapeId shapeId) => ReverseTable.ContainsKey(shapeId.value);

        public static (int Shape, int Option) Export(ShapeId shapeId,
            InteropReport report = null, string path = null)
        {
            if (ReverseTable.TryGetValue(shapeId.value, out var pair)) return pair;

            report?.Approximated("shape_not_representable",
                "Afterbeat has no name for this shape; those objects export as a Square.", path);
            return (0, 0);
        }

        // EVERY built-in shape exports, and that is what the packed id bought. A preset pair is
        // written when there is one; anything else is decoded back into its four axes and written as
        // the custom polygon that means the same thing - which the source game reads in preference
        // to its own table, so the family the pair names does not matter.
        //
        // What still cannot cross is a shape the LEVEL authored: its geometry has no description in
        // the target format at all, only a mesh, and there is nothing to write five numbers from.

        /// <summary>
        /// Writes a shape as an Afterbeat custom polygon. False for a level-authored shape and for
        /// the two synthesized arrows, which have no parameters to write.
        /// </summary>
        public static bool TryExportCustom(ShapeId shapeId, out int shape, out int option,
            out List<float> customShape)
        {
            shape = (int)ABShape.Square;
            option = ABShapeOptions.SquareCustom;
            customShape = null;

            if (!ShapeCatalogService.TryDecode(shapeId, out var parameters)) return false;
            if (parameters.Form.IsRightTriangle) return false; // not a regular polygon over there

            var sides = parameters.Form.Sides;
            var thickness = parameters.IsRing ? parameters.ThicknessFraction : 1f;
            var slices = (int)Math.Round(sides * parameters.Turns);
            if (slices < 1) slices = 1;

            customShape = new List<float>(5) { sides, 0f, thickness, slices, parameters.Invert ? 1f : 0f };
            return true;
        }

        /// <summary> Which quarter a shape covers, for a caller that has to say so out of band - the
        /// target format has no second quarter, so the lower one exports as the upper one turned. </summary>
        public static bool IsSecondQuarter(ShapeId shapeId)
            => ShapeCatalogService.TryDecode(shapeId, out var parameters)
               && parameters.Variant == ShapeSliceVariant.Second;

        #endregion
    }
}
