using BH.SDK.Interop.AfterBeat.Models;
using BH.SDK.Models;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Values;

namespace BH.SDK.Interop.AfterBeat.Export
{
    // The mirror of AfterBeatMetaImporter, and the asymmetry runs the other way here: this format
    // carries a licence, an age rating, content descriptors and per-resource attribution, and
    // Afterbeat's metadata has no field for any of it. That is the one loss worth naming out loud -
    // an exported level arrives somewhere else with its attribution stripped, which is a licensing
    // problem rather than a rendering one.

    /// <summary> <see cref="LevelMeta"/> back into a .vgm. </summary>
    public static class AfterBeatMetaExporter
    {
        /// <summary> What Afterbeat writes into beatmap.game_version when this converter produced
        /// the file. Honest about where the level came from rather than impersonating a build. </summary>
        public const string GameVersionTag = "bullet-hero-interop";

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
                    "Afterbeat reads a level's cover art from a level.jpg beside the level file rather than from its metadata; copy the logo there by hand.",
                    "metadata");
        }
    }
}
