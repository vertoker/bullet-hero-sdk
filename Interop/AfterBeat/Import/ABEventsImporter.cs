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
            ImportPlayerSize(level, context);
            ImportScreenLimit(level, context);

            // Every number below crosses on its own scale - see ABPostProcessingMap. None of
            // them survives being read as-is: the source ranges are the ones the source game's own
            // EventManager remaps by, and this format's are the URP volume's own.
            foreach (var key in source.GetEvents(ABEventTrack.Chromatic))
                post.Chromatics.Add(new ChromaticAberrationKey(
                    ABPostProcessingMap.ImportChromatic(key.GetFloat(0)),
                    IsActive(key, 0), Frame(key, framerate), Ease(key, report, path)));

            foreach (var key in source.GetEvents(ABEventTrack.Bloom))
                post.Blooms.Add(new BloomKey(
                    ABPostProcessingMap.ImportBloomIntensity(key.GetFloat(0)),
                    ABPostProcessingMap.ImportBloomScatter(
                        key.GetFloat(1, ABPostProcessingMap.DefaultBloomDiffusion)),
                    EffectColor(key, 2, ABColorMap.EffectColorWhite, context, path),
                    IsActive(key, 0), Frame(key, framerate), Ease(key, report, path)));

            // ev is [Intensity, Smoothness, ForceRound, unused, CenterX, CenterY, Colour] - note
            // the gap at index 3, which the source game reads on no branch at all.
            foreach (var key in source.GetEvents(ABEventTrack.Vignette))
                post.Vignettes.Add(new VignetteKey(
                    EffectColor(key, 6, ABColorMap.EffectColorBlack, context, path),
                    new Vector2Value(
                        ABPostProcessingMap.ImportVignetteCenter(key.GetFloat(4)),
                        ABPostProcessingMap.ImportVignetteCenter(key.GetFloat(5))),
                    ABPostProcessingMap.ImportVignetteIntensity(key.GetFloat(0)),
                    ABPostProcessingMap.ImportVignetteSmoothness(key.GetFloat(1)),
                    key.GetFloat(2) > 0.5f,
                    IsActive(key, 0), Frame(key, framerate), Ease(key, report, path)));

            // The one effect whose switch is != 0 rather than > 0 over there - a lens distortion
            // pinches as readily as it bulges, so a negative intensity is a live effect.
            foreach (var key in source.GetEvents(ABEventTrack.LensDistortion))
                post.Lenses.Add(new LensDistortionKey(
                    ABPostProcessingMap.ImportLensIntensity(key.GetFloat(0)),
                    Vector2Value.One,
                    new Vector2Value(
                        ABPostProcessingMap.ImportLensCenter(key.GetFloat(1)),
                        ABPostProcessingMap.ImportLensCenter(key.GetFloat(2))),
                    DefaultLensScale,
                    key.GetFloat(0) != 0f, Frame(key, framerate), Ease(key, report, path)));

            // ev is [Intensity, unused, Type, Response]. Slot 2 is not a size - the source game
            // casts it to URP's own FilmGrainLookup, which is the same preset table this format's
            // FilmGrainType is. Slot 3 is the luminance response, which this format's film grain
            // has no field for; slot 1 reaches a field the effect never reads.
            var grainKeys = source.GetEvents(ABEventTrack.Grain);
            foreach (var key in grainKeys)
                post.Grains.Add(new FilmGrainKey(
                    ABPostProcessingMap.ImportGrainType(key.GetFloat(2)),
                    ABPostProcessingMap.ImportGrainIntensity(key.GetFloat(0)),
                    IsActive(key, 0), Frame(key, framerate), Ease(key, report, path)));
            if (grainKeys.Count > 0)
                report.Approximated("grain_response",
                    "Afterbeat's film grain carries a luminance response; this format's carries a preset and an intensity, so the response was not imported.",
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
        // unconditionally ACTIVE would hand the player a dozen full-screen passes to run for a
        // level that asked for none.
        //
        // The source game answers this itself and needs no policy invented here: every
        // LSEffectsManager.Update* sets `active = intensity > 0` on the volume component before
        // writing anything into it, so an effect keyframed at zero is an effect that is OFF, and
        // one keyframed above zero is one the author reached for. Reproducing that rule per
        // keyframe gives a converted level exactly the passes the source level ran, which is both
        // cheaper than switching everything on and truer than switching everything off (the
        // previous behaviour, which lost every effect an author DID author until they found the
        // tickbox).

        /// <summary> Whether one imported post-processing keyframe arrives switched on: the source
        /// game's own rule, read off the component that decides it. </summary>
        private static bool IsActive(VgdEventKeyframe key, int intensityIndex)
            => key.GetFloat(intensityIndex) > 0f;

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
                var active = IsActive(key, 0);

                post.DigitalGlitches.Add(new DigitalGlitchKey(intensity, active, frame, ease));
                post.AnalogGlitches.Add(new AnalogGlitchKey(intensity, intensity, intensity, intensity,
                    active, frame, ease));
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

        // Afterbeat's world is calibrated around ITS player, and this engine's avatar is not the
        // same size - so a converted level plays with a player of the wrong size against content
        // that is otherwise exactly right, which is the one mismatch an author cannot fix by
        // editing the level. The size track exists for precisely this, and one key at the first
        // frame states it for the whole level; an author who wants it different edits that one key
        // rather than every object.
        //
        // The number is a measurement rather than a derivation - see the Calibration table in this
        // folder's README for what the source game's own player is (a 1x1 body, a hitbox of radius
        // 0.25, in a frame 40 units tall).
        private static void ImportPlayerSize(Level level, ABImportContext context)
        {
            if (level.Game.PlayerEvents.Sizes.Count > 0) return;

            level.Game.PlayerEvents.Sizes.Add(new FloatKey(
                new FloatValue(ImportedPlayerSize), FrameRules.MinFrame));

            context.Report.Info("player_size",
                "The player is scaled for this level so it matches the size Afterbeat's own player is against the same content; change the first keyframe of the player size track to taste.",
                null);
        }

        /// <summary> What a converted level scales the player by. </summary>
        public const float ImportedPlayerSize = 2f;

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

        // AN AFTERBEAT LEVEL IS AUTHORED AT 16:9, and the level document says so nowhere - the game
        // does, and only by what it never offers. Its window resolution list is ten entries and
        // nine of them are exactly 16:9 (480x270 up to 3840x2160); the tenth, 1360x768, is the
        // laptop panel that is 16:9 to within a thousandth. There is no 16:10, no 4:3 and no
        // ultrawide anywhere in it. So every author of every level saw the same frame, and every
        // decision they made about what is on screen was made inside it.
        //
        // The reason that has to be carried across rather than left to the player's window is that
        // nothing in the source game enforces it at PLAY time: the camera's zoom fixes the visible
        // HEIGHT (EventManager.Update writes it into orthographicSize) and the width follows the
        // aspect, while the player is clamped in VIEWPORT space (VGPlayer.ClampPlayerPosition), not
        // to any authored box. A converted level shown at 21:9 therefore gets a third more play
        // area, reveals the content its author parked off the sides, and is easier than it was
        // written to be - all of it silently, because every value in the file is still correct.
        //
        // Only the import writes this. The export does not: this is a fact about where the level
        // came from, not a field the target format has, and a level authored HERE at some other
        // aspect is not made 16:9 by being sent there.
        //
        // Written only when the level has no limit of its own - the check is there for the same
        // reason ImportBackground's is, since a converted level never does.
        private static void ImportScreenLimit(Level level, ABImportContext context)
        {
            if (level.Game.Events.ScreenLimits.Count > 0) return;

            level.Game.Events.ScreenLimits.Add(new ScreenLimitKey(
                new ScreenLimitFixed(new ScreenAspect(SourceAspectWidth, SourceAspectHeight)),
                FrameRules.MinFrame));

            context.Report.Info("screen_limit_source_aspect",
                $"Afterbeat only ever runs at {SourceAspectWidth}:{SourceAspectHeight}, so the level was authored for that frame and is pinned to it. Without the pin a wider screen would show what its author left outside it.",
                null);
        }

        /// <summary> The one frame Afterbeat runs at, and therefore the one every level was
        /// authored inside. </summary>
        public const int SourceAspectWidth = 16;
        public const int SourceAspectHeight = 9;

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

        // An effect's colour is an index into the theme's nine effect colours - except when it is
        // 9, which is one past the last of them and means "no theme colour at all". The source game
        // spells that out per effect (LSEffectsManager.UpdateBloomColor / UpdateVignetteColor /
        // UpdateGradientColorA|B: `_col.x == 9f ? LSColors.white : LiveTheme.GetEffectColor(...)`),
        // and the fallback is NOT the same colour for all of them - a bloom tints white, a vignette
        // darkens black. It is also the value an absent component reads as, since each of those
        // reads is GetVal(i, 9f).
        //
        // Clamping it into the palette instead - which is what a plain index import does - hands
        // every untouched effect in a converted level the theme's ninth effect colour, and every
        // real Afterbeat level writes 9 on effects its author never opened.

        /// <summary> The one index that is not a palette entry. </summary>
        public const int EffectColorNone = 9;

        private static IColor4 EffectColor(VgdEventKeyframe key, int index, Color4Value none,
            ABImportContext context, string path)
        {
            var paletteIndex = (int)key.GetFloat(index, EffectColorNone);
            if (paletteIndex == EffectColorNone) return none;

            return ABColorMap.Import(paletteIndex, 1f, ABPalette.Effects,
                context.ReferenceTheme, context.Report, path);
        }
    }
}
