using System.Linq;
using BH.SDK.Interop.AfterBeat;
using BH.SDK.Interop.AfterBeat.Import;
using BH.SDK.Models.Enums;
using BH.SDK.Rules;
using NUnit.Framework;

namespace BH.SDK.Tests.Interop.AfterBeat
{
    // Post-processing is the part of the two formats that agrees on every MEANING and, for three of
    // its numbers, on no RANGE either. Which three is not a matter of opinion: the source game's
    // EventManager remaps exactly chromatic, bloom diffusion and lens intensity on the way to a URP
    // volume, and hands every other value over as it stands. So a value it does not remap is
    // ALREADY a URP value and must cross untouched - scaling it here is as wrong as failing to
    // scale one it does remap, and less visible, since the result is merely too weak rather than
    // clamped.
    //
    // The cases below are the ends and the middle of each source range, all of them read off the
    // game rather than off an inspector.
    public class ABPostProcessingMapTests
    {
        // Not remapped: LSEffectsManager.UpdateBloom writes it into bloom.intensity as it stands.
        [TestCase(0f, 0f)]
        [TestCase(5f, 5f)]
        [TestCase(10f, 10f)]
        [TestCase(80f, 10f, TestName = "ImportBloomIntensity_PastThisFormatsRange_Clamps")]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void ImportBloomIntensity_CrossesUntouched(float source, float expected)
            => Assert.AreEqual(expected, ABPostProcessingMap.ImportBloomIntensity(source), 1e-4f);

        // Remapped: LSMath.Remap(ev[1], 5, 30, 0, 1), which is (d - 5) / 25 - so the bottom of the
        // source range is NO scatter, not a sixth of it.
        [TestCase(5f, 0f)]
        [TestCase(7f, 0.08f, TestName = "ImportBloomScatter_TheSourcesOwnDefault")]
        [TestCase(15f, 0.4f)]
        [TestCase(30f, 1f)]
        [TestCase(0f, 0f, TestName = "ImportBloomScatter_BelowTheSourceRange_Clamps")]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void ImportBloomScatter_RisesWithDiffusion(float source, float expected)
            => Assert.AreEqual(expected, ABPostProcessingMap.ImportBloomScatter(source), 1e-3f);

        // Remapped onto 0-3 against a volume whose range is 0-1, so the source range SATURATES at
        // 8/3 - reproducing that is the point, since dividing by 8 instead would render a level
        // authored at 3 as a third of the aberration its author saw.
        [TestCase(0f, 0f)]
        [TestCase(1f, 0.375f)]
        [TestCase(2.6667f, 1f)]
        [TestCase(8f, 1f, TestName = "ImportChromatic_TopOfTheSourceRange_Saturates")]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void ImportChromatic_SaturatesLikeTheSourceGame(float source, float expected)
            => Assert.AreEqual(expected, ABPostProcessingMap.ImportChromatic(source), 1e-4f);

        // Not remapped: UpdateVignette writes it into vignette.intensity, whose range is 0-1. The
        // old /100 made every imported vignette invisible.
        [TestCase(0f, 0f)]
        [TestCase(0.5f, 0.5f)]
        [TestCase(1f, 1f)]
        [TestCase(100f, 1f, TestName = "ImportVignetteIntensity_PastThisFormatsRange_Clamps")]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void ImportVignetteIntensity_CrossesUntouched(float source, float expected)
            => Assert.AreEqual(expected, ABPostProcessingMap.ImportVignetteIntensity(source), 1e-4f);

        // Not remapped either, with one substitution the source game makes itself: a smoothness of
        // exactly zero becomes URP's own minimum rather than a hard edge.
        [TestCase(0f, 0.01f)]
        [TestCase(0.5f, 0.5f)]
        [TestCase(1f, 1f)]
        [TestCase(25f, 1f)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void ImportVignetteSmoothness_CrossesUntouchedExceptZero(float source, float expected)
            => Assert.AreEqual(expected, ABPostProcessingMap.ImportVignetteSmoothness(source), 1e-4f);

        // Slot 2 of a grain keyframe is a FilmGrainLookup index, not a size - and this format's own
        // enum reserves 0 for "no grain", so the two are one apart.
        [TestCase(0f, FilmGrainType.Thin1)]
        [TestCase(1f, FilmGrainType.Thin2)]
        [TestCase(9f, FilmGrainType.Large02)]
        [TestCase(40f, FilmGrainType.Large02, TestName = "ImportGrainType_PastTheSourceClamp_Saturates")]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void ImportGrainType_IsTheSamePresetTableOffsetByOne(float source, FilmGrainType expected)
            => Assert.AreEqual(expected, ABPostProcessingMap.ImportGrainType(source));

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

            Assert.AreEqual(0.08f, bloom.Scatter, 1e-3f, "a diffusion of 7 is a tight bloom, not a third of one");

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
