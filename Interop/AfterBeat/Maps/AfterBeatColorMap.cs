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
    // first effect colour inside a bloom keyframe. AfterBeatPalette is that context.

    /// <summary> Which of Afterbeat's four colour palettes an index is read against. </summary>
    public enum AfterBeatPalette
    {
        Objects = 0,
        Parallax = 1,
        Effects = 2,
        Players = 3,
    }

    /// <summary> Afterbeat's (palette index, opacity) pairs, mapped onto this format's colours. </summary>
    public static class AfterBeatColorMap
    {
        /// <summary> Above this an opacity counts as fully opaque. One 8-bit step is well below
        /// anything an author can see, and an exactly-1.0 comparison would send a colour authored
        /// as 0.999 down the literal path for no visible reason. </summary>
        public const float OpaqueEpsilon = 1f / 512f;

        /// <summary> First <see cref="ThemeData"/> slot of one of Afterbeat's palettes. </summary>
        public static int GetBaseIndex(AfterBeatPalette palette) => palette switch
        {
            AfterBeatPalette.Objects => AfterBeatThemeMap.ObjectStartIndex,
            AfterBeatPalette.Parallax => AfterBeatThemeMap.ParallaxStartIndex,
            AfterBeatPalette.Effects => AfterBeatThemeMap.EffectStartIndex,
            AfterBeatPalette.Players => AfterBeatThemeMap.PlayerStartIndex,
            _ => AfterBeatThemeMap.ObjectStartIndex,
        };

        /// <summary> How many colours a palette holds. </summary>
        public static int GetCount(AfterBeatPalette palette)
            => palette == AfterBeatPalette.Players ? VgtTheme.PlayerCount : VgtTheme.ObjectCount;

        /// <summary> A palette index as a slot of the 64. Out-of-range indices clamp into the
        /// palette rather than reaching a neighbouring one - a bad index should read as the wrong
        /// object colour, never as an effect colour. </summary>
        public static int ToThemeIndex(int paletteIndex, AfterBeatPalette palette)
        {
            var baseIndex = GetBaseIndex(palette);
            var count = GetCount(palette);
            var clamped = Math.Clamp(paletteIndex, 0, count - 1);
            return Math.Clamp(baseIndex + clamped, ValueRules.MinThemeIndex, ValueRules.MaxThemeIndex);
        }

        /// <summary> The reverse - a slot of the 64 back to its palette index, or -1 when the slot
        /// is outside that palette. </summary>
        public static int ToPaletteIndex(int themeIndex, AfterBeatPalette palette)
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
        public static IColor4 Import(int paletteIndex, float opacity, AfterBeatPalette palette,
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
        public static (int Index, float Opacity) Export(IColor4 color, AfterBeatPalette palette,
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
        private static int MatchNearest(Color4Value color, AfterBeatPalette palette,
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
                case Color4X4Key corners:
                    report?.Approximated("color_gradient_flattened",
                        "Afterbeat has no per-corner object colour; gradients were averaged into one colour.", path);
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
