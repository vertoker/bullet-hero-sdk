using System;
using BH.SDK.Interop.AfterBeat;
using NUnit.Framework;

namespace BH.SDK.Tests.Interop.AfterBeat
{
    // The fit is pure 2x2 linear algebra and is tested as such - no documents, no importer. Every
    // assertion below is about ONE question: how close does R(angle)*diag(scale) get to the map
    // Afterbeat actually composes, R(rp)*S(sp)*R(rc)*S(sc)? The parent's own rotation cancels out
    // of both sides, so it never appears here.
    //
    // Two properties matter more than any single number and are swept rather than spot-checked:
    // the fit must be optimal (perturbing it in either variable makes it worse), and it must never
    // be worse than doing nothing.
    public class ABLinearFitTests
    {
        private const float Tolerance = 1e-4f;

        [TestCase(0f)]
        [TestCase(45f)]
        [TestCase(123f)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void KeepingRotation_UnderAUniformParent_ChangesNothing(float degrees)
        {
            var fit = ABLinearFit.KeepingRotation(3f, 3f, Radians(degrees));

            Assert.AreEqual(1f, fit.ScaleX, Tolerance);
            Assert.AreEqual(1f, fit.ScaleY, Tolerance);
        }

        // S(x, y)*R(90) == R(90)*S(y, x) - the one non-commuting composition that still lands on a
        // plain rotation and scale. This is the case the fit has to reproduce exactly, because it
        // is the one the importer got right before there was a fit at all.
        [TestCase(90f)]
        [TestCase(270f)]
        [TestCase(-90f, TestName = "KeepingRotation_AtAQuarterTurn_TradesTheParentsAxes(-90)")]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void KeepingRotation_AtAQuarterTurn_TradesTheParentsAxes(float degrees)
        {
            const float x = 8f;
            const float y = 2f;

            var fit = ABLinearFit.KeepingRotation(x, y, Radians(degrees));

            Assert.AreEqual(y / x, fit.ScaleX, Tolerance);
            Assert.AreEqual(x / y, fit.ScaleY, Tolerance);
        }

        // R(180) == -I commutes with a diagonal scale, so a half turn is already exact and must be
        // left alone. It used to be reported as shear, which is the same mistake as correcting it.
        [TestCase(0f)]
        [TestCase(180f)]
        [TestCase(-180f, TestName = "KeepingRotation_AtAStraightAngle_ChangesNothing(-180)")]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void KeepingRotation_AtAStraightAngle_ChangesNothing(float degrees)
        {
            var fit = ABLinearFit.KeepingRotation(8f, 2f, Radians(degrees));

            Assert.AreEqual(1f, fit.ScaleX, Tolerance);
            Assert.AreEqual(1f, fit.ScaleY, Tolerance);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void KeepingRotation_LeavesTheRotationWhereItWas()
        {
            var rotation = Radians(37f);

            var fit = ABLinearFit.KeepingRotation(8f, 2f, rotation);

            Assert.AreEqual(rotation, fit.Rotation, Tolerance);
        }

        // The child's own scale must not reach the answer: the fit is a property of the hop, so one
        // factor pair has to serve an animated scale track as well as a still one.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void KeepingRotation_IgnoresTheChildsOwnScale()
        {
            var one = ABLinearFit.KeepingRotation(8f, 2f, Radians(37f));
            var free = ABLinearFit.Free(8f, 2f, Radians(37f), 3f, 5f);

            Assert.AreNotEqual(one.ScaleX, free.ScaleX,
                "the free fit does read it - otherwise this proves nothing");
            Assert.AreEqual(one.ScaleX, ABLinearFit.KeepingRotation(8f, 2f, Radians(37f)).ScaleX,
                Tolerance);
        }

        [TestCase(17f)]
        [TestCase(45f)]
        [TestCase(88f)]
        [TestCase(150f)]
        [TestCase(-63f, TestName = "KeepingRotation_AtAnyAngle_BeatsLeavingItAlone(-63)")]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void KeepingRotation_AtAnyAngle_BeatsLeavingItAlone(float degrees)
        {
            const float px = 8f, py = 2f, cx = 3f, cy = 5f;
            var rotation = Radians(degrees);
            var target = Target(px, py, rotation, cx, cy);

            var fit = ABLinearFit.KeepingRotation(px, py, rotation);

            var fitted = Distance(Composed(rotation,
                px * cx * fit.ScaleX, py * cy * fit.ScaleY), target);
            var untouched = Distance(Composed(rotation, px * cx, py * cy), target);

            Assert.Less(fitted, untouched);
        }

        // Optimality, not just improvement: nudging either factor off the answer must cost.
        [TestCase(17f)]
        [TestCase(45f)]
        [TestCase(150f)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void KeepingRotation_AtAnyAngle_IsTheBestScaleThatRotationAllows(float degrees)
        {
            const float px = 8f, py = 2f, cx = 3f, cy = 5f;
            var rotation = Radians(degrees);
            var target = Target(px, py, rotation, cx, cy);

            var fit = ABLinearFit.KeepingRotation(px, py, rotation);
            var best = Distance(Composed(rotation,
                px * cx * fit.ScaleX, py * cy * fit.ScaleY), target);

            foreach (var nudgeX in new[] { 0.9f, 1f, 1.1f })
            foreach (var nudgeY in new[] { 0.9f, 1f, 1.1f })
            {
                if (Math.Abs(nudgeX - 1f) < Tolerance && Math.Abs(nudgeY - 1f) < Tolerance) continue;

                var nudged = Distance(Composed(rotation,
                    px * cx * fit.ScaleX * nudgeX, py * cy * fit.ScaleY * nudgeY), target);
                Assert.LessOrEqual(best, nudged, "nudged by " + nudgeX + ", " + nudgeY);
            }
        }

        [TestCase(90f)]
        [TestCase(270f)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Free_AtAQuarterTurn_ReachesTheSameAnswerWithoutTurning(float degrees)
        {
            const float x = 8f, y = 2f;
            var rotation = Radians(degrees);

            var fit = ABLinearFit.Free(x, y, rotation, 3f, 5f);

            Assert.AreEqual(rotation, fit.Rotation, Tolerance);
            Assert.AreEqual(y / x, fit.ScaleX, Tolerance);
            Assert.AreEqual(x / y, fit.ScaleY, Tolerance);
        }

        // Both branches of a half turn are equally good - R(a)*D and R(a + 180)*(-D) are the same
        // map - so the fit has to pick the one that leaves the source alone rather than the one
        // that flips the object and negates its scale to compensate.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Free_PicksTheBranchNearestTheSourceRotation()
        {
            var rotation = Radians(180f);

            var fit = ABLinearFit.Free(8f, 2f, rotation, 3f, 5f);

            Assert.AreEqual(rotation, fit.Rotation, Tolerance);
            Assert.AreEqual(1f, fit.ScaleX, Tolerance);
            Assert.AreEqual(1f, fit.ScaleY, Tolerance);
        }

        [TestCase(17f)]
        [TestCase(45f)]
        [TestCase(88f)]
        [TestCase(150f)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void Free_AtAnyAngle_IsAtLeastAsCloseAsKeepingTheRotation(float degrees)
        {
            const float px = 8f, py = 2f, cx = 3f, cy = 5f;
            var rotation = Radians(degrees);
            var target = Target(px, py, rotation, cx, cy);

            var kept = ABLinearFit.KeepingRotation(px, py, rotation);
            var free = ABLinearFit.Free(px, py, rotation, cx, cy);

            var keptError = Distance(Composed(rotation,
                px * cx * kept.ScaleX, py * cy * kept.ScaleY), target);
            var freeError = Distance(Composed(free.Rotation,
                px * cx * free.ScaleX, py * cy * free.ScaleY), target);

            Assert.LessOrEqual(freeError, keptError + Tolerance);
        }

        [TestCase(17f)]
        [TestCase(45f)]
        [TestCase(150f)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void Free_AtAnyAngle_IsTheBestRotationAndScaleThereIs(float degrees)
        {
            const float px = 8f, py = 2f, cx = 3f, cy = 5f;
            var rotation = Radians(degrees);
            var target = Target(px, py, rotation, cx, cy);

            var fit = ABLinearFit.Free(px, py, rotation, cx, cy);
            var best = Distance(Composed(fit.Rotation,
                px * cx * fit.ScaleX, py * cy * fit.ScaleY), target);

            // Every neighbour is re-fitted in scale before it is compared, so this measures the
            // ANGLE alone - an angle that only loses because its scale was left behind proves
            // nothing about the angle.
            foreach (var nudge in new[] { -5f, -1f, 1f, 5f })
            {
                var angle = fit.Rotation + Radians(nudge);
                var neighbour = ABLinearFit.KeepingRotation(px, py, angle);
                var error = Distance(Composed(angle,
                    px * cx * neighbour.ScaleX, py * cy * neighbour.ScaleY), target);

                Assert.LessOrEqual(best, error + Tolerance, "nudged by " + nudge + " degrees");
            }
        }

        [TestCase(0f)]
        [TestCase(90f)]
        [TestCase(180f)]
        [TestCase(270f)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Shear_AtEveryQuarterTurn_IsZero(float degrees)
        {
            Assert.AreEqual(0f, ABLinearFit.Shear(8f, 2f, Radians(degrees)), Tolerance);
        }

        [TestCase(0f)]
        [TestCase(45f)]
        [TestCase(123f)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Shear_UnderAUniformParent_IsZero(float degrees)
        {
            Assert.AreEqual(0f, ABLinearFit.Shear(3f, 3f, Radians(degrees)), Tolerance);
        }

        // What the report threshold rides on: a barely anisotropic parent leaves a rectangle a
        // rectangle, a badly squashed one leaves a streak, and the number has to tell them apart.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Shear_GrowsWithTheParentsAnisotropy()
        {
            var mild = ABLinearFit.Shear(1.02f, 1f, Radians(45f));
            var strong = ABLinearFit.Shear(8f, 2f, Radians(45f));

            Assert.Less(mild, 0.02f, "1.02:1 is a rectangle staying a rectangle");
            Assert.Greater(strong, 0.4f, "4:1 is the shape the corpus loses");
        }

        private static float Radians(float degrees) => degrees * (float)Math.PI / 180f;

        // 2x2 row-major, as [m11, m12, m21, m22].

        private static float[] Rotation(float radians)
        {
            var cos = (float)Math.Cos(radians);
            var sin = (float)Math.Sin(radians);
            return new[] { cos, -sin, sin, cos };
        }

        private static float[] Diagonal(float x, float y) => new[] { x, 0f, 0f, y };

        private static float[] Multiply(float[] a, float[] b) => new[]
        {
            a[0] * b[0] + a[1] * b[2], a[0] * b[1] + a[1] * b[3],
            a[2] * b[0] + a[3] * b[2], a[2] * b[1] + a[3] * b[3]
        };

        /// <summary> What Afterbeat composes: the parent's scale above the child's rotation. </summary>
        private static float[] Target(float parentX, float parentY, float rotation,
            float childX, float childY)
            => Multiply(Multiply(Diagonal(parentX, parentY), Rotation(rotation)),
                Diagonal(childX, childY));

        /// <summary> What this format composes: one rotation, one scale, nothing between them. </summary>
        private static float[] Composed(float rotation, float scaleX, float scaleY)
            => Multiply(Rotation(rotation), Diagonal(scaleX, scaleY));

        private static float Distance(float[] a, float[] b)
        {
            var sum = 0f;
            for (var i = 0; i < 4; i++) sum += (a[i] - b[i]) * (a[i] - b[i]);
            return (float)Math.Sqrt(sum);
        }
    }
}
