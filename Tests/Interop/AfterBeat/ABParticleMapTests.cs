using System.Collections.Generic;
using BH.SDK.Interop.AfterBeat;
using BH.SDK.Interop.AfterBeat.Models;
using NUnit.Framework;

namespace BH.SDK.Tests.Interop.AfterBeat
{
    // Every case here is one of the two mistakes this area invites. The first is reading the eight
    // parameters off csp, where BeatmapObject's own field order suggests they live; the second is
    // reading a short value array as zeroes, which is right for every other track and wrong for
    // exactly these indices - world space and the arc both default to something that is not zero.
    public class ABParticleMapTests
    {
        private static VgdObject Emitter(params float[] values)
        {
            var target = new VgdObject
            {
                Id = "emitter",
                ObjectType = (int)ABObjectType.Particles,
            };

            target.Move.Keyframes.Add(new VgdKeyframe
            {
                Time = 0f,
                Values = new List<float>(values),
            });

            return target;
        }

        /// <summary> The four position/scale components an authored emitter always carries, so a
        /// test only has to spell out the parameters it actually cares about. </summary>
        private static float[] WithLead(params float[] parameters)
        {
            var values = new List<float> { 0f, 0f, 0f, 0f };
            values.AddRange(parameters);
            return values.ToArray();
        }

        #region Reading at all

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void TryRead_AnObjectThatIsNotAnEmitter_ReadsNothing()
        {
            var source = Emitter(WithLead(500f));
            source.ObjectType = (int)ABObjectType.Hit;

            Assert.IsNull(ABParticleMap.TryRead(source));
            Assert.IsFalse(ABParticleMap.IsEmitter(source));
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void TryRead_Null_ReadsNothing()
        {
            Assert.IsNull(ABParticleMap.TryRead(null));
            Assert.IsFalse(ABParticleMap.IsEmitter(null));
        }

        // The parameters live on the first POSITION keyframe, never in csp. An emitter carrying a
        // custom-shape array and nothing else must therefore read as entirely default - if this
        // ever fails, the reader is looking at the wrong array.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void TryRead_CustomShapeArray_IsNotWhereTheParametersLive()
        {
            var source = Emitter(0f, 0f);
            source.CustomShape = new List<float> { 6f, 0.5f, 0.2f, 3f, 0f };

            var settings = ABParticleMap.TryRead(source);

            Assert.IsNotNull(settings);
            Assert.AreEqual(ABParticleMap.SpawnRatePerSecondDefault, settings.Value.SpawnRatePerSecond);
            Assert.AreEqual(ABParticleMap.StartSpeedDefault, settings.Value.StartSpeed);
        }

        #endregion

        #region Defaults

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void TryRead_AShortValueArray_AnswersTheDefaults_NotZeroes()
        {
            var settings = ABParticleMap.TryRead(Emitter(0f, 0f)).Value;

            Assert.AreEqual(0f, settings.SpawnRatePerSecond);
            Assert.AreEqual(0f, settings.SpawnRatePerUnit);
            Assert.IsTrue(settings.WorldSpace, "world space defaults to true, not to zero");
            Assert.IsFalse(settings.DespawnOnEnd);
            Assert.AreEqual(ABParticleEmitterShapeType.Rectangle, settings.EmitterShape);
            Assert.AreEqual(360f, settings.EmitterArc, "the arc defaults to a full circle");
            Assert.AreEqual(1f, settings.EmitterRadiusThickness);
            Assert.AreEqual(1f, settings.StartSpeed);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void TryRead_NoKeyframeAtAll_AnswersTheDefaults()
        {
            var source = new VgdObject { Id = "e", ObjectType = (int)ABObjectType.Particles };

            var settings = ABParticleMap.TryRead(source);

            Assert.IsNotNull(settings);
            Assert.IsTrue(settings.Value.WorldSpace);
            Assert.AreEqual(360f, settings.Value.EmitterArc);
        }

        #endregion

        #region Clamps

        [TestCase(-50f, 0f)]
        [TestCase(0f, 0f)]
        [TestCase(500f, 500f)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void TryRead_SpawnRatePerSecond_IsFlooredAtZero(float authored, float expected)
        {
            var settings = ABParticleMap.TryRead(Emitter(WithLead(authored))).Value;
            Assert.AreEqual(expected, settings.SpawnRatePerSecond);
        }

        [TestCase(-1f, 0f)]
        [TestCase(3f, 3f)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void TryRead_StartSpeed_IsFlooredAtZero(float authored, float expected)
        {
            var settings = ABParticleMap
                .TryRead(Emitter(WithLead(0f, 0f, 1f, 0f, 0f, 360f, 1f, authored))).Value;
            Assert.AreEqual(expected, settings.StartSpeed);
        }

        [TestCase(-90f, 0f)]
        [TestCase(0f, 0f)]
        [TestCase(180f, 180f)]
        [TestCase(360f, 360f)]
        [TestCase(720f, 360f)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void TryRead_EmitterArc_IsClampedToAWholeCircle(float authored, float expected)
        {
            var settings = ABParticleMap
                .TryRead(Emitter(WithLead(0f, 0f, 1f, 0f, 1f, authored))).Value;
            Assert.AreEqual(expected, settings.EmitterArc);
        }

        [TestCase(-0.5f, 0f)]
        [TestCase(0f, 0f)]
        [TestCase(0.9f, 0.9f)]
        [TestCase(1f, 1f)]
        [TestCase(4f, 1f)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void TryRead_RadiusThickness_IsClampedToAFraction(float authored, float expected)
        {
            var settings = ABParticleMap
                .TryRead(Emitter(WithLead(0f, 0f, 1f, 0f, 1f, 360f, authored))).Value;
            Assert.AreEqual(expected, settings.EmitterRadiusThickness);
        }

        #endregion

        #region The two enumerated reads

        // Rounded, then tested against 1 alone - so 0.6 is a circle and 2 is a box.
        [TestCase(0f, ABParticleEmitterShapeType.Rectangle)]
        [TestCase(0.4f, ABParticleEmitterShapeType.Rectangle)]
        [TestCase(0.6f, ABParticleEmitterShapeType.Circle)]
        [TestCase(1f, ABParticleEmitterShapeType.Circle)]
        [TestCase(1.4f, ABParticleEmitterShapeType.Circle)]
        [TestCase(2f, ABParticleEmitterShapeType.Rectangle)]
        [TestCase(-1f, ABParticleEmitterShapeType.Rectangle)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void TryRead_EmitterShape_IsCircleOnlyWhenItRoundsToOne(
            float authored, ABParticleEmitterShapeType expected)
        {
            var settings = ABParticleMap.TryRead(Emitter(WithLead(0f, 0f, 1f, 0f, authored))).Value;
            Assert.AreEqual(expected, settings.EmitterShape);
        }

        // Half a unit is the line, and it is inclusive - the source game asks >= 0.5f.
        [TestCase(0f, false)]
        [TestCase(0.49f, false)]
        [TestCase(0.5f, true)]
        [TestCase(1f, true)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void TryRead_WorldSpace_IsTruthyAtHalf(float authored, bool expected)
        {
            var settings = ABParticleMap.TryRead(Emitter(WithLead(0f, 0f, authored))).Value;
            Assert.AreEqual(expected, settings.WorldSpace);
        }

        [TestCase(0f, false)]
        [TestCase(0.49f, false)]
        [TestCase(0.5f, true)]
        [TestCase(1f, true)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void TryRead_DespawnOnEnd_IsTruthyAtHalf(float authored, bool expected)
        {
            var settings = ABParticleMap.TryRead(Emitter(WithLead(0f, 0f, 1f, authored))).Value;
            Assert.AreEqual(expected, settings.DespawnOnEnd);
        }

        #endregion

        #region Particle lifetime

        // ABTimeMap.GetLastKeyframeTime would answer zero here, because it skips a track holding one
        // keyframe - that rule belongs to the OBJECT's lifetime, not to a particle's.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void ResolveTimelineLength_CountsSingleKeyframeTracks_UnlikeTheObjectLifetime()
        {
            var source = Emitter(0f, 0f);
            source.Color.Keyframes.Add(new VgdKeyframe { Time = 2.5f, Values = new List<float> { 1f, 100f } });

            Assert.AreEqual(0f, ABTimeMap.GetLastKeyframeTime(source),
                "the object lifetime rule skips a one-keyframe track");
            Assert.AreEqual(2.5f, ABParticleMap.ResolveTimelineLength(source),
                "a particle's own life does not");
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void ResolveTimelineLength_TakesTheLargestTimeAcrossEveryTrack()
        {
            var source = Emitter(0f, 0f);
            source.Move.Keyframes.Add(new VgdKeyframe { Time = 1f, Values = new List<float> { 0f, 0f } });
            source.Scale.Keyframes.Add(new VgdKeyframe { Time = 3.25f, Values = new List<float> { 1f, 1f } });
            source.Rotate.Keyframes.Add(new VgdKeyframe { Time = 0.5f, Values = new List<float> { 0f } });

            Assert.AreEqual(3.25f, ABParticleMap.ResolveTimelineLength(source));
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void ResolveTimelineLength_EveryKeyframeAtZero_IsFlooredRatherThanZero()
        {
            Assert.AreEqual(ABParticleMap.MinTimelineLength,
                ABParticleMap.ResolveTimelineLength(Emitter(0f, 0f)));
        }

        // The source game answers before it looks at a keyframe, and it answers a whole second -
        // deliberately not the floor the keyframe path uses.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void ResolveTimelineLength_NoTracksAtAll_IsOneSecond()
        {
            var source = new VgdObject { Id = "e", ObjectType = (int)ABObjectType.Particles };
            source.Tracks = null;

            Assert.AreEqual(ABParticleMap.NoTracksTimelineLength,
                ABParticleMap.ResolveTimelineLength(source));
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void TryRead_CarriesTheResolvedLifetime()
        {
            var source = Emitter(0f, 0f);
            source.Scale.Keyframes.Add(new VgdKeyframe { Time = 0.84f, Values = new List<float> { 1f, 1f } });

            Assert.AreEqual(0.84f, ABParticleMap.TryRead(source).Value.TimelineLength, 1e-5f);
        }

        #endregion
    }
}
