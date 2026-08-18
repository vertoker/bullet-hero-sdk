using System.Collections.Generic;
using Newtonsoft.Json;

namespace BH.SDK.Interop.AfterBeat.Models
{
    // Every colour here is a bare "RRGGBB" string with NO alpha channel. Transparency in this
    // format is per-keyframe (a colour keyframe's second value) rather than per-theme, which is why
    // importing a theme can never lose alpha and exporting one can never carry it.
    //
    // The four arrays have fixed lengths the format guarantees - 4 players, 9 objects, 9 effects,
    // 9 parallax. They are read through the indexers below rather than directly so a short array in
    // a hand-edited file reads as "missing colour" instead of throwing.

    /// <summary> An Afterbeat theme - a whole .vgt file, or one entry of .vgd themes[]. </summary>
    public class VgtTheme : AfterBeatNode
    {
        public const int PlayerCount = 4;
        public const int ObjectCount = 9;
        public const int EffectCount = 9;
        public const int ParallaxCount = 9;

        /// <summary> Present only inside .vgd themes[]; a standalone .vgt has no id. </summary>
        [JsonProperty(AfterBeatNames.ThemeId)]
        public string Id { get; set; } = string.Empty;

        [JsonProperty(AfterBeatNames.ThemeName)]
        public string Name { get; set; } = string.Empty;

        [JsonProperty(AfterBeatNames.ThemeBackground)]
        public string Background { get; set; } = string.Empty;

        [JsonProperty(AfterBeatNames.ThemeGui)]
        public string Gui { get; set; } = string.Empty;

        /// <summary> GUI accents and the player's tail - one colour serving both. </summary>
        [JsonProperty(AfterBeatNames.ThemeGuiAccent)]
        public string GuiAccent { get; set; } = string.Empty;

        [JsonProperty(AfterBeatNames.ThemePlayers)]
        public List<string> Players { get; set; } = new();

        [JsonProperty(AfterBeatNames.ThemeObjects)]
        public List<string> Objects { get; set; } = new();

        [JsonProperty(AfterBeatNames.ThemeEffects)]
        public List<string> Effects { get; set; } = new();

        [JsonProperty(AfterBeatNames.ThemeParallax)]
        public List<string> Parallax { get; set; } = new();

        public string GetPlayer(int index) => Read(Players, index);
        public string GetObject(int index) => Read(Objects, index);
        public string GetEffect(int index) => Read(Effects, index);
        public string GetParallax(int index) => Read(Parallax, index);

        private static string Read(List<string> list, int index)
            => list != null && index >= 0 && index < list.Count ? list[index] : null;
    }
}
