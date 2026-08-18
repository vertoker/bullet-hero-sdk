using System.Collections.Generic;
using System.Linq;
using BH.SDK.Interop.AfterBeat;
using BH.SDK.Interop.AfterBeat.Export;
using BH.SDK.Interop.AfterBeat.Import;
using BH.SDK.Interop.AfterBeat.Models;
using BH.SDK.Models;
using BH.SDK.Models.Keyframes;
using BH.SDK.Models.Objects;
using BH.SDK.Models.Values;
using NUnit.Framework;

namespace BH.SDK.Tests.Interop.AfterBeat
{
    // The conversions that decide whether a converted level LOOKS like the source one, as opposed
    // to whether it holds the same data. Every one of them was wrong in a way no round trip could
    // see, because both directions shared the mistake:
    //
    //   colour opacity is a PERCENTAGE there and an alpha here, so every fade in every level
    //   clamped to fully opaque;
    //   camera zoom is an orthographic half-height there and a whole visible height here, so every
    //   level was framed at half its size, and a level that never set one got this engine's
    //   default instead of the source game's;
    //   text has no bounds at all there, so an imported text was laid out inside a one-by-one block;
    //   the source game must keyframe every post-processing effect whether the author used it or
    //   not, so a converted level ran a dozen full-screen passes it never asked for.
    //
    // Numbers here are the source game's own, read out of its assembly - see AB-DEFAULT-THEMES.md
    // for the same treatment of its themes.
    public class ABFidelityTests
    {
        private const int Framerate = 60;

        private static ABOptions Options() => new(Framerate);

        private static VgdObject Square(params float[] colorValues)
        {
            var target = new VgdObject
            {
                Id = "obj",
                ObjectType = (int)ABObjectType.Normal,
                Shape = (int)ABShape.Square,
                AutokillType = (int)ABAutokillType.FixedTime,
                AutokillOffset = 1f,
            };

            target.Color.Keyframes.Add(new VgdKeyframe
            {
                Time = 0f,
                Values = new List<float>(colorValues),
            });

            return target;
        }

        private static VgdLevel LevelOf(params VgdObject[] objects)
        {
            var level = new VgdLevel();
            level.Themes.Add(ABMockData.CreateTheme());
            foreach (var source in objects) level.Objects.Add(source);
            return level;
        }

        private static Level Import(VgdLevel level, ABOptions options = null)
            => ABLevelImporter.Import(level, null, options ?? Options()).Level;

        #region Colour opacity

        // The whole bug in one table: opacity is 0-100 over there. Read as an alpha it clamps, so
        // every value an author wrote between the two ends became fully opaque - a level with no
        // fades left in it - while the two ends happened to survive and hid the rest.
        [TestCase(100f, 1f)]
        [TestCase(50f, 0.5f)]
        [TestCase(5f, 0.05f)]
        [TestCase(0f, 0f)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_Opacity_IsAPercentage(float sourceOpacity, float expectedAlpha)
        {
            var level = Import(LevelOf(Square(0f, sourceOpacity)));
            var shape = level.Game.Objects.Values.OfType<ShapeObject>().Single();
            var key = shape.Colors.Single();

            switch (key)
            {
                case Color4Key { Value: Color4ThemeRef }:
                    Assert.AreEqual(1f, expectedAlpha, "only a fully opaque colour stays a theme reference");
                    return;
                case Color4Key { Value: Color4Value literal }:
                    Assert.AreEqual(expectedAlpha, literal.A, 1e-3f);
                    return;
                default:
                    Assert.Fail($"unexpected colour shape {key.GetType().Name}");
                    return;
            }
        }

        // A keyframe carrying only its palette index is fully opaque - the source game fills the
        // missing component with 100, and reading the format's own 0 there makes every such object
        // invisible. Two thirds of the colour keyframes in a real level are this shape.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_AColorKeyframeWithNoOpacity_IsOpaqueAndStillFollowsTheTheme()
        {
            var level = Import(LevelOf(Square(3f)));
            var shape = level.Game.Objects.Values.OfType<ShapeObject>().Single();

            var key = (Color4Key)shape.Colors.Single();
            var themeRef = key.Value as Color4ThemeRef;

            Assert.IsNotNull(themeRef, "an opaque colour keeps following the theme");
            Assert.AreEqual(ABThemeMap.ObjectStartIndex + 3, themeRef.ThemeColorIndex,
                "object palette index 3 is the fourth object slot of the theme");
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Hard)]
        public void Conversion_Opacity_SurvivesBothDirections()
        {
            var source = LevelOf(Square(2f, 40f));
            var imported = ABLevelImporter.Import(source, null, Options());
            var exported = ABLevelExporter.Export(imported.Level, null, Options());

            var returned = exported.Level.Objects.Single().Color.Keyframes.Single();
            Assert.AreEqual(40f, returned.GetValue(1), 1e-2f, "opacity is written back as a percentage");
        }

        #endregion

        #region Background

        // Afterbeat's background subsystem does not convert, but the colour behind everything does:
        // it is the theme's own background slot, and a reference to it keeps following the theme
        // track instead of freezing whichever theme happened to be first.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_TheCameraBackground_ComesFromTheThemesOwnBackgroundSlot()
        {
            var level = Import(LevelOf(Square(0f)));

            var background = level.Game.Events.Backgrounds.Single();
            var themeRef = background.Value as Color3ThemeRef;

            Assert.IsNotNull(themeRef, "the background follows the theme rather than being resolved once");
            Assert.AreEqual(ABThemeMap.BackgroundIndex, themeRef.ThemeColorIndex);
            Assert.AreEqual(0, background.Frame);
        }

        #endregion

        #region Camera zoom

        [TestCase(20f, 40f)]
        [TestCase(30f, 60f)]
        [TestCase(7f, 14f)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void ImportZoomValue_DoublesTheSourcesHalfHeight(float source, float expected)
            => Assert.AreEqual(expected, ABEventsImporter.ImportZoomValue(source), 1e-4f);

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_ALevelWithNoZoomTrack_IsFramedAtTheSourceGamesDefault()
        {
            var level = Import(LevelOf(Square(0f)));
            var zoom = level.Game.CameraEvents.Zooms.Single();

            Assert.AreEqual(0, zoom.Frame);
            Assert.AreEqual(
                ABEventsImporter.ImportZoomValue(ABEventsImporter.DefaultSourceZoom),
                ((FloatValue)zoom.Zoom).Value, 1e-4f);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Conversion_Zoom_SurvivesBothDirections()
        {
            var source = LevelOf(Square(0f));
            source.Events[(int)ABEventTrack.CameraZoom].Add(new VgdEventKeyframe
            {
                Time = 0f,
                Values = new Newtonsoft.Json.Linq.JArray { 30f },
            });

            var imported = ABLevelImporter.Import(source, null, Options());
            Assert.AreEqual(60f,
                ((FloatValue)imported.Level.Game.CameraEvents.Zooms.Single().Zoom).Value, 1e-4f);

            var exported = ABLevelExporter.Export(imported.Level, null, Options());
            var returned = exported.Level.GetEvents(ABEventTrack.CameraZoom).Single();
            Assert.AreEqual(30f, returned.GetFloat(0), 1e-3f);
        }

        #endregion

        #region Post-processing

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_PostProcessingKeyframes_ArriveSwitchedOff()
        {
            var source = LevelOf(Square(0f));
            foreach (var track in new[]
                     {
                         ABEventTrack.Chromatic, ABEventTrack.Bloom,
                         ABEventTrack.Vignette, ABEventTrack.LensDistortion,
                         ABEventTrack.Grain, ABEventTrack.Glitch,
                     })
                source.Events[(int)track].Add(new VgdEventKeyframe
                {
                    Time = 0f,
                    Values = new Newtonsoft.Json.Linq.JArray { 1f, 1f, 1f },
                });

            var post = Import(source).Game.PostProcessingEvents;

            Assert.IsTrue(post.Chromatics.All(key => !key.Active));
            Assert.IsTrue(post.Blooms.All(key => !key.Active));
            Assert.IsTrue(post.Vignettes.All(key => !key.Active));
            Assert.IsTrue(post.Lenses.All(key => !key.Active));
            Assert.IsTrue(post.Grains.All(key => !key.Active));
            Assert.IsTrue(post.AnalogGlitches.All(key => !key.Active));
            Assert.IsTrue(post.DigitalGlitches.All(key => !key.Active));

            Assert.IsNotEmpty(post.Blooms, "the authored numbers are still there to switch back on");
        }

        // Temporarily off, and DEFERRED rather than dropped - nothing about the mapping is in doubt,
        // this project's own colour curves are.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_TheHueTrack_IsNotWrittenToColorCurvesYet()
        {
            var source = LevelOf(Square(0f));
            source.Events[(int)ABEventTrack.Hue].Add(new VgdEventKeyframe
            {
                Time = 0f,
                Values = new Newtonsoft.Json.Linq.JArray { 0.5f },
            });

            var result = ABLevelImporter.Import(source, null, Options());

            Assert.IsEmpty(result.Level.Game.PostProcessingEvents.ColorCurveses);
            Assert.IsTrue(result.Report.Issues.Any(issue => issue.Code == "event_hue_curves"));
        }

        #endregion

        #region Object type

        [TestCase(ABObjectType.Normal, true)]
        [TestCase(ABObjectType.Hit, true)]
        [TestCase(ABObjectType.NoHit, false)]
        [TestCase(ABObjectType.Helper, false)]
        [TestCase(ABObjectType.Decoration, false)]
        [TestCase(ABObjectType.Particles, false)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_ObjectType_DecidesWhetherThereIsACollider(ABObjectType type, bool hits)
        {
            var source = Square(0f);
            source.ObjectType = (int)type;

            var shape = Import(LevelOf(source)).Game.Objects.Values.OfType<ShapeObject>().Single();
            Assert.AreEqual(hits, shape.ColliderId.IsEnabled());
        }

        // Both of the source game's own empties, and neither may arrive carrying a shape - it is a
        // transform other objects hang off, and drawing one draws something the source never did.
        [TestCase(ABObjectType.Empty)]
        [TestCase(ABObjectType.AlphaEmpty)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_BothEmptyTypes_DrawNothing(ABObjectType type)
        {
            var source = Square(0f);
            source.ObjectType = (int)type;

            var imported = Import(LevelOf(source)).Game.Objects.Values.Single();
            Assert.IsNotInstanceOf<ShapeObject>(imported);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_AParticleEmitter_IsReportedRatherThanReadAsAnOrdinaryObject()
        {
            var source = Square(0f);
            source.ObjectType = (int)ABObjectType.Particles;

            var result = ABLevelImporter.Import(LevelOf(source), null, Options());

            Assert.IsTrue(result.Report.Issues.Any(issue => issue.Code == "object_type_particles"));
            Assert.IsFalse(result.Report.Issues.Any(issue => issue.Code == "object_type_unknown"),
                "7 is a type the source game defines, not one nobody has seen");
        }

        #endregion

        #region Text

        private static VgdObject Text(string value)
        {
            var target = new VgdObject
            {
                Id = "text",
                ObjectType = (int)ABObjectType.NoHit,
                Shape = (int)ABShape.Text,
                Text = value,
                AutokillType = (int)ABAutokillType.FixedTime,
                AutokillOffset = 1f,
            };

            target.Scale.Keyframes.Add(new VgdKeyframe { Time = 0f, Values = new List<float> { 3f, 3f } });
            return target;
        }

        [TestCase("Welcome to the meme!", 20, 1)]
        [TestCase("Lobotomy\nIncoming", 8, 2)]
        [TestCase("", 1, 1)]
        [TestCase("<color=#FF0000>hi</color>", 2, 1)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_TextBounds_AreOneCellPerCharacterAndPerLine(string value, int columns, int lines)
        {
            var text = Import(LevelOf(Text(value))).Game.Objects.Values.OfType<TextObject>().Single();

            var size = (Vector2Value)text.Sizes.Single().Scale;
            Assert.AreEqual(columns * ABObjectImporter.TextColumnWidth, size.X, 1e-4f);
            Assert.AreEqual(lines * ABObjectImporter.TextLineHeight, size.Y, 1e-4f);
        }

        // The source object's scale is the only thing sizing its glyphs over there, so it has to
        // land on the multiplier here rather than on the block - which is the estimate above and
        // means nothing on its own.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_TextScale_BecomesTheScaleMultiplierRatherThanTheBlock()
        {
            var text = Import(LevelOf(Text("abc"))).Game.Objects.Values.OfType<TextObject>().Single();

            var scale = (Vector2Value)text.Scales.Single().Scale;
            Assert.AreEqual(3f, scale.X, 1e-4f);
            Assert.AreEqual(3f, scale.Y, 1e-4f);
            Assert.AreEqual(1, text.Sizes.Count, "the block is the estimate, not the source's scale");
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_AnOrdinaryObjectsScale_StillBecomesItsSize()
        {
            var source = Square(0f);
            source.Scale.Keyframes.Add(new VgdKeyframe { Time = 0f, Values = new List<float> { 4f, 5f } });

            var shape = Import(LevelOf(source)).Game.Objects.Values.OfType<ShapeObject>().Single();

            var size = (Vector2Value)shape.Sizes.Single().Scale;
            Assert.AreEqual(4f, size.X, 1e-4f);
            Assert.AreEqual(5f, size.Y, 1e-4f);
            Assert.IsEmpty(shape.Scales);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Hard)]
        public void Conversion_TextScale_SurvivesBothDirections()
        {
            var imported = ABLevelImporter.Import(LevelOf(Text("abc")), null, Options());
            var exported = ABLevelExporter.Export(imported.Level, null, Options());

            var returned = exported.Level.Objects.Single().Scale.Keyframes.Single();
            Assert.AreEqual(3f, returned.GetValue(0), 1e-3f);
            Assert.AreEqual(3f, returned.GetValue(1), 1e-3f);
        }

        #endregion
    }
}
