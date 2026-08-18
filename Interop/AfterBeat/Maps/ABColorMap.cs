using System;
using BH.SDK.Interop.AfterBeat.Models;
using BH.SDK.Models.Data;
using BH.SDK.Models.Enums.Values;
using BH.SDK.Models.Interfaces.Keyframes;
using BH.SDK.Models.Keyframes;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Values;
using BH.SDK.Rules;

namespace BH.SDK.Interop.AfterBeat
{
    // Afterbeat's object colour is a theme slot index PLUS its own opacity, as two independent
    // numbers. This format's Color4ThemeRef stores only the index and takes alpha from the theme
    // as well, so the pair cannot be carried by one value.
    //
    // Hence the hybrid, which is the author's decision rather than a technical one: at full opacity
    // the colour becomes a ThemeRef, keeping the level's theme switching alive, which is the thing
    // Project Arrhythmia levels are built around. Below full opacity it becomes a literal, resolved
    // against the level's reference theme, keeping the fade the author actually authored. A fading
    // object stops following the theme; the alternative was every fade in the level disappearing.
    //
    // Which palette an index means is decided by WHERE it was read, not by the number: the same 0
    // is the first object colour on an object, the first parallax colour on a background, and the
    // first effect colour inside a bloom keyframe. ABPalette is that context.

    /// <summary> Which of Afterbeat's four colour palettes an index is read against. </summary>
    public enum ABPalette
    {
        Objects = 0,
        Parallax = 1,
        Effects = 2,
        Players = 3,
    }

    /// <summary> Afterbeat's (palette index, opacity) pairs, mapped onto this format's colours. </summary>
    public static class ABColorMap
    {
        /// <summary> Above this an opacity counts as fully opaque. One 8-bit step is well below
        /// anything an author can see, and an exactly-1.0 comparison would send a colour authored
        /// as 0.999 down the literal path for no visible reason. </summary>
        public const float OpaqueEpsilon = 1f / 512f;

        /// <summary> What an effect whose colour index is the "none" sentinel tints with, when
        /// tinting it white is the source game's own answer - bloom and the screen gradient. </summary>
        public static Color4Value EffectColorWhite => Color4Value.white;

        /// <summary> The same, for the one effect whose answer is black instead - the vignette,
        /// which darkens rather than tints. </summary>
        public static Color4Value EffectColorBlack => Color4Value.black;

        /// <summary> First <see cref="ThemeData"/> slot of one of Afterbeat's palettes. </summary>
        public static int GetBaseIndex(ABPalette palette) => palette switch
        {
            ABPalette.Objects => ABThemeMap.ObjectStartIndex,
            ABPalette.Parallax => ABThemeMap.ParallaxStartIndex,
            ABPalette.Effects => ABThemeMap.EffectStartIndex,
            ABPalette.Players => ABThemeMap.PlayerStartIndex,
            _ => ABThemeMap.ObjectStartIndex,
        };

        /// <summary> How many colours a palette holds. </summary>
        public static int GetCount(ABPalette palette)
            => palette == ABPalette.Players ? VgtTheme.PlayerCount : VgtTheme.ObjectCount;

        /// <summary> A palette index as a slot of the 64. Out-of-range indices clamp into the
        /// palette rather than reaching a neighbouring one - a bad index should read as the wrong
        /// object colour, never as an effect colour. </summary>
        public static int ToThemeIndex(int paletteIndex, ABPalette palette)
        {
            var baseIndex = GetBaseIndex(palette);
            var count = GetCount(palette);
            var clamped = Math.Clamp(paletteIndex, 0, count - 1);
            return Math.Clamp(baseIndex + clamped, ValueRules.MinThemeIndex, ValueRules.MaxThemeIndex);
        }

        /// <summary> The reverse - a slot of the 64 back to its palette index, or -1 when the slot
        /// is outside that palette. </summary>
        public static int ToPaletteIndex(int themeIndex, ABPalette palette)
        {
            var baseIndex = GetBaseIndex(palette);
            var offset = themeIndex - baseIndex;
            return offset >= 0 && offset < GetCount(palette) ? offset : -1;
        }

        #region Import

        /// <summary>
        /// Builds a colour out of Afterbeat's index and opacity, applying the hybrid policy.
        /// <paramref name="referenceTheme"/> is only consulted on the literal branch and may be null,
        /// in which case a half-transparent colour falls back to white at the right alpha.
        /// </summary>
        public static IColor4 Import(int paletteIndex, float opacity, ABPalette palette,
            ThemeData referenceTheme, InteropReport report = null, string path = null)
        {
            var themeIndex = ToThemeIndex(paletteIndex, palette);
            var alpha = Math.Clamp(opacity, ValueRules.MinColor, ValueRules.MaxColor);

            if (alpha >= ValueRules.MaxColor - OpaqueEpsilon)
                return new Color4ThemeRef(themeIndex);

            report?.Approximated("color_opacity_literal",
                "Afterbeat keeps opacity separately from the theme colour, which this format's theme reference cannot. Semi-transparent colours were resolved to literals and no longer follow a theme change.",
                path);

            var resolved = Resolve(referenceTheme, themeIndex);
            return new Color4Value(resolved.R, resolved.G, resolved.B, alpha);
        }

        private static Color4Value Resolve(ThemeData theme, int themeIndex)
        {
            if (theme?.Matrix == null || themeIndex < 0 || themeIndex >= theme.Matrix.Length)
                return Color4Value.white;
            return theme.Matrix[themeIndex] ?? Color4Value.white;
        }

        #endregion

        #region Export

        /// <summary>
        /// Writes a colour back as Afterbeat's (index, opacity) pair. A ThemeRef crosses exactly; a
        /// literal has to be matched against the reference theme, which is a search rather than a
        /// lookup and is reported as an approximation.
        /// </summary>
        public static (int Index, float Opacity) Export(IColor4 color, ABPalette palette,
            ThemeData referenceTheme, InteropReport report = null, string path = null)
        {
            switch (color)
            {
                case null:
                    return (0, 1f);

                case Color4ThemeRef themeRef:
                {
                    var index = ToPaletteIndex(themeRef.ThemeColorIndex, palette);
                    if (index >= 0) return (index, 1f);

                    // A slot outside this palette - a free band, or an object pointed at an effect
                    // colour. Afterbeat cannot say that at all.
                    report?.Approximated("color_theme_slot_outside_palette",
                        "Some theme references point at slots Afterbeat's palettes do not cover; those colours export as the palette's first entry.",
                        path);
                    return (0, 1f);
                }

                case Color4Value literal:
                    return (MatchNearest(literal, palette, referenceTheme, report, path), literal.A);

                case Color4MinMax minMax:
                {
                    report?.Approximated("color_random_resolved",
                        "Afterbeat has no random colour; those colours export as the midpoint of their range.",
                        path);
                    var mid = new Color4Value(
                        (minMax.MinR + minMax.MaxR) * 0.5f,
                        (minMax.MinG + minMax.MaxG) * 0.5f,
                        (minMax.MinB + minMax.MaxB) * 0.5f,
                        (minMax.MinA + minMax.MaxA) * 0.5f);
                    return (MatchNearest(mid, palette, referenceTheme, report, path), mid.A);
                }

                default:
                    report?.Approximated("color_unknown_variant",
                        $"Colour variant '{color.GetModelType()}' has no Afterbeat equivalent; those colours export as the palette's first entry.",
                        path);
                    return (0, 1f);
            }
        }

        // Nearest by squared RGB distance, alpha ignored: alpha travels separately as the opacity
        // half of the pair, so including it here would pick a differently-coloured slot to satisfy
        // a channel the caller is about to write anyway.
        private static int MatchNearest(Color4Value color, ABPalette palette,
            ThemeData referenceTheme, InteropReport report, string path)
        {
            report?.Approximated("color_literal_matched",
                "Afterbeat objects can only reference a theme colour; literal colours were matched to the nearest slot of the level's theme.",
                path);

            if (referenceTheme?.Matrix == null) return 0;

            var baseIndex = GetBaseIndex(palette);
            var count = GetCount(palette);
            var best = 0;
            var bestDistance = float.MaxValue;

            for (var i = 0; i < count; i++)
            {
                var slot = Resolve(referenceTheme, baseIndex + i);
                var dr = slot.R - color.R;
                var dg = slot.G - color.G;
                var db = slot.B - color.B;
                var distance = dr * dr + dg * dg + db * db;
                if (distance >= bestDistance) continue;
                bestDistance = distance;
                best = i;
            }

            return best;
        }

        #endregion

        #region Four-corner colours

        // An Afterbeat object has a gradient of its own - two theme slots and a type on the object,
        // with the second slot living in the colour keyframe's third value. So the two-corner
        // keyframes do NOT have to be averaged away: only the four-corner one, which is a shape
        // that side has no version of at all.
        //
        // The gradient TYPE sits on the object rather than on the keyframe, so it is decided once
        // from the whole track. A track mixing gradient and flat keyframes gets the gradient, with
        // the flat ones writing the same slot at both ends - which is what a flat keyframe means
        // inside a gradient anyway.

        /// <summary> One colour keyframe as Afterbeat writes it: a slot at each end of the object's
        /// gradient plus the opacity the pair shares. </summary>
        public readonly struct ExportedColorKey
        {
            public int StartIndex { get; }
            public int EndIndex { get; }
            public float Opacity { get; }

            /// <summary> True when the two ends are genuinely different colours, i.e. when the
            /// object needs a gradient type at all. </summary>
            public bool IsGradient { get; }

            public ExportedColorKey(int startIndex, int endIndex, float opacity, bool isGradient)
            {
                StartIndex = startIndex;
                EndIndex = endIndex;
                Opacity = opacity;
                IsGradient = isGradient;
            }
        }

        /// <summary> One keyframe colour of any shape into the pair of slots Afterbeat stores. </summary>
        public static ExportedColorKey ExportKey(IColor4X4Key key, ABPalette palette,
            ThemeData referenceTheme, InteropReport report, string path)
        {
            switch (key)
            {
                case ColorHorizontalKey horizontal:
                    return Pair(horizontal.Color4Left, horizontal.Color4Right,
                        palette, referenceTheme, report, path);

                case ColorVerticalKey vertical:
                    report?.Approximated("gradient_direction",
                        "Afterbeat draws an object gradient along one axis of its own; a vertical gradient exports with its two colours in place but not its direction.",
                        path);
                    return Pair(vertical.Color4Bottom, vertical.Color4Top,
                        palette, referenceTheme, report, path);

                default:
                {
                    var (index, opacity) = Export(Flatten(key, report, path), palette,
                        referenceTheme, report, path);
                    return new ExportedColorKey(index, index, opacity, false);
                }
            }
        }

        private static ExportedColorKey Pair(IColor4 start, IColor4 end, ABPalette palette,
            ThemeData referenceTheme, InteropReport report, string path)
        {
            var (startIndex, startOpacity) = Export(start, palette, referenceTheme, report, path);
            var (endIndex, _) = Export(end, palette, referenceTheme, report, path);

            // Afterbeat carries ONE opacity for the pair, so the start's is the one that travels -
            // the end of a gradient fading separately is the half this format has and that one
            // does not.
            return new ExportedColorKey(startIndex, endIndex, startOpacity, startIndex != endIndex);
        }

        /// <summary> Averages a four-corner keyframe colour down to the single colour Afterbeat has
        /// a field for. A keyframe whose corners already agree is not an approximation and is not
        /// reported as one. </summary>
        public static IColor4 Flatten(IColor4X4Key key, InteropReport report = null, string path = null)
        {
            switch (key)
            {
                case null:
                    return Color4Value.white;
                case Color4Key single:
                    return single.Value;
                case ColorHorizontalKey horizontal:
                    report?.Approximated("color_gradient_flattened",
                        "Afterbeat has no per-corner object colour; gradients were averaged into one colour.", path);
                    return Average(horizontal.Color4Left, horizontal.Color4Right);
                case ColorVerticalKey vertical:
                    report?.Approximated("color_gradient_flattened",
                        "Afterbeat has no per-corner object colour; gradients were averaged into one colour.", path);
                    return Average(vertical.Color4Bottom, vertical.Color4Top);

                // The only one with nowhere to go: Afterbeat's gradient has two ends, not four
                // corners, so this is the shape that really is averaged away.
                case Color4X4Key corners:
                    report?.Approximated("color_corners_flattened",
                        "Afterbeat object colours have two ends, not four corners; a four-corner colour was averaged into one.",
                        path);
                    return Average(Average(corners.Color4BL, corners.Color4BR),
                        Average(corners.Color4TL, corners.Color4TR));
                default:
                    return Color4Value.white;
            }
        }

        // Averaging a ThemeRef with anything has no meaning - a slot index is not a quantity - so a
        // pair that is not both literal answers with whichever half is literal, or white.
        private static IColor4 Average(IColor4 a, IColor4 b)
        {
            var left = a as Color4Value;
            var right = b as Color4Value;
            if (left == null && right == null) return a ?? b ?? Color4Value.white;
            if (left == null) return right;
            if (right == null) return left;

            return new Color4Value(
                (left.R + right.R) * 0.5f,
                (left.G + right.G) * 0.5f,
                (left.B + right.B) * 0.5f,
                (left.A + right.A) * 0.5f);
        }

        #endregion
    }
}
