using System.Collections.Generic;
using System.Linq;
using BH.SDK.Generators;
using BH.SDK.Generators.Modifiers;
using BH.SDK.Generators.Utility;
using BH.SDK.Models;
using BH.SDK.Models.Keyframes;
using BH.SDK.Models.Objects;
using BH.SDK.Models.Primitives;
using BH.SDK.Models.Values;
using NUnit.Framework;

namespace BH.SDK.Tests.Generators
{
    // Modifiers are the half of the contract nothing exercised until now: Requirements.Selection,
    // context.Edit's whole-object snapshot, and a run whose GeneratorCost is legitimately zero
    // because it adds nothing while changing plenty.
    public class ModifierGeneratorsTests
    {
        private static Level CreateLevel(int framerate = 60)
        {
            var level = new Level();
            level.Settings.Framerate = framerate;
            level.Settings.FrameLength = 600;
            return level;
        }

        private static TextureObject AddObject(Level level, int layer, float x, params int[] frames)
        {
            var obj = new TextureObject
            {
                ObjectId = level.Settings.GetNextObjectId(),
                Name = $"obj_{layer}",
                Layer = layer,
                StartFrame = 0,
                EndFrame = 200,
            };
            foreach (var frame in frames)
                obj.Positions.Add(new PosKey(new Vector2Value(x, 0f), frame));
            level.Game.Objects.Add(obj.ObjectId, obj);
            return obj;
        }

        private static GeneratorContext Context(Level level, params ObjectId[] selection)
            => new(level, 0, 300, selection: selection.ToList());

        private static List<int> FramesOf(RectObject obj) => obj.Positions.Select(key => key.Frame).ToList();

        #region mod_quantize_keyframes

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Quantize_SnapsToTheNearestGridLine()
        {
            var level = CreateLevel();
            var obj = AddObject(level, 0, 0f, 0, 7, 9, 22);

            new QuantizeKeyframesGenerator().Run(Context(level, obj.ObjectId),
                new QuantizeKeyframesGenerator.Parameters
                {
                    UseBpm = false, StepFrames = 10, Mode = QuantizeMode.Nearest,
                });

            var frames = FramesOf(obj);
            Assert.AreEqual(0, frames[0]);
            Assert.AreEqual(10, frames[1], "7 rounds up");
            Assert.AreEqual(9, frames[2], "9 wants 10 too, but it is taken - it stays put");
            Assert.AreEqual(20, frames[3], "22 rounds down");
        }

        [TestCase(QuantizeMode.Floor, 10)]
        [TestCase(QuantizeMode.Ceil, 20)]
        [TestCase(QuantizeMode.Nearest, 20)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Quantize_ModeDecidesWhichWayAKeyMoves(QuantizeMode mode, int expected)
        {
            var level = CreateLevel();
            var obj = AddObject(level, 0, 0f, 16);

            new QuantizeKeyframesGenerator().Run(Context(level, obj.ObjectId),
                new QuantizeKeyframesGenerator.Parameters
                {
                    UseBpm = false, StepFrames = 10, Mode = mode,
                });

            Assert.AreEqual(expected, obj.Positions[0].Frame);
        }

        // The grid comes from the LEVEL's framerate, so the same BPM is a different number of frames
        // in a 30 fps and a 60 fps level - which is right, since a frame is a different duration.
        [TestCase(60, 120f, 1, 30)]
        [TestCase(30, 120f, 1, 15)]
        [TestCase(60, 120f, 2, 15)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Quantize_BpmGridFollowsTheLevelFramerate(int framerate, float bpm, int division, int step)
        {
            var level = CreateLevel(framerate);
            var obj = AddObject(level, 0, 0f, step + 1);

            new QuantizeKeyframesGenerator().Run(Context(level, obj.ObjectId),
                new QuantizeKeyframesGenerator.Parameters
                {
                    UseBpm = true, Bpm = bpm, Division = division, Mode = QuantizeMode.Nearest,
                });

            Assert.AreEqual(step, obj.Positions[0].Frame);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Quantize_OffsetShiftsTheWholeGrid()
        {
            var level = CreateLevel();
            var obj = AddObject(level, 0, 0f, 12);

            new QuantizeKeyframesGenerator().Run(Context(level, obj.ObjectId),
                new QuantizeKeyframesGenerator.Parameters
                {
                    UseBpm = false, StepFrames = 10, OffsetFrames = 5, Mode = QuantizeMode.Nearest,
                });

            Assert.AreEqual(15, obj.Positions[0].Frame, "grid lines sit at 5, 15, 25 ...");
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Quantize_OnlyTouchesSelectedTracks()
        {
            var level = CreateLevel();
            var obj = AddObject(level, 0, 0f, 7);
            obj.Sizes.Add(new ScaKey(new Vector2Value(1f, 1f), 7));

            new QuantizeKeyframesGenerator().Run(Context(level, obj.ObjectId),
                new QuantizeKeyframesGenerator.Parameters
                {
                    UseBpm = false, StepFrames = 10, Tracks = ObjectTrackMask.Positions,
                });

            Assert.AreEqual(10, obj.Positions[0].Frame);
            Assert.AreEqual(7, obj.Sizes[0].Frame, "an unselected track is left alone");
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Quantize_LeavesUnselectedObjectsUntouched()
        {
            var level = CreateLevel();
            var selected = AddObject(level, 0, 0f, 7);
            var other = AddObject(level, 1, 0f, 7);

            new QuantizeKeyframesGenerator().Run(Context(level, selected.ObjectId),
                new QuantizeKeyframesGenerator.Parameters { UseBpm = false, StepFrames = 10 });

            Assert.AreEqual(10, selected.Positions[0].Frame);
            Assert.AreEqual(7, other.Positions[0].Frame);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Hard)]
        public void Quantize_UndoRestoresEveryFrame()
        {
            var level = CreateLevel();
            var obj = AddObject(level, 0, 0f, 3, 7, 11, 19);
            var before = level.Game.Copy();

            var result = new QuantizeKeyframesGenerator().Run(Context(level, obj.ObjectId),
                new QuantizeKeyframesGenerator.Parameters { UseBpm = false, StepFrames = 5 });

            Assert.AreNotEqual(before, level.Game, "the run has to actually change something");
            result.Log.Revert();
            Assert.IsTrue(before.Equals(level.Game));
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Quantize_DeclaresItNeedsASelection()
        {
            var generator = new QuantizeKeyframesGenerator();
            Assert.AreEqual(GeneratorKind.Modifier, generator.Kind);
            Assert.IsTrue(generator.Requirements.HasFlag(GeneratorRequirements.Selection));
        }

        #endregion

        #region mod_stagger

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Stagger_DelaysEachObjectOneStepFurther()
        {
            var level = CreateLevel();
            var a = AddObject(level, 0, 0f, 0);
            var b = AddObject(level, 1, 0f, 0);
            var c = AddObject(level, 2, 0f, 0);

            new StaggerGenerator().Run(Context(level, a.ObjectId, b.ObjectId, c.ObjectId),
                new StaggerGenerator.Parameters { StepFrames = 5, Order = StaggerOrder.Selection });

            Assert.AreEqual(0, a.StartFrame);
            Assert.AreEqual(5, b.StartFrame);
            Assert.AreEqual(10, c.StartFrame);
            CollectionAssert.AreEqual(new[] { 0 }, FramesOf(a));
            CollectionAssert.AreEqual(new[] { 5 }, FramesOf(b));
            CollectionAssert.AreEqual(new[] { 10 }, FramesOf(c));
        }

        // Ordering is the whole point of the modifier: the same selection staggered by layer and by
        // position produces two different waves.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Stagger_OrdersByTheChosenKey()
        {
            var level = CreateLevel();
            var left = AddObject(level, 2, -10f, 0);
            var middle = AddObject(level, 0, 0f, 0);
            var right = AddObject(level, 1, 10f, 0);

            new StaggerGenerator().Run(Context(level, left.ObjectId, middle.ObjectId, right.ObjectId),
                new StaggerGenerator.Parameters { StepFrames = 10, Order = StaggerOrder.PositionX });

            Assert.AreEqual(0, left.StartFrame, "leftmost goes first");
            Assert.AreEqual(10, middle.StartFrame);
            Assert.AreEqual(20, right.StartFrame);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Stagger_ReverseFlipsWhoWaitsLongest()
        {
            var level = CreateLevel();
            var a = AddObject(level, 0, 0f, 0);
            var b = AddObject(level, 1, 0f, 0);

            new StaggerGenerator().Run(Context(level, a.ObjectId, b.ObjectId),
                new StaggerGenerator.Parameters
                {
                    StepFrames = 8, Order = StaggerOrder.Selection, Reverse = true,
                });

            Assert.AreEqual(8, a.StartFrame);
            Assert.AreEqual(0, b.StartFrame);
        }

        // The two halves are separately useful: bounds decide WHEN an object exists, keyframes
        // decide when it does what it does.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Stagger_CanShiftBoundsWithoutKeyframes()
        {
            var level = CreateLevel();
            var a = AddObject(level, 0, 0f, 40);
            var b = AddObject(level, 1, 0f, 40);

            new StaggerGenerator().Run(Context(level, a.ObjectId, b.ObjectId),
                new StaggerGenerator.Parameters
                {
                    StepFrames = 6, Order = StaggerOrder.Selection,
                    ShiftBounds = true, ShiftKeyframes = false,
                });

            Assert.AreEqual(6, b.StartFrame);
            Assert.AreEqual(40, b.Positions[0].Frame, "keyframes stay where they were");
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Stagger_ClampsToTheLevelTimeline()
        {
            var level = CreateLevel();
            level.Settings.FrameLength = 100;
            var a = AddObject(level, 0, 0f, 10);
            var b = AddObject(level, 1, 0f, 10);
            a.EndFrame = 90;
            b.EndFrame = 90;

            new StaggerGenerator().Run(Context(level, a.ObjectId, b.ObjectId),
                new StaggerGenerator.Parameters { StepFrames = 500, Order = StaggerOrder.Selection });

            Assert.AreEqual(99, b.StartFrame, "FrameLength is a count - the last legal frame is 99");
            Assert.AreEqual(99, b.Positions[0].Frame);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Hard)]
        public void Stagger_UndoRestoresBoundsAndKeyframes()
        {
            var level = CreateLevel();
            var a = AddObject(level, 0, 0f, 0, 20);
            var b = AddObject(level, 1, 0f, 0, 20);
            var before = level.Game.Copy();

            var result = new StaggerGenerator().Run(Context(level, a.ObjectId, b.ObjectId),
                new StaggerGenerator.Parameters { StepFrames = 7, Order = StaggerOrder.Selection });

            result.Log.Revert();
            Assert.IsTrue(before.Equals(level.Game));
        }

        #endregion

        #region gen_capacity_hint

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void CapacityHint_RecomputesPeakUsageFromTheLevel()
        {
            var level = CreateLevel();
            AddObject(level, 0, 0f, 0);
            AddObject(level, 1, 0f, 0);
            Assert.AreEqual(0, level.Settings.LimitHints.Instances, "nothing measured yet");

            new CapacityHintGenerator().Run(Context(level), new CapacityHintGenerator.Parameters());

            Assert.AreEqual(2, level.Settings.LimitHints.Instances);
            Assert.AreEqual(2, level.Settings.LimitHints.Textures);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void CapacityHint_NeedsLevelScope_AndCreatesNothing()
        {
            var level = CreateLevel();
            var generator = new CapacityHintGenerator();

            Assert.IsTrue(generator.Requirements.HasFlag(GeneratorRequirements.LevelScope));

            var result = generator.Run(Context(level), generator.CreateDefaultParameters());
            Assert.AreEqual(0, result.CreatedIds.Length);
            Assert.AreEqual(0, level.Game.Objects.Count);
        }

        #endregion
    }
}
