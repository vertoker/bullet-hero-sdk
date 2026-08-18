using System;
using System.Collections.Generic;
using System.Globalization;
using BH.SDK.Interop.AfterBeat.Models;
using BH.SDK.Models.Data;
using BH.SDK.Models.Values;
using BH.SDK.Rules;

namespace BH.SDK.Interop.AfterBeat
{
    // This is the one part of the two formats that already agreed before either knew about the
    // other: ThemeData's 64 slots are laid out on Project Arrhythmia's own convention, which is
    // what Afterbeat still writes. So the whole mapping is a table of indices, both directions are
    // lossless, and neither needs an approximation.
    //
    // Slot numbers in ThemeData's own comment are 1-BASED; the array is not. Every constant here is
    // the array index, i.e. the documented slot minus one, and that offset is the single easiest
    // thing to get wrong in this file.
    //
    // What does NOT survive is alpha, and only in one direction: .vgt colours are six hex digits
    // with no alpha channel at all, so an import always reads alpha 1 and an export always drops
    // whatever alpha a slot carried. Transparency in Afterbeat is a property of a colour KEYFRAME,
    // not of a theme.

    /// <summary> The 34 colours of a .vgt theme, mapped onto <see cref="ThemeData"/>'s 64 slots. </summary>
    public static class AfterBeatThemeMap
    {
        /// <summary> Answered when a slot is not found - documented as slot 1. </summary>
        public const int FallbackIndex = 0;

        public const int GuiIndex = 1;
        public const int BackgroundIndex = 2;
        public const int PlayerStartIndex = 3;
        public const int TailIndex = 7;
        public const int ObjectStartIndex = 16;
        public const int ParallaxStartIndex = 32;
        public const int EffectStartIndex = 48;

        #region Import

        /// <summary> Reads a whole theme. The id comes from the source's own string id when it has
        /// one (inside a .vgd), and from its name when it does not (a standalone .vgt). </summary>
        public static ThemeData Import(VgtTheme source, InteropReport report = null, string path = null)
        {
            var themeData = new ThemeData(
                AfterBeatIdMap.ToThemeId(string.IsNullOrEmpty(source?.Id) ? source?.Name : source.Id),
                source?.Name ?? string.Empty);

            if (source == null) return themeData;

            Write(themeData, GuiIndex, source.Gui, report, path);
            Write(themeData, FallbackIndex, source.Gui, report, path);
            Write(themeData, BackgroundIndex, source.Background, report, path);
            Write(themeData, TailIndex, source.GuiAccent, report, path);

            for (var i = 0; i < VgtTheme.PlayerCount; i++)
                Write(themeData, PlayerStartIndex + i, source.GetPlayer(i), report, path);
            for (var i = 0; i < VgtTheme.ObjectCount; i++)
                Write(themeData, ObjectStartIndex + i, source.GetObject(i), report, path);
            for (var i = 0; i < VgtTheme.ParallaxCount; i++)
                Write(themeData, ParallaxStartIndex + i, source.GetParallax(i), report, path);
            for (var i = 0; i < VgtTheme.EffectCount; i++)
                Write(themeData, EffectStartIndex + i, source.GetEffect(i), report, path);

            return themeData;
        }

        // The fallback slot deliberately takes the GUI colour rather than staying white: it is what
        // an unresolvable index renders as, and a level whose theme is dark reads a white fallback
        // as a bug rather than as a missing reference.
        private static void Write(ThemeData target, int index, string hex,
            InteropReport report, string path)
        {
            if (string.IsNullOrEmpty(hex)) return;
            if (index < 0 || index >= ValueRules.ThemeCount) return;

            if (!TryParseHex(hex, out var color))
            {
                report?.Approximated("theme_color_unreadable",
                    $"Theme colour '{hex}' is not six hex digits; that slot keeps its default.", path);
                return;
            }

            target.Matrix[index] = color;
        }

        #endregion

        #region Export

        /// <summary> Writes a theme back. Alpha is dropped - the target format has no channel for it. </summary>
        public static VgtTheme Export(ThemeData source, string id,
            InteropReport report = null, string path = null)
        {
            var theme = new VgtTheme
            {
                Id = id ?? string.Empty,
                Name = source?.Name ?? string.Empty,
            };

            if (source?.Matrix == null) return theme;

            theme.Gui = FormatHex(Read(source, GuiIndex));
            theme.Background = FormatHex(Read(source, BackgroundIndex));
            theme.GuiAccent = FormatHex(Read(source, TailIndex));

            theme.Players = Collect(source, PlayerStartIndex, VgtTheme.PlayerCount);
            theme.Objects = Collect(source, ObjectStartIndex, VgtTheme.ObjectCount);
            theme.Parallax = Collect(source, ParallaxStartIndex, VgtTheme.ParallaxCount);
            theme.Effects = Collect(source, EffectStartIndex, VgtTheme.EffectCount);

            if (HasTransparency(source))
                report?.Dropped("theme_alpha",
                    "Afterbeat theme colours have no alpha channel; the transparency of those slots is not exported.",
                    path);

            return theme;
        }

        private static List<string> Collect(ThemeData source, int start, int count)
        {
            var list = new List<string>(count);
            for (var i = 0; i < count; i++) list.Add(FormatHex(Read(source, start + i)));
            return list;
        }

        private static Color4Value Read(ThemeData source, int index)
        {
            if (source?.Matrix == null || index < 0 || index >= source.Matrix.Length) return Color4Value.white;
            return source.Matrix[index] ?? Color4Value.white;
        }

        private static bool HasTransparency(ThemeData source)
        {
            foreach (var index in ExportedIndices())
                if (Read(source, index).A < 1f) return true;
            return false;
        }

        // Only the slots the target format actually has a field for - the free bands and the
        // fallback are not exported, so their alpha is not a loss worth reporting.
        private static IEnumerable<int> ExportedIndices()
        {
            yield return GuiIndex;
            yield return BackgroundIndex;
            yield return TailIndex;
            for (var i = 0; i < VgtTheme.PlayerCount; i++) yield return PlayerStartIndex + i;
            for (var i = 0; i < VgtTheme.ObjectCount; i++) yield return ObjectStartIndex + i;
            for (var i = 0; i < VgtTheme.ParallaxCount; i++) yield return ParallaxStartIndex + i;
            for (var i = 0; i < VgtTheme.EffectCount; i++) yield return EffectStartIndex + i;
        }

        #endregion

        #region Hex

        /// <summary> Reads "RRGGBB", with or without a leading '#'. Alpha is always 1. </summary>
        public static bool TryParseHex(string hex, out Color4Value color)
        {
            color = Color4Value.white;
            if (string.IsNullOrEmpty(hex)) return false;

            var span = hex.AsSpan();
            if (span.Length > 0 && span[0] == '#') span = span[1..];
            if (span.Length != 6) return false;

            if (!byte.TryParse(span[..2], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var r)) return false;
            if (!byte.TryParse(span.Slice(2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var g)) return false;
            if (!byte.TryParse(span.Slice(4, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var b)) return false;

            color = new Color4Value(r / 255f, g / 255f, b / 255f, 1f);
            return true;
        }

        /// <summary> Writes "RRGGBB" - six digits, no '#', no alpha, which is what the format holds. </summary>
        public static string FormatHex(Color4Value color)
        {
            if (color == null) return "FFFFFF";
            var r = ToByte(color.R);
            var g = ToByte(color.G);
            var b = ToByte(color.B);
            return $"{r:X2}{g:X2}{b:X2}";
        }

        private static byte ToByte(float channel)
            => (byte)Math.Clamp((int)Math.Round(channel * 255f, MidpointRounding.AwayFromZero), 0, 255);

        #endregion
    }
}
