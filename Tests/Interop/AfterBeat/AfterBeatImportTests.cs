using System.Linq;
using BH.SDK.Interop;
using BH.SDK.Interop.AfterBeat;
using BH.SDK.Interop.AfterBeat.Import;
using BH.SDK.Interop.AfterBeat.Models;
using BH.SDK.Models.Enums;
using BH.SDK.Models.Objects;
using BH.SDK.Models.Primitives;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using NUnit.Framework;

namespace BH.SDK.Tests.Interop.AfterBeat
{
    // Whole-document import. The assertions here are deliberately about the four conversions
    // AfterBeatObjectImporter's own header calls out - keyframe locality, rotation, parent-relative
    // layers, and which of size/scale an Afterbeat "scale" becomes - because each of those produces
    // a level that loads and plays wrongly rather than one that fails.
    public class AfterBeatImportTests
    {
        private const int Framerate = 60;

        private static AfterBeatOptions Options() => new(Framerate);

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_MinimalLevel_ProducesObjectsThemesAndEvents()
        {
            var result = AfterBeatLevelImporter.Import(AfterBeatMockData.CreateLevel(), null, Options());

            Assert.IsNotNull(result.Level);
            Assert.AreEqual(Framerate, result.Level.Settings.Framerate);
            Assert.AreEqual(1, result.Level.Game.Objects.Count);
            Assert.AreEqual(1, result.Level.Resources.Themes.Count);
            Assert.AreEqual(1, result.Level.Game.Events.Markers.Count);
            Assert.AreEqual(1, result.Level.Game.Events.Checkpoints.Count);
        }

        // The one that is invisible in every other check: an object placed correctly whose keys sit
        // past its own death simply never moves.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_KeyframeFrames_StayLocalToTheObject()
        {
            var result = AfterBeatLevelImporter.Import(AfterBeatMockData.CreateLevel(), null, Options());
            var imported = result.Level.Game.Objects.Values.First();

            Assert.AreEqual(60, imported.Span.StartFrame, "object starts at 1s");
            CollectionAssert.AreEqual(new[] { 0, 120 }, imported.Positions.Select(k => k.Frame).ToArray());

            // Autokill Last Keyframe means the object dies AS it reaches its final keyframe, so
            // that keyframe lands exactly on the span's end boundary rather than inside it. It is
            // still needed: it is what every frame before it interpolates towards. Anything PAST
            // the boundary would be the real bug.
            foreach (var key in imported.Positions)
                Assert.LessOrEqual(key.Frame, imported.Span.FrameDuration,
                    "a key past the span's own end would be unreachable in both formats");
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_Rotation_IsAbsoluteRadians()
        {
            var level = new VgdLevel();
            level.Objects.Add(AfterBeatMockData.CreateRotatingObject());

            var result = AfterBeatLevelImporter.Import(level, null, Options());
            var imported = result.Level.Game.Objects.Values.First();

            var angles = imported.Rotations
                .OrderBy(k => k.Frame)
                .Select(k => ((FloatValue)k.Angle).Value)
                .ToArray();

            Assert.AreEqual(2, angles.Length);
            Assert.AreEqual(System.Math.PI / 2.0, angles[0], 1e-3f);
            Assert.AreEqual(System.Math.PI, angles[1], 1e-3f, "the second key is 90 more, not 90 again");
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_AfterbeatScale_BecomesSizeNotScale()
        {
            var result = AfterBeatLevelImporter.Import(AfterBeatMockData.CreateLevel(), null, Options());
            var imported = result.Level.Game.Objects.Values.First();

            Assert.AreEqual(1, imported.Sizes.Count);
            Assert.AreEqual(0, imported.Scales.Count);

            var size = (Vector2Value)imported.Sizes[0].Scale;
            Assert.AreEqual(2f, size.X, 1e-4f);
            Assert.AreEqual(3f, size.Y, 1e-4f);
        }

        // Depth is absolute there and Layer is parent-relative here, so a child at depth 15 under a
        // parent at depth 20 must store 5, not the 5 its own effective layer happens to equal.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_ChildLayer_IsRelativeToItsParent()
        {
            var result = AfterBeatLevelImporter.Import(AfterBeatMockData.CreateFullLevel(), null, Options());

            var shapes = result.Level.Game.Objects.Values.OfType<ShapeObject>().ToArray();
            var parent = shapes.Single(o => o.ParentObjectId == ObjectId.Null);
            var child = shapes.Single(o => o.ParentObjectId == parent.ObjectId);

            Assert.AreEqual(0, parent.Layer, "the source's default depth is this format's default layer");
            Assert.AreEqual(5, child.Layer);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_HitAndNoHit_DecideWhetherThereIsACollider()
        {
            var level = new VgdLevel();
            var hit = AfterBeatMockData.CreateObject("hit");
            var noHit = AfterBeatMockData.CreateObject("nohit");
            noHit.ObjectType = (int)AfterBeatObjectType.NoHit;
            level.Objects.Add(hit);
            level.Objects.Add(noHit);

            var result = AfterBeatLevelImporter.Import(level, null, Options());
            var shapes = result.Level.Game.Objects.Values.OfType<ShapeObject>().ToArray();

            Assert.AreEqual(1, shapes.Count(s => s.ColliderId.IsEnabled()));
            Assert.AreEqual(1, shapes.Count(s => !s.ColliderId.IsEnabled()));
            Assert.IsTrue(shapes.All(s => s.ShapeId.IsEnabled()), "both are still drawn");
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_EmptyObject_HasNoShapeAtAll()
        {
            var level = new VgdLevel();
            var empty = AfterBeatMockData.CreateObject("empty");
            empty.ObjectType = (int)AfterBeatObjectType.Empty;
            level.Objects.Add(empty);

            var result = AfterBeatLevelImporter.Import(level, null, Options());
            var imported = result.Level.Game.Objects.Values.First();

            Assert.AreEqual(ObjectType.RectObject, imported.GetModelType());
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_ParentedToCamera_ResolvesToTheCameraId()
        {
            var level = new VgdLevel();
            var child = AfterBeatMockData.CreateObject("child");
            child.ParentId = VgdObject.CameraParentId;
            level.Objects.Add(child);

            var result = AfterBeatLevelImporter.Import(level, null, Options());
            Assert.AreEqual(ObjectId.Camera, result.Level.Game.Objects.Values.First().ParentObjectId);
        }

        // The respawn position is the field Checkpoint grew for this; before it, this was a loss.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void Import_CheckpointPosition_Crosses()
        {
            var result = AfterBeatLevelImporter.Import(AfterBeatMockData.CreateLevel(), null, Options());
            var checkpoint = result.Level.Game.Events.Checkpoints[0];

            Assert.AreEqual(CheckpointSpace.World, checkpoint.Space);
            var position = (Vector2Value)checkpoint.Position;
            Assert.AreEqual(3f, position.X, 1e-4f);
            Assert.AreEqual(-4f, position.Y, 1e-4f);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_Bpm_BecomesOneBeatSegmentOverTheWholeLevel()
        {
            var result = AfterBeatLevelImporter.Import(AfterBeatMockData.CreateLevel(), null, Options());
            var beats = result.Level.Game.Events.Beats;

            Assert.AreEqual(1, beats.Count);
            Assert.AreEqual(128f, beats[0].Bpm, 1e-4f);
            Assert.AreEqual(result.Level.Settings.FrameDuration, beats[0].Span.FrameDuration);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_Events_ReachTheirOwnTracks()
        {
            var result = AfterBeatLevelImporter.Import(AfterBeatMockData.CreateFullLevel(), null, Options());
            var game = result.Level.Game;

            Assert.AreEqual(1, game.CameraEvents.Positions.Count);
            Assert.AreEqual(1, game.CameraEvents.Zooms.Count);
            Assert.AreEqual(1, game.CameraEvents.Rotations.Count);
            Assert.AreEqual(1, game.CameraEvents.Shakes.Count);
            Assert.AreEqual(1, game.PostProcessingEvents.Blooms.Count);
            Assert.AreEqual(1, game.PostProcessingEvents.Vignettes.Count);
            Assert.AreEqual(1, game.PostProcessingEvents.Chromatics.Count);
            Assert.AreEqual(1, game.Events.Themes.Count);
        }

        // The theme track names a theme by the same string the theme itself carries, so both sides
        // must derive the same Guid or every theme change in the level points at nothing.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_ThemeKeyframe_ResolvesToAThemeTheLevelActuallyHas()
        {
            var result = AfterBeatLevelImporter.Import(AfterBeatMockData.CreateFullLevel(), null, Options());
            var themeKey = result.Level.Game.Events.Themes[0];

            Assert.IsTrue(result.Level.Resources.Themes.ContainsKey(themeKey.ThemeId));
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_Prefabs_ProduceTemplatesAndPlacements()
        {
            var result = AfterBeatLevelImporter.Import(AfterBeatMockData.CreateFullLevel(), null, Options());

            Assert.AreEqual(1, result.Level.Resources.Prefabs.Count);
            var template = result.Level.Resources.Prefabs.Values.First();
            Assert.AreEqual(1, template.Objects.Count, "the template keeps its own object scope");

            var placement = result.Level.Game.Objects.Values.OfType<PrefabObject>().Single();
            Assert.IsTrue(placement.PrefabId.IsEnabled());
            Assert.AreEqual(template.PrefabId, placement.PrefabId);
            Assert.IsEmpty(placement.ObjectIds, "materializing is the host's job, not the importer's");
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_WithoutPrefabs_SkipsBothHalves()
        {
            var options = Options();
            options.ImportPrefabs = false;

            var result = AfterBeatLevelImporter.Import(AfterBeatMockData.CreateFullLevel(), null, options);

            Assert.IsEmpty(result.Level.Resources.Prefabs);
            Assert.IsEmpty(result.Level.Game.Objects.Values.OfType<PrefabObject>());
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_UnsupportedTracks_AreReportedNotThrown()
        {
            var level = AfterBeatMockData.CreateLevel();
            level.SetEvents(AfterBeatEventTrack.Gradient,
                new System.Collections.Generic.List<VgdEventKeyframe> { new() });
            level.SetEvents(AfterBeatEventTrack.Hue,
                new System.Collections.Generic.List<VgdEventKeyframe> { new() });
            level.SetEvents(AfterBeatEventTrack.PlayerForce,
                new System.Collections.Generic.List<VgdEventKeyframe> { new() });

            var result = AfterBeatLevelImporter.Import(level, null, Options());
            var codes = result.Report.Issues.Select(i => i.Code).ToArray();

            CollectionAssert.Contains(codes, "event_gradient");
            CollectionAssert.Contains(codes, "event_hue");
            CollectionAssert.Contains(codes, "event_player_force");

            // Player force is waiting on work, the other two are not - an author has to be able to
            // tell those apart.
            var playerForce = result.Report.Issues.Single(i => i.Code == "event_player_force");
            Assert.AreEqual(InteropSeverity.Deferred, playerForce.Severity);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_Parallax_BecomesCollidedlessObjectsBehindTheContent()
        {
            var level = AfterBeatMockData.CreateLevel();
            level.Parallax.Layers.Add(new VgdParallaxLayer
            {
                Depth = 1,
                Objects =
                {
                    new VgdParallaxObject
                    {
                        Id = "bg1",
                        Transform = { Position = new VgdVector2(1f, 2f), Scale = new VgdVector2(3f, 3f) },
                    },
                },
            });

            var result = AfterBeatLevelImporter.Import(level, null, Options());
            var background = result.Level.Game.Objects.Values
                .OfType<ShapeObject>()
                .Single(o => o.Layer < 0);

            Assert.IsFalse(background.ColliderId.IsEnabled(), "a background can never hit the player");
            Assert.AreEqual(1, background.Positions.Count);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_WithoutParallax_LeavesTheBackgroundOut()
        {
            var level = AfterBeatMockData.CreateLevel();
            level.Parallax.Layers.Add(new VgdParallaxLayer
            {
                Objects = { new VgdParallaxObject { Id = "bg1" } },
            });

            var options = Options();
            options.ImportParallax = false;

            var result = AfterBeatLevelImporter.Import(level, null, options);
            Assert.AreEqual(1, result.Level.Game.Objects.Count, "only the level's own object");
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_ShapeWithNoPreset_IsSynthesizedIntoLevelResources()
        {
            var level = new VgdLevel();
            var arrow = AfterBeatMockData.CreateObject("arrow");
            arrow.Shape = (int)AfterBeatShape.Arrow;
            arrow.ShapeOption = 0;
            level.Objects.Add(arrow);

            var result = AfterBeatLevelImporter.Import(level, null, Options());
            var imported = result.Level.Game.Objects.Values.OfType<ShapeObject>().Single();

            Assert.AreEqual(1, result.Level.Resources.CompositeShapes.Count);
            Assert.IsTrue(result.Level.Resources.CompositeShapes.ContainsKey(imported.ShapeId));

            var shape = result.Level.Resources.CompositeShapes[imported.ShapeId];
            Assert.GreaterOrEqual(shape.TriangleCount, ValueRules.MinShapeTriangles);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_TheSameSynthesizedShapeTwice_ProducesOneResource()
        {
            var level = new VgdLevel();
            for (var i = 0; i < 3; i++)
            {
                var arrow = AfterBeatMockData.CreateObject($"arrow{i}");
                arrow.Shape = (int)AfterBeatShape.Arrow;
                level.Objects.Add(arrow);
            }

            var result = AfterBeatLevelImporter.Import(level, null, Options());
            Assert.AreEqual(1, result.Level.Resources.CompositeShapes.Count);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void ImportJson_MalformedDocument_FailsWithoutThrowing()
        {
            var result = AfterBeatLevelImporter.ImportJson("{ this is not json", null, Options());

            Assert.IsNull(result.Level);
            Assert.IsTrue(result.Report.HasFailure);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void ImportJson_EmptyDocument_StillProducesALevel()
        {
            var result = AfterBeatLevelImporter.ImportJson("{}", null, Options());

            Assert.IsNotNull(result.Level);
            Assert.IsEmpty(result.Level.Game.Objects);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void ImportMeta_ReadsTitleAuthorsAndArtistLink()
        {
            var report = new InteropReport();
            var meta = AfterBeatMetaImporter.Import(AfterBeatMockData.CreateMeta(), report);

            Assert.AreEqual("Test Song", ((StringValue)meta.LevelName).Value);
            Assert.AreEqual(2, meta.LevelAuthors.Count);
            Assert.AreEqual("https://someband.bandcamp.com", meta.LevelAuthors[1].Url);
        }
    }
}
