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
    public class VgtTheme : ABNode
    {
        /// <summary> How many player colours a theme carries. </summary>
        public const int PlayerCount = 4;
        /// <summary> How many object colours it carries - the palette a level's content indexes into. </summary>
        public const int ObjectCount = 9;
        /// <summary> How many effect colours it carries. </summary>
        public const int EffectCount = 9;
        /// <summary> How many background colours it carries. </summary>
        public const int ParallaxCount = 9;

        /// <summary> Present only inside .vgd themes[]; a standalone .vgt has no id. </summary>
        [JsonProperty(ABNames.ThemeId)]
        public string Id { get; set; } = string.Empty;

        /// <summary> Author-facing name of the theme. </summary>
        [JsonProperty(ABNames.ThemeName)]
        public string Name { get; set; } = string.Empty;

        /// <summary> Colour behind everything. </summary>
        [JsonProperty(ABNames.ThemeBackground)]
        public string Background { get; set; } = string.Empty;

        /// <summary> Colour of the in-game interface. </summary>
        [JsonProperty(ABNames.ThemeGui)]
        public string Gui { get; set; } = string.Empty;

        /// <summary> GUI accents and the player's tail - one colour serving both. </summary>
        [JsonProperty(ABNames.ThemeGuiAccent)]
        public string GuiAccent { get; set; } = string.Empty;

        /// <summary> One colour per player. </summary>
        [JsonProperty(ABNames.ThemePlayers)]
        public List<string> Players { get; set; } = new();

        /// <summary> The palette gameplay objects reference by index. </summary>
        [JsonProperty(ABNames.ThemeObjects)]
        public List<string> Objects { get; set; } = new();

        /// <summary> The palette effects reference by index. </summary>
        [JsonProperty(ABNames.ThemeEffects)]
        public List<string> Effects { get; set; } = new();

        /// <summary> The palette background layers reference by index. </summary>
        [JsonProperty(ABNames.ThemeParallax)]
        public List<string> Parallax { get; set; } = new();

        /// <summary> One player colour, answering black for an index the file did not write. </summary>
        public string GetPlayer(int index) => Read(Players, index);
        /// <summary> One object colour, on the same terms. </summary>
        public string GetObject(int index) => Read(Objects, index);
        /// <summary> One effect colour, on the same terms. </summary>
        public string GetEffect(int index) => Read(Effects, index);
        /// <summary> One background colour, on the same terms. </summary>
        public string GetParallax(int index) => Read(Parallax, index);

        private static string Read(List<string> list, int index)
            => list != null && index >= 0 && index < list.Count ? list[index] : null;
    }
}
