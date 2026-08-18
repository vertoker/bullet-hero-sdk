using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BH.SDK.Interop.AfterBeat.Models
{
    // A level-global event keyframe's payload is NOT uniformly numeric: thirteen of the fourteen
    // tracks carry floats, and the theme track carries a single theme id STRING. That is why the
    // payload is a JArray rather than a List<float> - a float list would read the theme track as
    // zeroes and write it back as zeroes, turning every theme change in the level into a reference
    // to nothing.
    //
    // The tracks themselves have no names in the file. Their meaning is their INDEX in the outer
    // array, which AfterBeatEventTrack spells out.

    /// <summary> One keyframe on one of the fourteen level-global .vgd events[] tracks. </summary>
    public class VgdEventKeyframe : AfterBeatNode
    {
        /// <summary> Seconds from the start of the level - absolute, unlike an object keyframe's. </summary>
        [JsonProperty(AfterBeatNames.KeyframeTime)]
        public float Time { get; set; }

        [JsonProperty(AfterBeatNames.KeyframeEase)]
        public string Ease { get; set; } = AfterBeatEaseMap.DefaultEaseName;

        /// <summary> Floats on every track but Theme, which carries one string. </summary>
        [JsonProperty(AfterBeatNames.KeyframeValues)]
        public JArray Values { get; set; } = new();

        /// <summary> Reads one component as a number, answering <paramref name="fallback"/> when the
        /// file did not write it - which several tracks legitimately do (a glitch keyframe may carry
        /// one value, two, or three). </summary>
        public float GetFloat(int index, float fallback = 0f)
        {
            var token = Read(index);
            if (token == null) return fallback;
            return token.Type is JTokenType.Float or JTokenType.Integer ? token.Value<float>() : fallback;
        }

        /// <summary> Reads one component as a string - the theme track's only shape. </summary>
        public string GetString(int index)
        {
            var token = Read(index);
            return token?.Type == JTokenType.String ? token.Value<string>() : null;
        }

        private JToken Read(int index)
            => Values != null && index >= 0 && index < Values.Count ? Values[index] : null;
    }

    /// <summary> A checkpoint - the one .vgd event carrying a respawn position. </summary>
    public class VgdCheckpoint : AfterBeatNode
    {
        [JsonProperty(AfterBeatNames.CheckpointId)]
        public string Id { get; set; } = string.Empty;

        [JsonProperty(AfterBeatNames.CheckpointName)]
        public string Name { get; set; } = string.Empty;

        /// <summary> Seconds from the start of the level. </summary>
        [JsonProperty(AfterBeatNames.CheckpointTime)]
        public float Time { get; set; }

        [JsonProperty(AfterBeatNames.CheckpointPosition)]
        public VgdVector2 Position { get; set; } = new();
    }

    /// <summary> A timeline note - decorative, exactly like this project's own Marker. </summary>
    public class VgdMarker : AfterBeatNode
    {
        [JsonProperty(AfterBeatNames.MarkerId)]
        public string Id { get; set; } = string.Empty;

        [JsonProperty(AfterBeatNames.MarkerName)]
        public string Name { get; set; } = string.Empty;

        [JsonProperty(AfterBeatNames.MarkerDescription)]
        public string Description { get; set; } = string.Empty;

        /// <summary> An index into the editor's own note palette, not a colour value. </summary>
        [JsonProperty(AfterBeatNames.MarkerColor)]
        public int Color { get; set; }

        /// <summary> Seconds from the start of the level. </summary>
        [JsonProperty(AfterBeatNames.MarkerTime)]
        public float Time { get; set; }
    }

    /// <summary> A scripted event - the whole family has no equivalent in this project's format. </summary>
    public class VgdTrigger : AfterBeatNode
    {
        [JsonProperty(AfterBeatNames.TriggerActivator)]
        public int Activator { get; set; }

        [JsonProperty(AfterBeatNames.TriggerTime)]
        public VgdVector2 TimeRange { get; set; } = new();

        /// <summary> -1 means no limit. </summary>
        [JsonProperty(AfterBeatNames.TriggerRetrigger)]
        public int Retrigger { get; set; }

        [JsonProperty(AfterBeatNames.TriggerEvent)]
        public int Event { get; set; }

        [JsonProperty(AfterBeatNames.TriggerData)]
        public List<string> Data { get; set; } = new();
    }

    /// <summary> One of six hotkey slots that spawn a prefab in the editor. </summary>
    public class VgdPrefabSpawnSlot : AfterBeatNode
    {
        [JsonProperty(AfterBeatNames.SpawnExpanded)]
        public bool Expanded { get; set; }

        [JsonProperty(AfterBeatNames.SpawnActive)]
        public bool Active { get; set; }

        [JsonProperty(AfterBeatNames.SpawnPrefab)]
        public string PrefabId { get; set; } = string.Empty;

        [JsonProperty(AfterBeatNames.SpawnKeycodes)]
        public List<string> Keycodes { get; set; } = new();
    }
}
