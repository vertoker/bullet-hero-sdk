using System;

namespace BH.SDK.Interop.AfterBeat
{
    // THE ONE PLACE THE TWO TRANSFORM MODELS ARE RECONCILED. Afterbeat parents with plain Unity
    // transforms, so a child's linear map is the matrix product R(rp)*S(sp)*R(rc)*S(sc); this
    // format composes a rotation and a per-axis scale with nothing between them, which is
    // R(rp + rc)*S(sp*sc). The two agree only when S(sp) commutes with R(rc) - i.e. when the
    // parent's scale is uniform, or the child's rotation is a multiple of a quarter turn.
    //
    // Everywhere else the source map is a PARALLELOGRAM and the closest thing this format can hold
    // is a rectangle. There is still a best rectangle, and picking it is what this does: the
    // parent's own rotation R(rp) cancels off both sides, so what is left is the least-squares
    // problem "which R(angle)*diag(scale) is nearest S(sp)*R(rc)*S(sc)", solved in closed form.
    // Writing f(angle) = K + P*cos(2*angle) + Q*sin(2*angle) for the squared off-diagonal residue
    // makes the answer one atan2 - there is no iteration here and no need for any.
    //
    // TWO FITS, AND THE CHOICE BETWEEN THEM IS NOT A PREFERENCE. KeepingRotation solves for the
    // scale alone and is the safe one: it changes nothing an object's CHILDREN read, since this
    // format rotates a child's offset by its parent's rotation, so moving a parent's angle moves
    // the whole subtree under it. Free also solves for the angle and is strictly closer, and is
    // therefore only for objects that have no children and no pivot to swing - see the importer's
    // ResolveShearFits, which is the only caller that gets to make that judgement.
    //
    // KeepingRotation deliberately does NOT read the child's own scale, and that is what lets one
    // factor pair serve an animated scale track: the child's scale enters the product from the
    // RIGHT, so it scales the columns of the answer without moving the angle the columns sit at.
    // Free does read it, because the optimal angle depends on the child's aspect ratio.

    /// <summary> The closest rotation-and-scale to the matrix Afterbeat composes for one parenting
    /// hop. Exact at every quarter turn and under any uniform parent; an approximation - the best
    /// one there is - wherever the source is genuinely skewed. </summary>
    public readonly struct ABLinearFit
    {
        /// <summary> The angle the child should end up at, absolute, in radians. </summary>
        public float Rotation { get; }

        /// <summary> What to multiply the child's own authored scale by. Never the position: a
        /// parent's scale reaches a child's offset identically in both models, so folding the
        /// correction into the position would move the object instead of reshaping it. </summary>
        public float ScaleX { get; }

        /// <inheritdoc cref="ScaleX"/>
        public float ScaleY { get; }

        /// <summary> Whether this fit asks for nothing at all. </summary>
        public bool IsIdentity => Math.Abs(ScaleX - 1f) < Epsilon
                                  && Math.Abs(ScaleY - 1f) < Epsilon;

        private ABLinearFit(float rotation, float scaleX, float scaleY)
        {
            Rotation = rotation;
            ScaleX = scaleX;
            ScaleY = scaleY;
        }

        /// <summary> Below this a scale counts as uniform, a factor as one and an angle as
        /// unchanged - a hundredth of a unit shears nothing anybody can see. </summary>
        public const float Epsilon = 0.01f;

        /// <summary> Below this a parent's scale cannot be divided out of a child without producing
        /// an infinity - a zero-scaled parent draws nothing anyway. </summary>
        public const float MinScale = 1e-4f;

        /// <summary> The best scale for a child whose rotation must stay where the author put it.
        /// Reduces to the axis trade S(x, y)*R(90) == R(90)*S(y, x) at a quarter turn, and to
        /// nothing at all at a straight one. </summary>
        public static ABLinearFit KeepingRotation(float parentX, float parentY, float rotation)
            => At(parentX, parentY, rotation, rotation);

        /// <summary> The best rotation AND scale, for an object free to change both. Never further
        /// from the source than <see cref="KeepingRotation"/>, and the same answer wherever that
        /// one is already exact. </summary>
        public static ABLinearFit Free(float parentX, float parentY, float rotation,
            float childX, float childY)
        {
            if (Math.Abs(parentX) < MinScale || Math.Abs(parentY) < MinScale)
                return new ABLinearFit(rotation, 1f, 1f);

            double x = parentX, y = parentY, u = childX, v = childY;
            var sin = Math.Sin(rotation);
            var cos = Math.Cos(rotation);

            // f(angle) = K + P*cos(2*angle) + Q*sin(2*angle), minimal where (cos, sin) points
            // opposite (P, Q). Both fall to zero only on a degenerate child, which has no angle.
            var p = (x * x * sin * sin * v * v + y * y * sin * sin * u * u
                     - x * x * cos * cos * u * u - y * y * cos * cos * v * v) * 0.5;
            var q = -x * y * sin * cos * (u * u + v * v);
            if (Math.Abs(p) < double.Epsilon && Math.Abs(q) < double.Epsilon)
                return KeepingRotation(parentX, parentY, rotation);

            var angle = 0.5 * Math.Atan2(-q, -p);

            // R(a)*D and R(a + 180)*(-D) are the same map, so both branches are equally optimal.
            // The one nearest the source is the one that leaves the object where the author put it
            // instead of flipping it and negating its scale to compensate.
            var offset = WrapPi(angle - rotation);
            if (Math.Abs(offset) > Math.PI * 0.5)
                offset -= Math.Sign(offset) * Math.PI;

            return At(parentX, parentY, rotation, (float)(rotation + offset));
        }

        /// <summary> How much of one hop's composition no rotation-and-scale can hold, as a share
        /// of the whole - 0 where this format is exact, and rising with both the parent's
        /// anisotropy and how far the child's rotation sits from a quarter turn. Independent of the
        /// child's own scale, so it describes the HOP rather than any one object on it. </summary>
        public static float Shear(float parentX, float parentY, float rotation)
        {
            var norm = parentX * parentX + parentY * parentY;
            if (norm < MinScale * MinScale) return 0f;

            var skew = Math.Abs(Math.Sin(2.0 * rotation)) * Math.Abs(parentY - parentX);
            return (float)(skew / Math.Sqrt(2.0 * norm));
        }

        // The residue of R(-angle)*S(parent)*R(rotation) lands entirely in its off-diagonal, so the
        // best scale is simply its diagonal - and dividing that by the parent's scale is what the
        // child has to multiply its own by, since this format will multiply the parent's back in.
        private static ABLinearFit At(float parentX, float parentY, float rotation, float angle)
        {
            if (Math.Abs(parentX) < MinScale || Math.Abs(parentY) < MinScale)
                return new ABLinearFit(rotation, 1f, 1f);

            var sin = Math.Sin(rotation);
            var cos = Math.Cos(rotation);
            var sinFit = Math.Sin(angle);
            var cosFit = Math.Cos(angle);

            var scaleX = cosFit * cos + sinFit * sin * ((double)parentY / parentX);
            var scaleY = cosFit * cos + sinFit * sin * ((double)parentX / parentY);

            return new ABLinearFit(angle, (float)scaleX, (float)scaleY);
        }

        private static double WrapPi(double radians)
        {
            var wrapped = (radians + Math.PI) % (2.0 * Math.PI);
            if (wrapped < 0.0) wrapped += 2.0 * Math.PI;
            return wrapped - Math.PI;
        }
    }
}
