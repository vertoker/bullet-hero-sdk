using System;
using BH.SDK.Models.Data;
using BH.SDK.Models.Enums;
using BH.SDK.Models.Interfaces.Keyframes;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Keyframes;
using BH.SDK.Models.Values;

namespace BH.SDK.Interop.AfterBeat
{
    // Afterbeat draws an object's gradient with a shader of its own: two theme slots, a direction
    // in degrees and a length, evaluated per pixel. This format has no ramp at all - it has four
    // corner colours blended bilinearly across the object's local unit box - so the conversion is
    // a SAMPLING rather than a translation. The source ramp is evaluated at the four corners and
    // the result is whichever corner keyframe expresses those four samples most narrowly.
    //
    // A bilinear field reproduces any affine function exactly, and a linear ramp is affine
    // wherever its clamp is inactive, so the sampling is pixel-exact whenever
    // s >= |cos t| + |sin t| - a right-hand side that ranges over [1, sqrt(2)]. Below that the
    // real ramp has saturated flat bands no bilinear field can hold: the corner colours stay
    // right and the distribution comes out softer than the source. A radial ramp is rotationally
    // symmetric and is bilinear at no parameters at all, so its four corners always agree and it
    // arrives flat - but flat at the colour of its PERIPHERY, which is what its area is made of,
    // rather than at the colour of the single point at its centre.
    //
    // Which corners may become LITERALS is the whole of the design here. Afterbeat stores a theme
    // slot per end and the import keeps that as a Color4ThemeRef whenever the keyframe is opaque;
    // that reference is what makes an imported level follow a theme change. A corner sampled
    // strictly between the two ends is a BLEND of two theme colours, and no theme reference
    // expresses a blend, so it has to be baked into a literal and that object stops following the
    // theme. Hence the rule every branch below serves: a corner whose sample landed exactly on an
    // end keeps that end as it was, and only a corner that did not costs a reference. Baking is
    // the caller's choice (ABOptions.BakeGradientCorners); with it off, a corner that would have
    // been a blend snaps to its nearer end instead, trading the smooth ramp for a hard edge and
    // keeping every reference alive.

    /// <summary> Afterbeat's per-object colour ramp (gt/gr/gs), sampled into this format's
    /// four-corner colour keyframes. </summary>
    public static class ABGradientMap
    {
        /// <summary> Ramp lengths at or below this are treated as a hard edge rather than divided
        /// by. The source's own slider stops at 0.25; zero reaches here only from a file no
        /// Afterbeat editor wrote. </summary>
        public const float MinScale = 1e-4f;

        /// <summary> How far two corner samples may differ and still count as one colour. Far
        /// below an 8-bit step, and above the residue cos/sin leave at right angles - cos(90 deg)
        /// is 6e-17 rather than 0, so an exact comparison would call a vertical ramp diagonal. </summary>
        public const float SampleEpsilon = 1e-5f;

        /// <summary> The angle a <see cref="ColorHorizontalKey"/> runs at, left to right. </summary>
        public const float HorizontalRotation = 0f;

        /// <summary> The angle a <see cref="ColorVerticalKey"/> runs at, bottom to top. </summary>
        public const float VerticalRotation = 90f;

        /// <summary> The ramp length at which a two-ended keyframe reaches its ends exactly at the
        /// box edges, i.e. the one an axis-aligned gradient authored here is written back at. </summary>
        public const float NeutralScale = 1f;

        // What the radial ramp's own formula, 2 * length(uv - 0.5) / s, evaluates to at every
        // corner of the unit box: every corner is the same distance from the centre, which is
        // exactly why a radial gradient cannot be a corner fill.
        private const double RadialCornerDistance = 1.4142135623730951d;

        /// <summary>
        /// One colour keyframe of a gradient object. <paramref name="start"/> and
        /// <paramref name="end"/> are the two ends as <see cref="ABColorMap.Import"/> already
        /// resolved them; <paramref name="referenceTheme"/> is consulted only when a corner has to
        /// be baked into a literal.
        /// </summary>
        public static IColor4X4Key Build(ABGradientType gradient, float rotation, float scale,
            IColor4 start, IColor4 end, int frame, EaseType ease, ThemeData referenceTheme,
            bool allowBaking, InteropReport report = null, string path = null)
        {
            if (gradient == ABGradientType.None || start == null || end == null)
                return new Color4Key(start ?? end ?? Color4Value.white, frame, ease);

            // A gradient whose two ends are one colour renders flat over there too, so there is no
            // ramp to sample and nothing has been lost - 71 of the measured corpus' 307 gradient
            // keyframes are this. Sampling it anyway would bake its interior corners into literals
            // and report a theme reference dying to produce the very colour it already was.
            if (start.Equals(end))
                return new Color4Key(start.Copy(), frame, ease);

            var radial = gradient is ABGradientType.Radial or ABGradientType.InvertedRadial;
            var inverted = gradient is ABGradientType.InvertedLinear or ABGradientType.InvertedRadial;

            Sample(radial, rotation, scale, inverted,
                out var kBL, out var kBR, out var kTL, out var kTR);

            if (radial)
                report?.Dropped("gradient_radial",
                    "Afterbeat's radial object gradient is a shape this format's four-corner colour cannot hold; those objects arrive filled flat with the colour their edge had.",
                    path);

            if (!allowBaking)
            {
                var snapped = Snap(ref kBL) | Snap(ref kBR) | Snap(ref kTL) | Snap(ref kTR);
                if (snapped)
                    report?.Approximated("gradient_corners_snapped",
                        "Baking gradient corners is off, so corners falling between the ramp's two ends snapped to the nearer end; those gradients arrive with a hard edge instead of a blend, and keep following the level's theme.",
                        path);
            }

            var colorBL = Corner(kBL, start, end, referenceTheme, out var bakedBL);
            var colorBR = Corner(kBR, start, end, referenceTheme, out var bakedBR);
            var colorTL = Corner(kTL, start, end, referenceTheme, out var bakedTL);
            var colorTR = Corner(kTR, start, end, referenceTheme, out var bakedTR);

            if ((bakedBL || bakedBR || bakedTL || bakedTR) && (IsThemed(start) || IsThemed(end)))
                report?.Approximated("gradient_theme_flattened",
                    "A gradient corner falling between two theme colours is a blend, which no theme reference can express; those corners were baked into literal colours and no longer follow a theme change.",
                    path);

            // Narrowest keyframe the four samples fit into, so an axis-aligned ramp stays the
            // two-colour key it has always been and only a genuinely diagonal one costs four.
            if (Same(kBL, kBR) && Same(kBL, kTL) && Same(kBL, kTR))
                return new Color4Key(colorBL, frame, ease);
            if (Same(kBL, kTL) && Same(kBR, kTR))
                return new ColorHorizontalKey(colorBL, colorBR, frame, ease);
            if (Same(kBL, kBR) && Same(kTL, kTR))
                return new ColorVerticalKey(colorBL, colorTL, frame, ease);
            return new Color4X4Key(colorBL, colorBR, colorTL, colorTR, frame, ease);
        }

        /// <summary> The ramp at the four corners of the local unit box, already inverted when the
        /// source type asked for it. Corner order is BL, BR, TL, TR, matching every four-corner
        /// keyframe in this format. </summary>
        public static void Sample(bool radial, float rotation, float scale, bool inverted,
            out float bl, out float br, out float tl, out float tr)
        {
            var s = Math.Max(scale, MinScale);

            if (radial)
            {
                bl = br = tl = tr = Flip(Clamp01(RadialCornerDistance / s), inverted);
                return;
            }

            var radians = rotation * (Math.PI / 180d);
            var dx = Math.Cos(radians);
            var dy = Math.Sin(radians);

            bl = Flip(Linear(dx, dy, s, 0d, 0d), inverted);
            br = Flip(Linear(dx, dy, s, 1d, 0d), inverted);
            tl = Flip(Linear(dx, dy, s, 0d, 1d), inverted);
            tr = Flip(Linear(dx, dy, s, 1d, 1d), inverted);
        }

        /// <summary> Whether a linear ramp at these parameters is reproduced EXACTLY by the four
        /// corners, i.e. whether its clamp is inactive over the whole box. Always true at
        /// s >= sqrt(2), and at s >= 1 for an axis-aligned angle. </summary>
        public static bool IsExact(float rotation, float scale)
        {
            var radians = rotation * (Math.PI / 180d);
            return scale >= Math.Abs(Math.Cos(radians)) + Math.Abs(Math.Sin(radians));
        }

        private static double Linear(double dx, double dy, double s, double u, double v)
            => Clamp01(0.5d + (dx * (u - 0.5d) + dy * (v - 0.5d)) / s);

        // Quantizing here rather than at the comparisons is what makes SampleEpsilon mean one
        // thing. cos and sin leave a residue of about 1e-16 at every angle that should be exact:
        // at gr = 270 the right-hand corners come out at 1 - 9e-17 instead of 1, and a sample
        // that is not EXACTLY on an end reads as a blend - so a vertical gradient lost both of its
        // theme references to a difference no format can represent, let alone anybody see.
        private static float Flip(double k, bool inverted)
        {
            var value = inverted ? 1d - k : k;
            if (value <= SampleEpsilon) return 0f;
            if (value >= 1d - SampleEpsilon) return 1f;
            return (float)value;
        }

        private static double Clamp01(double value) => value < 0d ? 0d : value > 1d ? 1d : value;

        private static bool Same(float a, float b) => Math.Abs(a - b) <= SampleEpsilon;

        // A sample exactly halfway has no nearer end, so it goes to the one the ramp grows
        // towards - an object never snaps to the colour it was fading away from.
        private static bool Snap(ref float k)
        {
            if (k <= 0f || k >= 1f) return false;
            k = k >= 0.5f ? 1f : 0f;
            return true;
        }

        // Both ends are handed back as COPIES rather than as the instances the caller passed: the
        // same end reaches two corners of one keyframe routinely, and a shared instance there
        // makes editing one corner in the inspector silently edit the other.
        private static IColor4 Corner(float k, IColor4 start, IColor4 end, ThemeData referenceTheme,
            out bool baked)
        {
            baked = false;
            if (k <= 0f) return start.Copy();
            if (k >= 1f) return end.Copy();

            baked = true;
            var a = ABColorMap.ResolveSlot(referenceTheme, start);
            var b = ABColorMap.ResolveSlot(referenceTheme, end);
            return new Color4Value(
                Lerp(a.R, b.R, k),
                Lerp(a.G, b.G, k),
                Lerp(a.B, b.B, k),
                Lerp(a.A, b.A, k));
        }

        private static float Lerp(float a, float b, float t) => a + (b - a) * t;

        private static bool IsThemed(IColor4 color) => color is Color4ThemeRef;
    }
}
