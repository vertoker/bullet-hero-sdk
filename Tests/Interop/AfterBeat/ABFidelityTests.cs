using System.Collections.Generic;
using System.Linq;
using BH.SDK.Interop;
using BH.SDK.Interop.AfterBeat;
using BH.SDK.Interop.AfterBeat.Export;
using BH.SDK.Interop.AfterBeat.Import;
using BH.SDK.Interop.AfterBeat.Models;
using BH.SDK.Models;
using BH.SDK.Models.Keyframes;
using BH.SDK.Models.Objects;
using BH.SDK.Models.Primitives;
using BH.SDK.Models.Primitives.Resources;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
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

    /// <summary> How much of an imported level survives, measured rather than claimed - and what the import is
    /// expected to lose. </summary>
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

        #region Screen limit

        // Afterbeat offers ten window resolutions and every one of them is 16:9, so a level was
        // authored inside that frame - while nothing in the game enforces it at play time (the zoom
        // fixes the visible HEIGHT and the player is clamped in viewport space). Left unpinned, a
        // converted level shown wider hands the player more room and reveals whatever its author
        // parked off the sides.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_TheScreenLimit_IsPinnedToTheAspectTheSourceGameRunsAt()
        {
            var level = Import(LevelOf(Square(0f)));

            var limit = level.Game.Events.ScreenLimits.Single();
            var fixedLimit = limit.ScreenLimit as ScreenLimitFixed;

            Assert.IsNotNull(fixedLimit, "the frame is fixed, not merely bounded");
            Assert.AreEqual(ABEventsImporter.SourceAspectWidth, fixedLimit.Aspect.Width);
            Assert.AreEqual(ABEventsImporter.SourceAspectHeight, fixedLimit.Aspect.Height);
            Assert.AreEqual(0, limit.Frame);
        }

        // The export writes no limit either way - the target format has no field for one. What is
        // asserted here is the REPORT: a level pinned to the aspect Afterbeat runs at anyway loses
        // nothing by having it left out, and calling that a loss puts a finding on every level that
        // came from there in the first place.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Export_AScreenLimitAtTheSourceAspect_IsNotReportedAsALoss()
        {
            var imported = Import(LevelOf(Square(0f)));
            var report = new InteropReport();

            ABLevelExporter.Export(imported, null, null, report);

            Assert.IsFalse(report.Issues.Any(issue => issue.Code == "screen_limits"),
                "16:9 is the target format's own frame");
            Assert.IsTrue(report.Issues.Any(issue => issue.Code == "screen_limit_matches_source"));
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Export_AScreenLimitAtAnyOtherAspect_IsReportedAsALoss()
        {
            var imported = Import(LevelOf(Square(0f)));
            imported.Game.Events.ScreenLimits.Clear();
            imported.Game.Events.ScreenLimits.Add(new ScreenLimitKey(
                new ScreenLimitFixed(new ScreenAspect(21, 9)), 0));

            var report = new InteropReport();
            ABLevelExporter.Export(imported, null, null, report);

            Assert.IsTrue(report.Issues.Any(issue => issue.Code == "screen_limits"));
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

        #region Player size

        // Afterbeat's world is built around its own player, which is not this engine's size, so a
        // converted level states the size it needs rather than leaving the author to notice.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_ScalesThePlayerForTheWholeLevel()
        {
            var level = Import(LevelOf(Square(0f)));

            var key = level.Game.PlayerEvents.Sizes.Single();
            Assert.AreEqual(0, key.Frame, "one key on the first frame states it for the whole level");
            Assert.AreEqual(ABEventsImporter.ImportedPlayerSize, ((FloatValue)key.Value).Value, 1e-4f);
        }

        #endregion

        #region Camera-parented objects

        private static VgdObject CameraChild(string id, float sizeX, float sizeY)
        {
            var target = new VgdObject
            {
                Id = id,
                ParentId = VgdObject.CameraParentId,
                ObjectType = (int)ABObjectType.NoHit,
                Shape = (int)ABShape.Square,
                AutokillType = (int)ABAutokillType.FixedTime,
                AutokillOffset = 1f,
            };

            target.Scale.Keyframes.Add(new VgdKeyframe { Time = 0f, Values = new List<float> { sizeX, sizeY } });
            target.Color.Keyframes.Add(new VgdKeyframe { Time = 0f, Values = new List<float> { 0f } });
            return target;
        }

        private static void AddZoom(VgdLevel level, float time, float zoom)
            => level.Events[(int)ABEventTrack.CameraZoom].Add(new VgdEventKeyframe
            {
                Time = time,
                Values = new Newtonsoft.Json.Linq.JArray { zoom },
            });

        // Afterbeat hangs camera-parented content off a node scaled by zoom/20, so at the ordinary
        // authored zoom of 30 all of it is drawn half again as large as its own numbers say. This
        // format's camera carries no scale, so without rebuilding that node the content arrives at
        // two thirds of the size it was seen at.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_CameraParentedObjects_HangOffARebuiltScaleNode()
        {
            var source = LevelOf(CameraChild("pinned", 2f, 2f));
            AddZoom(source, 0f, 30f);

            var level = Import(source);

            var root = level.Game.Objects.Values
                .Single(o => o.ParentObjectId == ObjectId.Camera);
            Assert.AreEqual(ABLevelImporter.CameraScaleRootName, root.Name);
            Assert.IsNotInstanceOf<ShapeObject>(root, "the node is a transform, it draws nothing");

            var scale = (Vector2Value)root.Scales.Single().Scale;
            Assert.AreEqual(30f / ABEventsImporter.DefaultSourceZoom, scale.X, 1e-4f);
            Assert.AreEqual(30f / ABEventsImporter.DefaultSourceZoom, scale.Y, 1e-4f);

            var pinned = level.Game.Objects.Values.OfType<ShapeObject>().Single();
            Assert.AreEqual(root.ObjectId, pinned.ParentObjectId,
                "the camera parent resolves to the node, not to the camera");
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_TheScaleNode_FollowsEveryZoomKeyframe()
        {
            var source = LevelOf(CameraChild("pinned", 1f, 1f));
            AddZoom(source, 0f, 20f);
            AddZoom(source, 1f, 40f);

            var root = Import(source).Game.Objects.Values
                .Single(o => o.ParentObjectId == ObjectId.Camera);

            var scales = root.Scales.OrderBy(k => k.Frame).ToArray();
            Assert.AreEqual(2, scales.Length);
            Assert.AreEqual(1f, ((Vector2Value)scales[0].Scale).X, 1e-4f, "the neutral zoom is a factor of one");
            Assert.AreEqual(2f, ((Vector2Value)scales[1].Scale).X, 1e-4f);
        }

        // A level that never parents anything to the camera must read exactly as it did - the node
        // costs an id and a timeline row, and nothing would hang off it.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_ALevelWithNothingOnTheCamera_GetsNoScaleNode()
        {
            var source = LevelOf(Square(0f));
            AddZoom(source, 0f, 30f);

            var level = Import(source);

            Assert.IsEmpty(level.Game.Objects.Values.Where(o => o.Name == ABLevelImporter.CameraScaleRootName));
            Assert.AreEqual(1, level.Game.Objects.Count);
        }

        // The source game rebuilds the node itself, so writing it back out would scale that content
        // by the zoom twice. It is dropped and its children go back onto the camera.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Hard)]
        public void Conversion_TheScaleNode_IsFlattenedBackOntoTheCamera()
        {
            var source = LevelOf(CameraChild("pinned", 2f, 2f));
            AddZoom(source, 0f, 30f);

            var imported = ABLevelImporter.Import(source, null, Options());
            var exported = ABLevelExporter.Export(imported.Level, null, Options());

            var returned = exported.Level.Objects.Single();
            Assert.AreEqual(VgdObject.CameraParentId, returned.ParentId, "back on the camera itself");
            Assert.AreEqual(2f, returned.Scale.Keyframes.Single().GetValue(0), 1e-3f,
                "and at its own authored size, since that game applies the zoom factor itself");
        }

        #endregion

        #region Post-processing

        // An Afterbeat level has to carry a keyframe on all fourteen of its event tracks whether its
        // author touched one or not, so the question is which of them count as authored - and the
        // source game answers it rather than this converter: every LSEffectsManager.Update* sets
        // `active = intensity > 0` before writing anything, so an effect keyframed at zero is off
        // over there and is off here. Importing them all switched OFF (what this used to do) loses
        // every effect an author did reach for; importing them all ON runs a dozen full-screen
        // passes for a level that asked for none.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_PostProcessingKeyframes_FollowTheSourcesOwnActiveRule()
        {
            var tracks = new[]
            {
                ABEventTrack.Chromatic, ABEventTrack.Bloom,
                ABEventTrack.Vignette, ABEventTrack.LensDistortion,
                ABEventTrack.Grain, ABEventTrack.Glitch,
            };

            var source = LevelOf(Square(0f));
            foreach (var track in tracks)
            {
                source.Events[(int)track].Add(new VgdEventKeyframe
                {
                    Time = 0f,
                    Values = new Newtonsoft.Json.Linq.JArray { 0f, 0f, 0f },
                });
                source.Events[(int)track].Add(new VgdEventKeyframe
                {
                    Time = 1f,
                    Values = new Newtonsoft.Json.Linq.JArray { 1f, 1f, 1f },
                });
            }

            var post = Import(source).Game.PostProcessingEvents;

            AssertActiveFollowsIntensity(post.Chromatics.Select(key => key.Active), "chromatic");
            AssertActiveFollowsIntensity(post.Blooms.Select(key => key.Active), "bloom");
            AssertActiveFollowsIntensity(post.Vignettes.Select(key => key.Active), "vignette");
            AssertActiveFollowsIntensity(post.Lenses.Select(key => key.Active), "lens");
            AssertActiveFollowsIntensity(post.Grains.Select(key => key.Active), "grain");
            AssertActiveFollowsIntensity(post.AnalogGlitches.Select(key => key.Active), "analog glitch");
            AssertActiveFollowsIntensity(post.DigitalGlitches.Select(key => key.Active), "digital glitch");
        }

        private static void AssertActiveFollowsIntensity(IEnumerable<bool> active, string name)
        {
            var flags = active.ToList();
            Assert.AreEqual(2, flags.Count, $"{name}: both keyframes are imported");
            Assert.IsFalse(flags[0], $"{name}: a keyframe at zero intensity is off, as it is over there");
            Assert.IsTrue(flags[1], $"{name}: a keyframe the author gave a value to stays on");
        }

        // One rotation per keyframe over there IS a flat Hue vs Hue here, so the track crosses whole
        // and reports nothing. It was deferred for a while, and the blocker was this project's own
        // colour curves rather than the mapping - see docs/issues/COLOR_CURVES_HISTORY.md.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_TheHueTrack_LandsOnColorCurves()
        {
            var source = LevelOf(Square(0f));
            source.Events[(int)ABEventTrack.Hue].Add(new VgdEventKeyframe
            {
                Time = 0f,
                Values = new Newtonsoft.Json.Linq.JArray { 180f },
            });

            var result = ABLevelImporter.Import(source, null, Options());

            var key = result.Level.Game.PostProcessingEvents.ColorCurves.Single();
            Assert.IsNotNull(key.HueVsHue, "a rotation becomes a flat Hue vs Hue curve");
            foreach (var curveKey in key.HueVsHue.KeyFrames)
                Assert.AreEqual(ABPostProcessingMap.ImportHue(180f), curveKey.Value, 1e-4f);

            Assert.IsNull(key.Master, "no other curve is invented");
            Assert.IsNull(key.SatVsSat);
            Assert.IsFalse(result.Report.Issues.Any(issue => issue.Code == "event_hue_curves"));
        }

        // Zero rotation is the neutral, and the neutral is the absence of an edit - a converted level
        // must not arrive looking as though somebody had graded it.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_AHueKeyframeAtZero_CarriesNoCurve()
        {
            var source = LevelOf(Square(0f));
            source.Events[(int)ABEventTrack.Hue].Add(new VgdEventKeyframe
            {
                Time = 0f,
                Values = new Newtonsoft.Json.Linq.JArray { 0f },
            });

            var result = ABLevelImporter.Import(source, null, Options());

            var key = result.Level.Game.PostProcessingEvents.ColorCurves.Single();
            Assert.IsNull(key.HueVsHue);
        }

        #endregion

        #region Object type

        [TestCase(ABObjectType.Normal, true)]
        [TestCase(ABObjectType.Hit, true)]
        [TestCase(ABObjectType.NoHit, false)]
        [TestCase(ABObjectType.Helper, false)]
        [TestCase(ABObjectType.Decoration, false)]
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

        // The whole point of the emitter branch: the source game draws no standalone shape for one,
        // so importing it as a ShapeObject drew something the level never drew.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_AParticleEmitter_IsAnEffectRatherThanAShape()
        {
            var source = Square(0f);
            source.ObjectType = (int)ABObjectType.Particles;

            var level = Import(LevelOf(source));
            var imported = level.Game.Objects.Values.Single();

            Assert.IsInstanceOf<EffectObject>(imported);
            Assert.IsNotInstanceOf<ShapeObject>(imported);
            Assert.IsTrue(level.Resources.Effects.ContainsKey(((EffectObject)imported).EffectId));
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

        private static (string Value, InteropReport Report) ImportTextOf(string value)
        {
            var (text, report) = ImportTextObjectOf(value);
            return (((StringValue)text.Text).Value, report);
        }

        private static (TextObject Text, InteropReport Report) ImportTextObjectOf(string value)
        {
            var result = ABLevelImporter.Import(LevelOf(Text(value)), null, Options());
            var text = result.Level.Game.Objects.Values.OfType<TextObject>().Single();
            return (text, result.Report);
        }

        // The source game hands its authored string to TextMeshPro untouched, so every tag TMP
        // knows can appear in a level. <rotate> is the only one with nothing to play it here, and
        // an unparsed tag is not inert - it draws as its own characters. The malformed case is the
        // author's own corpus, not an invented one.
        [TestCase("a<rotate=30>b</rotate>c", "abc")]
        [TestCase("<rotate=-15rotat>x", "x")]
        [TestCase("<b>a</b><rotate=15>b", "<b>a</b>b")]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_TextRotateTags_AreRemovedAndReported(string value, string expected)
        {
            var (imported, report) = ImportTextOf(value);

            Assert.AreEqual(expected, imported);
            Assert.IsTrue(report.Issues.Any(issue => issue.Code == "text_rotate_tag"));
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_TextRotateInsideNoparse_IsContentAndSurvives()
        {
            const string value = "<noparse><rotate=30></noparse>";
            var (imported, report) = ImportTextOf(value);

            Assert.AreEqual(value, imported);
            Assert.IsFalse(report.Issues.Any(issue => issue.Code == "text_rotate_tag"));
        }

        // The source game has no font field - the typeface is a <font> tag inside the string, one of
        // the ten assets its Resources folder answers with - while a font here is a property of the
        // object. The tag therefore leaves the string and lands on the object, and the cases below
        // are the four ways that can go: one tag, the pre-migration spelling with the asset's own
        // "SDF" suffix, a name the source game itself resolves to nothing, and a string that changes
        // typeface halfway.
        [TestCase("<font=\"Inconsolata\">abc", "abc", 3)]
        [TestCase("<font=LiberationSans SDF>abc", "abc", 1)]
        [TestCase("<font=\"Oswald Bold SDF\">abc", "abc", 9)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_TextFontTag_LeavesTheStringAndLandsOnTheObject(string value, string expected,
            int fontResourceId)
        {
            var (text, report) = ImportTextObjectOf(value);

            Assert.AreEqual(expected, ((StringValue)text.Text).Value);
            Assert.AreEqual(fontResourceId, text.FontResourceId.value);
            Assert.IsTrue(report.Issues.Any(issue => issue.Code == "text_font_tag"));
        }

        // Afterbeat resolves a font by Resources.Load, so a name it does not ship loads nothing and
        // the text goes on drawing in whatever it was drawing. Nothing is lost by importing it as
        // the default, hence no issue is reported - but the tag still leaves the string, since
        // nothing here draws it as markup either.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_TextFontTagNamingNothing_IsTheDefaultAndNoLoss()
        {
            var (text, report) = ImportTextObjectOf("<font=\"Comic Sans\">abc");

            Assert.AreEqual("abc", ((StringValue)text.Text).Value);
            Assert.AreEqual(FontResourceId.Default, text.FontResourceId);
            Assert.IsFalse(report.Issues.Any(issue => issue.Code == "text_font_tag"));
            Assert.IsFalse(report.Issues.Any(issue => issue.Code == "text_font_mixed"));
        }

        // One object, one font: the typeface covering most of the string wins, and a tie goes to
        // whichever was written first. The nested case is what proves </font> pops rather than
        // clearing - "a" and "c" are Anton's, "bb" is Bangers', so Anton wins on the tie.
        [TestCase("<font=\"Inconsolata\">aaaa</font><font=\"Bangers\">b", 3)]
        [TestCase("<font=\"Bangers\">ab</font><font=\"Anton\">cd", 5)]
        [TestCase("<font=\"Anton\">a<font=\"Bangers\">bb</font>c", 6)]
        [TestCase("a<font=\"Anton\">bcd", 6)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_TextMixingFonts_KeepsTheOneCoveringMostOfIt(string value, int fontResourceId)
        {
            var (text, report) = ImportTextObjectOf(value);

            Assert.AreEqual(fontResourceId, text.FontResourceId.value);
            Assert.IsTrue(report.Issues.Any(issue => issue.Code == "text_font_mixed"));
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_TextFontInsideNoparse_IsContentAndSurvives()
        {
            const string value = "<noparse><font=\"Anton\"></noparse>";
            var (text, report) = ImportTextObjectOf(value);

            Assert.AreEqual(value, ((StringValue)text.Text).Value);
            Assert.AreEqual(FontResourceId.Default, text.FontResourceId);
            Assert.IsFalse(report.Issues.Any(issue => issue.Code == "text_font_tag"));
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_TextWithoutFontTag_KeepsTheDefaultAndReportsNothing()
        {
            var (text, report) = ImportTextObjectOf("plain text");

            Assert.AreEqual(FontResourceId.Default, text.FontResourceId);
            Assert.IsFalse(report.Issues.Any(issue => issue.Code == "text_font_tag"));
        }

        // Out again: the field becomes a tag on the front of the string. The pairing is not
        // injective, so a preset exports as its canonical source name - two of ours came from
        // Inconsolata and MajorMonoDisplay alike, and Inconsolata is the one written back.
        [TestCase("<font=\"Inconsolata\">abc", "<font=\"Inconsolata\">abc")]
        [TestCase("<font=\"MajorMonoDisplay\">abc", "<font=\"Inconsolata\">abc")]
        [TestCase("<font=\"Poorstory\">abc", "<font=\"Bangers SDF\">abc")]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Hard)]
        public void Export_TextFont_IsWrittenBackAsAFontTag(string value, string expected)
        {
            var imported = ABLevelImporter.Import(LevelOf(Text(value)), null, Options());
            var exported = ABLevelExporter.Export(imported.Level, null, Options());

            Assert.AreEqual(expected, exported.Level.Objects.Single().Text);
            Assert.IsTrue(exported.Report.Issues.Any(issue => issue.Code == "text_font_tag"));
        }

        // The default pairs with LiberationSans, which is what an untagged string draws in over
        // there - so writing a tag for it would put markup into every exported text to say what was
        // already true.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Hard)]
        public void Export_TextInTheDefaultFont_WritesNoTagAndReportsNothing()
        {
            var imported = ABLevelImporter.Import(LevelOf(Text("abc")), null, Options());
            var exported = ABLevelExporter.Export(imported.Level, null, Options());

            Assert.AreEqual("abc", exported.Level.Objects.Single().Text);
            Assert.IsFalse(exported.Report.Issues.Any(issue => issue.Code == "text_font_tag"));
            Assert.IsFalse(exported.Report.Issues.Any(issue => issue.Code == "text_font"));
        }

        // A font the level ships itself has no name Afterbeat could resolve, so it is the one case
        // that still loses the typeface outright.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Hard)]
        public void Export_TextInALevelsOwnFont_KeepsTheStringAndReportsTheLoss()
        {
            var imported = ABLevelImporter.Import(LevelOf(Text("abc")), null, Options());
            var text = imported.Level.Game.Objects.Values.OfType<TextObject>().Single();
            text.FontResourceId = new FontResourceId(-1);

            var exported = ABLevelExporter.Export(imported.Level, null, Options());

            Assert.AreEqual("abc", exported.Level.Objects.Single().Text);
            Assert.IsTrue(exported.Report.Issues.Any(issue => issue.Code == "text_font"));
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Hard)]
        public void Conversion_TextFont_SurvivesBothDirections()
        {
            var imported = ABLevelImporter.Import(LevelOf(Text("<font=\"Anton SDF\">abc")), null, Options());
            var exported = ABLevelExporter.Export(imported.Level, null, Options());
            var returned = ABLevelImporter.Import(exported.Level, null, Options());

            var text = returned.Level.Game.Objects.Values.OfType<TextObject>().Single();
            Assert.AreEqual(6, text.FontResourceId.value, "Anton pairs with Oi in both directions");
            Assert.AreEqual("abc", ((StringValue)text.Text).Value);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_TextWithoutRotate_KeepsEveryOtherTag()
        {
            const string value = "<align=left><b>hi</b>";
            var (imported, report) = ImportTextOf(value);

            Assert.AreEqual(value, imported);
            Assert.IsFalse(report.Issues.Any(issue => issue.Code == "text_rotate_tag"));
            Assert.IsTrue(report.Issues.Any(issue => issue.Code == "text_inline_tags"));
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

        #region What five real workshop levels found

        // Everything below was measured on the five Afterbeat workshop levels the author downloaded
        // rather than reasoned about, and every one of them was a loss no round trip could see -
        // both directions agreed, so the level survived a conversion and did not survive PLAYING.

        private static VgdObject Spinner(float deltaDegrees, ABRandomType random,
            params float[] randomValues)
        {
            var target = new VgdObject
            {
                Id = "spin",
                ObjectType = (int)ABObjectType.Normal,
                Shape = (int)ABShape.Square,
                AutokillType = (int)ABAutokillType.FixedTime,
                AutokillOffset = 2f,
            };

            target.Color.Keyframes.Add(new VgdKeyframe { Time = 0f, Values = new List<float> { 0f } });
            target.Rotate.Keyframes.Add(new VgdKeyframe
            {
                Time = 0f,
                Values = new List<float> { deltaDegrees },
                RandomType = (int)random,
                RandomValues = new List<float>(randomValues),
            });

            return target;
        }

        private static void AddShake(VgdLevel level, float time, params float[] values)
        {
            var array = new Newtonsoft.Json.Linq.JArray();
            foreach (var value in values) array.Add(value);

            level.Events[(int)ABEventTrack.CameraShake].Add(new VgdEventKeyframe
            {
                Time = time,
                Values = array,
            });
        }

        // Two thirds of every rotation keyframe in the corpus carries r != 0, and the importer read
        // the value and nothing else - so an object authored to spin by a random amount spun by a
        // fixed one, on 1 334 objects of a single level, and the report said nothing at all.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_RotationRandomization_SurvivesAsARange()
        {
            var imported = Import(LevelOf(Spinner(0f, ABRandomType.Linear, 360f, 0f, 0f)));

            var angle = imported.Game.Objects.Values.OfType<ShapeObject>().Single()
                .Rotations.Single().Angle;

            Assert.IsInstanceOf<FloatMinMax>(angle, "a randomized rotation must not flatten to a literal");
            var range = (FloatMinMax)angle;
            Assert.AreEqual(0f, range.Min, 1e-4f);
            Assert.AreEqual(360f * ABValueMap.DegreesToRadians, range.Max, 1e-4f,
                "the range crosses in radians, like the value it replaces");
        }

        // The range is a range of the DELTA, so it has to be measured from the running total rather
        // than from zero - the mistake that would make every later keyframe of a spinning object
        // roll around the origin instead of around where the object had turned to.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_ARandomRotationAfterAPlainOne_IsOffsetByWhatCameBefore()
        {
            var source = Spinner(90f, ABRandomType.None);
            source.Rotate.Keyframes.Add(new VgdKeyframe
            {
                Time = 1f,
                Values = new List<float> { 0f },
                RandomType = (int)ABRandomType.Linear,
                RandomValues = new List<float> { 90f, 0f, 0f },
            });

            var rotations = Import(LevelOf(source)).Game.Objects.Values.OfType<ShapeObject>()
                .Single().Rotations.OrderBy(k => k.Frame).ToArray();

            var range = (FloatMinMax)rotations[1].Angle;
            Assert.AreEqual(90f * ABValueMap.DegreesToRadians, range.Min, 1e-4f,
                "the low end is the accumulated 90 degrees, not zero");
            Assert.AreEqual(180f * ABValueMap.DegreesToRadians, range.Max, 1e-4f);
        }

        // One random keyframe crosses exactly; it is the COMPOUNDING between several that this
        // format cannot express, so that is the only case worth a line in the report.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_ASingleRandomRotation_IsNotReportedAsApproximated()
        {
            var result = ABLevelImporter.Import(
                LevelOf(Spinner(0f, ABRandomType.Linear, 360f, 0f, 0f)), null, Options());

            Assert.IsFalse(result.Report.Issues.Any(i => i.Code == "rotation_random_not_compounded"));
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_SeveralRandomRotations_AreReportedAsNotCompounding()
        {
            var source = Spinner(0f, ABRandomType.Linear, 360f, 0f, 0f);
            source.Rotate.Keyframes.Add(new VgdKeyframe
            {
                Time = 1f,
                Values = new List<float> { 0f },
                RandomType = (int)ABRandomType.Linear,
                RandomValues = new List<float> { 360f, 0f, 0f },
            });

            var result = ABLevelImporter.Import(LevelOf(source), null, Options());

            Assert.IsTrue(result.Report.Issues.Any(i => i.Code == "rotation_random_not_compounded"));
        }

        // A shake keyframe over there is four numbers and this read only the first, on 90% of the
        // corpus' shake keyframes. The two per-axis weights are exactly this format's own and cross
        // untouched; the speed blend is folded into a rate and an amplitude - see ImportShake.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_ADirectionalShake_StaysDirectional()
        {
            var source = LevelOf(Square(0f));
            AddShake(source, 0f, 2f, 1f, 0f, 0f);

            var shake = Import(source).Game.CameraEvents.Shakes.Single();

            Assert.AreEqual(2f, shake.Intensity, 1e-4f, "no speed blend means no amplitude change");
            Assert.AreEqual(1f, shake.IntensityX, 1e-4f);
            Assert.AreEqual(0f, shake.IntensityY, 1e-4f, "a sideways-only shake must not become circular");
            Assert.AreEqual(ABEventsImporter.ShakeSpeedSlow, shake.Speed, 1e-4f);
        }

        // A keyframe that omits the three optional numbers means the source's own defaults, which
        // are "no weighting" and "the slow generator" - not zero.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_AShakeWithOnlyItsStrength_TakesTheSourcesOwnDefaults()
        {
            var source = LevelOf(Square(0f));
            AddShake(source, 0f, 3f);

            var shake = Import(source).Game.CameraEvents.Shakes.Single();

            Assert.AreEqual(3f, shake.Intensity, 1e-4f);
            Assert.AreEqual(ABEventsImporter.DefaultShakeWeight, shake.IntensityX, 1e-4f);
            Assert.AreEqual(ABEventsImporter.DefaultShakeWeight, shake.IntensityY, 1e-4f);
        }

        // The fast generator is a quarter stronger over there, and this format has one noise rather
        // than two - so that difference has to come out of the intensity or a fully-blended shake
        // is quietly weaker than it was.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_AFullyBlendedShake_CarriesTheFastGeneratorsAmplitude()
        {
            var source = LevelOf(Square(0f));
            AddShake(source, 0f, 2f, 1f, 1f, 1f);

            var shake = Import(source).Game.CameraEvents.Shakes.Single();

            Assert.AreEqual(2f * ABEventsImporter.ShakeFastAmplitude, shake.Intensity, 1e-4f);
            Assert.AreEqual(ABEventsImporter.ShakeSpeedFast, shake.Speed, 1e-4f);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Hard)]
        public void Conversion_Shake_SurvivesBothDirections()
        {
            var source = LevelOf(Square(0f));
            AddShake(source, 0f, 2f, 1f, 0.25f, 1f);

            var imported = ABLevelImporter.Import(source, null, Options());
            var exported = ABLevelExporter.Export(imported.Level, null, Options());

            var returned = exported.Level.Events[(int)ABEventTrack.CameraShake].Single();
            Assert.AreEqual(2f, returned.GetFloat(0), 1e-3f, "the amplitude the blend folded in comes back out");
            Assert.AreEqual(1f, returned.GetFloat(1), 1e-3f);
            Assert.AreEqual(0.25f, returned.GetFloat(2), 1e-3f);
            Assert.AreEqual(1f, returned.GetFloat(3), 1e-3f);
        }

        // The node mirrors the camera's own zoom track, and the camera's cap is 512 while this was
        // capped at the object cap of 32 - so on the corpus' largest level every camera-parented
        // object froze at its 32nd keyframe, at t=103s of 229s, while the camera kept zooming.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_TheScaleNode_KeepsFarMoreThanTheOldThirtyTwoKeyframes()
        {
            var source = LevelOf(CameraChild("pinned", 1f, 1f));
            for (var i = 0; i < 64; i++) AddZoom(source, i * 0.5f, 20f + i);

            var root = Import(source).Game.Objects.Values
                .Single(o => o.ParentObjectId == ObjectId.Camera);

            Assert.AreEqual(64, root.Scales.Count,
                "the node follows the camera's zoom track, which is not bounded by the object cap");
        }

        // A marker's colour is an index into the editor's own seven-entry list, and every marker
        // used to arrive white - one corpus level colours 43 markers across six of the seven.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void Import_AMarkerColour_ComesFromTheEditorsOwnPalette()
        {
            var source = LevelOf(Square(0f));
            source.Markers.Add(new VgdMarker { Id = "m", Name = "drop", Color = 3, Time = 0f });

            var marker = Import(source).Game.Events.Markers.Single();
            var expected = ABEditorColors.GridColors[3];

            Assert.AreEqual(expected.R, marker.Color4.R, 1e-4f);
            Assert.AreEqual(expected.G, marker.Color4.G, 1e-4f);
            Assert.AreEqual(expected.B, marker.Color4.B, 1e-4f);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Hard)]
        public void Conversion_AMarkerColour_SurvivesBothDirections()
        {
            var source = LevelOf(Square(0f));
            source.Markers.Add(new VgdMarker { Id = "m", Name = "drop", Color = 5, Time = 0f });

            var imported = ABLevelImporter.Import(source, null, Options());
            var exported = ABLevelExporter.Export(imported.Level, null, Options());

            Assert.AreEqual(5, exported.Level.Markers.Single().Color);
        }

        // A colour index reaches this converter as a float, because every .vgd keyframe value does,
        // and five reads truncated it. A whole number can arrive as 3.9999999 - which is what the
        // lenient int reader exists for elsewhere - and truncating picks the ADJACENT theme colour
        // with nothing reported: the object simply renders in the wrong colour.
        [TestCase(3f, 3)]
        [TestCase(3.9999999f, 4)]
        [TestCase(4.0000001f, 4)]
        [TestCase(3.5f, 4)]
        [TestCase(-3.5f, -4)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void ToIndex_RoundsRatherThanTruncating(float value, int expected)
            => Assert.AreEqual(expected, ABColorMap.ToIndex(value));

        // Cutting a string at a fixed length cuts UTF-16 code units, and a non-BMP character is two
        // of them - so a cut landing between the halves leaves a string that is not valid UTF-16.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_TextCutMidSurrogatePair_KeepsTheStringValid()
        {
            // One astral character is two chars, so 512 of them fill the cap exactly and 513 put
            // the cut squarely between a pair's two halves.
            var source = Text(string.Concat(Enumerable.Repeat("\U0001F600", 513)));

            var imported = Import(LevelOf(source)).Game.Objects.Values.OfType<TextObject>().Single();
            var text = ((StringValue)imported.Text).Value;

            Assert.LessOrEqual(text.Length, ValueRules.MaxGameString);
            Assert.IsFalse(char.IsHighSurrogate(text[^1]), "the cut must not leave half a character");
            for (var i = 0; i < text.Length; i++)
                if (char.IsHighSurrogate(text[i]))
                {
                    Assert.Less(i + 1, text.Length, "a high surrogate needs its pair");
                    Assert.IsTrue(char.IsLowSurrogate(text[i + 1]));
                    i++;
                }
        }

        // The other half of the same cut: a tag the cut opens and never closes draws its own source
        // instead of formatting anything.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_TextCutInsideATag_DropsThePartialTag()
        {
            var source = Text(new string('a', ValueRules.MaxGameString - 4) + "<color=#FF0000>b</color>");

            var imported = Import(LevelOf(source)).Game.Objects.Values.OfType<TextObject>().Single();
            var text = ((StringValue)imported.Text).Value;

            var open = text.LastIndexOf('<');
            Assert.IsTrue(open < 0 || text.IndexOf('>', open) >= 0,
                "no '<' may be left without its '>'");
        }

        // Out of range is what the source game's own draw does with it, so a hand-edited index
        // must land the same on both sides rather than throwing.
        [TestCase(-4, 0)]
        [TestCase(99, 6)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void ImportMarkerColour_OutOfRange_ClampsLikeTheSourceGame(int index, int expected)
        {
            var colour = ABEditorColors.Import(index);
            Assert.AreEqual(ABEditorColors.GridColors[expected].R, colour.R, 1e-4f);
            Assert.AreEqual(ABEditorColors.GridColors[expected].B, colour.B, 1e-4f);
        }

        #endregion
    }
}