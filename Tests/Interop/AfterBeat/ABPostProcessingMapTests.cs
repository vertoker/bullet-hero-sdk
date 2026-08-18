using System.Linq;
using BH.SDK.Interop.AfterBeat;
using BH.SDK.Interop.AfterBeat.Import;
using BH.SDK.Rules;
using NUnit.Framework;

namespace BH.SDK.Tests.Interop.AfterBeat
{
    // Post-processing is the part of the two formats that agrees on every MEANING and on no RANGE,
    // and nothing in either document says so. Passing a number through untouched clamps at the far
    // end of this format's own range, which is a level that renders wrong while every value in it
    // is legal - a lens distortion authored at 30 becomes a permanent maximum fisheye.
    //
    // The cases below are the ends and the middle of each source range, taken from the mapping
    // specification next to the converter.
    public class ABPostProcessingMapTests
    {
        [TestCase(0f, 0f)]
        [TestCase(25f, 5f)]
        [TestCase(50f, 10f)]
        [TestCase(80f, 10f, TestName = "ImportBloomIntensity_PastTheSourceRange_Clamps")]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void ImportBloomIntensity_ScalesIntoThisFormatsRange(float source, float expected)
            => Assert.AreEqual(expected, ABPostProcessingMap.ImportBloomIntensity(source), 1e-4f);

        // The specification's reciprocal answers 6 at the low end and 1 at the high end, so after
        // clamping every legal input becomes maximum scatter and the parameter stops existing.
        // Direct instead: more diffusion is more scatter, which is also what URP means by it.
        [TestCase(5f, 0.1667f)]
        [TestCase(15f, 0.5f)]
        [TestCase(30f, 1f)]
        [TestCase(0f, 0.1667f, TestName = "ImportBloomScatter_BelowTheSourceRange_ReadsAsItsFloor")]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void ImportBloomScatter_RisesWithDiffusion(float source, float expected)
            => Assert.AreEqual(expected, ABPostProcessingMap.ImportBloomScatter(source), 1e-3f);

        [TestCase(0f, 0f)]
        [TestCase(4f, 0.5f)]
        [TestCase(8f, 1f)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void ImportChromatic_ScalesIntoThisFormatsRange(float source, float expected)
            => Assert.AreEqual(expected, ABPostProcessingMap.ImportChromatic(source), 1e-4f);

        [TestCase(0f, 0f)]
        [TestCase(50f, 0.5f)]
        [TestCase(100f, 1f)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void ImportVignetteIntensity_ScalesIntoThisFormatsRange(float source, float expected)
            => Assert.AreEqual(expected, ABPostProcessingMap.ImportVignetteIntensity(source), 1e-4f);

        // Saturating above 2 is deliberate - that is where the source format's own smoothness stops
        // being visually distinguishable.
        [TestCase(0f, 0.01f)]
        [TestCase(1f, 0.5f)]
        [TestCase(2f, 1f)]
        [TestCase(25f, 1f)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void ImportVignetteSmoothness_HalvesAndSaturates(float source, float expected)
            => Assert.AreEqual(expected, ABPostProcessingMap.ImportVignetteSmoothness(source), 1e-4f);

        // The one that actually broke a level: a real file carries -30..30 here.
        [TestCase(-80f, -1f)]
        [TestCase(-30f, -0.375f)]
        [TestCase(0f, 0f)]
        [TestCase(30f, 0.375f)]
        [TestCase(80f, 1f)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void ImportLensIntensity_ScalesIntoThisFormatsRange(float source, float expected)
            => Assert.AreEqual(expected, ABPostProcessingMap.ImportLensIntensity(source), 1e-4f);

        // Afterbeat measures the lens centre from the middle of the screen, this format from the
        // corner.
        [TestCase(-0.5f, 0f)]
        [TestCase(0f, 0.5f)]
        [TestCase(0.5f, 1f)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void ImportLensCenter_MovesTheOrigin(float source, float expected)
            => Assert.AreEqual(expected, ABPostProcessingMap.ImportLensCenter(source), 1e-4f);

        // A hue rotation of zero has to land exactly on the curve control's midpoint, or importing a
        // level that never touched its hue would rotate every colour in it.
        [TestCase(0f, 0.5f)]
        [TestCase(180f, 0f)]
        [TestCase(360f, 0.5f)]
        [TestCase(90f, 0.75f)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void ImportHue_WrapsAroundTheNeutralMidpoint(float degrees, float expected)
            => Assert.AreEqual(expected, ABPostProcessingMap.ImportHue(degrees), 1e-4f);

        [TestCase(0f)]
        [TestCase(12.5f)]
        [TestCase(45f)]
        [TestCase(-30f)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Lens_SurvivesARoundTrip(float source)
        {
            var imported = ABPostProcessingMap.ImportLensIntensity(source);
            Assert.AreEqual(source, ABPostProcessingMap.ExportLensIntensity(imported), 1e-3f);
        }

        [TestCase(5f)]
        [TestCase(17f)]
        [TestCase(30f)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void BloomScatter_SurvivesARoundTrip(float diffusion)
        {
            var imported = ABPostProcessingMap.ImportBloomScatter(diffusion);
            Assert.AreEqual(diffusion, ABPostProcessingMap.ExportBloomScatter(imported), 1e-3f);
        }

        [TestCase(0f)]
        [TestCase(90f)]
        [TestCase(270f)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Hue_SurvivesARoundTrip(float degrees)
        {
            var imported = ABPostProcessingMap.ImportHue(degrees);
            Assert.AreEqual(degrees, ABPostProcessingMap.ExportHue(imported), 1e-2f);
        }

        // The end an author actually sees: a level whose post-processing values are the source
        // format's own must import into values this format considers legal, not merely into values
        // it can hold.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_SourceRangeValues_LandInsideTheModelsOwnRules()
        {
            const string json = @"{ ""events"": [ [], [], [], [],
                [], [ { ""ev"": [8.0, 0.0] } ], [ { ""ev"": [5.0, 7.0, 9.0] } ],
                [ { ""ev"": [100.0, 25.0, 1.0, 0.0, 0.5, 0.5, 9.0] } ],
                [ { ""ev"": [-30.0, 0.0, 0.0] } ], [ { ""ev"": [1.0, 0.0, 0.1, 1.0] } ],
                [], [], [ { ""ev"": [180.0, 0.0, 0.0] } ], [] ] }";

            var post = ABLevelImporter.ImportJson(json, null, new ABOptions(60))
                .Level.Game.PostProcessingEvents;

            var bloom = post.Blooms.Single();
            Assert.LessOrEqual(bloom.Intensity, PostProcessingRules.Bloom.IntensityMax);
            Assert.LessOrEqual(bloom.Scatter, PostProcessingRules.Bloom.ScatterMax);

            Assert.LessOrEqual(post.Chromatics.Single().Intensity,
                PostProcessingRules.ChromaticAberration.IntensityMax);

            var vignette = post.Vignettes.Single();
            Assert.LessOrEqual(vignette.Intensity, PostProcessingRules.Vignette.IntensityMax);
            Assert.LessOrEqual(vignette.Smoothness, PostProcessingRules.Vignette.SmoothnessMax);

            var lens = post.Lenses.Single();
            Assert.GreaterOrEqual(lens.Intensity, PostProcessingRules.LensDistortion.IntensityMin);
            Assert.AreEqual(-0.375f, lens.Intensity, 1e-4f, "a real level's -30 is not a maximum fisheye");

            // The hue track's own mapping is still exact and still tested - it is only the WRITE
            // that is temporarily off, so this asserts the number rather than the keyframe.
            Assert.AreEqual(0f, ABPostProcessingMap.ImportHue(180f), 1e-4f, "180 degrees, wrapped");
            Assert.IsEmpty(post.ColorCurveses, "colour curves are not imported for now");
        }
    }
}
