using Newtonsoft.Json;

namespace BH.SDK.Interop.AfterBeat.Models
{
    // Four of song's fields (bpm, time, preview_start, preview_length) are documented as "possibly
    // unused, always <constant>". They are transcribed anyway so a round trip does not drop them,
    // and they are read by nothing: the level's real tempo lives in .vgd editor.bpm, not here.

    /// <summary> An Afterbeat level's metadata file - .vgm. A separate document from .vgd, exactly
    /// like this project's own metadata.json is separate from level.json. </summary>
    public class VgmMeta : ABNode
    {
        [JsonProperty(ABNames.MetaBeatmap)]
        public VgmBeatmap Beatmap { get; set; } = new();

        [JsonProperty(ABNames.MetaCreator)]
        public VgmCreator Creator { get; set; } = new();

        [JsonProperty(ABNames.MetaSong)]
        public VgmSong Song { get; set; } = new();

        [JsonProperty(ABNames.MetaArtist)]
        public VgmArtist Artist { get; set; } = new();

        [JsonProperty(ABNames.MetaReferences)]
        public VgmReferences References { get; set; } = new();
    }

    /// <summary> The level itself and its Steam Workshop listing. </summary>
    public class VgmBeatmap : ABNode
    {
        /// <summary> "YYYY-MM-DD_HH.MM.SS", in the saving machine's own local time. </summary>
        [JsonProperty(ABNames.MetaDateEdited)]
        public string DateEdited { get; set; } = string.Empty;

        /// <summary> Version of Afterbeat that last wrote the level. The nearest thing this format
        /// has to a format version - there is no other. </summary>
        [JsonProperty(ABNames.MetaGameVersion)]
        public string GameVersion { get; set; } = string.Empty;

        [JsonProperty(ABNames.MetaWorkshopId)]
        public long WorkshopId { get; set; }

        [JsonProperty(ABNames.MetaVisibility)]
        public int Visibility { get; set; }

        [JsonProperty(ABNames.MetaChangelog)]
        public string Changelog { get; set; } = string.Empty;
    }

    /// <summary> Who made the level, as Steam knows them. </summary>
    public class VgmCreator : ABNode
    {
        [JsonProperty(ABNames.MetaSteamName)]
        public string SteamName { get; set; } = string.Empty;

        [JsonProperty(ABNames.MetaSteamId)]
        public long SteamId { get; set; }
    }

    /// <summary> The song, plus a few level-wide settings that ended up here. </summary>
    public class VgmSong : ABNode
    {
        [JsonProperty(ABNames.MetaSongTitle)]
        public string Title { get; set; } = string.Empty;

        /// <summary> Description of the LEVEL, despite living under "song". </summary>
        [JsonProperty(ABNames.MetaSongDescription)]
        public string Description { get; set; } = string.Empty;

        [JsonProperty(ABNames.MetaSongDifficulty)]
        public int Difficulty { get; set; }

        [JsonProperty(ABNames.MetaSongBpm)]
        public float Bpm { get; set; } = 140f;

        [JsonProperty(ABNames.MetaSongTime)]
        public float Time { get; set; } = 60f;

        [JsonProperty(ABNames.MetaSongPreviewStart)]
        public float PreviewStart { get; set; } = -1f;

        [JsonProperty(ABNames.MetaSongPreviewLength)]
        public float PreviewLength { get; set; } = -1f;

        [JsonProperty(ABNames.MetaSongCamJiggle)]
        public int CamJiggle { get; set; }
    }

    /// <summary> Who made the song, and where to hear more of it. </summary>
    public class VgmArtist : ABNode
    {
        [JsonProperty(ABNames.MetaArtistName)]
        public string Name { get; set; } = string.Empty;

        [JsonProperty(ABNames.MetaArtistLinkType)]
        public int LinkType { get; set; }

        /// <summary> A FRAGMENT, not a URL - the service's own template turns it into one. </summary>
        [JsonProperty(ABNames.MetaArtistLink)]
        public string Link { get; set; } = string.Empty;
    }

    /// <summary> External works the level points at. </summary>
    public class VgmReferences : ABNode
    {
        [JsonProperty(ABNames.MetaReferenceGame)]
        public VgmGameReference Game { get; set; } = new();
    }

    /// <summary> Which game the song came from. </summary>
    public class VgmGameReference : ABNode
    {
        [JsonProperty(ABNames.MetaReferenceGameId)]
        public int Id { get; set; }

        /// <summary> Used only when <see cref="Id"/> is Custom. </summary>
        [JsonProperty(ABNames.MetaReferenceGameCustom)]
        public string Custom { get; set; } = string.Empty;
    }
}
