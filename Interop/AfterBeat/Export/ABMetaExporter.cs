using BH.SDK.Interop.AfterBeat.Models;
using BH.SDK.Models;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Values;

namespace BH.SDK.Interop.AfterBeat.Export
{
    // The mirror of ABMetaImporter, and the asymmetry runs the other way here: this format
    // carries a licence, an age rating, content descriptors and per-resource attribution, and
    // Afterbeat's metadata has no field for any of it. That is the one loss worth naming out loud -
    // an exported level arrives somewhere else with its attribution stripped, which is a licensing
    // problem rather than a rendering one.

    /// <summary> <see cref="LevelMeta"/> back into a .vgm. </summary>
    public static class ABMetaExporter
    {
        // game_version IS NOT A LABEL, it is the input to Afterbeat's own upgrade chain, and it is
        // parsed before a single object of the level is read: DataManager.GetVersion strips
        // [a-zA-Z[\]] with a regex, splits the rest on '.', and int.Parse'es the component asked
        // for. A hyphen is not in that character class, so the honest tag this used to write
        // ("bullet-hero-interop") survives the strip as "--" and throws a FormatException on the
        // first int.Parse - inside UpdateBeatmap, which GameManager wraps as "LoadData failed while
        // converting modern beatmap data". Every level this converter has ever exported failed to
        // load in the target game for that one string, with an error naming the beatmap rather than
        // the metadata.
        //
        // So it has to be three numbers, and which three is not free either: UpdateBeatmap gates a
        // dozen textual find-and-replace migrations on being at or below some version, one of which
        // INVERTS every prefab offset in the document below 24.1.4. The only value that runs none
        // of them is the game's own current one, which is also what a level saved by that game
        // carries - there is nothing to be honest about here that the version field can express.

        /// <summary> The Afterbeat build an exported level declares itself as. Must stay three
        /// dot-separated integers, and should track the newest version this converter has been
        /// checked against - see this block's header for why both halves matter. </summary>
        public const string GameVersionTag = "26.6.2";

        public static VgmMeta Export(LevelMeta source, Level level, InteropReport report = null)
        {
            var meta = new VgmMeta();
            meta.Beatmap.GameVersion = GameVersionTag;

            if (source == null) return meta;

            meta.Song.Title = ReadString(source.LevelName);
            meta.Song.Description = ReadString(source.LevelDescription);

            // The first author is the level's creator, matching how the import reads it back; any
            // further ones have nowhere to go, since Afterbeat records exactly one creator and one
            // artist.
            if (source.LevelAuthors is { Count: > 0 })
            {
                meta.Creator.SteamName = ReadString(source.LevelAuthors[0].Name);

                if (source.LevelAuthors.Count > 1)
                {
                    meta.Artist.Name = ReadString(source.LevelAuthors[1].Name);
                    report?.Approximated("meta_authors",
                        "Afterbeat records one creator and one artist; a level with more authors than that exports only the first two.",
                        "metadata");
                }
            }

            ReportUnsupported(source, report);
            return meta;
        }

        private static string ReadString(IString value)
            => value is StringValue literal ? literal.Value ?? string.Empty : string.Empty;

        private static void ReportUnsupported(LevelMeta source, InteropReport report)
        {
            if (report == null) return;

            report.Dropped("meta_licensing",
                "Afterbeat metadata has no licence, age rating, content descriptors or per-resource attribution; all of it is lost on export. Re-attach it wherever the exported level ends up.",
                "metadata");

            if (source.LevelLogo != null && !string.IsNullOrEmpty(source.LevelLogo.Uri))
                report.Info("meta_logo",
                    "Afterbeat reads a level's cover art from a cover.jpg beside the level file rather than from its metadata; a folder export writes one when the level's logo is a .jpg, and otherwise the cover has to be copied there by hand.",
                    "metadata");
        }
    }
}
