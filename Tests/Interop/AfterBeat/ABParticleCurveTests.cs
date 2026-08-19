using System.Collections.Generic;
using System.Linq;
using BH.SDK.Interop.AfterBeat;
using BH.SDK.Interop.AfterBeat.Import;
using BH.SDK.Interop.AfterBeat.Models;
using BH.SDK.Models;
using BH.SDK.Models.Data;
using BH.SDK.Models.Effects;
using BH.SDK.Models.Enums;
using BH.SDK.Models.Objects;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using NUnit.Framework;

namespace BH.SDK.Tests.Interop.AfterBeat
{
    // The hidden half of an emitter's tracks: values 0/1 animate the emitter, values 2/3 describe
    // ONE PARTICLE over its own life. Two things here are easy to get wrong and invisible when they
    // are - the curve's time axis is the PARTICLE's life rather than the level's, and an ease has to
    // be baked into tangents because a curve key here carries no easing at all.
    public class ABParticleCurveTests
    {
        private const int Framerate = 60;

        private static ABOptions Options() => new(Framerate);

        private static VgdObject Emitter()
        {
            var target = new VgdObject
            {
                Id = "emitter",
                Name = "Emitter",
                ObjectType = (int)ABObjectType.Particles,
                AutokillType = (int)ABAutokillType.LastKeyframe,
                Shape = (int)ABShape.Square,
                Depth = VgdObject.DefaultDepth,
            };

            target.Move.Keyframes.Add(new VgdKeyframe
            {
                Time = 0f,
                Values = new List<float> { 0f, 0f, 0f, 0f, 50f },
            });

            return target;
        }

        private static VgdLevel LevelOf(VgdObject source)
        {
            var level = ABMockData.CreateLevel();
            level.Objects = new List<VgdObject> { source };
            return level;
        }

        private static EffectData EffectOf(VgdObject source)
        {
            var level = ABLevelImporter.Import(LevelOf(source), null, Options()).Level;
            var placement = level.Game.Objects.Values.OfType<EffectObject>().Single();
            return level.Resources.Effects[placement.EffectId];
        }

        /// <summary> A scale keyframe carrying both halves: the emitter volume, then the particle's
        /// own size over its life. </summary>
        private static VgdKeyframe Scale(float time, float particleX, float particleY,
            string ease = "Linear")
            => new()
            {
                Time = time,
                Ease = ease,
                Values = new List<float> { 1f, 1f, particleX, particleY },
            };

        #region The size channel

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_NoHiddenSizeChannel_LeavesTheParticleSizeConstant()
        {
            var source = Emitter();
            source.Scale.Keyframes.Add(new VgdKeyframe
            {
                Time = 0f,
                Values = new List<float> { 2f, 2f },
            });

            Assert.IsInstanceOf<EffectScaleValue>(EffectOf(source).Scale);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_TheHiddenSizeChannel_BecomesCurvesOverLife()
        {
            var source = Emitter();
            source.Scale.Keyframes.Add(Scale(0f, 1f, 1f));
            source.Scale.Keyframes.Add(Scale(2f, 0f, 0.5f));

            var scale = (EffectScaleCurvesOverLife)EffectOf(source).Scale;

            Assert.AreEqual(1f, scale.CurveX.KeyFrames[0].Value, 1e-4f);
            Assert.AreEqual(0f, scale.CurveX.KeyFrames[^1].Value, 1e-4f);
            Assert.AreEqual(0.5f, scale.CurveY.KeyFrames[^1].Value, 1e-4f);
        }

        // The curve's axis is the PARTICLE's life, and a particle lives exactly as long as the
        // object's own animation - so the last keyframe always lands on 1 whatever T happens to be.
        [TestCase(2f)]
        [TestCase(0.25f)]
        [TestCase(9.3f)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_CurveTimes_AreNormalizedAgainstTheParticleLife(float lastTime)
        {
            var source = Emitter();
            source.Scale.Keyframes.Add(Scale(0f, 1f, 1f));
            source.Scale.Keyframes.Add(Scale(lastTime * 0.5f, 0.5f, 0.5f));
            source.Scale.Keyframes.Add(Scale(lastTime, 0f, 0f));

            var scale = (EffectScaleCurvesOverLife)EffectOf(source).Scale;

            Assert.AreEqual(0f, scale.CurveX.KeyFrames[0].Time, 1e-4f);
            Assert.AreEqual(0.5f, scale.CurveX.KeyFrames[1].Time, 1e-3f);
            Assert.AreEqual(1f, scale.CurveX.KeyFrames[^1].Time, 1e-4f);
        }

        // A channel authored on one keyframe is a constant, and a curve cannot hold fewer than two
        // keys - so it is held rather than dropped.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_ASingleKeyframeChannel_IsAFlatCurveRatherThanAnEmptyOne()
        {
            var source = Emitter();
            source.Scale.Keyframes.Add(Scale(0f, 0.25f, 0.25f));

            var scale = (EffectScaleCurvesOverLife)EffectOf(source).Scale;

            Assert.GreaterOrEqual(scale.CurveX.KeyFrames.Count, ValueRules.MinCurveKeys);
            Assert.AreEqual(0.25f, scale.CurveX.KeyFrames[0].Value, 1e-4f);
            Assert.AreEqual(0.25f, scale.CurveX.KeyFrames[^1].Value, 1e-4f);
        }

        // The source game samples every eased segment at 16 points; a curve here holds 16 in total,
        // so the budget is shared and the result still has to be a legal curve.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Hard)]
        public void Import_ManyEasedSegments_StillFitTheCurveBudget()
        {
            var source = Emitter();
            source.Scale.Keyframes.Add(Scale(0f, 0f, 0f));
            source.Scale.Keyframes.Add(Scale(1f, 1f, 1f, "OutElastic"));
            source.Scale.Keyframes.Add(Scale(2f, 0.2f, 0.2f, "InOutSine"));
            source.Scale.Keyframes.Add(Scale(3f, 1.5f, 1.5f, "OutCirc"));

            var scale = (EffectScaleCurvesOverLife)EffectOf(source).Scale;

            foreach (var curve in new[] { scale.CurveX, scale.CurveY })
            {
                Assert.LessOrEqual(curve.KeyFrames.Count, ValueRules.MaxCurveKeys);
                Assert.GreaterOrEqual(curve.KeyFrames.Count, ValueRules.MinCurveKeys);

                for (var i = 1; i < curve.KeyFrames.Count; i++)
                    Assert.Greater(curve.KeyFrames[i].Time, curve.KeyFrames[i - 1].Time,
                        "curve keys have to be strictly increasing in time");
            }
        }

        // An eased segment has to BEND - if the ease were dropped the curve would be the straight
        // line between its two ends, and every interior sample would sit on it.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Hard)]
        public void Import_AnEasedSegment_IsBakedRatherThanStraightened()
        {
            var source = Emitter();
            source.Scale.Keyframes.Add(Scale(0f, 0f, 0f));
            source.Scale.Keyframes.Add(Scale(1f, 1f, 1f, "OutCirc"));

            var curve = ((EffectScaleCurvesOverLife)EffectOf(source).Scale).CurveX;
            var bent = curve.KeyFrames.Any(key =>
                key.Time > 0.01f && key.Time < 0.99f && key.Value - key.Time > 0.05f);

            Assert.IsTrue(bent, "an OutCirc segment rises above the straight line between its ends");
        }

        #endregion

        #region The angle channel

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_AConstantParticleAngle_CrossesAsRadians()
        {
            var source = Emitter();
            source.Scale.Keyframes.Add(Scale(0f, 1f, 1f));
            source.Rotate.Keyframes.Add(new VgdKeyframe
            {
                Time = 0f,
                Values = new List<float> { 0f, 0f, 90f },
            });

            var angle = (EffectAngleValue)EffectOf(source).Angle;

            Assert.AreEqual(1.5707964f, ((FloatValue)angle.Angle).Value, 1e-4f);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_AnAnimatedParticleAngle_BecomesACurveInRadians()
        {
            var source = Emitter();
            source.Scale.Keyframes.Add(Scale(0f, 1f, 1f));
            source.Rotate.Keyframes.Add(new VgdKeyframe
            {
                Time = 0f,
                Values = new List<float> { 0f, 0f, 0f },
            });
            source.Rotate.Keyframes.Add(new VgdKeyframe
            {
                Time = 1f,
                Values = new List<float> { 0f, 0f, 180f },
            });

            var curve = ((EffectAngleCurvesOverLife)EffectOf(source).Angle).Curve;

            Assert.AreEqual(0f, curve.KeyFrames[0].Value, 1e-4f);
            Assert.AreEqual(3.1415927f, curve.KeyFrames[^1].Value, 1e-4f);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_NoHiddenAngleChannel_LeavesTheParticleAngleAlone()
        {
            var source = Emitter();
            source.Scale.Keyframes.Add(Scale(0f, 1f, 1f));
            source.Rotate.Keyframes.Add(new VgdKeyframe { Time = 0f, Values = new List<float> { 45f } });

            var angle = (EffectAngleValue)EffectOf(source).Angle;
            Assert.AreEqual(0f, ((FloatValue)angle.Angle).Value, 1e-4f);
        }

        #endregion

        #region The colour ramp

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Hard)]
        public void Import_TheColourTrack_BecomesAParticleRamp()
        {
            var source = Emitter();
            source.Scale.Keyframes.Add(Scale(0f, 1f, 1f));
            source.Color.Keyframes.Add(new VgdKeyframe { Time = 0f, Values = new List<float> { 1f, 100f } });
            source.Color.Keyframes.Add(new VgdKeyframe { Time = 1f, Values = new List<float> { 2f, 50f } });
            source.Color.Keyframes.Add(new VgdKeyframe { Time = 2f, Values = new List<float> { 3f, 0f } });

            var gradient = ((EffectColorGradientOverLife)EffectOf(source).Color).Gradient;

            Assert.AreEqual(3, gradient.ColorKeys.Count);
            Assert.AreEqual(3, gradient.AlphaKeys.Count);
            Assert.AreEqual(1f, gradient.AlphaKeys[0].Alpha, 1e-3f);
            Assert.AreEqual(0.5f, gradient.AlphaKeys[1].Alpha, 1e-3f);
            Assert.AreEqual(0f, gradient.AlphaKeys[^1].Alpha, 1e-3f);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Hard)]
        public void Import_AColourTrackLongerThanARamp_IsCappedRatherThanTruncated()
        {
            var source = Emitter();
            source.Scale.Keyframes.Add(Scale(0f, 1f, 1f));

            for (var i = 0; i < 20; i++)
                source.Color.Keyframes.Add(new VgdKeyframe
                {
                    Time = i * 0.5f,
                    Values = new List<float> { 1f, 100f },
                });

            var gradient = ((EffectColorGradientOverLife)EffectOf(source).Color).Gradient;

            Assert.AreEqual(ValueRules.MaxGradientKeys, gradient.ColorKeys.Count);
            Assert.AreEqual(1f, gradient.ColorKeys[^1].Time, 1e-4f,
                "the tail is what a particle dies as and must survive the cap");
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_ASingleColourKeyframe_IsHeldAcrossTheWholeRamp()
        {
            var source = Emitter();
            source.Scale.Keyframes.Add(Scale(0f, 1f, 1f));
            source.Color.Keyframes.Add(new VgdKeyframe { Time = 0f, Values = new List<float> { 1f, 100f } });

            var gradient = ((EffectColorGradientOverLife)EffectOf(source).Color).Gradient;

            Assert.AreEqual(ValueRules.MinGradientKeys, gradient.ColorKeys.Count);
            Assert.AreEqual(0f, gradient.ColorKeys[0].Time, 1e-4f);
            Assert.AreEqual(1f, gradient.ColorKeys[^1].Time, 1e-4f);
        }

        #endregion

        #region The ease evaluator

        // Baking is the only way an ease can cross, so the evaluator is pinned on its own: every
        // easing has to be an identity at both ends, or a baked curve would not start and end where
        // the author put it.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Evaluate_EveryEasing_PinsBothEnds()
        {
            foreach (EaseType ease in System.Enum.GetValues(typeof(EaseType)))
            {
                Assert.AreEqual(0f, ABEaseMap.Evaluate(ease, 0f), 1e-5f, $"{ease} at 0");
                Assert.AreEqual(1f, ABEaseMap.Evaluate(ease, 1f), 1e-5f, $"{ease} at 1");
            }
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Evaluate_Linear_IsTheIdentity()
        {
            Assert.AreEqual(0.25f, ABEaseMap.Evaluate(EaseType.Linear, 0.25f), 1e-5f);
            Assert.AreEqual(0.75f, ABEaseMap.Evaluate(EaseType.Linear, 0.75f), 1e-5f);
        }

        [TestCase(EaseType.OutQuad)]
        [TestCase(EaseType.OutCubic)]
        [TestCase(EaseType.OutCirc)]
        [TestCase(EaseType.OutExpo)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Evaluate_AnOutEasing_LeadsTheStraightLine(EaseType ease)
        {
            Assert.Greater(ABEaseMap.Evaluate(ease, 0.5f), 0.5f);
        }

        [TestCase(EaseType.InQuad)]
        [TestCase(EaseType.InCubic)]
        [TestCase(EaseType.InCirc)]
        [TestCase(EaseType.InExpo)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Evaluate_AnInEasing_TrailsTheStraightLine(EaseType ease)
        {
            Assert.Less(ABEaseMap.Evaluate(ease, 0.5f), 0.5f);
        }

        #endregion
    }
}
