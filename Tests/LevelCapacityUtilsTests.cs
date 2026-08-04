using System.Collections.Generic;
using BH.SDK.Models;
using BH.SDK.Models.Audio;
using BH.SDK.Models.Objects;
using BH.SDK.Models.Primitives;
using BH.SDK.Models.Primitives.Resources;
using BH.SDK.Utils;
using NUnit.Framework;

namespace BH.SDK.Tests
{
    [TestFixture]
    public class LevelCapacityUtilsTests
    {
        #region GetPeak

        [Test]
        public void GetPeak_NoIntervals_ReturnsZero()
        {
            var peak = LevelCapacityUtils.GetPeak(new List<int>(), new List<int>());
            Assert.AreEqual(0, peak);
        }

        [Test]
        public void GetPeak_DisjointIntervals_ReturnsOne()
        {
            // [0,10] then [11,20] - never alive at the same frame
            var peak = LevelCapacityUtils.GetPeak(new[] { 0, 11 }, new[] { 10, 20 });
            Assert.AreEqual(1, peak);
        }

        [Test]
        public void GetPeak_TouchingIntervals_BothEndsInclusive_ReturnsTwo()
        {
            // [0,10] and [10,20] share frame 10 - inclusive bounds mean they do overlap there
            var peak = LevelCapacityUtils.GetPeak(new[] { 0, 10 }, new[] { 10, 20 });
            Assert.AreEqual(2, peak);
        }

        [Test]
        public void GetPeak_NestedIntervals_ReturnsDepth()
        {
            var peak = LevelCapacityUtils.GetPeak(new[] { 0, 1, 2 }, new[] { 100, 50, 10 });
            Assert.AreEqual(3, peak);
        }

        [Test]
        public void GetPeak_SingleFrameIntervals_OnSameFrame_ReturnsCount()
        {
            var peak = LevelCapacityUtils.GetPeak(new[] { 5, 5, 5 }, new[] { 5, 5, 5 });
            Assert.AreEqual(3, peak);
        }

        [Test]
        public void GetPeak_PeakIsNotTotalCount()
        {
            // three objects, but never more than two at once: [0,10] [5,15] [20,30]
            var peak = LevelCapacityUtils.GetPeak(new[] { 0, 5, 20 }, new[] { 10, 15, 30 });
            Assert.AreEqual(2, peak);
        }

        [Test]
        public void GetPeak_InvertedInterval_IsIgnored()
        {
            // EndFrame < StartFrame - the object is never alive, so it must not take a slot
            var peak = LevelCapacityUtils.GetPeak(new[] { 0, 10 }, new[] { 10, 5 });
            Assert.AreEqual(1, peak);
        }

        #endregion

        #region GetPeakUsage

        [Test]
        public void GetPeakUsage_EmptyLevel_ReturnsZeroes()
        {
            var hint = LevelCapacityUtils.GetPeakUsage(new Level());

            Assert.AreEqual(0, hint.Instances);
            Assert.AreEqual(0, hint.Textures);
            Assert.AreEqual(0, hint.Effects);
            Assert.AreEqual(0, hint.Texts);
            Assert.AreEqual(0, hint.Tracks);
            Assert.IsFalse(hint.HasValue);
        }

        [Test]
        public void GetPeakUsage_NullLevel_ReturnsZeroes()
        {
            var hint = LevelCapacityUtils.GetPeakUsage(null);
            Assert.IsFalse(hint.HasValue);
        }

        [Test]
        public void GetPeakUsage_CountsEveryTypeAsInstance_AndItsOwnFamilySeparately()
        {
            var level = new Level();
            AddObject(level, new RectObject(), 1, 0, 100);
            AddObject(level, new TextureObject(), 2, 0, 100);
            AddObject(level, new TextureObject(), 3, 0, 100);
            AddObject(level, new EffectObject(), 4, 0, 100);
            AddObject(level, new TextObject(), 5, 0, 100);
            AddObject(level, new PrefabObject(), 6, 0, 100);

            var hint = LevelCapacityUtils.GetPeakUsage(level);

            Assert.AreEqual(6, hint.Instances);
            Assert.AreEqual(2, hint.Textures);
            Assert.AreEqual(1, hint.Effects);
            Assert.AreEqual(1, hint.Texts);
            Assert.IsTrue(hint.HasValue);
        }

        [Test]
        public void GetPeakUsage_FamiliesPeakIndependently()
        {
            var level = new Level();
            // textures live early, texts live late - each family peaks at 2, but only 2 instances
            // are ever alive at the same time despite there being 4 objects in total
            AddObject(level, new TextureObject(), 1, 0, 10);
            AddObject(level, new TextureObject(), 2, 0, 10);
            AddObject(level, new TextObject(), 3, 50, 60);
            AddObject(level, new TextObject(), 4, 50, 60);

            var hint = LevelCapacityUtils.GetPeakUsage(level);

            Assert.AreEqual(2, hint.Instances);
            Assert.AreEqual(2, hint.Textures);
            Assert.AreEqual(2, hint.Texts);
            Assert.AreEqual(0, hint.Effects);
        }

        [Test]
        public void GetPeakUsage_CountsAudioTracks()
        {
            var level = new Level();
            AddTrack(level, 1, 0, 100);
            AddTrack(level, 2, 50, 150);
            AddTrack(level, 3, 200, 300);

            var hint = LevelCapacityUtils.GetPeakUsage(level);

            Assert.AreEqual(2, hint.Tracks);
            Assert.AreEqual(0, hint.Instances);
        }

        #endregion

        private static void AddObject(Level level, RectObject levelObject, int id, int startFrame, int endFrame)
        {
            levelObject.ObjectId = new ObjectId(id);
            levelObject.StartFrame = startFrame;
            levelObject.EndFrame = endFrame;
            level.Game.Objects.Add(levelObject.ObjectId, levelObject);
        }
        private static void AddTrack(Level level, int id, int startFrame, int endFrame)
        {
            var track = new LevelTrack
            {
                AudioId = new AudioId(id),
                AudioResourceId = AudioResourceId.Null,
                StartFrame = startFrame,
                EndFrame = endFrame,
            };
            level.Audio.Tracks.Add(track.AudioId, track);
        }
    }
}
