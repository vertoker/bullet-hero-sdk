using System.Collections.Generic;
using System.Linq;
using BH.SDK.Interop.AfterBeat;
using BH.SDK.Interop.AfterBeat.Import;
using BH.SDK.Interop.AfterBeat.Models;
using BH.SDK.Models.Enums;
using BH.SDK.Models.Objects;
using NUnit.Framework;

namespace BH.SDK.Tests.Interop.AfterBeat
{
    // Afterbeat refuses damage from anything whose rendered alpha is below 1 (VGPlayer
    // .CheckForObjectCollision), which is why fading a splash out is how a splash is ended over
    // there. Nothing in this format could express that: ColliderId is a per-object constant, so an
    // imported fade arrived as a fully-armed, fully-invisible hitbox - the worst failure mode
    // available, since nothing on screen explains the death.
    //
    // The threshold is the part worth pinning hardest. It is BELOW 1, not "transparent": an object
    // held at 35% is decoration for its whole life over there, and a converter reading the rule as
    // "alpha 0 is harmless" leaves every one of those lethal.

    public class ABOpacityHitGateTests
    {
        private const int Framerate = 60;

        private static ABOptions Options() => new(Framerate);

        private static VgdObject Fading(params (float Time, float Opacity)[] keys)
        {
            var target = new VgdObject
            {
                Id = "obj",
                ObjectType = (int)ABObjectType.Normal,
                Shape = (int)ABShape.Square,
                AutokillType = (int)ABAutokillType.FixedTime,
                AutokillOffset = 2f,
            };

            foreach (var (time, opacity) in keys)
                target.Color.Keyframes.Add(new VgdKeyframe
                {
                    Time = time,
                    Values = new List<float> { 0f, opacity },
                });

            return target;
        }

        private static List<ShapeObject> Import(VgdObject source)
        {
            var level = new VgdLevel();
            level.Objects.Add(source);

            var result = ABLevelImporter.Import(level, null, Options());
            return result.Level.Game.Objects.Values.OfType<ShapeObject>().ToList();
        }

        #region Range math

        private static ABOpacityHitGate.OpacitySample Sample(int frame, float opacity)
            => new(frame, opacity);

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Ranges_FullyOpaqueTrack_IsOneWholeSpanRange()
        {
            var ranges = ABOpacityHitGate.ResolveOpaqueRanges(
                new[] { Sample(0, 1f), Sample(30, 1f) }, 120);

            Assert.AreEqual(1, ranges.Count);
            Assert.AreEqual(0, ranges[0].Start);
            Assert.AreEqual(120, ranges[0].Duration);
        }

        // The fade stops the collider at the keyframe it STARTS from, not at the one it ends on:
        // every frame in between is already below full opacity over there.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Ranges_FadeOut_EndsAtTheKeyframeTheFadeBeginsOn()
        {
            var ranges = ABOpacityHitGate.ResolveOpaqueRanges(
                new[] { Sample(0, 1f), Sample(30, 1f), Sample(60, 0f) }, 120);

            Assert.AreEqual(1, ranges.Count);
            Assert.AreEqual(0, ranges[0].Start);
            Assert.AreEqual(30, ranges[0].Duration);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Ranges_NeverFullyOpaque_IsEmpty()
        {
            var ranges = ABOpacityHitGate.ResolveOpaqueRanges(
                new[] { Sample(0, 0.35f), Sample(60, 0.35f) }, 120);

            Assert.AreEqual(0, ranges.Count);
        }

        // 99% is already harmless over there, so this is the case a converter written against
        // "transparent means harmless" gets wrong.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Ranges_JustBelowFullOpacity_IsNotOpaque()
        {
            var ranges = ABOpacityHitGate.ResolveOpaqueRanges(
                new[] { Sample(0, 0.99f) }, 60);

            Assert.AreEqual(0, ranges.Count);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Ranges_FadeInAndOut_IsTheOpaqueStretchBetweenThem()
        {
            var ranges = ABOpacityHitGate.ResolveOpaqueRanges(
                new[] { Sample(0, 0f), Sample(30, 1f), Sample(60, 1f), Sample(90, 0f) }, 120);

            // [30, 60) and nothing else: a segment is opaque only when BOTH of its ends are, so the
            // fade-in [0, 30) and the fade-out [60, 90) are both out - the stretch BETWEEN them is
            // what the name says, and it ends where the fade-out begins rather than where it ends.
            Assert.AreEqual(1, ranges.Count);
            Assert.AreEqual(30, ranges[0].Start);
            Assert.AreEqual(30, ranges[0].Duration);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Ranges_TwoSeparateOpaqueStretches_AreTwoRanges()
        {
            var ranges = ABOpacityHitGate.ResolveOpaqueRanges(
                new[]
                {
                    Sample(0, 1f), Sample(20, 1f), Sample(40, 0f),
                    Sample(60, 1f), Sample(80, 1f),
                }, 120);

            // [0, 20) and [60, 120): the second one starts where the fade-in ARRIVES rather than at
            // the key after it, and runs to the end of the object because the last key's value is
            // held forward. The two adjacent opaque segments [60, 80) and [80, 120) merge into one.
            Assert.AreEqual(2, ranges.Count);
            Assert.AreEqual(0, ranges[0].Start);
            Assert.AreEqual(20, ranges[0].Duration);
            Assert.AreEqual(60, ranges[1].Start);
            Assert.AreEqual(60, ranges[1].Duration);
        }

        // The first key's value is held backwards to the object's own start - a track whose first
        // key sits late has one stretch more than it has keys.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Ranges_FirstKeyAfterTheStart_CoversTheHeldStretchBeforeIt()
        {
            var ranges = ABOpacityHitGate.ResolveOpaqueRanges(
                new[] { Sample(30, 1f), Sample(60, 0f) }, 120);

            Assert.AreEqual(1, ranges.Count);
            Assert.AreEqual(0, ranges[0].Start);
            Assert.AreEqual(30, ranges[0].Duration);
        }

        #endregion

        #region Easing

        // The curve between two keys is part of the rule, not a playback detail: the source game
        // re-reads the material on every damage check, so what matters is the alpha the object is
        // DRAWN with on a frame. Reading only the two ends of a segment answered this wrong in both
        // directions - it called an Instant hold decoration for its whole segment, and an
        // overshooting curve harmless on frames where it is at full alpha.

        private static ABOpacityHitGate.OpacitySample Eased(int frame, float opacity, EaseType ease)
            => new(frame, opacity, ease);

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Ranges_InstantEase_HoldsFullOpacityUntilTheKeyframeItself()
        {
            // Afterbeat's "Instant" does not interpolate: the object stays at 100% right up to the
            // frame the next key lands on, and hits for every one of those frames.
            var ranges = ABOpacityHitGate.ResolveOpaqueRanges(
                new[] { Sample(0, 1f), Eased(60, 0f, EaseType.Constant) }, 120);

            Assert.AreEqual(1, ranges.Count);
            Assert.AreEqual(0, ranges[0].Start);
            Assert.AreEqual(59, ranges[0].Duration,
                "only the last cell of the hold straddles the drop");
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Ranges_LinearFade_StillEndsWhereTheFadeBegins()
        {
            // The conservative half of the rule, restated against the curve: a frame counts only
            // when it is at full alpha across its whole cell, so a fade arms nothing extra.
            var ranges = ABOpacityHitGate.ResolveOpaqueRanges(
                new[] { Sample(0, 1f), Eased(60, 0f, EaseType.Linear) }, 120);

            Assert.AreEqual(0, ranges.Count);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Ranges_OvershootingFadeIn_IsOpaqueWhileTheCurveIsAtOrAboveOne()
        {
            // OutBack rises past its target and settles back onto it. The frames it spends at or
            // above full alpha are frames the source game does damage on, and the old two-ends
            // reading dropped every one of them.
            var ranges = ABOpacityHitGate.ResolveOpaqueRanges(
                new[] { Sample(0, 0f), Eased(60, 1f, EaseType.OutBack) }, 120);

            Assert.GreaterOrEqual(ranges.Count, 1, "the overshoot is a hittable stretch");
            var first = ranges[0];
            Assert.Less(first.Start, 60, "it starts before the keyframe the curve is heading for");

            var opacityAtStart = ABOpacityHitGate.ResolveOpacity(
                new[] { Sample(0, 0f), Eased(60, 1f, EaseType.OutBack) }, first.Start);
            Assert.GreaterOrEqual(opacityAtStart, 1f);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Opacity_IsHeldBeforeTheFirstKeyAndAfterTheLast()
        {
            var samples = new[] { Sample(30, 1f), Eased(60, 0.5f, EaseType.Linear) };

            Assert.AreEqual(1f, ABOpacityHitGate.ResolveOpacity(samples, 0f), 1e-5f);
            Assert.AreEqual(1f, ABOpacityHitGate.ResolveOpacity(samples, 30f), 1e-5f);
            Assert.AreEqual(0.75f, ABOpacityHitGate.ResolveOpacity(samples, 45f), 1e-5f);
            Assert.AreEqual(0.5f, ABOpacityHitGate.ResolveOpacity(samples, 90f), 1e-5f);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void Import_AnInstantDrop_HitsForTheWholeHold()
        {
            // End to end, because the easing has to survive CollectSamples as well as the maths:
            // an object that snaps from 100% to 0% keeps its hitbox for the whole visible stretch.
            var source = Fading((0f, 100f), (1f, 0f));
            source.Color.Keyframes[1].Ease = ABEaseMap.InstantEaseName;

            var shapes = Import(source);
            var collider = shapes.Single(s => !s.ShapeId.IsEnabled());

            Assert.AreEqual(0, collider.Span.StartFrame - shapes.Single(s => s.ShapeId.IsEnabled()).Span.StartFrame);
            Assert.AreEqual(Framerate - 1, collider.Span.FrameDuration,
                "one second of hold at 60 fps, less the cell the drop happens on");
        }

        #endregion

        #region Threshold option

        // The option exists because the rule it relaxes is invisible in this editor: a ring that
        // grows for three seconds while fading is correctly lethal for a tenth of one, and that
        // reads exactly like a hitbox lost in conversion. Lowering the threshold is a deliberate
        // break with the source level, so what is pinned here is that it breaks it in the direction
        // asked for and in no other.

        private static List<ShapeObject> Import(VgdObject source, float threshold)
        {
            var level = new VgdLevel();
            level.Objects.Add(source);

            var options = Options();
            options.OpacityHitThreshold = threshold;

            var result = ABLevelImporter.Import(level, null, options);
            return result.Level.Game.Objects.Values.OfType<ShapeObject>().ToList();
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Ranges_LoweredThreshold_ArmsTheColliderForMoreOfTheFade()
        {
            var samples = new[] { Sample(0, 1f), Eased(60, 0f, EaseType.Linear) };

            var ranges = ABOpacityHitGate.ResolveOpaqueRanges(samples, 120, 0.5f);

            Assert.AreEqual(1, ranges.Count);
            Assert.AreEqual(0, ranges[0].Start);
            Assert.AreEqual(30, ranges[0].Duration,
                "half a linear fade is where alpha drops through 0.5");
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Ranges_ThresholdDoesNotMoveAnObjectHeldBelowIt()
        {
            // 35% stays decoration at a 50% threshold - the option widens windows, it does not
            // invent them.
            var ranges = ABOpacityHitGate.ResolveOpaqueRanges(
                new[] { Sample(0, 0.35f), Sample(60, 0.35f) }, 120, 0.5f);

            Assert.AreEqual(0, ranges.Count);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_ThresholdZero_LeavesTheObjectHoldingItsOwnCollider()
        {
            var shapes = Import(Fading((0f, 100f), (1f, 100f), (2f, 0f)), 0f);

            Assert.AreEqual(1, shapes.Count, "the gate is off, so no child is made");
            Assert.IsTrue(shapes[0].ColliderId.IsEnabled(), "the collider stays where the source had it");
            Assert.IsTrue(shapes[0].ShapeId.IsEnabled());
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_LoweredThreshold_WidensTheColliderChild()
        {
            var faithful = Import(Fading((0f, 100f), (1f, 100f), (2f, 0f)))
                .Single(s => !s.ShapeId.IsEnabled());
            var relaxed = Import(Fading((0f, 100f), (1f, 100f), (2f, 0f)), 0.5f)
                .Single(s => !s.ShapeId.IsEnabled());

            Assert.AreEqual(faithful.Span.StartFrame, relaxed.Span.StartFrame);
            Assert.Greater(relaxed.Span.FrameDuration, faithful.Span.FrameDuration,
                "a lower threshold keeps the hitbox alive further into the fade");
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Options_ThresholdIsClampedIntoTheAlphaRange()
        {
            Assert.AreEqual(0f, new ABOptions { OpacityHitThreshold = -3f }.Sanitized().OpacityHitThreshold);
            Assert.AreEqual(1f, new ABOptions { OpacityHitThreshold = 7f }.Sanitized().OpacityHitThreshold);
            Assert.AreEqual(1f, new ABOptions().OpacityHitThreshold,
                "the default has to be the source game's own rule");
        }

        #endregion

        #region Import

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_AnOpaqueObject_KeepsItsOwnColliderAndGainsNothing()
        {
            var shapes = Import(Fading((0f, 100f)));

            Assert.AreEqual(1, shapes.Count, "an object that never fades needs no extra object");
            Assert.IsTrue(shapes[0].ColliderId.IsEnabled());
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_AnObjectHeldBelowFullOpacity_HasNoColliderAtAll()
        {
            var shapes = Import(Fading((0f, 35f), (2f, 35f)));

            Assert.AreEqual(1, shapes.Count);
            Assert.IsFalse(shapes[0].ColliderId.IsEnabled(),
                "35% opacity is decoration over there, for the object's whole life");
            Assert.IsTrue(shapes[0].ShapeId.IsEnabled(), "it is still drawn");
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_AFadingObject_KeepsDrawingAndHandsItsColliderToAChild()
        {
            var shapes = Import(Fading((0f, 100f), (1f, 100f), (2f, 0f)));

            Assert.AreEqual(2, shapes.Count, "the drawn object plus one collider child");

            var drawn = shapes.Single(s => s.ShapeId.IsEnabled());
            var collider = shapes.Single(s => !s.ShapeId.IsEnabled());

            Assert.IsFalse(drawn.ColliderId.IsEnabled(), "the fading object stops hitting on its own");
            Assert.IsTrue(collider.ColliderId.IsEnabled());
            Assert.AreEqual(drawn.ObjectId, collider.ParentObjectId,
                "the collider follows the object's motion by being its child");

            Assert.AreEqual(drawn.Span.StartFrame, collider.Span.StartFrame);
            Assert.Less(collider.Span.FrameDuration, drawn.Span.FrameDuration,
                "the hitbox ends where the fade begins, the drawing goes on");
        }

        // Anchor-stretched with no size of its own is what makes the child's rect - and therefore
        // its hitbox - identical to its parent's whatever the parent's own pivot and size do.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_TheColliderChild_IsStretchedOverItsParentsWholeRect()
        {
            var shapes = Import(Fading((0f, 100f), (1f, 100f), (2f, 0f)));
            var collider = shapes.Single(s => !s.ShapeId.IsEnabled());

            Assert.AreEqual(1, collider.AnchorsMin.Count);
            Assert.AreEqual(1, collider.AnchorsMax.Count);
            Assert.AreEqual(1, collider.Sizes.Count);
            Assert.AreEqual(0, collider.Positions.Count, "an empty track is the engine's own default");
            Assert.AreEqual(0, collider.Scales.Count);
            Assert.AreEqual(0, collider.Rotations.Count);
            Assert.AreEqual(0, collider.Layer, "layer is parent-relative; the child draws nothing anyway");
            Assert.IsTrue(collider.Active);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_AnObjectThatNeverHit_IsUntouchedByTheGate()
        {
            var source = Fading((0f, 100f), (1f, 100f), (2f, 0f));
            source.ObjectType = (int)ABObjectType.NoHit;

            var shapes = Import(source);

            Assert.AreEqual(1, shapes.Count, "nothing to gate, so no child is created");
            Assert.IsFalse(shapes[0].ColliderId.IsEnabled());
        }

        #endregion
    }
}
