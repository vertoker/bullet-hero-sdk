using Newtonsoft.Json;

namespace BH.SDK.Interop.AfterBeat.Models
{
    // Four of song's fields (bpm, time, preview_start, preview_length) are documented as "possibly
    // unused, always <constant>". They are transcribed anyway so a round trip does not drop them,
    // and they are read by nothing: the level's real tempo lives in .vgd editor.bpm, not here.

    /// <summary> An Afterbeat level's metadata file - .vgm. A separate document from .vgd, exactly
    /// like this project's own metadata.json is separate from level.json. </summary>
    public class VgmMeta : AfterBeatNode
    {
        [JsonProperty(AfterBeatNames.MetaBeatmap)]
        public VgmBeatmap Beatmap { get; set; } = new();

        [JsonProperty(AfterBeatNames.MetaCreator)]
        public VgmCreator Creator { get; set; } = new();

        [JsonProperty(AfterBeatNames.MetaSong)]
        public VgmSong Song { get; set; } = new();

        [JsonProperty(AfterBeatNames.MetaArtist)]
        public VgmArtist Artist { get; set; } = new();

        [JsonProperty(AfterBeatNames.MetaReferences)]
        public VgmReferences References { get; set; } = new();
    }

    /// <summary> The level itself and its Steam Workshop listing. </summary>
    public class VgmBeatmap : AfterBeatNode
    {
        /// <summary> "YYYY-MM-DD_HH.MM.SS", in the saving machine's own local time. </summary>
        [JsonProperty(AfterBeatNames.MetaDateEdited)]
        public string DateEdited { get; set; } = string.Empty;

        /// <summary> Version of Afterbeat that last wrote the level. The nearest thing this format
        /// has to a format version - there is no other. </summary>
        [JsonProperty(AfterBeatNames.MetaGameVersion)]
        public string GameVersion { get; set; } = string.Empty;

        [JsonProperty(AfterBeatNames.MetaWorkshopId)]
        public long WorkshopId { get; set; }

        [JsonProperty(AfterBeatNames.MetaVisibility)]
        public int Visibility { get; set; }

        [JsonProperty(AfterBeatNames.MetaChangelog)]
        public string Changelog { get; set; } = string.Empty;
    }

    /// <summary> Who made the level, as Steam knows them. </summary>
    public class VgmCreator : AfterBeatNode
    {
        [JsonProperty(AfterBeatNames.MetaSteamName)]
        public string SteamName { get; set; } = string.Empty;

        [JsonProperty(AfterBeatNames.MetaSteamId)]
        public long SteamId { get; set; }
    }

    /// <summary> The song, plus a few level-wide settings that ended up here. </summary>
    public class VgmSong : AfterBeatNode
    {
        [JsonProperty(AfterBeatNames.MetaSongTitle)]
        public string Title { get; set; } = string.Empty;

        /// <summary> Description of the LEVEL, despite living under "song". </summary>
        [JsonProperty(AfterBeatNames.MetaSongDescription)]
        public string Description { get; set; } = string.Empty;

        [JsonProperty(AfterBeatNames.MetaSongDifficulty)]
        public int Difficulty { get; set; }

        [JsonProperty(AfterBeatNames.MetaSongBpm)]
        public float Bpm { get; set; } = 140f;

        [JsonProperty(AfterBeatNames.MetaSongTime)]
        public float Time { get; set; } = 60f;

        [JsonProperty(AfterBeatNames.MetaSongPreviewStart)]
        public float PreviewStart { get; set; } = -1f;

        [JsonProperty(AfterBeatNames.MetaSongPreviewLength)]
        public float PreviewLength { get; set; } = -1f;

        [JsonProperty(AfterBeatNames.MetaSongCamJiggle)]
        public int CamJiggle { get; set; }
    }

    /// <summary> Who made the song, and where to hear more of it. </summary>
    public class VgmArtist : AfterBeatNode
    {
        [JsonProperty(AfterBeatNames.MetaArtistName)]
        public string Name { get; set; } = string.Empty;

        [JsonProperty(AfterBeatNames.MetaArtistLinkType)]
        public int LinkType { get; set; }

        /// <summary> A FRAGMENT, not a URL - the service's own template turns it into one. </summary>
        [JsonProperty(AfterBeatNames.MetaArtistLink)]
        public string Link { get; set; } = string.Empty;
    }

    /// <summary> External works the level points at. </summary>
    public class VgmReferences : AfterBeatNode
    {
        [JsonProperty(AfterBeatNames.MetaReferenceGame)]
        public VgmGameReference Game { get; set; } = new();
    }

    /// <summary> Which game the song came from. </summary>
    public class VgmGameReference : AfterBeatNode
    {
        [JsonProperty(AfterBeatNames.MetaReferenceGameId)]
        public int Id { get; set; }

        /// <summary> Used only when <see cref="Id"/> is Custom. </summary>
        [JsonProperty(AfterBeatNames.MetaReferenceGameCustom)]
        public string Custom { get; set; } = string.Empty;
    }
}
