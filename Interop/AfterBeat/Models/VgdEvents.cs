using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BH.SDK.Interop.AfterBeat.Models
{
    // A level-global event keyframe's payload is NOT uniformly numeric: thirteen of the fourteen
    // tracks carry floats, and the theme track carries a single theme id STRING. The two live under
    // DIFFERENT KEYS - "ev" and "evs" - which is a thing no description of the format says and only
    // a real file shows. A theme keyframe carries "evs" and no "ev" at all, so reading it out of
    // "ev" finds nothing, every theme change resolves to no theme, and the whole level renders
    // untinted. Both are kept as JArray rather than typed lists so a track that mixes them (none
    // does today) still survives a round trip.
    //
    // The tracks themselves have no names in the file. Their meaning is their INDEX in the outer
    // array, which ABEventTrack spells out.

    /// <summary> One keyframe on one of the fourteen level-global .vgd events[] tracks. </summary>
    public class VgdEventKeyframe : ABNode
    {
        /// <summary> Seconds from the start of the level - absolute, unlike an object keyframe's. </summary>
        [JsonProperty(ABNames.KeyframeTime)]
        public float Time { get; set; }

        [JsonProperty(ABNames.KeyframeEase)]
        public string Ease { get; set; } = ABEaseMap.DefaultEaseName;

        /// <summary> Floats on every track but Theme, which carries one string. </summary>
        [JsonProperty(ABNames.KeyframeValues)]
        public JArray Values { get; set; } = new();

        /// <summary> The theme track's own payload - one theme id. Null on every other track, which
        /// the serializer's own NullValueHandling.Ignore keeps out of the written document. </summary>
        [JsonProperty(ABNames.KeyframeValuesStrings)]
        public JArray Strings { get; set; }

        /// <summary> Reads one component as a number, answering <paramref name="fallback"/> when the
        /// file did not write it - which several tracks legitimately do (a glitch keyframe may carry
        /// one value, two, or three). </summary>
        public float GetFloat(int index, float fallback = 0f)
        {
            var token = Read(index);
            if (token == null) return fallback;
            return token.Type is JTokenType.Float or JTokenType.Integer ? token.Value<float>() : fallback;
        }

        /// <summary> Reads one component as a string - the theme track's only shape. Looks in the
        /// string payload first and falls back to the numeric one, so a document that spells the id
        /// the way the format's description implies still reads. </summary>
        public string GetString(int index)
        {
            var token = Read(Strings, index) ?? Read(Values, index);
            return token?.Type == JTokenType.String ? token.Value<string>() : null;
        }

        /// <summary> Writes one string payload, the shape the theme track is read back from. The
        /// numeric payload is dropped rather than left empty: a theme keyframe in a real document
        /// carries "evs" and nothing else. </summary>
        public void SetString(string value)
        {
            Strings = new JArray { value ?? string.Empty };
            Values = null;
        }

        private JToken Read(int index) => Read(Values, index);

        private static JToken Read(JArray array, int index)
            => array != null && index >= 0 && index < array.Count ? array[index] : null;
    }

    /// <summary> A checkpoint - the one .vgd event carrying a respawn position. </summary>
    public class VgdCheckpoint : ABNode
    {
        [JsonProperty(ABNames.CheckpointId)]
        public string Id { get; set; } = string.Empty;

        [JsonProperty(ABNames.CheckpointName)]
        public string Name { get; set; } = string.Empty;

        /// <summary> Seconds from the start of the level. </summary>
        [JsonProperty(ABNames.CheckpointTime)]
        public float Time { get; set; }

        [JsonProperty(ABNames.CheckpointPosition)]
        public VgdVector2 Position { get; set; } = new();
    }

    /// <summary> A timeline note - decorative, exactly like this project's own Marker. </summary>
    public class VgdMarker : ABNode
    {
        [JsonProperty(ABNames.MarkerId)]
        public string Id { get; set; } = string.Empty;

        [JsonProperty(ABNames.MarkerName)]
        public string Name { get; set; } = string.Empty;

        [JsonProperty(ABNames.MarkerDescription)]
        public string Description { get; set; } = string.Empty;

        /// <summary> An index into the editor's own note palette, not a colour value. </summary>
        [JsonProperty(ABNames.MarkerColor)]
        public int Color { get; set; }

        /// <summary> Seconds from the start of the level. </summary>
        [JsonProperty(ABNames.MarkerTime)]
        public float Time { get; set; }
    }

    // Freehand notes drawn over the editor's canvas: a stroke of points, a time, and a marker it
    // hangs off. Undocumented, and a real level carries them by the hundred. Read rather than left
    // to the extension data for one reason - they are AUTHORED, unlike the rest of the editor block,
    // so an author is told they do not cross instead of noticing their notes are gone.

    /// <summary> One freehand annotation stroke. </summary>
    public class VgdAnnotation : ABNode
    {
        [JsonProperty(ABNames.AnnotationId)]
        public string Id { get; set; } = string.Empty;

        /// <summary> The marker this note is attached to, if any. </summary>
        [JsonProperty(ABNames.AnnotationMarker)]
        public string MarkerId { get; set; } = string.Empty;

        /// <summary> Seconds from the start of the level. </summary>
        [JsonProperty(ABNames.AnnotationTime)]
        public float Time { get; set; }

        /// <summary> The stroke itself, in the editor's own canvas space. </summary>
        [JsonProperty(ABNames.AnnotationPoints)]
        public List<VgdVector2> Points { get; set; } = new();

        /// <summary> An index into the editor's own note palette, like a marker's. </summary>
        [JsonProperty(ABNames.AnnotationColor)]
        public int Color { get; set; }
    }

    /// <summary> A scripted event - the whole family has no equivalent in this project's format. </summary>
    public class VgdTrigger : ABNode
    {
        [JsonProperty(ABNames.TriggerActivator)]
        public int Activator { get; set; }

        [JsonProperty(ABNames.TriggerTime)]
        public VgdVector2 TimeRange { get; set; } = new();

        /// <summary> -1 means no limit. </summary>
        [JsonProperty(ABNames.TriggerRetrigger)]
        public int Retrigger { get; set; }

        [JsonProperty(ABNames.TriggerEvent)]
        public int Event { get; set; }

        [JsonProperty(ABNames.TriggerData)]
        public List<string> Data { get; set; } = new();
    }

    /// <summary> One of six hotkey slots that spawn a prefab in the editor. </summary>
    public class VgdPrefabSpawnSlot : ABNode
    {
        [JsonProperty(ABNames.SpawnExpanded)]
        public bool Expanded { get; set; }

        [JsonProperty(ABNames.SpawnActive)]
        public bool Active { get; set; }

        [JsonProperty(ABNames.SpawnPrefab)]
        public string PrefabId { get; set; } = string.Empty;

        [JsonProperty(ABNames.SpawnKeycodes)]
        public List<string> Keycodes { get; set; } = new();
    }
}
