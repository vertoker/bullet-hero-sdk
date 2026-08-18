using System;
using BH.SDK.Interop;
using BH.SDK.Interop.AfterBeat;
using BH.SDK.Interop.AfterBeat.Models;
using BH.SDK.Models.Enums;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using NUnit.Framework;

namespace BH.SDK.Tests.Interop.AfterBeat
{
    // The pure maps, one fixture per direction pair. These are the conversions that go wrong
    // silently in a real level - a rotation that looks fine until the object spins, a time that
    // rounds two keyframes onto one frame - so each is pinned on its own rather than only through
    // a whole-level round trip.
    public class ABEaseMapTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Import_EveryDocumentedName_Resolves()
        {
            foreach (var name in ABEaseMap.KnownNames)
            {
                var report = new InteropReport();
                var ease = ABEaseMap.Import(name, report);
                Assert.IsTrue(Enum.IsDefined(typeof(EaseType), ease), $"'{name}' resolved to {ease}");
            }
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Import_UnknownName_IsLinearAndReported()
        {
            var report = new InteropReport();
            var ease = ABEaseMap.Import("InOutNonsense", report);

            Assert.AreEqual(EaseType.Linear, ease);
            Assert.AreEqual(InteropSeverity.Approximated, report.Worst);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Import_Bounce_IsApproximatedNotDropped()
        {
            var report = new InteropReport();
            var ease = ABEaseMap.Import("OutBounce", report);

            Assert.AreEqual(EaseType.OutElastic, ease);
            Assert.AreEqual(InteropSeverity.Approximated, report.Worst);
        }

        // Every EaseType has to produce a name Afterbeat accepts - an export that emits a name its
        // reader does not know makes the whole file unreadable, not just that keyframe.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Export_EveryEaseType_ProducesAKnownName()
        {
            foreach (EaseType ease in Enum.GetValues(typeof(EaseType)))
            {
                var name = ABEaseMap.Export(ease);
                CollectionAssert.Contains((System.Collections.ICollection)ABEaseMap.KnownNames, name);
            }
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void RoundTrip_SharedEases_AreStable()
        {
            var shared = new[]
            {
                EaseType.Linear, EaseType.Constant, EaseType.InSine, EaseType.OutQuad,
                EaseType.InOutExpo, EaseType.OutCirc, EaseType.InBack, EaseType.InOutElastic,
            };

            foreach (var ease in shared)
                Assert.AreEqual(ease, ABEaseMap.Import(ABEaseMap.Export(ease)));
        }
    }

    public class ABTimeMapTests
    {
        [TestCase(0f, 60, 0)]
        [TestCase(1f, 60, 60)]
        [TestCase(1.5f, 60, 90)]
        [TestCase(1f, 30, 30)]
        [TestCase(1f, 144, 144)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void ToFrame_ConvertsSeconds(float seconds, int framerate, int expected)
            => Assert.AreEqual(expected, ABTimeMap.ToFrame(seconds, framerate));

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void ToFrame_RoundsToNearest_NotDown()
        {
            // 0.99 of a frame belongs on the frame it is nearly on, not on the one before it.
            Assert.AreEqual(1, ABTimeMap.ToFrame(1f / 60f * 0.99f, 60));
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void ToFrame_NeverLeavesTheLegalRange()
        {
            Assert.AreEqual(FrameRules.MinFrame, ABTimeMap.ToFrame(-100f, 60));
            Assert.AreEqual(FrameRules.MaxFrame, ABTimeMap.ToFrame(1e9f, 60));
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void ResolveEndTime_LastKeyframe_UsesTheFurthestTrack()
        {
            var source = ABMockData.CreateObject();
            // Move ends at 2s, the object starts at 1s.
            Assert.AreEqual(3f, ABTimeMap.ResolveEndTime(source), 1e-4f);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void ResolveEndTime_EveryAutokillType_MeansSomethingDifferent()
        {
            var source = ABMockData.CreateObject();
            source.AutokillOffset = 4f;

            source.AutokillType = (int)ABAutokillType.LastKeyframeOffset;
            Assert.AreEqual(7f, ABTimeMap.ResolveEndTime(source), 1e-4f);

            source.AutokillType = (int)ABAutokillType.FixedTime;
            Assert.AreEqual(5f, ABTimeMap.ResolveEndTime(source), 1e-4f);

            source.AutokillType = (int)ABAutokillType.SongTime;
            Assert.AreEqual(4f, ABTimeMap.ResolveEndTime(source), 1e-4f);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void ResolveSpan_IsNeverShorterThanOneFrame()
        {
            var source = ABMockData.CreateObject();
            source.AutokillType = (int)ABAutokillType.FixedTime;
            source.AutokillOffset = 0f;

            var span = ABTimeMap.ResolveSpan(source, 60);
            Assert.AreEqual(FrameRules.MinFrameDuration, span.FrameDuration);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void ExportSpan_UsesFixedTime_SoItDoesNotDependOnKeyframes()
        {
            var target = new VgdObject();
            ABTimeMap.ExportSpan(new Models.Primitives.FrameSpan(60, 120), 60, target);

            Assert.AreEqual((int)ABAutokillType.FixedTime, target.AutokillType);
            Assert.AreEqual(1f, target.StartTime, 1e-4f);
            Assert.AreEqual(2f, target.AutokillOffset, 1e-4f);
        }
    }

    public class ABRotationTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Accumulate_TurnsRelativeDegreesIntoAbsoluteRadians()
        {
            var accumulated = 0f;
            var first = ABValueMap.AccumulateRotation(90f, ref accumulated);
            var second = ABValueMap.AccumulateRotation(90f, ref accumulated);

            Assert.AreEqual(Math.PI / 2.0, first, 1e-4f);
            Assert.AreEqual(Math.PI, second, 1e-4f);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Differentiate_IsTheInverseOfAccumulate()
        {
            var deltas = new[] { 90f, 45f, -30f, 180f };

            var accumulated = 0f;
            var absolute = new float[deltas.Length];
            for (var i = 0; i < deltas.Length; i++)
                absolute[i] = ABValueMap.AccumulateRotation(deltas[i], ref accumulated);

            var previous = 0f;
            for (var i = 0; i < deltas.Length; i++)
                Assert.AreEqual(deltas[i],
                    ABValueMap.DifferentiateRotation(absolute[i], ref previous), 1e-3f);
        }
    }

    public class ABValueMapTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void ImportFloat_NoRandom_IsALiteral()
        {
            var key = new VgdKeyframe { RandomType = (int)ABRandomType.None };
            Assert.IsInstanceOf<FloatValue>(ABValueMap.ImportFloat(5f, key));
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        // "er" is the OTHER END of the range, not an offset from the value - the source game rolls
        // Random.Range(GetVal(0), GetRandVal(0)). So a value of 5 with an er of 3 rolls over
        // [3, 5], NOT over [5, 8]; reading it as an offset widened every random range in a level by
        // whatever value it was anchored at.
        public void ImportFloat_LinearRandom_RangesBetweenValueAndRandom()
        {
            var key = new VgdKeyframe
            {
                RandomType = (int)ABRandomType.Linear,
                RandomValues = new System.Collections.Generic.List<float> { 3f, 0f, 0f },
            };

            var value = ABValueMap.ImportFloat(5f, key);
            Assert.IsInstanceOf<FloatMinMax>(value);

            var range = (FloatMinMax)value;
            Assert.AreEqual(3f, range.Min, 1e-4f);
            Assert.AreEqual(5f, range.Max, 1e-4f);
        }

        // The one the format's own table has no entry for, and the source game implements: the
        // same range as Linear, rounded to whole numbers.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void ImportFloat_LinearRounded_IsAWholeNumberStep()
        {
            var key = new VgdKeyframe
            {
                RandomType = (int)ABRandomType.LinearRounded,
                RandomValues = new System.Collections.Generic.List<float> { 9f, 0f, 0f },
            };

            var value = ABValueMap.ImportFloat(2f, key);
            Assert.IsInstanceOf<FloatMinMaxStep>(value);

            var step = (FloatMinMaxStep)value;
            Assert.AreEqual(2f, step.Min, 1e-4f);
            Assert.AreEqual(9f, step.Max, 1e-4f);
            Assert.AreEqual(ABValueMap.WholeNumberInterval, step.Step, 1e-4f);
        }

        // A Toggle picks one of exactly two values, and a step equal to the range's own width is
        // what makes only the two ends reachable - which is why the step variant is used here
        // rather than a plain range.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void ImportFloat_Toggle_IsATwoValuedStep()
        {
            var key = new VgdKeyframe
            {
                RandomType = (int)ABRandomType.Toggle,
                RandomValues = new System.Collections.Generic.List<float> { 4f, 0f, 0f },
            };

            var value = ABValueMap.ImportFloat(1f, key);
            Assert.IsInstanceOf<FloatMinMaxStep>(value);

            var step = (FloatMinMaxStep)value;
            Assert.AreEqual(1f, step.Min, 1e-4f);
            Assert.AreEqual(4f, step.Max, 1e-4f);
            Assert.AreEqual(step.Max - step.Min, step.Step, 1e-4f);
        }

        // Type 4 MULTIPLIES the value by a factor drawn from er[0]..er[1] - it does not accumulate
        // onto the previous keyframe's roll, which is what it was believed to do and why it used to
        // be dropped. Multiplying a fixed value by a range is a range, so it crosses exactly.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void ImportFloat_Scale_IsTheValueTimesTheFactorRange()
        {
            var key = new VgdKeyframe
            {
                RandomType = (int)ABRandomType.Scale,
                RandomValues = new System.Collections.Generic.List<float> { 0.5f, 2f, 0f },
            };

            var value = ABValueMap.ImportFloat(4f, key);
            Assert.IsInstanceOf<FloatMinMax>(value);

            var range = (FloatMinMax)value;
            Assert.AreEqual(2f, range.Min, 1e-4f);
            Assert.AreEqual(8f, range.Max, 1e-4f);
        }
    }
}
