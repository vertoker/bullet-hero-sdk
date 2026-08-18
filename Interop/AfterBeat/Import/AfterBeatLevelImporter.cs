using System;
using BH.SDK.Interop.AfterBeat.Models;
using BH.SDK.Models;
using BH.SDK.Rules;

namespace BH.SDK.Interop.AfterBeat.Import
{
    // The whole level, in the order the pieces depend on each other:
    //
    //   themes first, because every colour anywhere in the level is resolved against one;
    //   then the level's own length, because parallax and prefab placements are spanned by it;
    //   then prefab templates, because a placement references one;
    //   then objects, parallax and placements, which all write into the same scope;
    //   then the level-global event tracks, which reference themes by the ids step one derived.
    //
    // The length itself is MEASURED rather than read: Afterbeat stores no level length at all - a
    // level is as long as its song, and the song is a file this library cannot open. So the
    // timeline is stretched to the last frame anything in the file reaches, and the host is free to
    // stretch it further once it knows the clip's real duration.

    /// <summary> A whole .vgd (plus its .vgm) into a <see cref="Level"/> and a <see cref="LevelMeta"/>. </summary>
    public static class AfterBeatLevelImporter
    {
        /// <summary> Padding added past the last frame anything reaches, so the level does not end
        /// on the exact frame its final object dies. </summary>
        public const int TailFrames = 60;

        /// <summary> The result of one import: the two documents plus everything given up on the
        /// way. </summary>
        public readonly struct Result
        {
            public Level Level { get; }
            public LevelMeta Meta { get; }
            public InteropReport Report { get; }

            public Result(Level level, LevelMeta meta, InteropReport report)
            {
                Level = level;
                Meta = meta;
                Report = report;
            }
        }

        /// <summary> Reads the two documents as text. A document that is not JSON at all is
        /// reported as a failure rather than thrown, so one unreadable metadata file does not cost
        /// the level. </summary>
        public static Result ImportJson(string levelJson, string metaJson,
            AfterBeatOptions options = null, InteropReport report = null)
        {
            report ??= new InteropReport();

            if (!AfterBeatSerialization.TryDeserialize<VgdLevel>(levelJson, out var source, out var error))
            {
                report.Failed("level_unreadable", $"The .vgd could not be read: {error}", "level");
                return new Result(null, null, report);
            }

            VgmMeta meta = null;
            if (!string.IsNullOrWhiteSpace(metaJson)
                && !AfterBeatSerialization.TryDeserialize<VgmMeta>(metaJson, out meta, out var metaError))
                report.Failed("meta_unreadable",
                    $"The metadata document could not be read: {metaError}. The level was imported without it.",
                    "metadata");

            return Import(source, meta, options, report);
        }

        /// <summary> Reads two already-parsed documents. </summary>
        public static Result Import(VgdLevel source, VgmMeta meta,
            AfterBeatOptions options = null, InteropReport report = null)
        {
            report ??= new InteropReport();
            options = (options ?? new AfterBeatOptions()).Sanitized();

            if (source == null)
            {
                report.Failed("level_missing", "There was no level document to import.", "level");
                return new Result(null, null, report);
            }

            var level = new Level();
            level.Settings.Framerate = options.Framerate;

            var context = new AfterBeatImportContext(options, report,
                level.Game, level.Settings, level.Resources.CompositeShapes);

            ImportThemes(source, level, context);
            level.Settings.FrameDuration = MeasureDuration(source, options);

            if (options.ImportPrefabs) ImportPrefabs(source, level, context);
            AfterBeatObjectImporter.ImportAll(source.Objects, context, "objects");
            if (options.ImportParallax)
                AfterBeatParallaxImporter.ImportAll(source.Parallax, context,
                    level.Settings.FrameDuration, "parallax_settings");
            if (options.ImportPrefabs) ImportPlacements(source, level, context);

            AfterBeatEventsImporter.ImportAll(source, level, context, "events");
            AfterBeatEventsImporter.ImportEvents(source, level, context, "events");
            AfterBeatEventsImporter.ImportBeats(source, level, context);

            var levelMeta = AfterBeatMetaImporter.Import(meta, report);
            return new Result(level, levelMeta, report);
        }

        #region Steps

        // Themes are read first and the first one becomes the reference every literal colour is
        // resolved against. "First" rather than "the one the theme track starts on" because a
        // colour is resolved once at import while the active theme changes over time - there is no
        // single right answer, and the first is the one an author can predict.
        private static void ImportThemes(VgdLevel source, Level level, AfterBeatImportContext context)
        {
            if (source.Themes == null) return;

            for (var i = 0; i < source.Themes.Count; i++)
            {
                var sourceTheme = source.Themes[i];
                if (sourceTheme == null) continue;

                var theme = AfterBeatThemeMap.Import(sourceTheme, context.Report, $"themes[{i}]");
                level.Resources.Themes[theme.ThemeId] = theme;
                context.ReferenceTheme ??= theme;
            }
        }

        private static void ImportPrefabs(VgdLevel source, Level level, AfterBeatImportContext context)
        {
            if (source.Prefabs == null) return;

            for (var i = 0; i < source.Prefabs.Count; i++)
            {
                var sourcePrefab = source.Prefabs[i];
                if (sourcePrefab == null) continue;

                if (level.Resources.Prefabs.Count >= LevelRules.MaxPrefabs)
                {
                    context.Report.Dropped("prefabs_over_cap",
                        $"This format allows {LevelRules.MaxPrefabs} prefabs per level; the rest were dropped along with their placements.",
                        "prefabs");
                    break;
                }

                var prefab = AfterBeatPrefabImporter.ImportTemplate(sourcePrefab, context.Options,
                    context.Report, level.Resources.CompositeShapes, context.ReferenceTheme, $"prefabs[{i}]");
                if (prefab == null) continue;

                level.Resources.Prefabs[prefab.PrefabId] = prefab;
            }
        }

        // Placements are minted into the LEVEL's scope, after the ordinary objects, so an id table
        // built for the objects cannot collide with them. Their materialized children do not exist
        // yet - see AfterBeatPrefabImporter's header.
        private static void ImportPlacements(VgdLevel source, Level level, AfterBeatImportContext context)
        {
            if (source.PrefabPlacements == null) return;

            for (var i = 0; i < source.PrefabPlacements.Count; i++)
            {
                var sourcePlacement = source.PrefabPlacements[i];
                if (sourcePlacement == null) continue;

                var placement = AfterBeatPrefabImporter.ImportPlacement(sourcePlacement, context,
                    level.Settings.FrameDuration, $"prefab_objects[{i}]");
                if (placement == null) continue;

                level.Game.Objects[placement.ObjectId] = placement;
            }

            if (source.PrefabPlacements.Count > 0)
                context.Report.Info("placements_need_materializing",
                    "Prefab placements were imported as placements. They draw nothing until the editor materializes them, which it does once on load.",
                    "prefab_objects");
        }

        #endregion

        #region Length

        // Afterbeat stores no level length, so it is measured off everything that carries a time:
        // object lifetimes, checkpoints, markers, and every level-global event keyframe. Missing any
        // of those would produce a timeline that ends before content the file still contains.
        private static int MeasureDuration(VgdLevel source, AfterBeatOptions options)
        {
            var seconds = 0f;

            if (source.Objects != null)
                foreach (var obj in source.Objects)
                {
                    if (obj == null) continue;
                    var end = AfterBeatTimeMap.ResolveEndTime(obj);
                    if (end > seconds) seconds = end;
                }

            if (source.Checkpoints != null)
                foreach (var checkpoint in source.Checkpoints)
                    if (checkpoint != null && checkpoint.Time > seconds) seconds = checkpoint.Time;

            if (source.Markers != null)
                foreach (var marker in source.Markers)
                    if (marker != null && marker.Time > seconds) seconds = marker.Time;

            if (source.Events != null)
                foreach (var track in source.Events)
                {
                    if (track == null) continue;
                    foreach (var key in track)
                        if (key != null && key.Time > seconds) seconds = key.Time;
                }

            var frames = AfterBeatTimeMap.ToFrame(seconds, options.Framerate) + TailFrames;
            return Math.Clamp(frames, FrameRules.MinFrameDuration, FrameRules.MaxFrameDuration);
        }

        #endregion
    }
}
