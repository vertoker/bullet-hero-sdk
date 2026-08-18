using System;
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
    // Fourteen fixed arrays, addressed by position. Twelve of them land on a track this format
    // already has - hue among them, on colour curves, which is not the track it is named after but
    // is the one that does the same thing. Two do not, and each is a different KIND of gap:
    //
    //   gradient - a full-screen colour overlay. This format's Backgrounds track is a single
    //              colour, not two with a rotation, so there is nowhere to put one; the level
    //              loses it.
    //   player force - a force applied to the player. PlayerEvents already has Velocities and
    //              VelocityPoints written and commented out, and VelocityPoint exists as a keyframe
    //              type, so this one is waiting on work rather than on a decision. It is reported
    //              as DEFERRED rather than dropped so an author can tell the difference.
    //
    // Level-global event times are ABSOLUTE here, unlike an object keyframe's, so nothing is
    // rebased. Camera rotation is degrees there and radians here, but unlike an object's it is not
    // relative to the previous keyframe - there is nothing to accumulate.

    /// <summary> The fourteen .vgd events[] arrays into this format's level-global tracks. </summary>
    public static class ABEventsImporter
    {
        public static void ImportAll(VgdLevel source, Level level, ABImportContext context, string path)
        {
            if (source == null || level == null) return;

            var report = context.Report;
            var framerate = context.Options.Framerate;
            var camera = level.Game.CameraEvents;
            var post = level.Game.PostProcessingEvents;

            foreach (var key in source.GetEvents(ABEventTrack.CameraPosition))
                camera.Positions.Add(new PosKey(
                    new Vector2Value(key.GetFloat(0), key.GetFloat(1)),
                    Frame(key, framerate), Ease(key, report, path)));

            ImportZoom(source, camera, context, path);

            foreach (var key in source.GetEvents(ABEventTrack.CameraRotation))
                camera.Rotations.Add(new AngleKey(
                    new FloatValue(key.GetFloat(0) * ABValueMap.DegreesToRadians),
                    Frame(key, framerate), Ease(key, report, path)));

            // Afterbeat's shake is one number. This format splits intensity from speed and from the
            // two per-axis amounts, so the single number becomes the overall intensity and the rest
            // keep their own defaults - a shake of the right size at this engine's own rate.
            foreach (var key in source.GetEvents(ABEventTrack.CameraShake))
                camera.Shakes.Add(new ShakeKey(key.GetFloat(0), DefaultShakeSpeed,
                    Frame(key, framerate), Ease(key, report, path)));

            ImportThemes(source, level, context, path);
            ImportBackground(level, context);

            // Every number below crosses on its own scale - see ABPostProcessingMap. None of
            // them survives being read as-is: the source ranges are Project Arrhythmia's inspector
            // ranges and this format's are the URP volume's own.
            foreach (var key in source.GetEvents(ABEventTrack.Chromatic))
                post.Chromatics.Add(new ChromaticAberrationKey(
                    ABPostProcessingMap.ImportChromatic(key.GetFloat(0)),
                    EffectsActive, Frame(key, framerate), Ease(key, report, path)));

            foreach (var key in source.GetEvents(ABEventTrack.Bloom))
                post.Blooms.Add(new BloomKey(
                    ABPostProcessingMap.ImportBloomIntensity(key.GetFloat(0)),
                    ABPostProcessingMap.ImportBloomScatter(key.GetFloat(1)),
                    EffectColor(key, 2, context, path),
                    EffectsActive, Frame(key, framerate), Ease(key, report, path)));

            // ev is [Intensity, Smoothness, ForceRound, unused, CenterX, CenterY, Colour] - note
            // the gap at index 3, which the format documents as always zero.
            foreach (var key in source.GetEvents(ABEventTrack.Vignette))
                post.Vignettes.Add(new VignetteKey(
                    EffectColor(key, 6, context, path),
                    new Vector2Value(
                        ABPostProcessingMap.ImportVignetteCenter(key.GetFloat(4)),
                        ABPostProcessingMap.ImportVignetteCenter(key.GetFloat(5))),
                    ABPostProcessingMap.ImportVignetteIntensity(key.GetFloat(0)),
                    ABPostProcessingMap.ImportVignetteSmoothness(key.GetFloat(1)),
                    key.GetFloat(2) > 0.5f,
                    EffectsActive, Frame(key, framerate), Ease(key, report, path)));

            foreach (var key in source.GetEvents(ABEventTrack.LensDistortion))
                post.Lenses.Add(new LensDistortionKey(
                    ABPostProcessingMap.ImportLensIntensity(key.GetFloat(0)),
                    Vector2Value.One,
                    new Vector2Value(
                        ABPostProcessingMap.ImportLensCenter(key.GetFloat(1)),
                        ABPostProcessingMap.ImportLensCenter(key.GetFloat(2))),
                    DefaultLensScale,
                    EffectsActive, Frame(key, framerate), Ease(key, report, path)));

            // ev is [Intensity, unused, Size, Mix]. Grain SIZE and MIX have no field on this
            // format's own film grain, which carries a type and an intensity instead.
            var grainKeys = source.GetEvents(ABEventTrack.Grain);
            foreach (var key in grainKeys)
                post.Grains.Add(new FilmGrainKey(FilmGrainType.Thin1,
                    ABPostProcessingMap.ImportGrainIntensity(key.GetFloat(0)),
                    EffectsActive, Frame(key, framerate), Ease(key, report, path)));
            if (grainKeys.Count > 0)
                report.Approximated("grain_shape",
                    "Afterbeat's film grain carries a size and a mix; this format's carries a preset and an intensity, so only the intensity was imported.",
                    path);

            ImportGlitch(source, post, context, path);
            ImportHue(source, post, context, path);
            ImportPlayerForce(source, level.Game.PlayerEvents, context, path);
            ImportUnsupported(source, context, path);
        }

        // Afterbeat has to write a keyframe on every one of its fourteen event tracks - a track
        // cannot be empty over there - so a converted level arrives with every post-processing
        // effect keyframed whether its author ever touched one or not. Here an empty track is
        // legal and means "this effect does not exist in this level", so importing those keys as
        // ACTIVE hands the player a dozen full-screen passes to run for a level that asked for
        // none. They are imported switched off instead: the authored numbers are all still there,
        // one tick per effect turns any of them back on, and nothing is guessed about which ones
        // the author meant.

        /// <summary> Whether an imported post-processing keyframe arrives switched on. </summary>
        public const bool EffectsActive = false;

        /// <summary> How fast an imported camera shake oscillates. Afterbeat carries no rate of its
        /// own, so this is this engine's own. </summary>
        public const float DefaultShakeSpeed = 20f;

        /// <summary> Lens distortion scale Afterbeat has no field for. 1 is "no extra zoom", which
        /// is what a level authored without the field expects to see. </summary>
        public const float DefaultLensScale = 1f;

        // The theme track is the one whose payload is a STRING - the id of a theme defined inside
        // the same file. It resolves through the same derived-Guid rule the themes themselves were
        // imported with, so the two always agree.
        //
        // A level whose theme track ends up EMPTY renders every theme-referenced colour against no
        // theme at all, which is a whole level with no colour in it rather than a level missing one
        // keyframe. So an empty track is filled with the level's own first theme instead of being
        // left as it is - the level always had a theme, this format just needs it said out loud.
        private static void ImportThemes(VgdLevel source, Level level,
            ABImportContext context, string path)
        {
            var framerate = context.Options.Framerate;
            foreach (var key in source.GetEvents(ABEventTrack.Theme))
            {
                var sourceId = key.GetString(0);
                if (string.IsNullOrEmpty(sourceId))
                {
                    context.Report.Approximated("theme_key_unreadable",
                        "A theme keyframe names no theme; it was skipped.", path);
                    continue;
                }

                level.Game.Events.Themes.Add(new ThemeKeyframe(
                    ABIdMap.ToThemeId(sourceId), Frame(key, framerate),
                    Ease(key, context.Report, path)));
            }

            if (level.Game.Events.Themes.Count > 0) return;

            foreach (var pair in level.Resources.Themes)
            {
                level.Game.Events.Themes.Add(new ThemeKeyframe(pair.Key, FrameRules.MinFrame));
                context.Report.Info("theme_track_synthesized",
                    "The level defines themes but never switches to one, so its first theme was placed on the first frame - without it every theme-referenced colour resolves to nothing.",
                    path);
                return;
            }
        }

        // Afterbeat has one glitch with an intensity, a width and a speed; this format has two
        // separate effects, an analog one with four independent amounts and a digital one with a
        // single intensity. The intensity drives both, which is what the one authored number meant.
        private static void ImportGlitch(VgdLevel source, PostProcessingEvents post,
            ABImportContext context, string path)
        {
            var keys = source.GetEvents(ABEventTrack.Glitch);
            if (keys.Count == 0) return;

            var framerate = context.Options.Framerate;
            foreach (var key in keys)
            {
                var intensity = ABPostProcessingMap.ImportGlitchIntensity(key.GetFloat(0));
                var frame = Frame(key, framerate);
                var ease = Ease(key, context.Report, path);

                post.DigitalGlitches.Add(new DigitalGlitchKey(intensity, EffectsActive, frame, ease));
                post.AnalogGlitches.Add(new AnalogGlitchKey(intensity, intensity, intensity, intensity,
                    EffectsActive, frame, ease));
            }

            context.Report.Approximated("glitch_split",
                "Afterbeat's single glitch effect became this format's analog and digital pair; its width and speed have no equivalent.",
                path);
        }

        // Camera zoom is the one camera number that does NOT cross as itself, and reading it as
        // itself frames a converted level at half the size its author saw. Afterbeat's zoom IS its
        // camera's orthographic size (EventManager.Update: ForegroundCamera.orthographicSize =
        // GetCameraZoom()), i.e. HALF the visible height; this format's Zoom is the whole visible
        // height (BuildCameraJob builds a screen size of (aspect * zoom, zoom) and RectTransform2D
        // halves it into orthographicSize). So one source unit is two of ours.
        //
        // A level with no zoom keyframe at all is the other half of the same problem: this format
        // would fall back to its own default zoom, which is a quarter of what the source game
        // shows. Afterbeat's own fallback is 20 - the same constant its Update writes when the
        // field is zero - so that is what an empty track becomes.
        private static void ImportZoom(VgdLevel source, CameraEvents camera,
            ABImportContext context, string path)
        {
            var framerate = context.Options.Framerate;
            var keys = source.GetEvents(ABEventTrack.CameraZoom);

            foreach (var key in keys)
                camera.Zooms.Add(new ZoomKey(new FloatValue(ImportZoomValue(key.GetFloat(0))),
                    Frame(key, framerate), Ease(key, context.Report, path)));

            if (keys.Count > 0) return;

            camera.Zooms.Add(new ZoomKey(new FloatValue(ImportZoomValue(DefaultSourceZoom)),
                FrameRules.MinFrame));
            context.Report.Info("camera_zoom_default",
                "This level never sets a camera zoom, so the source game's own default was written on its first frame - without it the level would be framed at this engine's default instead.",
                path);
        }

        /// <summary> Afterbeat's zoom when a level never sets one - what its own EventManager falls
        /// back to. </summary>
        public const float DefaultSourceZoom = 20f;

        /// <summary> Source zoom (an orthographic half-height) as this format's zoom (a whole
        /// visible height). </summary>
        public static float ImportZoomValue(float sourceZoom)
            => Math.Clamp(sourceZoom * 2f, ValueRules.MinZoom, ValueRules.MaxZoom);

        /// <summary> And back. </summary>
        public static float ExportZoomValue(float zoom) => zoom * 0.5f;

        // Afterbeat's background is a whole subsystem of its own (see ABParallaxImporter),
        // and this format's is a camera clear colour plus ordinary objects - so the two do not
        // convert. The one piece that does is the THEME's own background colour, which is what an
        // Afterbeat level actually shows behind everything when its parallax is empty, i.e. most of
        // the time. Written as a theme REFERENCE rather than as the resolved colour, so it keeps
        // following the theme track exactly as the source level's background does.
        //
        // Only when the level has no background track of its own, which a converted level never
        // does - the check is there because this is the one import that writes a track nothing in
        // the source document asked for.
        private static void ImportBackground(Level level, ABImportContext context)
        {
            if (level.Game.Events.Backgrounds.Count > 0) return;

            level.Game.Events.Backgrounds.Add(new Color3Key(
                new Color3ThemeRef(ABThemeMap.BackgroundIndex), FrameRules.MinFrame));

            context.Report.Info("background_from_theme",
                "Afterbeat's background subsystem has no equivalent here; the theme's own background colour was placed on the camera so the level keeps the colour behind it.",
                null);
        }

        // Afterbeat rotates the whole picture's hue with one number; this format has no hue effect,
        // but its colour curves carry a Hue vs Hue control that does exactly that, so the track
        // belongs there. It is NOT written today: this project's own colour curves have a bug that
        // has nothing to do with the conversion, and a converted level is not the place to meet it.
        // Reported as deferred rather than dropped, since nothing about the mapping is in doubt -
        // delete this early return and the block below is the whole feature.
        private static void ImportHue(VgdLevel source, PostProcessingEvents post,
            ABImportContext context, string path)
        {
            var keys = source.GetEvents(ABEventTrack.Hue);
            if (keys.Count == 0) return;

            context.Report.Deferred("event_hue_curves",
                "Afterbeat's global hue shift maps onto this format's colour curves, which are temporarily not imported; those keyframes are missing from the converted level.",
                path);
        }

        // The force the level applies to the player. This format carries the track but the player
        // does not read it yet, so the level arrives complete and plays as if the track were not
        // there - which is why it is reported as DEFERRED: nothing was lost, something is late.
        private static void ImportPlayerForce(VgdLevel source, PlayerEvents player,
            ABImportContext context, string path)
        {
            var keys = source.GetEvents(ABEventTrack.PlayerForce);
            if (keys.Count == 0) return;

            var framerate = context.Options.Framerate;
            foreach (var key in keys)
            {
                if (player.Velocities.Count >= LevelRules.MaxPlayerKeys)
                {
                    context.Report.Dropped("player_force_over_cap",
                        $"This format allows {LevelRules.MaxPlayerKeys} player keys per track; the rest of the force track was dropped.",
                        path);
                    break;
                }

                player.Velocities.Add(new Velocity(
                    new Vector2Value(key.GetFloat(0), key.GetFloat(1)),
                    Frame(key, framerate), Ease(key, context.Report, path)));
            }

            context.Report.Deferred("event_player_force",
                "Afterbeat's player force track was imported, but this format's player does not apply it yet - the level plays as if the track were not there.",
                path);
        }

        private static void ImportUnsupported(VgdLevel source, ABImportContext context, string path)
        {
            var report = context.Report;

            if (source.GetEvents(ABEventTrack.Gradient).Count > 0)
                report.Dropped("event_gradient",
                    "Afterbeat's full-screen gradient overlay has no equivalent here and is not imported.", path);

            // Not editor bookkeeping, which is why this one is reported at all: an annotation is
            // something the author drew by hand, and losing it silently is losing notes.
            if (source.Annotations is { Count: > 0 })
                report.Dropped("annotations",
                    "Afterbeat lets an author draw freehand notes over the editor's canvas; this format has no equivalent and they are not imported.",
                    path);

            if (source.Triggers is { Count: > 0 })
                report.Dropped("triggers",
                    "Afterbeat's triggers (visual novel, player control, time control) have no equivalent here and are not imported.",
                    path);
        }

        /// <summary> Markers and checkpoints - the two flat event lists. </summary>
        public static void ImportEvents(VgdLevel source, Level level, ABImportContext context, string path)
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
                        ABTimeMap.ToFrame(marker.Time, framerate)));
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
                        ABTimeMap.ToFrame(checkpoint.Time, framerate),
                        new Vector2Value(checkpoint.Position?.X ?? 0f, checkpoint.Position?.Y ?? 0f),
                        CheckpointSpace.World));
                }
        }

        /// <summary> Afterbeat's editor tempo as this format's beat map - one segment covering the
        /// whole level, which is what a single BPM and offset describe. </summary>
        public static void ImportBeats(VgdLevel source, Level level, ABImportContext context)
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
            => ABTimeMap.ToFrame(key.Time, framerate);

        private static EaseType Ease(VgdEventKeyframe key, InteropReport report, string path)
            => ABEaseMap.Import(key.Ease, report, path);

        private static IColor4 EffectColor(VgdEventKeyframe key, int index,
            ABImportContext context, string path)
            => ABColorMap.Import((int)key.GetFloat(index), 1f, ABPalette.Effects,
                context.ReferenceTheme, context.Report, path);
    }
}
