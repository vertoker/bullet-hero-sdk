using System;
using System.Collections.Generic;
using BH.SDK.Interop.AfterBeat.Models;
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
    public static class AfterBeatPrefabImporter
    {
        /// <summary> One .vgp - or one entry of .vgd prefabs[] - into a template. </summary>
        public static Prefab ImportTemplate(VgpPrefab source, AfterBeatOptions options,
            InteropReport report, IDictionary<ShapeId, CompositeShape> shapes,
            ThemeData referenceTheme, string path)
        {
            if (source == null) return null;

            var prefab = new Prefab
            {
                PrefabId = AfterBeatIdMap.ToPrefabId(string.IsNullOrEmpty(source.Id) ? source.Name : source.Id),
                Name = string.IsNullOrEmpty(source.Name) ? source.Id ?? string.Empty : source.Name,
            };

            // A Prefab is one of the two places in this format where the scope and the id counter
            // are the SAME object - at level scope they are Level.Game and Level.Settings.
            var context = new AfterBeatImportContext(options, report, prefab, prefab, shapes)
            {
                ReferenceTheme = referenceTheme,
            };

            AfterBeatObjectImporter.ImportAll(source.Objects, context, $"{path}.objs");
            prefab.FrameDuration = MeasureDuration(prefab);

            if (!string.IsNullOrEmpty(source.Preview))
                report?.Dropped("prefab_preview",
                    "Afterbeat prefabs carry a preview image; this format has no field for one and it is not imported.",
                    path);

            if (source.Offset != 0f)
                report?.Dropped("prefab_lead_time",
                    "Afterbeat prefabs carry a lead time that shifts a whole placement in time; this format has no equivalent, so placements start where they sit.",
                    path);

            return prefab;
        }

        /// <summary> One .vgd prefab_objects entry into a placement in the hosting scope. </summary>
        public static PrefabObject ImportPlacement(VgdPrefabPlacement source,
            AfterBeatImportContext context, int levelFrameDuration, string path)
        {
            if (source == null) return null;

            var placement = new PrefabObject
            {
                ObjectId = context.Mint(source.Id),
                PrefabId = string.IsNullOrEmpty(source.PrefabId)
                    ? PrefabId.Null
                    : AfterBeatIdMap.ToPrefabId(source.PrefabId),
                Name = context.Options.KeepObjectNames ? source.Id ?? string.Empty : string.Empty,
                Active = true,

                // A placement carries no time of its own in this format's transcription of .vgd, so
                // it spans the level and lets the template's own objects decide when anything
                // plays. A child's span has to lie inside its parent's to play at all, and this is
                // the only span that cannot cut one short.
                Span = new FrameSpan(FrameRules.MinFrame,
                    Math.Max(FrameRules.MinFrameDuration, levelFrameDuration)),
                Layer = ValueRules.DefaultLayer,
            };

            var x = source.GetValue(VgdPrefabPlacement.TrackIndex.Position, 0);
            var y = source.GetValue(VgdPrefabPlacement.TrackIndex.Position, 1);
            var width = source.GetValue(VgdPrefabPlacement.TrackIndex.Scale, 0, 1f);
            var height = source.GetValue(VgdPrefabPlacement.TrackIndex.Scale, 1, 1f);
            var rotation = source.GetValue(VgdPrefabPlacement.TrackIndex.Rotation, 0);

            placement.Positions.Add(new PosKey(new Vector2Value(x, y), FrameRules.MinFrame));
            placement.Scales.Add(new ScaKey(new Vector2Value(width, height), FrameRules.MinFrame));
            placement.Rotations.Add(new AngleKey(
                new FloatValue(rotation * AfterBeatValueMap.DegreesToRadians), FrameRules.MinFrame));

            if (string.IsNullOrEmpty(source.PrefabId))
                context.Report.Approximated("placement_without_prefab",
                    "Some prefab placements name no prefab; Afterbeat removes those on load, and they were imported as empty placements.",
                    path);

            return placement;
        }

        // A template's own length is not stored in a .vgp, so it is measured: the last frame any of
        // its objects reaches. A template shorter than its content would clip the content.
        private static int MeasureDuration(Prefab prefab)
        {
            var end = FrameRules.MinFrameDuration;
            foreach (var pair in prefab.Objects)
            {
                var objectEnd = pair.Value?.Span.EndFrame ?? 0;
                if (objectEnd > end) end = objectEnd;
            }
            return Math.Clamp(end, FrameRules.MinFrameDuration, FrameRules.MaxFrameDuration);
        }
    }
}
