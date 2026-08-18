using System;
using BH.SDK.Interop.AfterBeat.Models;
using BH.SDK.Models.Enums;
using BH.SDK.Models.Keyframes;
using BH.SDK.Models.Objects;
using BH.SDK.Models.Primitives;
using BH.SDK.Models.Values;
using BH.SDK.Rules;

namespace BH.SDK.Interop.AfterBeat.Import
{
    // Afterbeat has a background subsystem: five layers of objects that never collide, draw behind
    // everything, take their colour from a palette of their own, and animate by looping towards a
    // second transform forever. This project has no such subsystem and is not growing one - the
    // whole thing is expressible as ordinary objects, which is the engine-not-a-game call the root
    // CLAUDE.md asks for.
    //
    // So each parallax object becomes a collider-less object on a negative layer, and its loop is
    // BAKED into keyframes. Baking is what makes this lossy in a way worth understanding: a loop is
    // endless and a keyframe track is not, so the bake covers the level's own length and stops. The
    // per-track keyframe cap is the real limit - a fast loop over a long level wants thousands of
    // keys and gets MaxObjectKeys, so a long level's background loops fewer times than it did.
    //
    // What is NOT carried: depth of field (no equivalent), per-layer depth as a parallax FACTOR
    // (this format has no camera-relative depth, so the layers only order themselves), and the
    // layer's palette override is applied at import rather than kept live.

    /// <summary> Afterbeat's five background layers into ordinary objects. </summary>
    public static class ABParallaxImporter
    {
        // The background takes ONE LAYER PER OBJECT, counted through the whole block rather than a
        // fixed band per source layer. A fixed band spends the same draw order whether the layer
        // holds twenty objects or none, so a level with four background objects still pushed the
        // timeline's floor fifty rows down and left the background sitting alone at the bottom of a
        // shot mostly made of empty rows. Counting objects makes the band exactly as tall as there
        // is something to put in it, and the far end of the range stays free for content.

        /// <summary> Reads the whole parallax block into the context's scope. </summary>
        public static void ImportAll(VgdParallaxSettings settings, ABImportContext context,
            int levelFrameDuration, string path)
        {
            if (settings?.Layers == null || context?.Scope?.Objects == null) return;

            var report = context.Report;

            if (settings.DepthOfFieldActive)
                report.Dropped("parallax_depth_of_field",
                    "Afterbeat's background depth of field has no equivalent here and is not imported.", path);

            // Below whatever the objects ended up occupying rather than below a fixed number: the
            // Background band moves with the level and with the layer mode, and a fixed base landed
            // in the middle of it - a backdrop drawn in front of the content standing on it.
            var baseLayer = context.LowestContentLayer - Math.Max(1, context.Options.ParallaxLayerOffset);

            var drawn = 0;
            var clamped = false;

            for (var layerIndex = 0; layerIndex < settings.Layers.Count; layerIndex++)
            {
                var layer = settings.Layers[layerIndex];
                if (layer?.Objects == null) continue;

                for (var i = 0; i < layer.Objects.Count; i++)
                {
                    var source = layer.Objects[i];
                    if (source == null) continue;

                    var objectLayer = baseLayer - drawn;
                    if (objectLayer < ValueRules.MinLayer) clamped = true;

                    var imported = Import(source, layer, objectLayer, context, levelFrameDuration,
                        $"{path}.l[{layerIndex}].o[{i}]");
                    if (imported == null) continue;

                    context.Scope.Objects[imported.ObjectId] = imported;
                    drawn++;
                }
            }

            if (clamped)
                report.Approximated("parallax_layers_clamped",
                    "This level's background has more objects than there is draw order left below its base layer, so the deepest of them share a layer.",
                    path);
        }

        private static RectObject Import(VgdParallaxObject source, VgdParallaxLayer layer,
            int baseLayer, ABImportContext context, int levelFrameDuration, string path)
        {
            var report = context.Report;
            var framerate = context.Options.Framerate;

            var shapeId = ABShapeMap.Import(source.Shape?.Shape ?? 0, source.Shape?.ShapeOption ?? 0,
                context.Shapes, report, path);

            var target = new ShapeObject
            {
                // A background object cannot hit the player in Afterbeat either, so this is not an
                // approximation - it is the same rule expressed the way this format expresses it.
                ObjectId = context.Mint(source.Id),
                ShapeId = shapeId,
                ColliderId = ShapeId.Null,
                ShaderType = ShaderType.Auto,
                Active = true,
                Name = context.Options.KeepObjectNames ? source.Id ?? string.Empty : string.Empty,
                Layer = Math.Clamp(baseLayer, ValueRules.MinLayer, ValueRules.MaxLayer),
                Span = new FrameSpan(FrameRules.MinFrame,
                    Math.Max(FrameRules.MinFrameDuration, levelFrameDuration)),
            };

            // The layer's own colour overrides the object's, which is what the format says; nothing
            // here can express "follow whichever the layer currently says".
            var paletteIndex = layer.Color != 0 ? layer.Color : source.Color;
            var color = ABColorMap.Import(paletteIndex, 1f, ABPalette.Parallax,
                context.ReferenceTheme, report, path);
            target.Colors.Add(new Color4Key(color, FrameRules.MinFrame));

            Bake(source, target, context, levelFrameDuration, framerate, path);
            return target;
        }

        // Two keyframes per cycle - the base transform and the loop's target - repeated until the
        // level ends or the track fills up. Both ends of every cycle are written so the motion
        // returns rather than drifting, which is what a loop does.
        private static void Bake(VgdParallaxObject source, ShapeObject target,
            ABImportContext context, int levelFrameDuration, int framerate, string path)
        {
            var transform = source.Transform ?? new VgdParallaxTransform();
            var animation = source.Animation ?? new VgdParallaxAnimation();

            var basePosition = new Vector2Value(transform.Position?.X ?? 0f, transform.Position?.Y ?? 0f);
            var baseScale = new Vector2Value(transform.Scale?.X ?? 0f, transform.Scale?.Y ?? 0f);
            var baseRotation = transform.Rotation * ABValueMap.DegreesToRadians;

            // A loop shorter than two frames has no room for a there-and-back pair at this
            // framerate, and writing one anyway puts two keyframes on one frame, which the format
            // forbids outright. It is a static object as far as this import is concerned.
            var loopFrames = ABTimeMap.ToFrame(animation.Length, framerate);
            if (!animation.IsActive || loopFrames < 2)
            {
                target.Positions.Add(new PosKey(basePosition, FrameRules.MinFrame));
                target.Sizes.Add(new ScaKey(baseScale, FrameRules.MinFrame));
                target.Rotations.Add(new AngleKey(new FloatValue(baseRotation), FrameRules.MinFrame));
                return;
            }

            var loopPosition = new Vector2Value(animation.Position?.X ?? 0f, animation.Position?.Y ?? 0f);
            var loopScale = new Vector2Value(animation.Scale?.X ?? 0f, animation.Scale?.Y ?? 0f);
            var loopRotation = animation.Rotation * ABValueMap.DegreesToRadians;

            var periodFrames = Math.Max(2, loopFrames);
            var startFrame = Math.Max(0, ABTimeMap.ToFrame(animation.Delay, framerate));
            var maxKeys = context.Options.MaxParallaxLoopKeys;

            var truncated = false;
            var frame = startFrame;
            var written = 0;

            // The half-cycle is where the loop reaches its target; the full cycle is where it is
            // back. Writing both is what makes it a loop rather than a one-way ramp.
            while (frame < levelFrameDuration)
            {
                if (written + 2 > maxKeys)
                {
                    truncated = true;
                    break;
                }

                var half = frame + periodFrames / 2;
                var end = frame + periodFrames;

                if (animation.LoopPosition)
                {
                    target.Positions.Add(new PosKey(basePosition.Copy(), frame, FrameRules.DefaultEase));
                    if (half < levelFrameDuration)
                        target.Positions.Add(new PosKey(loopPosition.Copy(), half, FrameRules.DefaultEase));
                }
                if (animation.LoopScale)
                {
                    target.Sizes.Add(new ScaKey(baseScale.Copy(), frame, FrameRules.DefaultEase));
                    if (half < levelFrameDuration)
                        target.Sizes.Add(new ScaKey(loopScale.Copy(), half, FrameRules.DefaultEase));
                }
                if (animation.LoopRotation)
                {
                    target.Rotations.Add(new AngleKey(new FloatValue(baseRotation), frame, FrameRules.DefaultEase));
                    if (half < levelFrameDuration)
                        target.Rotations.Add(new AngleKey(new FloatValue(loopRotation), half, FrameRules.DefaultEase));
                }

                written += 2;
                frame = end;
            }

            // A switch that was off leaves its track empty, which this format reads as "use the
            // engine default" rather than as "stay where you were" - so the base value is written.
            if (!animation.LoopPosition) target.Positions.Add(new PosKey(basePosition, FrameRules.MinFrame));
            if (!animation.LoopScale) target.Sizes.Add(new ScaKey(baseScale, FrameRules.MinFrame));
            if (!animation.LoopRotation)
                target.Rotations.Add(new AngleKey(new FloatValue(baseRotation), FrameRules.MinFrame));

            if (truncated)
                context.Report.Approximated("parallax_loop_truncated",
                    $"A background loop needs more than {maxKeys} keyframes to cover the whole level; it was baked as far as the limit allows and stops after that.",
                    path);
        }
    }
}
