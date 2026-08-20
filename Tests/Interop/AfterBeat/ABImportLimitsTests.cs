using System.Collections.Generic;
using System.Linq;
using BH.SDK.Interop;
using BH.SDK.Interop.AfterBeat;
using BH.SDK.Interop.AfterBeat.Import;
using BH.SDK.Interop.AfterBeat.Models;
using BH.SDK.Models.Objects;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using NUnit.Framework;

namespace BH.SDK.Tests.Interop.AfterBeat
{
    // WHERE A SOURCE LEVEL IS BIGGER THAN THIS FORMAT ALLOWS. Afterbeat bounds almost nothing an
    // author writes, so a real level arrives holding things this one forbids outright, and each of
    // them fails LOUDLY rather than degrading: a keyframe over a track's cap or a string past the
    // player's buffer length fails validation, and two keyframes on one frame do worse than that -
    // LevelStateBuilder builds its NativeSortedLists straight off these lists and THROWS on the
    // duplicate key, so the imported level cannot be opened at all.
    //
    // Two source times a hundredth of a second apart round onto one frame at any ordinary
    // framerate, since the source grid is 10 ms and a frame is coarser. The object tracks were
    // deduplicated and capped from the start; the level-global ones and the text were not.
    public class ABImportLimitsTests
    {
        private const int Framerate = 60;

        // 0.01s and 0.02s both round to frame 1 at 60 fps, and 0.03s does not - so a fixture built
        // from the three has exactly one collision and one survivor beside it.
        private const float CollidingTimeA = 0.01f;
        private const float CollidingTimeB = 0.02f;
        private const float DistinctTime = 0.03f;

        private static ABOptions Options() => new(Framerate);

        private static VgdEventKeyframe Event(float time, params float[] values)
            => new()
            {
                Time = time,
                Values = Newtonsoft.Json.Linq.JArray.FromObject(values),
            };

        private static VgdLevel LevelWithCollidingEvents()
        {
            var level = ABMockData.CreateFullLevel();

            level.SetEvents(ABEventTrack.CameraPosition, new List<VgdEventKeyframe>
            {
                Event(CollidingTimeA, 1f, 2f),
                Event(CollidingTimeB, 3f, 4f),
                Event(DistinctTime, 5f, 6f),
            });
            level.SetEvents(ABEventTrack.CameraZoom, new List<VgdEventKeyframe>
            {
                Event(CollidingTimeA, 12f),
                Event(CollidingTimeB, 40f),
            });
            level.SetEvents(ABEventTrack.CameraRotation, new List<VgdEventKeyframe>
            {
                Event(CollidingTimeA, 45f),
                Event(CollidingTimeB, 90f),
            });
            level.SetEvents(ABEventTrack.Bloom, new List<VgdEventKeyframe>
            {
                Event(CollidingTimeA, 1f, 0.5f, 2f),
                Event(CollidingTimeB, 2f, 0.5f, 2f),
            });

            return level;
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_CollidingEventKeys_LeaveOneKeyframePerFrame()
        {
            var game = ABLevelImporter.Import(LevelWithCollidingEvents(), null, Options()).Level.Game;

            AssertUniqueFrames(game.CameraEvents.Positions.Select(key => key.Frame), "camera positions");
            AssertUniqueFrames(game.CameraEvents.Zooms.Select(key => key.Frame), "camera zooms");
            AssertUniqueFrames(game.CameraEvents.Rotations.Select(key => key.Frame), "camera rotations");
            AssertUniqueFrames(game.PostProcessingEvents.Blooms.Select(key => key.Frame), "blooms");
        }

        // The survivor is the LATER keyframe, which is what a timeline does when a key is dragged
        // onto another - and the only choice that keeps the track reading as the author left it.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_CollidingEventKeys_KeepTheLaterValue()
        {
            var camera = ABLevelImporter.Import(LevelWithCollidingEvents(), null, Options())
                .Level.Game.CameraEvents;

            var collided = (Vector2Value)camera.Positions.Single(key => key.Frame == 1).Pos;

            Assert.AreEqual(3f, collided.X, "later keyframe's x");
            Assert.AreEqual(4f, collided.Y, "later keyframe's y");
            Assert.AreEqual(2, camera.Positions.Count, "the distinct third key is untouched");
        }

        // A dropped keyframe is a change to the level, and the whole point of the report is that an
        // author is told about every one of those rather than finding it in playback.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_CollidingEventKeys_ReportTheApproximation()
        {
            var result = ABLevelImporter.Import(LevelWithCollidingEvents(), null, Options());

            Assert.IsTrue(result.Report.Issues.Any(issue => issue.Code == "keys_collided"),
                "an import that silently drops a keyframe says nothing about the level it changed");
        }

        // The camera-scale node and the camera's own Zooms track are two readings of ONE source
        // event, so a collision resolved differently between them frames the level by one zoom
        // keyframe while everything parented to the camera is scaled by the other.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_CameraScaleRoot_ResolvesZoomCollisionsLikeTheCameraTrack()
        {
            var source = LevelWithCollidingEvents();
            var parented = ABMockData.CreateObject("cam-child");
            parented.ParentId = VgdObject.CameraParentId;
            source.Objects.Add(parented);

            var level = ABLevelImporter.Import(source, null, Options()).Level;
            var root = level.Game.Objects.Values
                .Single(obj => obj.Name == ABLevelImporter.CameraScaleRootName);

            CollectionAssert.AreEqual(
                level.Game.CameraEvents.Zooms.Select(key => key.Frame).ToArray(),
                root.Scales.Select(key => key.Frame).ToArray(),
                "camera zoom and camera scale must agree on which keyframe survived");

            var scale = (Vector2Value)root.Scales.Single(key => key.Frame == 1).Scale;
            Assert.AreEqual(40f / ABEventsImporter.DefaultSourceZoom, scale.X, 1e-5f,
                "the later zoom is the one the scale node carries");
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void DeduplicateByFrame_KeepsTheLastOfEachFrameInFirstSeenOrder()
        {
            var keyframes = new List<(int Frame, string Tag)>
            {
                (5, "a"), (1, "b"), (5, "c"), (9, "d"), (1, "e"),
            };

            ABTimeMap.DeduplicateByFrame(keyframes, key => key.Frame);

            CollectionAssert.AreEqual(new[] { 5, 1, 9 }, keyframes.Select(key => key.Frame).ToArray());
            CollectionAssert.AreEqual(new[] { "c", "e", "d" }, keyframes.Select(key => key.Tag).ToArray());
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void DeduplicateByFrame_LeavesACleanTrackAloneAndReportsNothing()
        {
            var keyframes = new List<int> { 3, 1, 2 };
            var report = new InteropReport();

            ABTimeMap.DeduplicateByFrame(keyframes, frame => frame, report, "events");

            CollectionAssert.AreEqual(new[] { 3, 1, 2 }, keyframes);
            CollectionAssert.IsEmpty(report.Issues);
        }

        // The cap runs AFTER the deduplication, and the order is what this pins: a keyframe about to
        // merge into its neighbour must not spend a slot, or a track sitting just over the limit
        // loses real content at its end to make room for keys that were never going to survive.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_ThemeTrackOverTheCap_IsCutToWhatTheFormatAllows()
        {
            var source = ABMockData.CreateFullLevel();
            var keys = new List<VgdEventKeyframe>();
            for (var i = 0; i < LevelRules.MaxThemeEvents + 64; i++)
            {
                var key = new VgdEventKeyframe { Time = i * 0.1f };
                key.Values = new Newtonsoft.Json.Linq.JArray { ABMockData.ThemeSourceId };
                keys.Add(key);
            }
            source.SetEvents(ABEventTrack.Theme, keys);

            var result = ABLevelImporter.Import(source, null, Options());

            Assert.AreEqual(LevelRules.MaxThemeEvents, result.Level.Game.Events.Themes.Count);
            Assert.IsTrue(result.Report.Issues.Any(issue => issue.Code == "event_keys_over_cap"),
                "a level-global track cut short says so");
        }

        // Afterbeat's string is unbounded and real levels use that - blocks of ten thousand
        // characters and more - while this format's is the fixed slot length of the player's own
        // per-frame text buffers, a number the runtime cannot address past.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_TextLongerThanTheBuffer_IsCutToFitIt()
        {
            var source = ABMockData.CreateLevel();
            var text = ABMockData.CreateObject("long-text");
            text.Shape = (int)ABShape.Text;
            text.Text = new string('x', ValueRules.MaxGameString + 500);
            source.Objects.Add(text);

            var result = ABLevelImporter.Import(source, null, Options());
            var imported = result.Level.Game.Objects.Values.OfType<TextObject>().Single();

            Assert.AreEqual(ValueRules.MaxGameString, ((StringValue)imported.Text).Value.Length);
            Assert.IsTrue(result.Report.Issues.Any(issue => issue.Code == "text_over_cap"),
                "a cut string is content lost and is reported as such");
        }

        private static void AssertUniqueFrames(IEnumerable<int> frames, string track)
        {
            var list = frames.ToArray();
            CollectionAssert.AllItemsAreUnique(list, $"{track} hold one keyframe per frame");
        }
    }
}
