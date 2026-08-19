using System.Collections.Generic;
using System.Linq;
using BH.SDK.Interop;
using BH.SDK.Interop.AfterBeat;
using BH.SDK.Interop.AfterBeat.Import;
using BH.SDK.Interop.AfterBeat.Models;
using BH.SDK.Models.Effects;
using BH.SDK.Models.Objects;
using BH.SDK.Models.Values;
using NUnit.Framework;

namespace BH.SDK.Tests.Interop.AfterBeat
{
    // Each loss is reported BY NAME and only when the source actually used the thing. A converter
    // that says "some particle details were lost" on every emitter tells an author nothing they can
    // act on, and buries the one emitter that lost something that mattered - so every case here
    // comes in a pair: it fires on a document using the feature, and stays silent on one that does
    // not.
    public class ABParticleLossTests
    {
        private const int Framerate = 60;

        private static ABOptions Options() => new(Framerate);

        /// <summary> An emitter using nothing that cannot cross: no hidden velocity channel, no
        /// world space, the default start speed, a still emitter volume and no gradient. </summary>
        private static VgdObject Plain()
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

            // ev[4] = 100, ev[5] = 0, ev[6] = 0 (local), ev[7] = 0, ev[8..10] default, ev[11] = 1.
            target.Move.Keyframes.Add(new VgdKeyframe
            {
                Time = 0f,
                Values = new List<float> { 0f, 0f, 0f, 0f, 100f, 0f, 0f, 0f, 0f, 360f, 1f, 1f },
            });
            target.Scale.Keyframes.Add(new VgdKeyframe
            {
                Time = 0f,
                Values = new List<float> { 2f, 2f },
            });
            target.Scale.Keyframes.Add(new VgdKeyframe
            {
                Time = 1f,
                Values = new List<float> { 2f, 2f },
            });

            return target;
        }

        private static InteropReport ReportOf(VgdObject source)
        {
            var level = ABMockData.CreateLevel();
            level.Objects = new List<VgdObject> { source };
            return ABLevelImporter.Import(level, null, Options()).Report;
        }

        private static bool Has(InteropReport report, string code)
            => report.Issues.Any(issue => issue.Code == code);

        private static void SetParameter(VgdObject source, int index, float value)
        {
            var values = source.Move.Keyframes[0].Values;
            while (values.Count <= index) values.Add(0f);
            values[index] = value;
        }

        #region Each loss, reported only when it is real

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_SpawnPerUnit_IsReportedOnlyWhenNonZero()
        {
            Assert.IsFalse(Has(ReportOf(Plain()), "particle_spawn_per_unit"));

            var using_ = Plain();
            SetParameter(using_, ABParticleMap.SpawnRatePerUnitIndex, 4f);
            Assert.IsTrue(Has(ReportOf(using_), "particle_spawn_per_unit"));
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_WorldSpace_IsReportedOnlyWhenUsed()
        {
            Assert.IsFalse(Has(ReportOf(Plain()), "particle_world_space"));

            var using_ = Plain();
            SetParameter(using_, ABParticleMap.WorldSpaceIndex, 1f);
            Assert.IsTrue(Has(ReportOf(using_), "particle_world_space"));
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_StartSpeed_IsReportedOnlyWhenItLeavesTheDefault()
        {
            Assert.IsFalse(Has(ReportOf(Plain()), "particle_start_speed"));

            var using_ = Plain();
            SetParameter(using_, ABParticleMap.StartSpeedIndex, 3f);
            Assert.IsTrue(Has(ReportOf(using_), "particle_start_speed"));
        }

        // The largest approximation in the whole conversion, so it has to be named rather than
        // folded into a general one.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_TheVelocityCurve_IsReportedOnlyWhenTheChannelIsUsed()
        {
            // The fixture carries the indices as zeros, because every parameter past them has to
            // be written for the array to reach ev[4] at all - and zeros are a particle that does
            // not travel.
            Assert.IsFalse(Has(ReportOf(Plain()), "particle_velocity_curve"),
                "an index that exists but holds zero is not a channel in use");

            var using_ = Plain();
            using_.Move.Keyframes.Add(new VgdKeyframe
            {
                Time = 1f,
                Values = new List<float> { 0f, 0f, 6f, 0f },
            });
            Assert.IsTrue(Has(ReportOf(using_), "particle_velocity_curve"));
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_AnAnimatedEmitterVolume_IsReportedOnlyWhenItMoves()
        {
            Assert.IsFalse(Has(ReportOf(Plain()), "particle_emitter_volume_animated"));

            var using_ = Plain();
            using_.Scale.Keyframes[1].Values = new List<float> { 8f, 8f };
            Assert.IsTrue(Has(ReportOf(using_), "particle_emitter_volume_animated"));
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_AGradientMaterial_IsReportedOnlyWhenAuthored()
        {
            Assert.IsFalse(Has(ReportOf(Plain()), "particle_gradient_material"));

            var using_ = Plain();
            using_.GradientType = 1;
            Assert.IsTrue(Has(ReportOf(using_), "particle_gradient_material"));
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_TheColourRamp_ReportsLosingItsThemeReference()
        {
            var plain = Plain();
            Assert.IsFalse(Has(ReportOf(plain), "particle_color_theme_lost"),
                "an emitter with no colour track keeps its default tint and loses nothing");

            var using_ = Plain();
            using_.Color.Keyframes.Add(new VgdKeyframe
            {
                Time = 0f,
                Values = new List<float> { 1f, 100f },
            });
            Assert.IsTrue(Has(ReportOf(using_), "particle_color_theme_lost"));
        }

        #endregion

        #region The velocity itself

        // A channel that starts at zero and ends at four, over a one-second life, is four units per
        // second - not the zero its own first value reads as.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_TheVelocityChannel_CrossesAsItsDerivative()
        {
            var source = Plain();
            source.Move.Keyframes.Add(new VgdKeyframe
            {
                Time = 1f,
                Values = new List<float> { 0f, 0f, 4f, -2f },
            });

            var level = ABMockData.CreateLevel();
            level.Objects = new List<VgdObject> { source };

            var imported = ABLevelImporter.Import(level, null, Options()).Level;
            var placement = imported.Game.Objects.Values.OfType<EffectObject>().Single();
            var forces = imported.Resources.Effects[placement.EffectId].Forces;

            var min = (Vector2Value)forces.StartVelocityMin;
            var max = (Vector2Value)forces.StartVelocityMax;

            Assert.AreEqual(4f, min.X, 1e-3f, "four units over one second of life");
            Assert.AreEqual(-2f, min.Y, 1e-3f);
            Assert.AreEqual(min.X, max.X, "Afterbeat has no velocity spread at all");
            Assert.AreEqual(min.Y, max.Y);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_NoVelocityChannel_LeavesTheParticleStill()
        {
            var source = Plain();

            var level = ABMockData.CreateLevel();
            level.Objects = new List<VgdObject> { source };

            var imported = ABLevelImporter.Import(level, null, Options()).Level;
            var placement = imported.Game.Objects.Values.OfType<EffectObject>().Single();
            var velocity = (Vector2Value)imported.Resources.Effects[placement.EffectId]
                .Forces.StartVelocityMin;

            Assert.AreEqual(0f, velocity.X, 1e-5f);
            Assert.AreEqual(0f, velocity.Y, 1e-5f);
        }

        #endregion

        #region Nothing lost

        // An emitter using none of the above has to come back clean of every particle code - that is
        // what makes the codes worth reading at all.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_AnEmitterUsingNothingLost_ReportsNoParticleLoss()
        {
            var report = ReportOf(Plain());
            var lost = new[]
            {
                "particle_spawn_per_unit", "particle_world_space", "particle_start_speed",
                "particle_velocity_curve", "particle_emitter_volume_animated",
                "particle_gradient_material", "particle_color_theme_lost",
            };

            foreach (var code in lost)
                Assert.IsFalse(Has(report, code), code);
        }

        #endregion
    }
}
