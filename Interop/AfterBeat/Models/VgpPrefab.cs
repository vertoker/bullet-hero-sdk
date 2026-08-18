using System.Collections.Generic;
using Newtonsoft.Json;

namespace BH.SDK.Interop.AfterBeat.Models
{
    /// <summary>
    /// An Afterbeat prefab - a whole .vgp file, or one entry of .vgd prefabs[]. Its objects use the
    /// same shape as a level's own.
    /// </summary>
    public class VgpPrefab : ABNode
    {
        /// <summary> Present only inside .vgd prefabs[]; a standalone .vgp has no id. </summary>
        [JsonProperty(ABNames.PrefabId)]
        public string Id { get; set; } = string.Empty;

        [JsonProperty(ABNames.PrefabName)]
        public string Name { get; set; } = string.Empty;

        [JsonProperty(ABNames.PrefabDescription)]
        public string Description { get; set; } = string.Empty;

        /// <summary> Base64 of a 64x64 preview image. Carried, never decoded - the SDK has no image
        /// loader and is not getting one. </summary>
        [JsonProperty(ABNames.PrefabPreview)]
        public string Preview { get; set; } = string.Empty;

        [JsonProperty(ABNames.PrefabType)]
        public int Type { get; set; } = (int)ABPrefabType.Misc1;

        /// <summary> Lead time in seconds - how far ahead of a placement its content begins. </summary>
        [JsonProperty(ABNames.PrefabOffset)]
        public float Offset { get; set; }

        [JsonProperty(ABNames.PrefabObjectsList)]
        public List<VgdObject> Objects { get; set; } = new();
    }

    /// <summary> A placed instance of a prefab - .vgd prefab_objects[]. Carries one static
    /// position/scale/rotation rather than keyframe tracks. </summary>
    public class VgdPrefabPlacement : ABNode
    {
        /// <summary> Positional meaning of each entry of <see cref="Tracks"/>. </summary>
        public static class TrackIndex
        {
            public const int Position = 0;
            public const int Scale = 1;
            public const int Rotation = 2;
            public const int Count = 3;
        }

        [JsonProperty(ABNames.PlacementId)]
        public string Id { get; set; } = string.Empty;

        /// <summary> Which prefab this instantiates. An unresolvable one is removed by Afterbeat
        /// itself on load, so it is legitimately absent rather than an error. </summary>
        [JsonProperty(ABNames.PlacementPrefabId)]
        public string PrefabId { get; set; } = string.Empty;

        /// <summary> Where the placement sits, in seconds from the start of the level. The
        /// template's own objects are timed relative to THIS. </summary>
        [JsonProperty(ABNames.PlacementTime)]
        public float StartTime { get; set; }

        [JsonProperty(ABNames.PlacementEditor)]
        public VgdObjectEditor Editor { get; set; } = new();

        /// <summary> Exactly three single-value tracks - see <see cref="TrackIndex"/>. </summary>
        [JsonProperty(ABNames.PlacementTracks)]
        public List<VgdPlacementValue> Tracks { get; set; } = CreateTracks();

        /// <summary> Reads one component of one track, answering <paramref name="fallback"/> for
        /// anything the file did not write. </summary>
        public float GetValue(int track, int component, float fallback = 0f)
        {
            if (Tracks == null || track < 0 || track >= Tracks.Count) return fallback;
            var values = Tracks[track]?.Values;
            if (values == null || component < 0 || component >= values.Count) return fallback;
            return values[component];
        }

        public static List<VgdPlacementValue> CreateTracks()
        {
            var tracks = new List<VgdPlacementValue>(TrackIndex.Count);
            for (var i = 0; i < TrackIndex.Count; i++) tracks.Add(new VgdPlacementValue());
            return tracks;
        }
    }

    /// <summary> One of a placement's three value slots - an object wrapping a bare float array. </summary>
    public class VgdPlacementValue : ABNode
    {
        [JsonProperty(ABNames.KeyframeValues)]
        public List<float> Values { get; set; } = new();
    }
}
