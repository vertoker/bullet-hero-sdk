using System.Collections.Generic;
using BH.SDK.Interop.AfterBeat.Models;
using BH.SDK.Models;
using BH.SDK.Models.Enums;
using BH.SDK.Models.Events;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Game;
using BH.SDK.Models.Keyframes;
using BH.SDK.Models.PostProcessing;
using BH.SDK.Models.Primitives;
using BH.SDK.Models.Values;
using BH.SDK.Rules;

namespace BH.SDK.Interop.AfterBeat.Import
{
    // Fourteen fixed arrays, addressed by position. Eleven of them land on a track this format
    // already has; three do not, and each is a different KIND of gap:
    //
    //   gradient - a full-screen colour overlay. This format has no such effect and no plan for
    //              one; the level loses it.
    //   hue      - a global hue rotation. Same.
    //   player force - a force applied to the player. PlayerEvents already has Velocities and
    //              VelocityPoints written and commented out, and VelocityPoint exists as a keyframe
    //              type, so this one is waiting on work rather than on a decision. It is reported
    //              as DEFERRED rather than dropped so an author can tell the difference.
    //
    // Level-global event times are ABSOLUTE here, unlike an object keyframe's, so nothing is
    // rebased. Camera rotation is degrees there and radians here, but unlike an object's it is not
    // relative to the previous keyframe - there is nothing to accumulate.

    /// <summary> The fourteen .vgd events[] arrays into this format's level-global tracks. </summary>
    public static class AfterBeatEventsImporter
    {
        public static void ImportAll(VgdLevel source, Level level, AfterBeatImportContext context, string path)
        {
            if (source == null || level == null) return;

            var report = context.Report;
            var framerate = context.Options.Framerate;
            var camera = level.Game.CameraEvents;
            var post = level.Game.PostProcessingEvents;

            foreach (var key in source.GetEvents(AfterBeatEventTrack.CameraPosition))
                camera.Positions.Add(new PosKey(
                    new Vector2Value(key.GetFloat(0), key.GetFloat(1)),
                    Frame(key, framerate), Ease(key, report, path)));

            foreach (var key in source.GetEvents(AfterBeatEventTrack.CameraZoom))
                camera.Zooms.Add(new ZoomKey(new FloatValue(key.GetFloat(0)),
                    Frame(key, framerate), Ease(key, report, path)));

            foreach (var key in source.GetEvents(AfterBeatEventTrack.CameraRotation))
                camera.Rotations.Add(new AngleKey(
                    new FloatValue(key.GetFloat(0) * AfterBeatValueMap.DegreesToRadians),
                    Frame(key, framerate), Ease(key, report, path)));

            // Afterbeat's shake is one number. This format splits intensity from speed and from the
            // two per-axis amounts, so the single number becomes the overall intensity and the rest
            // keep their own defaults - a shake of the right size at this engine's own rate.
            foreach (var key in source.GetEvents(AfterBeatEventTrack.CameraShake))
                camera.Shakes.Add(new ShakeKey(key.GetFloat(0), DefaultShakeSpeed,
                    Frame(key, framerate), Ease(key, report, path)));

            ImportThemes(source, level, context, path);

            foreach (var key in source.GetEvents(AfterBeatEventTrack.Chromatic))
                post.Chromatics.Add(new ChromaticAberrationKey(key.GetFloat(0),
                    true, Frame(key, framerate), Ease(key, report, path)));

            foreach (var key in source.GetEvents(AfterBeatEventTrack.Bloom))
                post.Blooms.Add(new BloomKey(key.GetFloat(0), key.GetFloat(1),
                    EffectColor(key, 2, context, path),
                    true, Frame(key, framerate), Ease(key, report, path)));

            // ev is [Intensity, Smoothness, ForceRound, unused, CenterX, CenterY, Colour] - note
            // the gap at index 3, which the format documents as always zero.
            foreach (var key in source.GetEvents(AfterBeatEventTrack.Vignette))
                post.Vignettes.Add(new VignetteKey(
                    EffectColor(key, 6, context, path),
                    new Vector2Value(key.GetFloat(4), key.GetFloat(5)),
                    key.GetFloat(0), key.GetFloat(1), key.GetFloat(2) > 0.5f,
                    true, Frame(key, framerate), Ease(key, report, path)));

            foreach (var key in source.GetEvents(AfterBeatEventTrack.LensDistortion))
                post.Lenses.Add(new LensDistortionKey(key.GetFloat(0),
                    Vector2Value.One,
                    new Vector2Value(key.GetFloat(1), key.GetFloat(2)),
                    DefaultLensScale,
                    true, Frame(key, framerate), Ease(key, report, path)));

            // ev is [Intensity, unused, Size, Mix]. Grain SIZE and MIX have no field on this
            // format's own film grain, which carries a type and an intensity instead.
            var grainKeys = source.GetEvents(AfterBeatEventTrack.Grain);
            foreach (var key in grainKeys)
                post.Grains.Add(new FilmGrainKey(FilmGrainType.Thin1, key.GetFloat(0),
                    true, Frame(key, framerate), Ease(key, report, path)));
            if (grainKeys.Count > 0)
                report.Approximated("grain_shape",
                    "Afterbeat's film grain carries a size and a mix; this format's carries a preset and an intensity, so only the intensity was imported.",
                    path);

            ImportGlitch(source, post, context, path);
            ImportUnsupported(source, context, path);
        }

        /// <summary> How fast an imported camera shake oscillates. Afterbeat carries no rate of its
        /// own, so this is this engine's own. </summary>
        public const float DefaultShakeSpeed = 20f;

        /// <summary> Lens distortion scale Afterbeat has no field for. 1 is "no extra zoom", which
        /// is what a level authored without the field expects to see. </summary>
        public const float DefaultLensScale = 1f;

        // The theme track is the one whose payload is a STRING - the id of a theme defined inside
        // the same file. It resolves through the same derived-Guid rule the themes themselves were
        // imported with, so the two always agree.
        private static void ImportThemes(VgdLevel source, Level level,
            AfterBeatImportContext context, string path)
        {
            var framerate = context.Options.Framerate;
            foreach (var key in source.GetEvents(AfterBeatEventTrack.Theme))
            {
                var sourceId = key.GetString(0);
                if (string.IsNullOrEmpty(sourceId))
                {
                    context.Report.Approximated("theme_key_unreadable",
                        "A theme keyframe names no theme; it was skipped.", path);
                    continue;
                }

                level.Game.Events.Themes.Add(new ThemeKeyframe(
                    AfterBeatIdMap.ToThemeId(sourceId), Frame(key, framerate),
                    Ease(key, context.Report, path)));
            }
        }

        // Afterbeat has one glitch with an intensity, a width and a speed; this format has two
        // separate effects, an analog one with four independent amounts and a digital one with a
        // single intensity. The intensity drives both, which is what the one authored number meant.
        private static void ImportGlitch(VgdLevel source, PostProcessingEvents post,
            AfterBeatImportContext context, string path)
        {
            var keys = source.GetEvents(AfterBeatEventTrack.Glitch);
            if (keys.Count == 0) return;

            var framerate = context.Options.Framerate;
            foreach (var key in keys)
            {
                var intensity = key.GetFloat(0);
                var frame = Frame(key, framerate);
                var ease = Ease(key, context.Report, path);

                post.DigitalGlitches.Add(new DigitalGlitchKey(intensity, true, frame, ease));
                post.AnalogGlitches.Add(new AnalogGlitchKey(intensity, intensity, intensity, intensity,
                    true, frame, ease));
            }

            context.Report.Approximated("glitch_split",
                "Afterbeat's single glitch effect became this format's analog and digital pair; its width and speed have no equivalent.",
                path);
        }

        private static void ImportUnsupported(VgdLevel source, AfterBeatImportContext context, string path)
        {
            var report = context.Report;

            if (source.GetEvents(AfterBeatEventTrack.Gradient).Count > 0)
                report.Dropped("event_gradient",
                    "Afterbeat's full-screen gradient overlay has no equivalent here and is not imported.", path);

            if (source.GetEvents(AfterBeatEventTrack.Hue).Count > 0)
                report.Dropped("event_hue",
                    "Afterbeat's global hue shift has no equivalent here and is not imported.", path);

            // TODO import into PlayerEvents.Velocities/VelocityPoints once those are uncommented -
            // the keyframe type (VelocityPoint) and the fields already exist, so this is waiting on
            // the player side rather than on anything in this converter.
            if (source.GetEvents(AfterBeatEventTrack.PlayerForce).Count > 0)
                report.Deferred("event_player_force",
                    "Afterbeat's player force track is not imported yet - this format's player velocity events are written but not switched on. It will be added later.",
                    path);

            if (source.Triggers is { Count: > 0 })
                report.Dropped("triggers",
                    "Afterbeat's triggers (visual novel, player control, time control) have no equivalent here and are not imported.",
                    path);
        }

        /// <summary> Markers and checkpoints - the two flat event lists. </summary>
        public static void ImportEvents(VgdLevel source, Level level, AfterBeatImportContext context, string path)
        {
            if (source == null || level == null) return;

            var framerate = context.Options.Framerate;
            var events = level.Game.Events;

            if (source.Markers != null)
                foreach (var marker in source.Markers)
                {
                    if (marker == null) continue;
                    if (events.Markers.Count >= LevelRules.MaxMarkerEvents)
                    {
                        context.Report.Dropped("markers_over_cap",
                            $"This format allows {LevelRules.MaxMarkerEvents} markers; the rest were dropped.", path);
                        break;
                    }

                    events.Markers.Add(new Marker(marker.Name ?? string.Empty,
                        marker.Description ?? string.Empty, Color4Value.white,
                        AfterBeatTimeMap.ToFrame(marker.Time, framerate)));
                }

            if (source.Checkpoints != null)
                foreach (var checkpoint in source.Checkpoints)
                {
                    if (checkpoint == null) continue;
                    if (events.Checkpoints.Count >= LevelRules.MaxCheckpointEvents)
                    {
                        context.Report.Dropped("checkpoints_over_cap",
                            $"This format allows {LevelRules.MaxCheckpointEvents} checkpoints; the rest were dropped.", path);
                        break;
                    }

                    // The respawn position crosses exactly, in world space, because this format
                    // grew a position for its own checkpoints.
                    events.Checkpoints.Add(new Checkpoint(
                        checkpoint.Name ?? string.Empty, true, Color4Value.white,
                        AfterBeatTimeMap.ToFrame(checkpoint.Time, framerate),
                        new Vector2Value(checkpoint.Position?.X ?? 0f, checkpoint.Position?.Y ?? 0f),
                        CheckpointSpace.World));
                }
        }

        /// <summary> Afterbeat's editor tempo as this format's beat map - one segment covering the
        /// whole level, which is what a single BPM and offset describe. </summary>
        public static void ImportBeats(VgdLevel source, Level level, AfterBeatImportContext context)
        {
            var bpm = source?.Editor?.Bpm;
            if (bpm == null || level == null) return;
            if (bpm.Value < LevelRules.MinBpm || bpm.Value > LevelRules.MaxBpm) return;

            var framerate = context.Options.Framerate;
            var duration = level.Settings.FrameDuration;

            level.Game.Events.Beats.Add(new BeatSegment(
                new FrameSpan(FrameRules.MinFrame, duration),
                bpm.Value, bpm.Offset * framerate, LevelRules.DefaultBeatsPerBar,
                string.Empty, Color4Value.white));
        }

        private static int Frame(VgdEventKeyframe key, int framerate)
            => AfterBeatTimeMap.ToFrame(key.Time, framerate);

        private static EaseType Ease(VgdEventKeyframe key, InteropReport report, string path)
            => AfterBeatEaseMap.Import(key.Ease, report, path);

        private static IColor4 EffectColor(VgdEventKeyframe key, int index,
            AfterBeatImportContext context, string path)
            => AfterBeatColorMap.Import((int)key.GetFloat(index), 1f, AfterBeatPalette.Effects,
                context.ReferenceTheme, context.Report, path);
    }
}
