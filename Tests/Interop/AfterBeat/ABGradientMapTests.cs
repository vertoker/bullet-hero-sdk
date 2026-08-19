using BH.SDK.Interop;
using BH.SDK.Interop.AfterBeat;
using BH.SDK.Models.Data;
using BH.SDK.Models.Enums;
using BH.SDK.Models.Interfaces.Keyframes;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Keyframes;
using BH.SDK.Models.Primitives;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using NUnit.Framework;

namespace BH.SDK.Tests.Interop.AfterBeat
{
    // The map is pure - no documents, no importer, no level - so every case here is one ramp
    // sampled at four corners. Two questions run through all of them and are worth naming, since
    // an assertion that only checks the colours would pass while getting either one wrong:
    //
    // 1. WHICH KEYFRAME the four samples collapse into. An axis-aligned ramp has to stay the
    //    two-colour keyframe it has always been; spending a Color4X4Key on it would be correct
    //    output and a worse level to edit.
    // 2. WHETHER A THEME REFERENCE SURVIVED. A corner landing exactly on an end keeps its
    //    Color4ThemeRef and the object goes on following the level's theme; a corner between the
    //    ends cannot, and becomes a literal. Asserting the colour VALUE cannot tell those apart -
    //    a baked corner sitting on an end has the right value and the wrong type - so these tests
    //    assert the VARIANT, not the numbers.
    public class ABGradientMapTests
    {
        private const int Frame = 0;
        private const float Tolerance = 1e-4f;

        /// <summary> Two object slots of the reference theme, far enough apart that a blend of
        /// them is unmistakably neither. </summary>
        private const int StartSlot = 0;
        private const int EndSlot = 1;

        #region Axis-aligned ramps keep their two-colour keyframe and their theme references

        [TestCase(0f, TestName = "Build_AlongX_StaysHorizontal(0)")]
        [TestCase(360f, TestName = "Build_AlongX_StaysHorizontal(360)")]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Build_AlongX_StaysHorizontal(float rotation)
        {
            var key = Build(ABGradientType.Linear, rotation, 1f);

            var horizontal = AssertIs<ColorHorizontalKey>(key);
            AssertThemeSlot(horizontal.Color4Left, StartSlot);
            AssertThemeSlot(horizontal.Color4Right, EndSlot);
        }

        [TestCase(90f)]
        [TestCase(270f)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Build_AlongY_BecomesVertical(float rotation)
        {
            var key = Build(ABGradientType.Linear, rotation, 1f);

            var vertical = AssertIs<ColorVerticalKey>(key);
            var upwards = rotation == 90f;
            AssertThemeSlot(vertical.Color4Bottom, upwards ? StartSlot : EndSlot);
            AssertThemeSlot(vertical.Color4Top, upwards ? EndSlot : StartSlot);
        }

        // The one the importer got wrong for as long as it existed: 180 and 270 are the same AXIS
        // as 0 and 90, so they must land on the same keyframe type with the two ends traded, not
        // on the untraded one and not on four corners.
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Build_AtAStraightAngle_TradesTheEnds()
        {
            var key = Build(ABGradientType.Linear, 180f, 1f);

            var horizontal = AssertIs<ColorHorizontalKey>(key);
            AssertThemeSlot(horizontal.Color4Left, EndSlot);
            AssertThemeSlot(horizontal.Color4Right, StartSlot);
        }

        // A ramp SHORTER than the box saturates before it reaches the edges, so every corner still
        // lands exactly on an end - 43 objects of the measured corpus sit at gs = 0.25, and losing
        // their theme references to a blend that is not there would be the expensive kind of wrong.
        [TestCase(0.25f)]
        [TestCase(0.75f)]
        [TestCase(1f)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void Build_WithASaturatingRamp_KeepsBothThemeReferences(float scale)
        {
            var key = Build(ABGradientType.Linear, 0f, scale);

            var horizontal = AssertIs<ColorHorizontalKey>(key);
            AssertThemeSlot(horizontal.Color4Left, StartSlot);
            AssertThemeSlot(horizontal.Color4Right, EndSlot);
        }

        #endregion

        #region Ramps that cannot keep them

        // Longer than the box, so neither edge reaches an end: both corners are blends and both
        // references are spent. Still horizontal - the AXIS is intact, only the colours are frozen.
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void Build_WithARampLongerThanTheBox_BakesBothEnds()
        {
            var report = new InteropReport();
            var key = Build(ABGradientType.Linear, 0f, 4f, report: report);

            var horizontal = AssertIs<ColorHorizontalKey>(key);
            Assert.IsInstanceOf<Color4Value>(horizontal.Color4Left);
            Assert.IsInstanceOf<Color4Value>(horizontal.Color4Right);
            AssertReported(report, "gradient_theme_flattened");
        }

        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void Build_AtADiagonal_SpendsFourCorners()
        {
            var report = new InteropReport();
            var key = Build(ABGradientType.Linear, 45f, 1f, report: report);

            var corners = AssertIs<Color4X4Key>(key);

            // The diagonal's own two ends still land exactly on the ramp's, so they keep their
            // references; only the two corners across the other diagonal are blends.
            AssertThemeSlot(corners.Color4BL, StartSlot);
            AssertThemeSlot(corners.Color4TR, EndSlot);
            Assert.IsInstanceOf<Color4Value>(corners.Color4BR);
            Assert.IsInstanceOf<Color4Value>(corners.Color4TL);
            AssertReported(report, "gradient_theme_flattened");
        }

        // Nothing was lost when both ends were already literals, so nothing may be reported - the
        // code names a theme reference dying, and a report nobody can act on is noise.
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void Build_BakingTwoLiterals_ReportsNothing()
        {
            var report = new InteropReport();

            ABGradientMap.Build(ABGradientType.Linear, 45f, 1f,
                new Color4Value(1f, 0f, 0f, 1f), new Color4Value(0f, 0f, 1f, 1f),
                Frame, EaseType.Linear, Theme(), true, report);

            CollectionAssert.DoesNotContain(Codes(report), "gradient_theme_flattened");
        }

        #endregion

        #region Baking switched off

        // The trade the option exists for: the same diagonal that spent two references above keeps
        // all of them, at the cost of a hard edge where the blend was.
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void Build_WithBakingOff_SnapsInsteadOfBaking()
        {
            var report = new InteropReport();
            var key = Build(ABGradientType.Linear, 45f, 1f, baking: false, report: report);

            var corners = AssertIs<Color4X4Key>(key);
            AssertThemeSlot(corners.Color4BL, StartSlot);
            AssertThemeSlot(corners.Color4TR, EndSlot);
            AssertThemeSlot(corners.Color4BR, EndSlot);
            AssertThemeSlot(corners.Color4TL, EndSlot);
            AssertReported(report, "gradient_corners_snapped");
            CollectionAssert.DoesNotContain(Codes(report), "gradient_theme_flattened");
        }

        // A ramp that already saturates has nothing to snap, so switching baking off must change
        // neither the keyframe nor the report.
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void Build_WithBakingOff_LeavesASaturatedRampAlone()
        {
            var report = new InteropReport();
            var key = Build(ABGradientType.Linear, 0f, 1f, baking: false, report: report);

            var horizontal = AssertIs<ColorHorizontalKey>(key);
            AssertThemeSlot(horizontal.Color4Left, StartSlot);
            AssertThemeSlot(horizontal.Color4Right, EndSlot);
            CollectionAssert.DoesNotContain(Codes(report), "gradient_corners_snapped");
        }

        #endregion

        #region Radial

        // Every corner of the box is the same distance from its centre, so a radial ramp has
        // exactly one corner colour however it is parameterised - and at any length the box
        // outgrows, that colour is the ramp's far end rather than the centre's.
        [TestCase(0.5f)]
        [TestCase(1f)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Build_Radial_FlattensToItsEdgeColour(float scale)
        {
            var report = new InteropReport();
            var key = Build(ABGradientType.Radial, 0f, scale, report: report);

            var single = AssertIs<Color4Key>(key);
            AssertThemeSlot(single.Value, EndSlot);
            AssertReported(report, "gradient_radial");
        }

        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Build_InvertedRadial_FlattensToTheOtherEnd()
        {
            var key = Build(ABGradientType.InvertedRadial, 0f, 1f);

            AssertThemeSlot(AssertIs<Color4Key>(key).Value, StartSlot);
        }

        #endregion

        #region Sampling

        // The claim the whole design rests on: where the clamp never bites, the four corners ARE
        // the ramp, so a bilinear blend of them reproduces it exactly. Checked against the ramp
        // evaluated directly at the box centre, which is the point a bilinear field is least
        // constrained at.
        [TestCase(0f, 1.5f)]
        [TestCase(45f, 1.5f)]
        [TestCase(30f, 2f)]
        [TestCase(123f, 4f)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Sample_WhereTheClampNeverBites_IsExact(float rotation, float scale)
        {
            Assert.IsTrue(ABGradientMap.IsExact(rotation, scale), "the case itself must be exact");

            ABGradientMap.Sample(false, rotation, scale, false,
                out var bl, out var br, out var tl, out var tr);

            // A bilinear field over the unit box reads its centre as the mean of its corners; an
            // affine one reads it as 0.5 by construction, whatever the direction or the length.
            Assert.AreEqual(0.5f, (bl + br + tl + tr) / 4f, Tolerance);

            // Affine also means the two diagonals agree - the property a saturated ramp breaks.
            Assert.AreEqual(bl + tr, br + tl, Tolerance);
        }

        [TestCase(0f, 1f, ExpectedResult = true)]
        [TestCase(90f, 1f, ExpectedResult = true)]
        [TestCase(45f, 1f, ExpectedResult = false)]
        [TestCase(45f, 1.5f, ExpectedResult = true)]
        [TestCase(0f, 0.25f, ExpectedResult = false)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public bool IsExact_MatchesTheClampCondition(float rotation, float scale)
            => ABGradientMap.IsExact(rotation, scale);

        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Sample_Inverted_MirrorsTheRamp()
        {
            ABGradientMap.Sample(false, 0f, 2f, false,
                out var bl, out var br, out _, out _);
            ABGradientMap.Sample(false, 0f, 2f, true,
                out var invertedBL, out var invertedBR, out _, out _);

            Assert.AreEqual(1f - bl, invertedBL, Tolerance);
            Assert.AreEqual(1f - br, invertedBR, Tolerance);
        }

        // A malformed file is the only way here, and dividing by it would make every corner NaN -
        // which reads as a black object rather than as a broken one.
        [TestCase(0f)]
        [TestCase(-1f)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Sample_WithNoRampLength_StaysFinite(float scale)
        {
            ABGradientMap.Sample(false, 30f, scale, false,
                out var bl, out var br, out var tl, out var tr);

            foreach (var k in new[] { bl, br, tl, tr })
            {
                Assert.IsFalse(float.IsNaN(k));
                Assert.That(k, Is.InRange(0f, 1f));
            }
        }

        #endregion

        #region Degenerate inputs

        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Build_WithNoGradient_IsASingleColour()
        {
            var key = Build(ABGradientType.None, 90f, 4f);

            AssertThemeSlot(AssertIs<Color4Key>(key).Value, StartSlot);
        }

        // Both ends being one colour is a real authored state over there - 71 of the corpus' 307
        // gradient keyframes have it - and it renders flat, so there is no ramp to sample. Under a
        // diagonal it is also the case that would otherwise bake two corners into literals and
        // report a theme reference dying, to produce the very colour it already was.
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void Build_WithBothEndsEqual_CollapsesToOneColour()
        {
            var report = new InteropReport();

            var key = ABGradientMap.Build(ABGradientType.Linear, 45f, 1f,
                new Color4ThemeRef(Slot(StartSlot)), new Color4ThemeRef(Slot(StartSlot)),
                Frame, EaseType.Linear, Theme(), true, report);

            AssertThemeSlot(AssertIs<Color4Key>(key).Value, StartSlot);
            CollectionAssert.DoesNotContain(Codes(report), "gradient_theme_flattened");
        }

        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Build_KeepsTheFrameAndEase()
        {
            var key = ABGradientMap.Build(ABGradientType.Linear, 0f, 1f,
                new Color4ThemeRef(Slot(StartSlot)), new Color4ThemeRef(Slot(EndSlot)),
                42, EaseType.InOutCubic, Theme(), true);

            Assert.AreEqual(42, key.Frame);
            Assert.AreEqual(EaseType.InOutCubic, key.Ease);
        }

        // Two corners of one keyframe routinely resolve to the same end, and handing both the same
        // instance makes editing one of them edit the other - silently, and only in the editor.
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void Build_NeverSharesAColourInstanceBetweenCorners()
        {
            var key = Build(ABGradientType.Linear, 90f, 1f);
            var corners = Corners(key);

            for (var i = 0; i < corners.Length; i++)
            for (var j = i + 1; j < corners.Length; j++)
                Assert.AreNotSame(corners[i], corners[j]);
        }

        #endregion

        #region Helpers

        private static IColor4X4Key Build(ABGradientType gradient, float rotation, float scale,
            bool baking = true, InteropReport report = null)
            => ABGradientMap.Build(gradient, rotation, scale,
                new Color4ThemeRef(Slot(StartSlot)), new Color4ThemeRef(Slot(EndSlot)),
                Frame, EaseType.Linear, Theme(), baking, report);

        /// <summary> An object palette index as the slot of the 64 it actually occupies. </summary>
        private static int Slot(int paletteIndex)
            => ABColorMap.ToThemeIndex(paletteIndex, ABPalette.Objects);

        private static ThemeData Theme()
        {
            var matrix = new Color4Value[ValueRules.ThemeCount];
            for (var i = 0; i < matrix.Length; i++)
                matrix[i] = new Color4Value(i / (float)matrix.Length, 0f, 0f, 1f);

            return new ThemeData { ThemeId = ThemeId.NewId(), Name = "reference", Matrix = matrix };
        }

        private static T AssertIs<T>(IColor4X4Key key) where T : class, IColor4X4Key
        {
            Assert.IsInstanceOf<T>(key, $"expected {typeof(T).Name}, got {key?.GetType().Name}");
            return (T)key;
        }

        private static void AssertThemeSlot(IColor4 color, int paletteIndex)
        {
            Assert.IsInstanceOf<Color4ThemeRef>(color,
                $"expected a live theme reference, got {color?.GetType().Name}");
            Assert.AreEqual(Slot(paletteIndex), ((Color4ThemeRef)color).ThemeColorIndex);
        }

        private static IColor4[] Corners(IColor4X4Key key) => key switch
        {
            Color4Key single => new[] { single.Value },
            ColorHorizontalKey h => new[] { h.Color4Left, h.Color4Right },
            ColorVerticalKey v => new[] { v.Color4Bottom, v.Color4Top },
            Color4X4Key c => new[] { c.Color4BL, c.Color4BR, c.Color4TL, c.Color4TR },
            _ => new IColor4[0],
        };

        private static string[] Codes(InteropReport report)
        {
            var codes = new string[report.Issues.Count];
            for (var i = 0; i < codes.Length; i++) codes[i] = report.Issues[i].Code;
            return codes;
        }

        private static void AssertReported(InteropReport report, string code)
            => CollectionAssert.Contains(Codes(report), code);

        #endregion
    }
}
