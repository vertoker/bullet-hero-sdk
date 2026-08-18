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
using Newtonsoft.Json.Linq;

namespace BH.SDK.Interop.AfterBeat.Export
{
    // Exporting is lossy by construction and there is no way to make it otherwise: this format grew
    // past the one it is writing to. What the exporter owes the author is therefore not fidelity
    // but HONESTY - every one of these ends up in the report rather than being quietly skipped:
    //
    //   particle effects, audio tracks beyond the first, level-authored geometry, anchors, pivots
    //   past the first keyframe, per-corner colours, per-character text effects, random values,
    //   every beat segment after the first, checkpoint spaces other than World, post-processing
    //   with no Afterbeat counterpart, and per-instance prefab overrides.
    //
    // The single biggest structural gap is AUDIO. An Afterbeat level is one song in a folder, with
    // no track list, no offsets and no effects; this format has a whole mixer. Nothing about that
    // can cross, so the export says so once and moves on.

    /// <summary> A <see cref="Level"/> and its <see cref="LevelMeta"/> back into Afterbeat's
    /// documents. </summary>
    public static class AfterBeatLevelExporter
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
            AfterBeatOptions options = null, InteropReport report = null)
        {
            report ??= new InteropReport();

            if (level == null)
            {
                report.Failed("level_missing", "There was no level to export.", "level");
                return new Result(null, null, report);
            }

            options = (options ?? new AfterBeatOptions(level.Settings.Framerate)).Sanitized();

            // The export's framerate is the LEVEL's, not the caller's preference: frames are being
            // turned back into seconds, and reading them at any other rate retimes the whole level.
            options.Framerate = level.Settings.Framerate;

            var target = new VgdLevel();
            var context = new AfterBeatExportContext(options, report, level.Game);

            ExportThemes(level, target, context);
            target.Objects = AfterBeatObjectExporter.ExportAll(context, "objects");
            ExportPrefabs(level, target, context);
            ExportPlacements(level, target, context);
            ExportEvents(level, target, context);
            ExportBpm(level, target, context);
            ReportUnsupported(level, report);

            return new Result(target, AfterBeatMetaExporter.Export(meta, level, report), report);
        }

        #region Themes and prefabs

        private static void ExportThemes(Level level, VgdLevel target, AfterBeatExportContext context)
        {
            foreach (var pair in level.Resources.Themes)
            {
                var theme = pair.Value;
                if (theme == null) continue;

                context.ReferenceTheme ??= theme;
                target.Themes.Add(AfterBeatThemeMap.Export(theme, ToThemeSourceId(pair.Key),
                    context.Report, "themes"));
            }
        }

        // Afterbeat names a theme with an arbitrary string, so a Guid's own text is a perfectly good
        // one and is stable across exports of the same level - which is what the theme keyframes
        // referencing it need.
        private static string ToThemeSourceId(ThemeId id) => id.value.ToString("N");

        private static void ExportPrefabs(Level level, VgdLevel target, AfterBeatExportContext context)
        {
            foreach (var pair in level.Resources.Prefabs)
            {
                var prefab = pair.Value;
                if (prefab == null) continue;

                var prefabContext = new AfterBeatExportContext(context.Options, context.Report, prefab)
                {
                    ReferenceTheme = context.ReferenceTheme,
                };

                target.Prefabs.Add(new VgpPrefab
                {
                    Id = pair.Key.value.ToString("N"),
                    Name = prefab.Name ?? string.Empty,
                    Type = (int)AfterBeatPrefabType.Misc1,
                    Objects = AfterBeatObjectExporter.ExportAll(prefabContext, "prefab.objs"),
                });
            }
        }

        private static void ExportPlacements(Level level, VgdLevel target, AfterBeatExportContext context)
        {
            var hasOverrides = false;

            foreach (var pair in level.Game.Objects)
            {
                if (pair.Value is not PrefabObject placement) continue;

                if (placement.Modifications is { Count: > 0 }) hasOverrides = true;

                var vgd = new VgdPrefabPlacement
                {
                    Id = AfterBeatExportContext.ToSourceId(placement.ObjectId),
                    PrefabId = placement.PrefabId.IsEnabled() ? placement.PrefabId.value.ToString("N") : string.Empty,
                };

                var (x, y) = FirstVector(placement.Positions, 0f, 0f, context);
                var (width, height) = FirstScale(placement.Scales, context);
                var rotation = FirstAngle(placement.Rotations, context);

                vgd.Tracks[VgdPrefabPlacement.TrackIndex.Position].Values = new List<float> { x, y };
                vgd.Tracks[VgdPrefabPlacement.TrackIndex.Scale].Values = new List<float> { width, height };
                vgd.Tracks[VgdPrefabPlacement.TrackIndex.Rotation].Values = new List<float> { rotation };

                target.PrefabPlacements.Add(vgd);
            }

            if (hasOverrides)
                context.Report.Dropped("prefab_modifications",
                    "Afterbeat prefab placements cannot override a template's fields; per-instance overrides are not exported.",
                    "prefab_objects");
        }

        private static (float X, float Y) FirstVector(List<PosKey> keys, float fallbackX, float fallbackY,
            AfterBeatExportContext context)
        {
            if (keys == null || keys.Count == 0) return (fallbackX, fallbackY);
            return AfterBeatValueMap.ExportVector(keys[0].Pos, context.Report, "prefab_objects");
        }

        private static (float Width, float Height) FirstScale(List<ScaKey> keys, AfterBeatExportContext context)
        {
            if (keys == null || keys.Count == 0) return (1f, 1f);
            return AfterBeatValueMap.ExportVector(keys[0].Scale, context.Report, "prefab_objects");
        }

        private static float FirstAngle(List<AngleKey> keys, AfterBeatExportContext context)
        {
            if (keys == null || keys.Count == 0) return 0f;
            return AfterBeatValueMap.ExportFloat(keys[0].Angle, context.Report, "prefab_objects")
                   * AfterBeatValueMap.RadiansToDegrees;
        }

        #endregion

        #region Events

        private static void ExportEvents(Level level, VgdLevel target, AfterBeatExportContext context)
        {
            var framerate = context.Options.Framerate;
            var camera = level.Game.CameraEvents;
            var post = level.Game.PostProcessingEvents;
            var events = level.Game.Events;

            target.SetEvents(AfterBeatEventTrack.CameraPosition, Map(camera.Positions, framerate, context,
                key =>
                {
                    var (x, y) = AfterBeatValueMap.ExportVector(key.Pos, context.Report, "camera");
                    return new List<float> { x, y };
                }));

            target.SetEvents(AfterBeatEventTrack.CameraZoom, Map(camera.Zooms, framerate, context,
                key => new List<float> { AfterBeatValueMap.ExportFloat(key.Zoom, context.Report, "camera") }));

            target.SetEvents(AfterBeatEventTrack.CameraRotation, Map(camera.Rotations, framerate, context,
                key => new List<float>
                {
                    AfterBeatValueMap.ExportFloat(key.Angle, context.Report, "camera")
                    * AfterBeatValueMap.RadiansToDegrees,
                }));

            target.SetEvents(AfterBeatEventTrack.CameraShake, Map(camera.Shakes, framerate, context,
                key => new List<float> { key.Intensity }));

            target.SetEvents(AfterBeatEventTrack.Theme, MapThemes(events.Themes, framerate, context));

            target.SetEvents(AfterBeatEventTrack.Chromatic, Map(post.Chromatics, framerate, context,
                key => new List<float> { key.Intensity, 0f }));

            target.SetEvents(AfterBeatEventTrack.Bloom, Map(post.Blooms, framerate, context,
                key => new List<float> { key.Intensity, key.Scatter, EffectIndex(key.Color4, context) }));

            target.SetEvents(AfterBeatEventTrack.Vignette, Map(post.Vignettes, framerate, context,
                key =>
                {
                    var (centerX, centerY) = AfterBeatValueMap.ExportVector(key.Center, context.Report, "vignette");
                    return new List<float>
                    {
                        key.Intensity, key.Smoothness, key.Rounded ? 1f : 0f, 0f,
                        centerX, centerY, EffectIndex(key.Color4, context),
                    };
                }));

            target.SetEvents(AfterBeatEventTrack.LensDistortion, Map(post.Lenses, framerate, context,
                key =>
                {
                    var (centerX, centerY) = AfterBeatValueMap.ExportVector(key.Center, context.Report, "lens");
                    return new List<float> { key.Intensity, centerX, centerY };
                }));

            target.SetEvents(AfterBeatEventTrack.Grain, Map(post.Grains, framerate, context,
                key => new List<float> { key.Intensity, 0f, 0f, 0f }));

            target.SetEvents(AfterBeatEventTrack.Glitch, Map(post.DigitalGlitches, framerate, context,
                key => new List<float> { key.Intensity }));

            ExportMarkersAndCheckpoints(level, target, context);
        }

        private static float EffectIndex(IColor4 color, AfterBeatExportContext context)
        {
            var (index, _) = AfterBeatColorMap.Export(color, AfterBeatPalette.Effects,
                context.ReferenceTheme, context.Report, "events");
            return index;
        }

        private static List<VgdEventKeyframe> Map<T>(List<T> keys, int framerate,
            AfterBeatExportContext context, Func<T, List<float>> read) where T : IKeyframe
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
                    Time = AfterBeatTimeMap.ToSeconds(key.Frame, framerate),
                    Ease = AfterBeatEaseMap.Export(key.Ease, context.Report, "events"),
                    Values = JArray.FromObject(values),
                });
            }

            return exported;
        }

        private static List<VgdEventKeyframe> MapThemes(List<ThemeKeyframe> keys,
            int framerate, AfterBeatExportContext context)
        {
            var exported = new List<VgdEventKeyframe>();
            if (keys == null) return exported;

            var sorted = new List<ThemeKeyframe>(keys);
            sorted.Sort((a, b) => a.Frame.CompareTo(b.Frame));

            foreach (var key in sorted)
                exported.Add(new VgdEventKeyframe
                {
                    Time = AfterBeatTimeMap.ToSeconds(key.Frame, framerate),
                    Ease = AfterBeatEaseMap.Export(key.Ease, context.Report, "events"),
                    Values = new JArray { ToThemeSourceId(key.ThemeId) },
                });

            return exported;
        }

        private static void ExportMarkersAndCheckpoints(Level level, VgdLevel target,
            AfterBeatExportContext context)
        {
            var framerate = context.Options.Framerate;
            var events = level.Game.Events;

            foreach (var marker in events.Markers)
                target.Markers.Add(new VgdMarker
                {
                    Id = marker.Frame.ToString(),
                    Name = marker.Name ?? string.Empty,
                    Description = marker.Description ?? string.Empty,
                    Time = AfterBeatTimeMap.ToSeconds(marker.Frame, framerate),
                });

            var spacedCheckpoint = false;
            foreach (var checkpoint in events.Checkpoints)
            {
                if (checkpoint.Space != CheckpointSpace.World) spacedCheckpoint = true;

                var (x, y) = AfterBeatValueMap.ExportVector(checkpoint.Position, context.Report, "checkpoints");
                target.Checkpoints.Add(new VgdCheckpoint
                {
                    Id = checkpoint.Frame.ToString(),
                    Name = checkpoint.Name ?? string.Empty,
                    Time = AfterBeatTimeMap.ToSeconds(checkpoint.Frame, framerate),
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
        private static void ExportBpm(Level level, VgdLevel target, AfterBeatExportContext context)
        {
            var beats = level.Game.Events.Beats;
            if (beats == null || beats.Count == 0) return;

            var first = beats[0];
            target.Editor.Bpm.Value = first.Bpm;
            target.Editor.Bpm.ValueDuplicate = first.Bpm;
            target.Editor.Bpm.Offset = AfterBeatTimeMap.ToSeconds(first.Span.StartFrame, context.Options.Framerate)
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
        }

        private static void ReportPostProcessing(Level level, InteropReport report)
        {
            var post = level.Game.PostProcessingEvents;
            var unsupported =
                post.MotionBlurs is { Count: > 0 } ||
                post.ColorCurveses is { Count: > 0 } ||
                post.LiftGammaGains is { Count: > 0 } ||
                post.ShadowsMidtonesHighlightses is { Count: > 0 } ||
                post.WhiteBalances is { Count: > 0 };

            if (unsupported)
                report.Dropped("postprocessing_unsupported",
                    "Motion blur, colour curves, lift/gamma/gain, shadows/midtones/highlights and white balance have no Afterbeat equivalent and are not exported.",
                    "postprocessing");

            if (post.AnalogGlitches is { Count: > 0 })
                report.Approximated("glitch_merged",
                    "Afterbeat has one glitch effect; the analog and digital tracks were merged into it, keeping the digital one's intensity.",
                    "postprocessing");
        }
    }
}
