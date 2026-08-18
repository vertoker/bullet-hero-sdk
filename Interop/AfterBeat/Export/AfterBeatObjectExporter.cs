using System;
using System.Collections.Generic;
using BH.SDK.Interop.AfterBeat.Models;
using BH.SDK.Models.Enums;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Keyframes;
using BH.SDK.Models.Keyframes;
using BH.SDK.Models.Objects;
using BH.SDK.Models.Primitives;
using BH.SDK.Models.Values;

namespace BH.SDK.Interop.AfterBeat.Export
{
    // Every conversion the importer does, backwards, plus the ones that only exist going out:
    //
    //   Rotation is differentiated back into per-keyframe degree deltas, which needs the track in
    //   order and a running previous value - the exact mirror of accumulating it on the way in.
    //   A track that was reordered in between would export a different animation, so it is sorted
    //   first: the format does not guarantee keyframe order and this direction depends on it.
    //
    //   Effects, anchors and per-corner colours have nowhere to go. Each is reported once rather
    //   than per object, because a level using effects uses a lot of them.
    //
    //   Afterbeat has no "active" flag on an object, so an inactive object would silently become a
    //   visible one. It is skipped instead and reported - a level that plays differently is worse
    //   than a level missing a decoration the author had already turned off.

    /// <summary> One <see cref="RectObject"/> into one Afterbeat object. </summary>
    public static class AfterBeatObjectExporter
    {
        /// <summary> Deepest colour Afterbeat draws. </summary>
        public const int MaxDepth = 60;

        /// <summary> Exports every object of a scope, skipping the ones with no representation. </summary>
        public static List<VgdObject> ExportAll(AfterBeatExportContext context, string pathPrefix)
        {
            var exported = new List<VgdObject>();
            if (context?.Scope?.Objects == null) return exported;

            foreach (var pair in context.Scope.Objects)
            {
                var source = pair.Value;
                if (source == null) continue;

                var path = $"{pathPrefix}[{pair.Key.value}]";
                var target = Export(source, context, path);
                if (target != null) exported.Add(target);
            }

            return exported;
        }

        /// <summary> One object, or null when it has no Afterbeat equivalent at all. </summary>
        public static VgdObject Export(RectObject source, AfterBeatExportContext context, string path)
        {
            if (source == null) return null;

            var report = context.Report;

            switch (source)
            {
                case EffectObject:
                    report.Dropped("effects",
                        "Afterbeat has no particle effects; effect objects are not exported.", path);
                    return null;
                case PrefabObject:
                    // Placements are exported by the level exporter into prefab_objects, which is a
                    // different array from objects - not a loss, just not this method's job.
                    return null;
            }

            if (!source.Active)
            {
                report.Dropped("inactive_objects",
                    "Afterbeat objects have no active switch; objects turned off here are not exported.", path);
                return null;
            }

            var framerate = context.Options.Framerate;
            var target = new VgdObject
            {
                Id = AfterBeatExportContext.ToSourceId(source.ObjectId),
                Name = source.Name ?? string.Empty,
                ParentId = context.ToParentId(source.ParentObjectId),
                Depth = ToDepth(context.GetEffectiveLayer(source)),
            };

            AfterBeatTimeMap.ExportSpan(source.Span, framerate, target);
            ApplyShape(source, target, context, path);
            ApplyOrigin(source, target);

            ExportPositions(source, target, framerate, context, path);
            ExportSizes(source, target, framerate, context, path);
            ExportRotations(source, target, framerate, context, path);
            ExportColors(source, target, context, path);
            ReportUnsupported(source, report, path);

            return target;
        }

        // The inverse of the importer's mapping, clamped into what Afterbeat accepts: it has 61
        // depths and this format has 2001 layers, so a level using the whole range loses ordering
        // at the extremes rather than everywhere.
        private static int ToDepth(int effectiveLayer)
            => Math.Clamp(VgdObject.DefaultDepth - effectiveLayer, 0, MaxDepth);

        private static void ApplyShape(RectObject source, VgdObject target,
            AfterBeatExportContext context, string path)
        {
            switch (source)
            {
                case TextObject text:
                    target.Shape = (int)AfterBeatShape.Text;
                    target.ObjectType = (int)AfterBeatObjectType.NoHit;
                    target.Text = ReadText(text, context, path);
                    return;

                case ShapeObject shape:
                {
                    var (main, option) = AfterBeatShapeMap.Export(shape.ShapeId, context.Report, path);
                    target.Shape = main;
                    target.ShapeOption = option;
                    target.ObjectType = shape.ColliderId.IsEnabled()
                        ? (int)AfterBeatObjectType.Hit
                        : (int)AfterBeatObjectType.NoHit;

                    if (shape.ColliderId.IsEnabled() && shape.ColliderId != shape.ShapeId)
                        context.Report.Approximated("collider_differs",
                            "Afterbeat hits objects with the shape they are drawn as; objects whose collider differs from their shape export with the drawn one.",
                            path);
                    return;
                }

                default:
                    target.ObjectType = (int)AfterBeatObjectType.Empty;
                    return;
            }
        }

        private static string ReadText(TextObject source, AfterBeatExportContext context, string path)
        {
            switch (source.Text)
            {
                case StringValue literal:
                    return literal.Value ?? string.Empty;
                case null:
                    return string.Empty;
                default:
                    context.Report.Approximated("text_localized",
                        "Afterbeat text is a single string; localized text exports as its default language.",
                        path);
                    return string.Empty;
            }
        }

        // Only the pivot's FIRST keyframe can cross - Afterbeat's origin is a static field, not a
        // track. Anything animating its pivot loses the animation, which is reported.
        private static void ApplyOrigin(RectObject source, VgdObject target)
        {
            if (source.Pivots == null || source.Pivots.Count == 0) return;

            var pivot = source.Pivots[0]?.Value;
            if (pivot is not Vector2Value literal) return;

            target.Origin = new VgdVector2(
                Import.AfterBeatObjectImporter.DefaultPivot - literal.X,
                Import.AfterBeatObjectImporter.DefaultPivot - literal.Y);
        }

        #region Tracks

        private static void ExportPositions(RectObject source, VgdObject target, int framerate,
            AfterBeatExportContext context, string path)
        {
            var track = target.GetTrack(VgdObject.TrackIndex.Move);
            if (track == null || source.Positions == null) return;

            foreach (var key in Sorted(source.Positions))
            {
                var (x, y) = AfterBeatValueMap.ExportVector(key.Pos, context.Report, path);
                track.Keyframes.Add(NewKeyframe(key.Frame, key.Ease, framerate, x, y));
            }
        }

        private static void ExportSizes(RectObject source, VgdObject target, int framerate,
            AfterBeatExportContext context, string path)
        {
            var track = target.GetTrack(VgdObject.TrackIndex.Scale);
            if (track == null || source.Sizes == null) return;

            foreach (var key in Sorted(source.Sizes))
            {
                var (x, y) = AfterBeatValueMap.ExportVector(key.Scale, context.Report, path);
                track.Keyframes.Add(NewKeyframe(key.Frame, key.Ease, framerate, x, y));
            }

            if (source.Scales is { Count: > 0 })
                context.Report.Approximated("scale_track_dropped",
                    "Afterbeat has one size per object; this format's separate scale multiplier has no field there and is not exported.",
                    path);
        }

        // The one track that cannot be exported keyframe by keyframe - see this file's header.
        private static void ExportRotations(RectObject source, VgdObject target, int framerate,
            AfterBeatExportContext context, string path)
        {
            var track = target.GetTrack(VgdObject.TrackIndex.Rotate);
            if (track == null || source.Rotations == null) return;

            var previous = 0f;
            foreach (var key in Sorted(source.Rotations))
            {
                var radians = AfterBeatValueMap.ExportFloat(key.Angle, context.Report, path);
                var delta = AfterBeatValueMap.DifferentiateRotation(radians, ref previous);
                track.Keyframes.Add(NewKeyframe(key.Frame, key.Ease, framerate, delta));
            }
        }

        private static void ExportColors(RectObject source, VgdObject target,
            AfterBeatExportContext context, string path)
        {
            var track = target.GetTrack(VgdObject.TrackIndex.Color);
            if (track == null) return;

            var framerate = context.Options.Framerate;

            switch (source)
            {
                case ShapeObject shape when shape.Colors != null:
                    foreach (var key in SortedKeys(shape.Colors))
                    {
                        var flattened = AfterBeatColorMap.Flatten(key, context.Report, path);
                        var (index, opacity) = AfterBeatColorMap.Export(flattened,
                            AfterBeatPalette.Objects, context.ReferenceTheme, context.Report, path);
                        track.Keyframes.Add(NewKeyframe(key.Frame, key.Ease, framerate,
                            index, opacity, index));
                    }
                    return;

                case TextObject text when text.Colors != null:
                    foreach (var key in Sorted(text.Colors))
                    {
                        var (index, opacity) = AfterBeatColorMap.Export(key.Value,
                            AfterBeatPalette.Objects, context.ReferenceTheme, context.Report, path);
                        track.Keyframes.Add(NewKeyframe(key.Frame, key.Ease, framerate,
                            index, opacity, index));
                    }
                    return;
            }
        }

        #endregion

        #region Shared

        private static VgdKeyframe NewKeyframe(int localFrame, EaseType ease,
            int framerate, params float[] values)
            => new()
            {
                Time = AfterBeatTimeMap.ToSeconds(localFrame, framerate),
                Ease = AfterBeatEaseMap.Export(ease),
                Values = new List<float>(values),
            };

        // The format guarantees keyframe frames are UNIQUE, not sorted, and both the rotation
        // differentiation and Afterbeat's own reader assume order. Sorting a copy leaves the model
        // alone - an export must not reorder the level it is exporting.
        private static IEnumerable<T> Sorted<T>(List<T> keyframes) where T : Keyframe
        {
            var copy = new List<T>(keyframes);
            copy.Sort((a, b) => a.Frame.CompareTo(b.Frame));
            return copy;
        }

        private static IEnumerable<IColor4X4Key> SortedKeys(List<IColor4X4Key> keyframes)
        {
            var copy = new List<IColor4X4Key>(keyframes);
            copy.Sort((a, b) => a.Frame.CompareTo(b.Frame));
            return copy;
        }

        private static void ReportUnsupported(RectObject source, InteropReport report, string path)
        {
            if (source.AnchorsMin is { Count: > 0 } || source.AnchorsMax is { Count: > 0 })
                report.Dropped("anchors",
                    "Afterbeat objects have no anchors; anchor tracks are not exported.", path);

            if (source.Pivots is { Count: > 1 })
                report.Approximated("pivot_animated",
                    "Afterbeat's object origin is static; an animated pivot exports as its first keyframe.",
                    path);

            if (source is TextObject text && (text.Fillments is { Count: > 0 } || text.Appearings is { Count: > 0 }))
                report.Dropped("text_effects",
                    "Afterbeat has no per-character fill or appearing effects; those tracks are not exported.",
                    path);
        }

        #endregion
    }
}
