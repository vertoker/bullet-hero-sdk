using System;
using System.Collections.Generic;
using BH.SDK.Interop.AfterBeat.Models;
using BH.SDK.Generators.Modifiers;
using BH.SDK.Models.Data;
using BH.SDK.Models.Keyframes;
using BH.SDK.Models.Objects;
using BH.SDK.Models.Primitives;
using BH.SDK.Models.Values;
using BH.SDK.Rules;

namespace BH.SDK.Interop.AfterBeat.Import
{
    // A .vgp holds template objects; a .vgd prefab_objects entry PLACES one. Both concepts exist
    // here too, so this is one of the few parts of the format that crosses structurally rather than
    // by approximation.
    //
    // What does NOT happen here is materialization. A placement in this format draws nothing until
    // PrefabMaterializer copies the template's objects into the hosting scope, and that service
    // lives in the consuming Unity project, not in the SDK. So an imported level arrives with real
    // templates and real placements and an empty ObjectIds table, and the host is expected to
    // resync once after the import. An import used somewhere without a materializer produces a
    // level whose prefabs are present in the file and invisible on screen - which is the same thing
    // that happens to a placement created by hand, so it is a known state rather than a broken one.
    //
    // The template gets its OWN ObjectId namespace, minted by the Prefab itself: ids are
    // scope-relative in this format, so a template object and a level object may share the number 1
    // without being related.

    /// <summary> Afterbeat prefabs and their placements. </summary>
    public static class ABPrefabImporter
    {
        /// <summary> One .vgp - or one entry of .vgd prefabs[] - into a template. </summary>
        public static Prefab ImportTemplate(VgpPrefab source, ABOptions options,
            InteropReport report, IDictionary<ShapeId, CompositeShape> shapes,
            ThemeData referenceTheme, string path)
        {
            if (source == null) return null;

            var prefab = new Prefab
            {
                PrefabId = ABIdMap.ToPrefabId(string.IsNullOrEmpty(source.Id) ? source.Name : source.Id),
                Name = string.IsNullOrEmpty(source.Name) ? source.Id ?? string.Empty : source.Name,
            };

            // A Prefab is one of the two places in this format where the scope and the id counter
            // are the SAME object - at level scope they are Level.Game and Level.Settings.
            var context = new ABImportContext(options, report, prefab, prefab, shapes)
            {
                ReferenceTheme = referenceTheme,
            };

            ABObjectImporter.ImportAll(source.Objects, context, $"{path}.objs");
            prefab.FrameDuration = MeasureDuration(prefab);

            if (!string.IsNullOrEmpty(source.Preview))
                report?.Dropped("prefab_preview",
                    "Afterbeat prefabs carry a preview image; this format has no field for one and it is not imported.",
                    path);

            // A template's own placements. Not imported: materializing a placement is the consuming
            // host's job (see this file's header), and a nested one needs it to recurse into a
            // template it has already materialized. Reported rather than dropped silently, because
            // a template carrying one draws visibly less than it should.
            if (source.Placements is { Count: > 0 })
                report?.Deferred("prefab_nested_placements",
                    "Some Afterbeat prefabs place other prefabs inside themselves; nested placements are not imported yet, so those templates arrive missing that content.",
                    path);

            return prefab;
        }

        /// <summary> Records a template's lead time so the placements can apply it. Separate from
        /// <see cref="ImportTemplate"/> because a template imported on its own - a bare .vgp - has
        /// no placement to apply it to and no context to record it in. </summary>
        public static void RegisterLeadTime(VgpPrefab source, Prefab prefab, ABImportContext context)
        {
            if (source == null || prefab == null || context == null) return;
            if (source.Offset != 0f) context.PrefabLeadTimes[prefab.PrefabId] = source.Offset;
        }

        /// <summary> One .vgd prefab_objects entry into a placement in the hosting scope.
        /// <paramref name="placementIndex"/> is its position in the source list, which is what gives
        /// it a timeline row of its own. </summary>
        public static PrefabObject ImportPlacement(VgdPrefabPlacement source,
            ABImportContext context, int levelFrameDuration, string path,
            IReadOnlyDictionary<PrefabId, Prefab> templates = null, int placementIndex = 0)
        {
            if (source == null) return null;

            var prefabId = string.IsNullOrEmpty(source.PrefabId)
                ? PrefabId.Null
                : ABIdMap.ToPrefabId(source.PrefabId);

            var placement = new PrefabObject
            {
                ObjectId = context.Mint(source.Id),
                PrefabId = prefabId,
                Name = context.Options.KeepObjectNames ? source.Id ?? string.Empty : string.Empty,
                Active = true,
                Span = ResolveSpan(source, context, prefabId, levelFrameDuration, templates),
                Layer = ResolveLayer(context, placementIndex, path),
                ParentObjectId = context.ResolveParent(source.ParentId, path),
            };

            // See VgdPrefabPlacement.RepeatCount: the source game stores it and draws nothing from
            // it, so neither does this - but a level author who set it meant something by it.
            if (source.RepeatCount > 0)
                context.Report.Deferred("placement_repeat",
                    "Some prefab placements are marked to repeat; the source game does not draw the repetitions either, so they were imported as the single placement they render as.",
                    path);

            var x = source.GetValue(VgdPrefabPlacement.TrackIndex.Position, 0);
            var y = source.GetValue(VgdPrefabPlacement.TrackIndex.Position, 1);
            var width = source.GetValue(VgdPrefabPlacement.TrackIndex.Scale, 0, 1f);
            var height = source.GetValue(VgdPrefabPlacement.TrackIndex.Scale, 1, 1f);
            var rotation = source.GetValue(VgdPrefabPlacement.TrackIndex.Rotation, 0);

            placement.Positions.Add(new PosKey(new Vector2Value(x, y), FrameRules.MinFrame));
            placement.Scales.Add(new ScaKey(new Vector2Value(width, height), FrameRules.MinFrame));
            placement.Rotations.Add(new AngleKey(
                new FloatValue(rotation * ABValueMap.DegreesToRadians), FrameRules.MinFrame));

            if (string.IsNullOrEmpty(source.PrefabId))
                context.Report.Approximated("placement_without_prefab",
                    "Some prefab placements name no prefab; Afterbeat removes those on load, and they were imported as empty placements.",
                    path);

            return placement;
        }

        // A placement's own start is what times everything inside it: the template's objects are
        // authored relative to the placement, and the materializer shifts every copy by this span's
        // start frame. Reading it as zero puts an entire prefab library in the first second of the
        // level, which is what happened for as long as the placement had no time field at all.
        //
        // The template's LEAD TIME is part of that start rather than something lost with the
        // template: the source game spawns every copied object at `placement.t - prefab.Offset +
        // its own st` (ObjectManager.AddPrefabToLevel), so a lead time of half a second means the
        // whole placement plays half a second EARLIER, and nothing else about it changes. Which is
        // exactly a shift of this span - so it crosses, and only a placement pushed before frame
        // zero by one loses anything (the clamp in FromFrames, matching the source game's own).
        //
        // The length is the TEMPLATE's, not the level's. Spanning the level instead would work
        // (a child must lie inside its parent, and a level-long parent cuts nothing short), but it
        // makes every placement's clip cover the whole timeline, which is unreadable in the editor
        // and useless to Trim/Blade. A placement whose template is missing keeps the old behaviour,
        // since there is no length to take.
        private static FrameSpan ResolveSpan(VgdPrefabPlacement source, ABImportContext context,
            PrefabId prefabId, int levelFrameDuration, IReadOnlyDictionary<PrefabId, Prefab> templates)
        {
            context.PrefabLeadTimes.TryGetValue(prefabId, out var leadTime);
            var startFrame = ABTimeMap.ToFrame(source.StartTime - leadTime, context.Options.Framerate);

            var duration = levelFrameDuration;
            if (templates != null && templates.TryGetValue(prefabId, out var template) && template != null)
                duration = template.FrameDuration;

            return ABTimeMap.FromFrames(startFrame, startFrame + duration);
        }

        // Layer is parent-relative here and a placement's materialized children hang off it, so one
        // number moves a whole subtree - which is the cheapest way to give the several thousand
        // objects a prefab-heavy level materializes into more than a single timeline row.
        //
        // A level with more placements than there are layers left above the base wraps rather than
        // piling the remainder on the last one. Wrapping puts two placements on one row, which is
        // exactly what the level looked like before any of this; being told is the point.
        private static int ResolveLayer(ABImportContext context, int placementIndex, string path)
        {
            var baseLayer = context.HighestContentLayer
                            + Math.Max(1, context.Options.PlacementLayerOffset);

            var room = ValueRules.MaxLayer - baseLayer + 1;
            if (room <= 0) return ValueRules.MaxLayer;

            if (placementIndex >= room)
                context.Report.Approximated("placement_layers_wrapped",
                    $"This level has more prefab placements than the {room} draw-order layers left above their base, so some of them share a layer.",
                    path);

            return baseLayer + placementIndex % room;
        }

        // A template's own length is not stored in a .vgp, so it is measured: the last frame any of
        // its objects reaches. A template shorter than its content would clip the content.
        //
        // KEYFRAMES count, not just spans, and that is the part that is easy to miss. A keyframe's
        // frame is local to its object's span and may sit past the span's own end - under the source
        // format's "die on the last keyframe" rule the final key lands exactly ON the end boundary,
        // and an object cut short by a Fixed Time autokill can carry keys well beyond it. Measuring
        // spans alone produced templates whose own keyframes were out of range the moment the level
        // was validated, on real content, in numbers (32 findings on one level).
        private static int MeasureDuration(Prefab prefab)
        {
            var end = FrameRules.MinFrameDuration;
            foreach (var pair in prefab.Objects)
            {
                var obj = pair.Value;
                if (obj == null) continue;

                if (obj.Span.EndFrame > end) end = obj.Span.EndFrame;

                foreach (var track in ObjectTracks.Of(obj, ObjectTrackMask.All))
                for (var i = 0; i < track.Count; i++)
                {
                    // +1 because a duration is a COUNT: a key on frame 32 needs a length of 33.
                    var reach = obj.Span.StartFrame + track.FrameAt(i) + 1;
                    if (reach > end) end = reach;
                }
            }
            return Math.Clamp(end, FrameRules.MinFrameDuration, FrameRules.MaxFrameDuration);
        }
    }
}
