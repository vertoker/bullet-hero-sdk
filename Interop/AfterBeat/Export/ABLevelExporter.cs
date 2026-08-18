using System;
using System.Collections.Generic;
using BH.SDK.Interop.AfterBeat.Models;
using BH.SDK.Models;
using BH.SDK.Models.Data;
using BH.SDK.Models.Enums;
using BH.SDK.Models.Events;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Keyframes;
using BH.SDK.Models.Objects;
using BH.SDK.Models.Primitives;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using Newtonsoft.Json.Linq;

namespace BH.SDK.Interop.AfterBeat.Export
{
    // Exporting is lossy by construction and there is no way to make it otherwise: this format grew
    // past the one it is writing to. What the exporter owes the author is therefore not fidelity
    // but HONESTY - every one of these ends up in the report rather than being quietly skipped:
    //
    //   particle effects, audio tracks beyond the first, level-authored geometry, anchors, pivots
    //   past the first keyframe, per-corner colours, per-character text effects, random values,
    //   every beat segment after the first, checkpoint spaces other than World, and post-processing
    //   with no Afterbeat counterpart.
    //
    // The single biggest structural gap is AUDIO. An Afterbeat level is one song in a folder, with
    // no track list, no offsets and no effects; this format has a whole mixer. Nothing about that
    // can cross, so the export says so once and moves on.

    /// <summary> A <see cref="Level"/> and its <see cref="LevelMeta"/> back into Afterbeat's
    /// documents. </summary>
    public static class ABLevelExporter
    {
        /// <summary> Everything one export produced. Each document is a separate file on disk, the
        /// same way this project keeps level and metadata apart. </summary>
        public readonly struct Result
        {
            public VgdLevel Level { get; }
            public VgmMeta Meta { get; }
            public InteropReport Report { get; }

            public Result(VgdLevel level, VgmMeta meta, InteropReport report)
            {
                Level = level;
                Meta = meta;
                Report = report;
            }
        }

        public static Result Export(Level level, LevelMeta meta,
            ABOptions options = null, InteropReport report = null)
        {
            report ??= new InteropReport();

            if (level == null)
            {
                report.Failed("level_missing", "There was no level to export.", "level");
                return new Result(null, null, report);
            }

            options = (options ?? new ABOptions(level.Settings.Framerate)).Sanitized();

            // The export's framerate is the LEVEL's, not the caller's preference: frames are being
            // turned back into seconds, and reading them at any other rate retimes the whole level.
            options.Framerate = level.Settings.Framerate;

            var target = new VgdLevel();
            var context = new ABExportContext(options, report, level.Game);

            ExportThemes(level, target, context);
            target.Objects = ABObjectExporter.ExportAll(context, "objects");
            ExportPrefabs(level, target, context);
            ExportPlacements(level, target, context);
            ExportEvents(level, target, context);
            ExportBpm(level, target, context);
            ReportUnsupported(level, report);

            return new Result(target, ABMetaExporter.Export(meta, level, report), report);
        }

        #region Themes and prefabs

        private static void ExportThemes(Level level, VgdLevel target, ABExportContext context)
        {
            foreach (var pair in level.Resources.Themes)
            {
                var theme = pair.Value;
                if (theme == null) continue;

                context.ReferenceTheme ??= theme;
                target.Themes.Add(ABThemeMap.Export(theme, ToThemeSourceId(pair.Key),
                    context.Report, "themes"));
            }
        }

        // Afterbeat names a theme with an arbitrary string, so a Guid's own text is a perfectly good
        // one and is stable across exports of the same level - which is what the theme keyframes
        // referencing it need.
        private static string ToThemeSourceId(ThemeId id) => id.value.ToString("N");

        private static void ExportPrefabs(Level level, VgdLevel target, ABExportContext context)
        {
            foreach (var pair in level.Resources.Prefabs)
            {
                var prefab = pair.Value;
                if (prefab == null) continue;

                var prefabContext = new ABExportContext(context.Options, context.Report, prefab)
                {
                    ReferenceTheme = context.ReferenceTheme,
                };

                target.Prefabs.Add(new VgpPrefab
                {
                    Id = pair.Key.value.ToString("N"),
                    Name = prefab.Name ?? string.Empty,
                    Type = (int)ABPrefabType.Misc1,
                    Objects = ABObjectExporter.ExportAll(prefabContext, "prefab.objs"),
                });
            }
        }

        // Placements are NOT written as prefab_objects, and that is the one structural decision this
        // exporter makes rather than transcribes.
        //
        // A placement draws nothing here by itself: PrefabMaterializer writes real, permanently
        // id'd copies of the template into the level, and those copies are what the objects loop
        // above already exported. Writing the placement as well would hand Afterbeat a second copy
        // of the same content to expand on load, so every prefab in the level would draw twice.
        //
        // Exporting the copies rather than the placements is also the more faithful half: an
        // override (PrefabObject.Modifications) is already baked into the copy it belongs to, while
        // an Afterbeat placement has no way to express one at all.
        private static void ExportPlacements(Level level, VgdLevel target, ABExportContext context)
        {
            var placements = 0;
            var unmaterialized = 0;

            foreach (var pair in level.Game.Objects)
            {
                if (pair.Value is not PrefabObject placement) continue;

                placements++;
                if (placement.ObjectIds is not { Count: > 0 }) unmaterialized++;
            }

            if (placements > 0)
                context.Report.Info("prefabs_flattened",
                    "Prefab placements were exported as the objects they materialize into rather than as placements, so Afterbeat does not draw their content a second time.",
                    "prefab_objects");

            if (unmaterialized > 0)
                context.Report.Dropped("placement_not_materialized",
                    "Some prefab placements have never been materialized, so they own no objects to export and their content is missing from the exported level.",
                    "prefab_objects");
        }

        #endregion

        #region Events

        private static void ExportEvents(Level level, VgdLevel target, ABExportContext context)
        {
            var framerate = context.Options.Framerate;
            var camera = level.Game.CameraEvents;
            var post = level.Game.PostProcessingEvents;
            var events = level.Game.Events;

            target.SetEvents(ABEventTrack.CameraPosition, Map(camera.Positions, framerate, context,
                key =>
                {
                    var (x, y) = ABValueMap.ExportVector(key.Pos, context.Report, "camera");
                    return new List<float> { x, y };
                }));

            // Halved on the way out for the same reason the import doubles it: over there the zoom
            // IS the camera's orthographic size, here it is the whole visible height.
            target.SetEvents(ABEventTrack.CameraZoom, Map(camera.Zooms, framerate, context,
                key => new List<float>
                {
                    Import.ABEventsImporter.ExportZoomValue(
                        ABValueMap.ExportFloat(key.Zoom, context.Report, "camera")),
                }));

            target.SetEvents(ABEventTrack.CameraRotation, Map(camera.Rotations, framerate, context,
                key => new List<float>
                {
                    ABValueMap.ExportFloat(key.Angle, context.Report, "camera")
                    * ABValueMap.RadiansToDegrees,
                }));

            target.SetEvents(ABEventTrack.CameraShake, Map(camera.Shakes, framerate, context,
                key => new List<float> { key.Intensity }));

            target.SetEvents(ABEventTrack.Theme, MapThemes(events.Themes, framerate, context));

            // Each of these undoes exactly what the import did - see ABPostProcessingMap.
            target.SetEvents(ABEventTrack.Chromatic, Map(post.Chromatics, framerate, context,
                key => new List<float>
                {
                    ABPostProcessingMap.ExportChromatic(key.Intensity), 0f,
                }));

            target.SetEvents(ABEventTrack.Bloom, Map(post.Blooms, framerate, context,
                key => new List<float>
                {
                    ABPostProcessingMap.ExportBloomIntensity(key.Intensity),
                    ABPostProcessingMap.ExportBloomScatter(key.Scatter),
                    EffectIndex(key.Color4, context),
                }));

            target.SetEvents(ABEventTrack.Vignette, Map(post.Vignettes, framerate, context,
                key =>
                {
                    var (centerX, centerY) = ABValueMap.ExportVector(key.Center, context.Report, "vignette");
                    return new List<float>
                    {
                        ABPostProcessingMap.ExportVignetteIntensity(key.Intensity),
                        ABPostProcessingMap.ExportVignetteSmoothness(key.Smoothness),
                        key.Rounded ? 1f : 0f, 0f,
                        centerX, centerY, EffectIndex(key.Color4, context),
                    };
                }));

            target.SetEvents(ABEventTrack.LensDistortion, Map(post.Lenses, framerate, context,
                key =>
                {
                    var (centerX, centerY) = ABValueMap.ExportVector(key.Center, context.Report, "lens");
                    return new List<float>
                    {
                        ABPostProcessingMap.ExportLensIntensity(key.Intensity),
                        ABPostProcessingMap.ExportLensCenter(centerX),
                        ABPostProcessingMap.ExportLensCenter(centerY),
                    };
                }));

            // ev is [Intensity, unused, Size, Mix]. This format's grain has neither of the last
            // two, and writing them as zero authors a grain with no grains in it - so they get the
            // source format's own middle-of-the-road values instead.
            target.SetEvents(ABEventTrack.Grain, Map(post.Grains, framerate, context,
                key => new List<float>
                {
                    ABPostProcessingMap.ExportGrainIntensity(key.Intensity), 0f,
                    DefaultExportedGrainSize, DefaultExportedGrainMix,
                }));

            // ev is [Intensity, Width, Speed]. The last two have no counterpart here and no reader
            // there either (Afterbeat leaves both unimplemented), but a keyframe shaped like every
            // other one is what its parser expects.
            target.SetEvents(ABEventTrack.Glitch, Map(post.DigitalGlitches, framerate, context,
                key => new List<float>
                {
                    ABPostProcessingMap.ExportGlitchIntensity(key.Intensity),
                    DefaultExportedGlitchWidth, DefaultExportedGlitchSpeed,
                }));

            target.SetEvents(ABEventTrack.Hue, Map(post.ColorCurveses, framerate, context,
                key => new List<float>
                {
                    ABPostProcessingMap.ExportHue(key.HueVsHue), 0f, 0f,
                }));

            // Afterbeat's force track is a flat direction, so the directional half crosses and the
            // radial one (VelocityPoints) has nowhere to go.
            target.SetEvents(ABEventTrack.PlayerForce, Map(level.Game.PlayerEvents.Velocities,
                framerate, context, key =>
                {
                    var (x, y) = ABValueMap.ExportVector(key.Force, context.Report, "player_force");
                    return new List<float> { x, y, 0f };
                }));

            if (level.Game.PlayerEvents.VelocityPoints is { Count: > 0 })
                context.Report.Dropped("velocity_points",
                    "Afterbeat pushes the player in one flat direction; the points that push or pull from a place have no equivalent and are not exported.",
                    "player_events");

            ExportMarkersAndCheckpoints(level, target, context);
        }

        /// <summary> Grain size Afterbeat is given when this format has none to give. Its own
        /// authoring default - a grain of zero size is a grain nobody can see. </summary>
        public const float DefaultExportedGrainSize = 1f;

        /// <summary> How much of the grain Afterbeat mixes in when this format says nothing. </summary>
        public const float DefaultExportedGrainMix = 1f;

        /// <summary> Glitch width and speed Afterbeat is given when this format has neither. </summary>
        public const float DefaultExportedGlitchWidth = 1f;
        public const float DefaultExportedGlitchSpeed = 1f;

        private static float EffectIndex(IColor4 color, ABExportContext context)
        {
            var (index, _) = ABColorMap.Export(color, ABPalette.Effects,
                context.ReferenceTheme, context.Report, "events");
            return index;
        }

        private static List<VgdEventKeyframe> Map<T>(List<T> keys, int framerate,
            ABExportContext context, Func<T, List<float>> read) where T : IKeyframe
        {
            var exported = new List<VgdEventKeyframe>();
            if (keys == null) return exported;

            var sorted = new List<T>(keys);
            sorted.Sort((a, b) => a.Frame.CompareTo(b.Frame));

            foreach (var key in sorted)
            {
                var values = read(key);
                if (values == null) continue;

                exported.Add(new VgdEventKeyframe
                {
                    Time = ABTimeMap.ToSeconds(key.Frame, framerate),
                    Ease = ABEaseMap.Export(key.Ease, context.Report, "events"),
                    Values = JArray.FromObject(values),
                });
            }

            return exported;
        }

        private static List<VgdEventKeyframe> MapThemes(List<ThemeKeyframe> keys,
            int framerate, ABExportContext context)
        {
            var exported = new List<VgdEventKeyframe>();
            if (keys == null) return exported;

            var sorted = new List<ThemeKeyframe>(keys);
            sorted.Sort((a, b) => a.Frame.CompareTo(b.Frame));

            foreach (var key in sorted)
            {
                var exportedKey = new VgdEventKeyframe
                {
                    Time = ABTimeMap.ToSeconds(key.Frame, framerate),
                    Ease = ABEaseMap.Export(key.Ease, context.Report, "events"),
                };
                exportedKey.SetString(ToThemeSourceId(key.ThemeId));
                exported.Add(exportedKey);
            }

            return exported;
        }

        private static void ExportMarkersAndCheckpoints(Level level, VgdLevel target,
            ABExportContext context)
        {
            var framerate = context.Options.Framerate;
            var events = level.Game.Events;

            foreach (var marker in events.Markers)
                target.Markers.Add(new VgdMarker
                {
                    Id = marker.Frame.ToString(),
                    Name = marker.Name ?? string.Empty,
                    Description = marker.Description ?? string.Empty,
                    Time = ABTimeMap.ToSeconds(marker.Frame, framerate),
                });

            var spacedCheckpoint = false;
            foreach (var checkpoint in events.Checkpoints)
            {
                if (checkpoint.Space != CheckpointSpace.World) spacedCheckpoint = true;

                var (x, y) = ABValueMap.ExportVector(checkpoint.Position, context.Report, "checkpoints");
                target.Checkpoints.Add(new VgdCheckpoint
                {
                    Id = checkpoint.Frame.ToString(),
                    Name = checkpoint.Name ?? string.Empty,
                    Time = ABTimeMap.ToSeconds(checkpoint.Frame, framerate),
                    Position = new VgdVector2(x, y),
                });
            }

            if (spacedCheckpoint)
                context.Report.Approximated("checkpoint_space",
                    "Afterbeat respawn positions are always in world space; checkpoints using a camera-relative space export as if they were world-space.",
                    "checkpoints");
        }

        // Afterbeat carries ONE tempo for the whole level, in its editor block. The first segment is
        // the one exported because it is the one an author hears first; every other segment is a
        // tempo the exported file cannot change to.
        private static void ExportBpm(Level level, VgdLevel target, ABExportContext context)
        {
            var beats = level.Game.Events.Beats;
            if (beats == null || beats.Count == 0) return;

            var first = beats[0];
            target.Editor.Bpm.Value = first.Bpm;
            target.Editor.Bpm.ValueDuplicate = first.Bpm;
            target.Editor.Bpm.Offset = ABTimeMap.ToSeconds(first.Span.StartFrame, context.Options.Framerate)
                                       + first.Offset / context.Options.Framerate;

            if (beats.Count > 1)
                context.Report.Dropped("beats_single_tempo",
                    "Afterbeat stores one tempo per level; only the first beat segment was exported.",
                    "beats");
        }

        #endregion

        private static void ReportUnsupported(Level level, InteropReport report)
        {
            if (level.Audio?.Tracks is { Count: > 0 })
                report.Dropped("audio_tracks",
                    "An Afterbeat level is one song file in its folder - it has no track list, offsets, speeds or audio effects, so none of this level's audio setup is exported. Put the song in the exported folder by hand.",
                    "audio");

            if (level.Resources.Effects is { Count: > 0 })
                report.Dropped("effect_resources",
                    "Afterbeat has no particle effects; effect resources are not exported.", "resources.effects");

            if (level.Resources.CompositeShapes is { Count: > 0 })
                report.Dropped("shape_resources",
                    "Afterbeat has a fixed shape library; level-authored geometry is not exported and those objects fall back to a square.",
                    "resources.shapes");

            if (level.Resources.Textures is { Count: > 0 })
                report.Dropped("texture_resources",
                    "Afterbeat objects carry no texture; images used by this level are not exported.",
                    "resources.textures");

            ReportPostProcessing(level, report);
            ReportSilentDrops(level, report);
        }

        // Everything below has no Afterbeat counterpart AND no conversion that could have been got
        // wrong, which is exactly why none of it used to be reported: an export that says nothing
        // reads as an export that lost nothing. Each of these is content an author authored.
        private static void ReportSilentDrops(Level level, InteropReport report)
        {
            var events = level.Game.Events;
            var player = level.Game.PlayerEvents;

            if (events.ScreenLimits is { Count: > 0 })
                report.Dropped("screen_limits",
                    "Afterbeat has no playable-area limits; this level's screen limit track is not exported.",
                    "events");

            if (events.Backgrounds is { Count: > 0 })
                report.Dropped("backgrounds",
                    "Afterbeat takes its background colour from the active theme rather than from a track of its own, so this level's background track is not exported.",
                    "events");

            // Velocities are NOT on this list - they have Afterbeat's own force track to go to.
            if (player.Visibles is { Count: > 0 } || player.Controls is { Count: > 0 }
                || player.Collisions is { Count: > 0 })
                report.Dropped("player_events",
                    "Afterbeat levels cannot hide the player, take control away or turn collision off; those tracks are not exported.",
                    "player_events");

            if (level.Settings.Seed != LevelRules.NullSeed)
                report.Dropped("seed",
                    "Afterbeat resolves randomness as it plays rather than from a seed; this level's seed is not exported and its random values were resolved to their midpoints.",
                    "settings");

            if (HasColouredMarkerOrCheckpoint(level))
                report.Dropped("event_colors",
                    "Afterbeat picks a marker's colour from its own editor palette and gives a checkpoint none at all, so the colours set here are not exported.",
                    "events");

            foreach (var checkpoint in events.Checkpoints)
            {
                if (checkpoint == null || checkpoint.Active) continue;
                report.Dropped("checkpoint_inactive",
                    "Afterbeat checkpoints are always active; a checkpoint turned off here exports as an ordinary one.",
                    "checkpoints");
                break;
            }

            if (level.Resources.Prefabs is { Count: > 0 })
                report.Info("prefab_type",
                    "Afterbeat sorts prefabs into categories this format does not have; every exported prefab is written as Misc 1.",
                    "prefabs");
        }

        // Level.Hints is advisory by definition - a cache of what the level needs, rebuilt on load -
        // so it is the one thing on this list that is not worth a line in a report an author reads.
        private static bool HasColouredMarkerOrCheckpoint(Level level)
        {
            foreach (var marker in level.Game.Events.Markers)
                if (marker != null && IsColoured(marker.Color4)) return true;

            foreach (var checkpoint in level.Game.Events.Checkpoints)
                if (checkpoint != null && IsColoured(checkpoint.Color4)) return true;

            return false;
        }

        // Anything that is not a plain white literal carries a decision the export cannot take with
        // it - including a theme reference, which has no colour of its own to compare.
        private static bool IsColoured(IColor4 color)
            => color is not Color4Value literal || !literal.Equals(Color4Value.white);

        private static void ReportPostProcessing(Level level, InteropReport report)
        {
            var post = level.Game.PostProcessingEvents;
            var unsupported =
                post.MotionBlurs is { Count: > 0 } ||
                post.LiftGammaGains is { Count: > 0 } ||
                post.ShadowsMidtonesHighlightses is { Count: > 0 } ||
                post.WhiteBalances is { Count: > 0 };

            if (unsupported)
                report.Dropped("postprocessing_unsupported",
                    "Motion blur, lift/gamma/gain, shadows/midtones/highlights and white balance have no Afterbeat equivalent and are not exported.",
                    "postprocessing");

            // Colour curves DO have a destination - Afterbeat's hue track - but only half of one:
            // it rotates hue and knows nothing about saturation.
            if (post.ColorCurveses is { Count: > 0 })
                report.Approximated("color_curves_hue_only",
                    "Afterbeat has a hue rotation but no saturation curve; only the hue half of this level's colour curves was exported.",
                    "postprocessing");

            if (post.AnalogGlitches is { Count: > 0 })
                report.Approximated("glitch_merged",
                    "Afterbeat has one glitch effect; the analog and digital tracks were merged into it, keeping the digital one's intensity.",
                    "postprocessing");
        }
    }
}
