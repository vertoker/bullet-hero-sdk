using System.Collections.Generic;
using BH.SDK.Interop.AfterBeat.Models;
using BH.SDK.Models;
using BH.SDK.Models.Meta;
using BH.SDK.Models.Primitives;
using BH.SDK.Models.Values;

namespace BH.SDK.Interop.AfterBeat.Import
{
    // .vgm is Afterbeat's metadata document and LevelMeta is this project's - two separate files on
    // both sides, describing the same level. The mapping is mostly plain, and what does not map is
    // interesting in one direction only:
    //
    //   Afterbeat has, and this format does not: Steam Workshop id and visibility, a changelog, a
    //   difficulty rating, a cam-jiggle preference, and the artist's streaming links as a
    //   (service, fragment) pair rather than a URL. The links are the only one worth reconstructing
    //   - a URL is what an Author here holds - so they are, and the rest is reported.
    //
    //   This format has, and Afterbeat does not: a licence, an age rating, content descriptors and
    //   per-resource attribution. An import leaves those unset rather than inventing them, which is
    //   the only honest answer - nobody can infer a licence from a level file.

    /// <summary> .vgm into <see cref="LevelMeta"/>. </summary>
    public static class AfterBeatMetaImporter
    {
        /// <summary> The URL template each Afterbeat link type expands into. </summary>
        public static string BuildArtistUrl(int linkType, string link)
        {
            if (string.IsNullOrEmpty(link)) return string.Empty;

            return (AfterBeatLinkType)linkType switch
            {
                AfterBeatLinkType.Spotify => $"https://open.spotify.com/artist/{link}",
                AfterBeatLinkType.Soundcloud => $"https://soundcloud.com/{link}",
                AfterBeatLinkType.Bandcamp => $"https://{link}.bandcamp.com",
                AfterBeatLinkType.YoutubeMusic => $"https://music.youtube.com/channel/{link}",
                AfterBeatLinkType.Newgrounds => $"https://{link}.newgrounds.com",
                _ => string.Empty,
            };
        }

        public static LevelMeta Import(VgmMeta source, InteropReport report = null, string path = "metadata")
        {
            var meta = new LevelMeta
            {
                LevelId = LevelId.NewId(),
            };

            if (source == null) return meta;

            var song = source.Song ?? new VgmSong();
            meta.LevelName = new StringValue(song.Title ?? string.Empty);
            meta.LevelDescription = new StringValue(song.Description ?? string.Empty);

            meta.LevelAuthors ??= new List<Author>();

            var creator = source.Creator;
            if (!string.IsNullOrEmpty(creator?.SteamName))
                meta.LevelAuthors.Add(new Author(new StringValue(creator.SteamName), string.Empty));

            var artist = source.Artist;
            if (!string.IsNullOrEmpty(artist?.Name))
                meta.LevelAuthors.Add(new Author(new StringValue(artist.Name),
                    BuildArtistUrl(artist.LinkType, artist.Link)));

            ReportUnsupported(source, report, path);
            return meta;
        }

        private static void ReportUnsupported(VgmMeta source, InteropReport report, string path)
        {
            if (report == null) return;

            var beatmap = source.Beatmap;
            if (beatmap != null && beatmap.WorkshopId != 0)
                report.Dropped("meta_workshop",
                    "Afterbeat metadata carries a Steam Workshop id and visibility; this format has no such field and they are not imported.",
                    path);

            if (!string.IsNullOrEmpty(beatmap?.Changelog))
                report.Dropped("meta_changelog",
                    "Afterbeat metadata carries a changelog; this format has no such field and it is not imported.",
                    path);

            if (source.Song != null && source.Song.Difficulty != 0)
                report.Dropped("meta_difficulty",
                    "Afterbeat rates a level's difficulty; this format does not, so the rating is not imported.",
                    path);

            report.Info("meta_licensing_unset",
                "Afterbeat metadata says nothing about licensing, age rating or attribution, so those were left unset rather than guessed. Fill them in before publishing.",
                path);
        }
    }
}
