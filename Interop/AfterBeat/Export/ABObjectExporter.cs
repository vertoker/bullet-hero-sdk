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
using BH.SDK.Rules;

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
    public static class ABObjectExporter
    {
        /// <summary> Exports every object of a scope, skipping the ones with no representation. </summary>
        public static List<VgdObject> ExportAll(ABExportContext context, string pathPrefix)
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
        public static VgdObject Export(RectObject source, ABExportContext context, string path)
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
                    // A placement is a reference, not content: the objects it materialized into are
                    // ordinary members of this same scope and are exported on their own. Writing it
                    // as well would give Afterbeat a second copy to expand - see the level
                    // exporter's ExportPlacements.
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
                Id = ABExportContext.ToSourceId(source.ObjectId),
                Name = source.Name ?? string.Empty,
                ParentId = context.ToParentId(source.ParentObjectId),
            };

            ApplyDrawOrder(context.GetEffectiveLayer(source), target);

            ABTimeMap.ExportSpan(source.Span, framerate, target);
            ApplyShape(source, target, context, path);
            ApplyOrigin(source, target);

            ExportPositions(source, target, framerate, context, path);
            ExportSizes(source, target, framerate, context, path);
            ExportRotations(source, target, framerate, context, path);
            ExportColors(source, target, context, path);
            ReportUnsupported(source, report, path);

            return target;
        }

        // The inverse of the importer's OnlyDepth mapping - the one of the four layer modes that is
        // a bijection - so a level that came from Afterbeat under it goes back unchanged, and one
        // authored here lands where the same layer would have drawn.
        //
        // Which BAND a layer belongs to is decided the same way the import decided it: layer 0 is
        // the frontmost depth of the Default band (the source game's player sits just under it),
        // the whole 61-wide stretch above that is AbovePlayer, the 61 below is Default's own rest,
        // and everything under THAT is Background. Clamped at both ends, since this format has 2001
        // layers to spend and Afterbeat has 183 - a level using the whole range loses ordering at
        // the extremes rather than everywhere.
        private static void ApplyDrawOrder(int effectiveLayer, VgdObject target)
        {
            const int span = ABLayerMap.DepthSpan;

            var band = effectiveLayer >= 1 ? ABRenderLayer.AbovePlayer
                : effectiveLayer > -span ? ABRenderLayer.Default
                : ABRenderLayer.Background;

            var frontmost = band switch
            {
                ABRenderLayer.AbovePlayer => span,
                ABRenderLayer.Default => 0,
                _ => -span,
            };

            target.RenderLayer = (int)band;
            target.Depth = Math.Clamp(frontmost - effectiveLayer,
                VgdObject.MinDepth, VgdObject.MaxDepth);
        }

        private static void ApplyShape(RectObject source, VgdObject target,
            ABExportContext context, string path)
        {
            switch (source)
            {
                case TextObject text:
                    target.Shape = (int)ABShape.Text;
                    target.ObjectType = (int)ABObjectType.NoHit;
                    target.Text = ReadText(text, context, path);
                    return;

                case ShapeObject shape:
                {
                    var (main, option) = ABShapeMap.Export(shape.ShapeId, context.Report, path);
                    target.Shape = main;
                    target.ShapeOption = option;
                    // Normal rather than Hit: a real level writes 0 for its hitting objects, and
                    // the documented 4 is Solid in the numbering those files use - an object that
                    // pushes the player rather than one that merely hurts them.
                    target.ObjectType = shape.ColliderId.IsEnabled()
                        ? (int)ABObjectType.Normal
                        : (int)ABObjectType.NoHit;

                    if (shape.ColliderId.IsEnabled() && shape.ColliderId != shape.ShapeId)
                        context.Report.Approximated("collider_differs",
                            "Afterbeat hits objects with the shape they are drawn as; objects whose collider differs from their shape export with the drawn one.",
                            path);
                    return;
                }

                default:
                    // AlphaEmpty (6) rather than Empty (3): both mean "no geometry" over there, and
                    // 6 is the one real levels are written with - 3 appears in no file measured.
                    target.ObjectType = (int)ABObjectType.AlphaEmpty;
                    return;
            }
        }

        private static string ReadText(TextObject source, ABExportContext context, string path)
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
                Import.ABObjectImporter.DefaultPivot - literal.X,
                Import.ABObjectImporter.DefaultPivot - literal.Y);
        }

        #region Tracks

        private static void ExportPositions(RectObject source, VgdObject target, int framerate,
            ABExportContext context, string path)
        {
            var track = target.GetTrack(VgdObject.TrackIndex.Move);
            if (track == null || source.Positions == null) return;

            foreach (var key in Sorted(source.Positions))
            {
                var (x, y) = ABValueMap.ExportVector(key.Pos, context.Report, path);
                track.Keyframes.Add(NewKeyframe(key.Frame, key.Ease, framerate, x, y));
            }
        }

        private static void ExportSizes(RectObject source, VgdObject target, int framerate,
            ABExportContext context, string path)
        {
            var track = target.GetTrack(VgdObject.TrackIndex.Scale);
            if (track == null) return;

            // Text goes the way it came in: over there a text object's scale is the only thing
            // sizing its glyphs, while here that is Scale and Size is only the block they lay out
            // in - a block this converter estimated on the way in and that means nothing on the
            // far side. See ABObjectImporter.ApplyTextSize.
            var isText = source is TextObject;
            var exported = isText ? source.Scales : source.Sizes;
            var dropped = isText ? source.Sizes : source.Scales;
            if (exported == null) return;

            foreach (var key in Sorted(exported))
            {
                var (x, y) = ABValueMap.ExportVector(key.Scale, context.Report, path);
                track.Keyframes.Add(NewKeyframe(key.Frame, key.Ease, framerate, x, y));
            }

            if (!isText && dropped is { Count: > 0 })
                context.Report.Approximated("scale_track_dropped",
                    "Afterbeat has one size per object; this format's separate scale multiplier has no field there and is not exported.",
                    path);
        }

        // The one track that cannot be exported keyframe by keyframe - see this file's header.
        private static void ExportRotations(RectObject source, VgdObject target, int framerate,
            ABExportContext context, string path)
        {
            var track = target.GetTrack(VgdObject.TrackIndex.Rotate);
            if (track == null || source.Rotations == null) return;

            var previous = 0f;
            foreach (var key in Sorted(source.Rotations))
            {
                var radians = ABValueMap.ExportFloat(key.Angle, context.Report, path);
                var delta = ABValueMap.DifferentiateRotation(radians, ref previous);
                track.Keyframes.Add(NewKeyframe(key.Frame, key.Ease, framerate, delta));
            }
        }

        private static void ExportColors(RectObject source, VgdObject target,
            ABExportContext context, string path)
        {
            var track = target.GetTrack(VgdObject.TrackIndex.Color);
            if (track == null) return;

            var framerate = context.Options.Framerate;

            switch (source)
            {
                case ShapeObject shape when shape.Colors != null:
                {
                    var gradient = false;
                    foreach (var key in SortedKeys(shape.Colors))
                    {
                        var exported = ABColorMap.ExportKey(key, ABPalette.Objects,
                            context.ReferenceTheme, context.Report, path);
                        gradient |= exported.IsGradient;
                        track.Keyframes.Add(NewKeyframe(key.Frame, key.Ease, framerate,
                            exported.StartIndex, ToSourceOpacity(exported.Opacity), exported.EndIndex));
                    }

                    // The type lives on the object, so it is decided once the whole track has been
                    // read - a gradient anywhere in it means the object has one.
                    if (gradient) target.GradientType = (int)ABGradientType.Linear;
                    return;
                }

                case TextObject text when text.Colors != null:
                    foreach (var key in Sorted(text.Colors))
                    {
                        var (index, opacity) = ABColorMap.Export(key.Value,
                            ABPalette.Objects, context.ReferenceTheme, context.Report, path);
                        track.Keyframes.Add(NewKeyframe(key.Frame, key.Ease, framerate,
                            index, ToSourceOpacity(opacity), index));
                    }
                    return;
            }
        }

        /// <summary> Alpha back to the percentage Afterbeat stores - see the importer's own
        /// OpacityScale, which is the half of this pair that actually bit. </summary>
        private static float ToSourceOpacity(float alpha)
            => alpha * Import.ABObjectImporter.OpacityScale;

        #endregion

        #region Shared

        private static VgdKeyframe NewKeyframe(int localFrame, EaseType ease,
            int framerate, params float[] values)
            => new()
            {
                Time = ABTimeMap.ToSeconds(localFrame, framerate),
                Ease = ABEaseMap.Export(ease),
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

            if (source is TextObject text)
            {
                if (text.Fillments is { Count: > 0 } || text.Appearings is { Count: > 0 })
                    report.Dropped("text_effects",
                        "Afterbeat has no per-character fill or appearing effects; those tracks are not exported.",
                        path);

                ReportTextSetup(text, report, path);
            }

            if (source is ShapeObject shape && shape.ShaderType != ShaderType.Auto)
                report.Dropped("shader_type",
                    "Afterbeat decides how a shape is drawn on its own; an explicitly opaque or transparent shape exports without that choice.",
                    path);
        }

        // An Afterbeat text object is a string and a colour. Everything this format wraps around one
        // - which font it is set in, how big, how it is aligned, whether it wraps - has no field
        // there at all. None of it is a conversion that could be got wrong, which is exactly why it
        // used to leave no trace: an author sent a level away and learned about it by looking.
        private static void ReportTextSetup(TextObject source, InteropReport report, string path)
        {
            if (source.FontResourceId.IsValid())
                report.Dropped("text_font",
                    "Afterbeat text uses the game's own font; the font this level set is not exported.", path);

            if (source.FontSizes is { Count: > 0 })
                report.Dropped("text_font_size",
                    "Afterbeat text has no font size of its own; this level's size track is not exported.", path);

            if (!string.IsNullOrEmpty(source.AppearingMask))
                report.Dropped("text_appearing_mask",
                    "Afterbeat has no appearing mask on text; it is not exported.", path);

            if (source.WordWrap != TextRules.WordWrap_Default
                || source.HorizontalAlignment != TextRules.HorizontalAlignment_Default
                || source.VerticalAlignment != TextRules.VerticalAlignment_Default)
                report.Dropped("text_layout",
                    "Afterbeat text is centred and never wraps; this level's alignment and word wrap are not exported.",
                    path);
        }

        #endregion
    }
}
