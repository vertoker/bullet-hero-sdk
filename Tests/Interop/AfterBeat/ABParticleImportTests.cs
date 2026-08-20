using System.Collections.Generic;
using System.Linq;
using BH.SDK.Interop.AfterBeat;
using BH.SDK.Interop.AfterBeat.Import;
using BH.SDK.Interop.AfterBeat.Models;
using BH.SDK.Models;
using BH.SDK.Models.Data;
using BH.SDK.Models.Effects;
using BH.SDK.Models.Objects;
using BH.SDK.Models.Primitives;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using BH.SDK.Validations;
using NUnit.Framework;

namespace BH.SDK.Tests.Interop.AfterBeat
{
    // What an emitter becomes, rather than what its parameters read as - that half is
    // ABParticleMapTests. The two questions this fixture exists for are that the emitter stops being
    // a shape at all, and that two emitters authored the same way land on ONE effect resource with
    // an id that survives being imported again.
    public class ABParticleImportTests
    {
        private const int Framerate = 60;

        private static ABOptions Options() => new(Framerate);

        private static VgdObject Emitter(string id = "emitter", params float[] parameters)
        {
            var target = new VgdObject
            {
                Id = id,
                Name = "Emitter",
                ObjectType = (int)ABObjectType.Particles,
                StartTime = 0f,
                AutokillType = (int)ABAutokillType.LastKeyframe,
                Shape = (int)ABShape.Square,
                ShapeOption = 0,
                Depth = VgdObject.DefaultDepth,
            };

            var values = new List<float> { 0f, 0f, 0f, 0f };
            values.AddRange(parameters);

            target.Move.Keyframes.Add(new VgdKeyframe { Time = 0f, Values = values });
            target.Scale.Keyframes.Add(new VgdKeyframe { Time = 0f, Values = new List<float> { 2f, 4f } });
            target.Scale.Keyframes.Add(new VgdKeyframe { Time = 1.5f, Values = new List<float> { 2f, 4f } });

            return target;
        }

        private static VgdLevel LevelOf(params VgdObject[] objects)
        {
            var level = ABMockData.CreateLevel();
            level.Objects = objects.ToList();
            return level;
        }

        private static Level Import(VgdLevel source)
            => ABLevelImporter.Import(source, null, Options()).Level;

        private static EffectData EffectOf(Level level)
        {
            var placement = level.Game.Objects.Values.OfType<EffectObject>().Single();
            return level.Resources.Effects[placement.EffectId];
        }

        #region The placement

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_AnEmitter_LandsAsAPlacementWithItsOwnResource()
        {
            var level = Import(LevelOf(Emitter()));

            var placement = level.Game.Objects.Values.OfType<EffectObject>().Single();
            Assert.IsTrue(placement.EffectId.IsEnabled());
            Assert.AreEqual(1, level.Resources.Effects.Count);
            Assert.AreEqual(placement.EffectId, level.Resources.Effects[placement.EffectId].EffectId);
        }

        // Loop is what gates the graph's constant-rate spawner - without it ev[4] would be read as
        // one burst rather than as a rate.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_TheSpawnRate_BecomesALoopingParticleCount()
        {
            var effect = EffectOf(Import(LevelOf(Emitter("e", 100f))));

            Assert.IsTrue(effect.Core.Loop);
            Assert.AreEqual(100u, effect.Core.ParticleCount);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_AnAbsurdSpawnRate_IsClampedRatherThanWrapped()
        {
            var effect = EffectOf(Import(LevelOf(Emitter("e", 500000f))));
            Assert.AreEqual(EffectRules.Core.ParticleCount_Max, effect.Core.ParticleCount);
        }

        // Afterbeat assigns one startLifetime with no spread at all, so both ends are the same
        // number - and it is the particle's life, not the object's.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_TheParticleLifetime_HasNoSpread()
        {
            var effect = EffectOf(Import(LevelOf(Emitter("e", 10f))));
            var bounds = (Vector2Value)effect.Core.LifetimeBounds;

            Assert.AreEqual(1.5f, bounds.X, 1e-4f);
            Assert.AreEqual(bounds.X, bounds.Y);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_TheObjectShape_BecomesTheParticleMesh()
        {
            var effect = EffectOf(Import(LevelOf(Emitter())));
            Assert.IsTrue(effect.Core.ParticleShapeId.IsEnabled());
        }

        // The scale track drives the emitter VOLUME over there, not the object's own extent, so it
        // must not reach the placement's transform - it would scale the whole system on top of the
        // volume it already described.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_TheScaleTrack_ReachesTheEmitterVolume_NotThePlacementTransform()
        {
            var level = Import(LevelOf(Emitter()));
            var placement = level.Game.Objects.Values.OfType<EffectObject>().Single();
            var rectangle = (EffectShapeRectangle)EffectOf(level).Shape;
            var size = (Vector2Value)rectangle.Size;

            Assert.AreEqual(2f, size.X, 1e-4f);
            Assert.AreEqual(4f, size.Y, 1e-4f);
            Assert.IsEmpty(placement.Scales, "an emitter has no transform scale of its own");
            Assert.IsEmpty(placement.Sizes, "and no size either");
        }

        // The emitter still MOVES and still turns - only the scale track is re-purposed.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_ThePositionTrack_StillMovesThePlacement()
        {
            var source = Emitter();
            source.Move.Keyframes.Add(new VgdKeyframe
            {
                Time = 1f,
                Values = new List<float> { 5f, -5f },
            });

            var placement = Import(LevelOf(source)).Game.Objects.Values.OfType<EffectObject>().Single();
            Assert.AreEqual(2, placement.Positions.Count);
        }

        #endregion

        #region The emitter volume

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_AnEmitterShapeOfZero_IsARectangle()
        {
            Assert.IsInstanceOf<EffectShapeRectangle>(
                EffectOf(Import(LevelOf(Emitter("e", 0f, 0f, 1f, 0f, 0f)))).Shape);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_AnEmitterShapeOfOne_IsACircle()
        {
            Assert.IsInstanceOf<EffectShapeCircle>(
                EffectOf(Import(LevelOf(Emitter("e", 0f, 0f, 1f, 0f, 1f)))).Shape);
        }

        // Afterbeat writes the arc in degrees and this format stores radians, exactly like every
        // other angle that crosses.
        [TestCase(360f, 6.2831855f)]
        [TestCase(180f, 3.1415927f)]
        [TestCase(0f, 0f)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_TheEmitterArc_CrossesAsRadians(float degrees, float expected)
        {
            var effect = EffectOf(Import(LevelOf(Emitter("e", 0f, 0f, 1f, 0f, 1f, degrees))));
            var circle = (EffectShapeCircle)effect.Shape;

            Assert.AreEqual(expected, ((FloatValue)circle.Arc).Value, 1e-4f);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_TheRadiusThickness_CrossesUnchanged()
        {
            var effect = EffectOf(Import(LevelOf(Emitter("e", 0f, 0f, 1f, 0f, 1f, 360f, 0.9f))));
            var circle = (EffectShapeCircle)effect.Shape;

            Assert.AreEqual(0.9f, ((FloatValue)circle.Thickness).Value, 1e-4f);
        }

        // A circle emitter over there is an ellipse: shape.radius keeps the prefab's own one and
        // the authored scale multiplies it per axis. So both extents cross, the horizontal as the
        // radius and the vertical as a ratio of it, and nothing is halved on the way.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_ACircleEmitter_KeepsBothSemiAxes()
        {
            var circle = (EffectShapeCircle)EffectOf(Import(LevelOf(Emitter("e", 0f, 0f, 1f, 0f, 1f)))).Shape;

            Assert.AreEqual(2f, ((FloatValue)circle.Radius).Value, 1e-4f, "the scale track's own x");
            Assert.AreEqual(2f, ((FloatValue)circle.Aspect).Value, 1e-4f, "4 over 2, not the larger axis");
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_ACircleEmitterWithNoWidth_FallsBackToACircle()
        {
            var source = Emitter("e", 0f, 0f, 1f, 0f, 1f);
            foreach (var keyframe in source.Scale.Keyframes)
                keyframe.Values = new List<float> { 0f, 3f };

            var circle = (EffectShapeCircle)EffectOf(Import(LevelOf(source))).Shape;

            Assert.AreEqual(3f, ((FloatValue)circle.Radius).Value, 1e-4f);
            Assert.AreEqual(EffectRules.Shape.CircleAspect_Default,
                ((FloatValue)circle.Aspect).Value, 1e-4f, "no width means no ratio to describe");
        }

        // A mirrored scale is the same spawn volume, so it is taken absolutely rather than clamped
        // to zero - clamping would collapse the emitter instead.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_AMirroredScale_IsTheSameVolume()
        {
            var source = Emitter();
            source.Scale.Keyframes[0].Values = new List<float> { -2f, -4f };

            var size = (Vector2Value)((EffectShapeRectangle)EffectOf(Import(LevelOf(source))).Shape).Size;

            Assert.AreEqual(2f, size.X, 1e-4f);
            Assert.AreEqual(4f, size.Y, 1e-4f);
        }

        #endregion

        #region Lifetime and span

        // Every number here is the same 1.5s: the scale track's last keyframe decides the object's
        // own length AND, because it is the largest keyframe time on the object, a particle's life.
        private const int SpanFrames = 90;

        // Over there length becomes logicalLength + particleMaxLifetime, so the particles alive when
        // emission stops still finish their life. Here the span carries that tail and the stop frame
        // is where the object used to end.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_ANonDespawningEmitter_OutlivesItsOwnEmission()
        {
            var level = Import(LevelOf(Emitter()));
            var placement = level.Game.Objects.Values.OfType<EffectObject>().Single();
            var effect = EffectOf(level);

            Assert.AreEqual(SpanFrames * 2, placement.Span.FrameDuration,
                "the span carries one particle life past the emission");
            Assert.IsTrue(effect.HasStopLocalFrame);
            Assert.AreEqual(SpanFrames, effect.StopLocalFrame,
                "emission stops where the object used to end");
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_ADespawningEmitter_IsKilledWithItsParticles()
        {
            var level = Import(LevelOf(Emitter("e", 0f, 0f, 1f, 1f)));
            var placement = level.Game.Objects.Values.OfType<EffectObject>().Single();

            Assert.AreEqual(SpanFrames, placement.Span.FrameDuration, "no tail at all");
            Assert.IsFalse(EffectOf(level).HasStopLocalFrame);
        }

        // The stop frame lives on the shared EffectData, so two emitters that agree on every
        // parameter but not on how long they run genuinely are two definitions. Sharing one here
        // would give one of them the other's emission length.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_TwoEmittersDifferingOnlyInLength_DoNotShareOneResource()
        {
            var longer = Emitter("b");
            longer.Scale.Keyframes[1].Time = 3f;

            var level = Import(LevelOf(Emitter("a"), longer));

            Assert.AreEqual(2, level.Resources.Effects.Count);
        }

        // A root object is not bounded by the level's own length, so an emitter whose tail runs past
        // the end is ordinary authored data rather than something validation has to catch.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_AnEmitterOutlivingTheLevel_StillValidates()
        {
            var level = Import(LevelOf(Emitter()));

            // The mock level's own markers sit past this point too, and a marker outside the
            // timeline is a different finding entirely - drop them so the assertion is about the
            // emitter and nothing else.
            level.Game.Events.Markers.Clear();
            level.Game.Events.Checkpoints.Clear();
            level.Settings.FrameDuration = SpanFrames;

            var placement = level.Game.Objects.Values.OfType<EffectObject>().Single();
            Assert.Greater(placement.Span.FrameDuration, level.Settings.FrameDuration);

            var validation = new ValidationFacade().Validate(level);
            Assert.IsFalse(validation.HasErrors, validation.ToString());
        }

        #endregion

        #region Resource identity

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_TwoIdenticalEmitters_ShareOneResource()
        {
            var level = Import(LevelOf(Emitter("a", 100f), Emitter("b", 100f)));

            var ids = level.Game.Objects.Values.OfType<EffectObject>()
                .Select(placement => placement.EffectId).Distinct().ToArray();

            Assert.AreEqual(1, ids.Length);
            Assert.AreEqual(1, level.Resources.Effects.Count);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_TwoDifferentEmitters_DoNotShareOneResource()
        {
            var level = Import(LevelOf(Emitter("a", 100f), Emitter("b", 200f)));

            var ids = level.Game.Objects.Values.OfType<EffectObject>()
                .Select(placement => placement.EffectId).Distinct().ToArray();

            Assert.AreEqual(2, ids.Length);
            Assert.AreEqual(2, level.Resources.Effects.Count);
        }

        // The ABIdMap contract, and the reason the id is derived rather than freshly generated: a
        // level imported twice has to describe the same effect both times, or every reference
        // between the two runs dangles.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Hard)]
        public void Import_TheSameDocumentTwice_ProducesTheSameEffectId()
        {
            var first = Import(LevelOf(Emitter("a", 100f), Emitter("b", 250f)));
            var second = Import(LevelOf(Emitter("a", 100f), Emitter("b", 250f)));

            CollectionAssert.AreEquivalent(
                first.Resources.Effects.Keys.ToArray(),
                second.Resources.Effects.Keys.ToArray());
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Hard)]
        public void Import_EveryParameterThatReachesTheDefinition_MovesTheEffectId()
        {
            var baseline = EffectIdOf(Emitter("e", 100f, 0f, 1f, 0f, 1f, 360f, 1f));

            Assert.AreNotEqual(baseline, EffectIdOf(Emitter("e", 200f, 0f, 1f, 0f, 1f, 360f, 1f)),
                "spawn rate");
            Assert.AreNotEqual(baseline, EffectIdOf(Emitter("e", 100f, 0f, 1f, 0f, 0f, 360f, 1f)),
                "emitter shape");
            Assert.AreNotEqual(baseline, EffectIdOf(Emitter("e", 100f, 0f, 1f, 0f, 1f, 180f, 1f)),
                "arc");
            Assert.AreNotEqual(baseline, EffectIdOf(Emitter("e", 100f, 0f, 1f, 0f, 1f, 360f, 0.5f)),
                "radius thickness");
            Assert.AreNotEqual(baseline, EffectIdOf(Squashed(Emitter("e", 100f, 0f, 1f, 0f, 1f, 360f, 1f))),
                "emitter volume");
        }

        /// <summary> The same emitter spawning inside a different volume - the one parameter the
        /// signature used to leave out. </summary>
        private static VgdObject Squashed(VgdObject source)
        {
            foreach (var keyframe in source.Scale.Keyframes)
                keyframe.Values = new List<float> { 7f, 4f };

            return source;
        }

        private static EffectId EffectIdOf(VgdObject source)
            => Import(LevelOf(source)).Game.Objects.Values.OfType<EffectObject>().Single().EffectId;

        #endregion
    }
}
