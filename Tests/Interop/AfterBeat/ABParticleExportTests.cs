using System.Collections.Generic;
using System.Linq;
using BH.SDK.Interop;
using BH.SDK.Interop.AfterBeat;
using BH.SDK.Interop.AfterBeat.Export;
using BH.SDK.Interop.AfterBeat.Import;
using BH.SDK.Interop.AfterBeat.Models;
using BH.SDK.Models;
using BH.SDK.Models.Data;
using BH.SDK.Models.Effects;
using BH.SDK.Models.Objects;
using BH.SDK.Models.Primitives;
using BH.SDK.Models.Values;
using NUnit.Framework;

namespace BH.SDK.Tests.Interop.AfterBeat
{
    // The export is deliberately NOT the inverse of the import, and that is the honest position: an
    // Afterbeat emitter is a much smaller thing than an effect here, so most of what an effect can
    // be has nowhere to go. What this fixture pins is that the subset which CAN cross does, and that
    // everything else is reported BY NAME rather than as a category an author cannot act on.
    public class ABParticleExportTests
    {
        private const int Framerate = 60;

        private static ABOptions Options() => new(Framerate);

        /// <summary> A level holding one effect placement and the definition it points at. </summary>
        private static Level LevelWith(EffectData data)
        {
            var level = new Level();
            var effectId = new EffectId(System.Guid.NewGuid());

            data.EffectId = effectId;
            level.Resources.Effects[effectId] = data;

            var placement = new EffectObject
            {
                ObjectId = new ObjectId(1),
                EffectId = effectId,
                Name = "Emitter",
                Active = true,
                Span = new FrameSpan(0, 60),
            };
            placement.Positions.Add(new Models.Keyframes.PosKey(
                new Vector2Value(0f, 0f), 0, Models.Enums.EaseType.Linear));

            level.Game.Objects[placement.ObjectId] = placement;
            return level;
        }

        private static EffectData Plain() => new()
        {
            Core =
            {
                ParticleCount = 120u,
                Loop = true,
                LifetimeBounds = new Vector2Value(1.5f, 1.5f),
                ParticleShapeId = ShapeId.Null,
            },
            Shape = new EffectShapeRectangle { Size = new Vector2Value(3f, 5f) },
        };

        private static (VgdObject Object, InteropReport Report) Export(EffectData data)
        {
            var result = ABLevelExporter.Export(LevelWith(data), null, Options());
            var emitter = result.Level?.Objects?.FirstOrDefault();
            return (emitter, result.Report);
        }

        private static float Value(VgdObject target, int trackIndex, int valueIndex)
            => target.GetTrack(trackIndex).Keyframes[0].GetValue(valueIndex);

        private static bool Has(InteropReport report, string code)
            => report.Issues.Any(issue => issue.Code == code);

        #region What crosses

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Export_AnEffectPlacement_IsWrittenAsAnEmitter()
        {
            var (emitter, _) = Export(Plain());

            Assert.IsNotNull(emitter, "an effect object is content now, not something to drop");
            Assert.AreEqual((int)ABObjectType.Particles, emitter.ObjectType);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Export_TheParticleCount_BecomesTheSpawnRate()
        {
            var (emitter, _) = Export(Plain());

            Assert.AreEqual(120f,
                Value(emitter, VgdObject.TrackIndex.Move, ABParticleMap.SpawnRatePerSecondIndex),
                1e-3f);
        }

        // Emission stopping early is what a stop frame IS, so an effect carrying one is an emitter
        // that outlives its own emission rather than one killed with its particles.
        [TestCase(true, 0f)]
        [TestCase(false, 1f)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Export_TheStopFrame_DecidesDespawnOnEnd(bool hasStop, float expected)
        {
            var data = Plain();
            data.HasStopLocalFrame = hasStop;
            data.StopLocalFrame = 30;

            var (emitter, _) = Export(data);

            Assert.AreEqual(expected,
                Value(emitter, VgdObject.TrackIndex.Move, ABParticleMap.DespawnOnEndIndex), 1e-3f);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Export_ARectangleEmitter_WritesItsVolumeOnTheScaleTrack()
        {
            var (emitter, _) = Export(Plain());

            Assert.AreEqual((int)ABParticleEmitterShapeType.Rectangle,
                Value(emitter, VgdObject.TrackIndex.Move, ABParticleMap.EmitterShapeIndex), 1e-3f);
            Assert.AreEqual(3f, Value(emitter, VgdObject.TrackIndex.Scale, 0), 1e-3f);
            Assert.AreEqual(5f, Value(emitter, VgdObject.TrackIndex.Scale, 1), 1e-3f);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Export_ACircleEmitter_WritesItsArcInDegrees()
        {
            var data = Plain();
            data.Shape = new EffectShapeCircle
            {
                Radius = new FloatValue(2f),
                Arc = new FloatValue(3.1415927f),
                Thickness = new FloatValue(0.5f),
            };

            var (emitter, _) = Export(data);

            Assert.AreEqual((int)ABParticleEmitterShapeType.Circle,
                Value(emitter, VgdObject.TrackIndex.Move, ABParticleMap.EmitterShapeIndex), 1e-3f);
            Assert.AreEqual(180f,
                Value(emitter, VgdObject.TrackIndex.Move, ABParticleMap.EmitterArcIndex), 1e-2f);
            Assert.AreEqual(0.5f,
                Value(emitter, VgdObject.TrackIndex.Move, ABParticleMap.EmitterRadiusThicknessIndex),
                1e-3f);
            Assert.AreEqual(4f, Value(emitter, VgdObject.TrackIndex.Scale, 0), 1e-3f,
                "a radius of two is a four-wide volume");
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Export_TheParticleSizeCurve_ReachesTheHiddenScaleChannel()
        {
            var data = Plain();
            data.Scale = new EffectScaleCurvesOverLife
            {
                CurveX = ABCurveMap.Flat(0.25f),
                CurveY = ABCurveMap.Flat(0.75f),
            };

            var (emitter, _) = Export(data);

            Assert.AreEqual(0.25f,
                Value(emitter, VgdObject.TrackIndex.Scale, ABParticleMap.ParticleScaleXIndex), 1e-3f);
            Assert.AreEqual(0.75f,
                Value(emitter, VgdObject.TrackIndex.Scale, ABParticleMap.ParticleScaleYIndex), 1e-3f);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Export_TheParticleAngle_CrossesBackAsDegrees()
        {
            var data = Plain();
            data.Angle = new EffectAngleValue { Angle = new FloatValue(1.5707964f) };

            var (emitter, _) = Export(data);

            Assert.AreEqual(90f,
                Value(emitter, VgdObject.TrackIndex.Rotate, ABParticleMap.ParticleAngleIndex), 1e-2f);
        }

        #endregion

        #region What does not, named one by one

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Export_APlainEmitter_LosesNothingWorthNaming()
        {
            var (_, report) = Export(Plain());

            foreach (var code in new[]
                     {
                         "particle_shape_unsupported", "particle_shape_spread",
                         "particle_scale_variant", "particle_angle_variant",
                         "particle_color_variant", "particle_forces", "particle_texture",
                         "particle_render_off", "particle_burst", "particle_lifetime_spread",
                     })
                Assert.IsFalse(Has(report, code), code);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Export_AnEmitterShapeAfterbeatHasNot_IsReported()
        {
            var data = Plain();
            data.Shape = new EffectShapeTorus();

            Assert.IsTrue(Has(Export(data).Report, "particle_shape_unsupported"));
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Export_ARandomScaleVariant_IsReported()
        {
            var data = Plain();
            data.Scale = new EffectScaleRandomUniform();

            Assert.IsTrue(Has(Export(data).Report, "particle_scale_variant"));
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Export_TheForcesGroup_IsReported()
        {
            var data = Plain();
            data.Forces.StartGravityMin = new FloatValue(-9.81f);

            Assert.IsTrue(Has(Export(data).Report, "particle_forces"));
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Export_AOneShotEffect_IsReportedAsContinuous()
        {
            var data = Plain();
            data.Core.Loop = false;

            Assert.IsTrue(Has(Export(data).Report, "particle_burst"));
        }

        // Afterbeat gives every particle of an emitter one lifetime; a range has nowhere to go.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Export_ALifetimeSpread_IsReported()
        {
            var data = Plain();
            data.Core.LifetimeBounds = new Vector2Value(1f, 4f);

            Assert.IsTrue(Has(Export(data).Report, "particle_lifetime_spread"));
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Export_AnEffectNothingPlaces_IsReportedAsUnwritable()
        {
            var level = LevelWith(Plain());
            level.Resources.Effects[new EffectId(System.Guid.NewGuid())] = new EffectData();

            var report = ABLevelExporter.Export(level, null, Options()).Report;

            Assert.IsTrue(Has(report, "effect_resources"));
        }

        // A placement whose definition is missing cannot be written as anything, and guessing would
        // put an emitter into the level the author never authored.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Export_APlacementWithNoDefinition_IsDroppedRatherThanGuessed()
        {
            var level = LevelWith(Plain());
            level.Resources.Effects.Clear();

            var result = ABLevelExporter.Export(level, null, Options());

            Assert.IsTrue(Has(result.Report, "effect_unresolved"));
            Assert.IsEmpty(result.Level.Objects);
        }

        #endregion

        #region Round trip

        // The one property a round trip has to hold: an emitter that went out as an emitter comes
        // back as one, still pointing at a definition the level holds.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Hard)]
        public void RoundTrip_AnEmitter_StaysAnEmitter()
        {
            var exported = ABLevelExporter.Export(LevelWith(Plain()), null, Options());
            Assert.IsNotNull(exported.Level);

            var reimported = ABLevelImporter.Import(exported.Level, null, Options()).Level;
            var placement = reimported.Game.Objects.Values.OfType<EffectObject>().Single();

            Assert.IsTrue(reimported.Resources.Effects.ContainsKey(placement.EffectId));
            Assert.AreEqual(120u, reimported.Resources.Effects[placement.EffectId].Core.ParticleCount);
        }

        #endregion
    }
}
