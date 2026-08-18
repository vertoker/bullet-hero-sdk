using System.Collections.Generic;
using Newtonsoft.Json;

namespace BH.SDK.Interop.AfterBeat.Models
{
    // The extension is .vgd by a typo - it was meant to be .vgb, Vitamin Games Beatmap, after the
    // legacy .lsb it replaced. Nothing here depends on that; it is recorded because the name looks
    // like it should stand for something and does not.
    //
    // There is no version field anywhere in this document. Compatibility is expressed purely as
    // "optional, defaults to X" per key, which is why every property here carries the documented
    // default as its initial value rather than leaving it at the CLR's.

    /// <summary> An Afterbeat level file - .vgd. Themes and prefabs live INSIDE it; the standalone
    /// .vgt/.vgp files are a sharing format, not part of a level folder. </summary>
    public class VgdLevel : AfterBeatNode
    {
        /// <summary> How many keyframe arrays events[] always holds. </summary>
        public const int EventTrackCount = 14;

        [JsonProperty(AfterBeatNames.Editor)]
        public VgdEditor Editor { get; set; } = new();

        [JsonProperty(AfterBeatNames.Triggers)]
        public List<VgdTrigger> Triggers { get; set; } = new();

        [JsonProperty(AfterBeatNames.EditorPrefabSpawn)]
        public List<VgdPrefabSpawnSlot> PrefabSpawnSlots { get; set; } = new();

        [JsonProperty(AfterBeatNames.ParallaxSettings)]
        public VgdParallaxSettings Parallax { get; set; } = new();

        [JsonProperty(AfterBeatNames.Checkpoints)]
        public List<VgdCheckpoint> Checkpoints { get; set; } = new();

        [JsonProperty(AfterBeatNames.Objects)]
        public List<VgdObject> Objects { get; set; } = new();

        [JsonProperty(AfterBeatNames.PrefabObjects)]
        public List<VgdPrefabPlacement> PrefabPlacements { get; set; } = new();

        [JsonProperty(AfterBeatNames.Prefabs)]
        public List<VgpPrefab> Prefabs { get; set; } = new();

        [JsonProperty(AfterBeatNames.Themes)]
        public List<VgtTheme> Themes { get; set; } = new();

        [JsonProperty(AfterBeatNames.Markers)]
        public List<VgdMarker> Markers { get; set; } = new();

        /// <summary> Fourteen arrays, addressed by <see cref="AfterBeatEventTrack"/>. </summary>
        [JsonProperty(AfterBeatNames.Events)]
        public List<List<VgdEventKeyframe>> Events { get; set; } = CreateEvents();

        /// <summary> One track, or an empty list when the file is short of the full fourteen -
        /// which a hand-edited or older file can be. </summary>
        public IReadOnlyList<VgdEventKeyframe> GetEvents(AfterBeatEventTrack track)
        {
            var index = (int)track;
            if (Events == null || index < 0 || index >= Events.Count) return System.Array.Empty<VgdEventKeyframe>();
            return Events[index] ?? (IReadOnlyList<VgdEventKeyframe>)System.Array.Empty<VgdEventKeyframe>();
        }

        /// <summary> Replaces one track, growing the outer list to the full fourteen if needed -
        /// a shorter events[] is not a legal document. </summary>
        public void SetEvents(AfterBeatEventTrack track, List<VgdEventKeyframe> keyframes)
        {
            Events ??= CreateEvents();
            while (Events.Count < EventTrackCount) Events.Add(new List<VgdEventKeyframe>());
            Events[(int)track] = keyframes ?? new List<VgdEventKeyframe>();
        }

        public static List<List<VgdEventKeyframe>> CreateEvents()
        {
            var events = new List<List<VgdEventKeyframe>>(EventTrackCount);
            for (var i = 0; i < EventTrackCount; i++) events.Add(new List<VgdEventKeyframe>());
            return events;
        }
    }

    /// <summary> Everything in a .vgd that only the editor reads. </summary>
    public class VgdEditor : AfterBeatNode
    {
        [JsonProperty(AfterBeatNames.EditorGeneral)]
        public VgdEditorGeneral General { get; set; } = new();

        [JsonProperty(AfterBeatNames.EditorBpm)]
        public VgdEditorBpm Bpm { get; set; } = new();

        [JsonProperty(AfterBeatNames.EditorGrid)]
        public VgdEditorGrid Grid { get; set; } = new();

        [JsonProperty(AfterBeatNames.EditorPreview)]
        public VgdEditorPreview Preview { get; set; } = new();

        [JsonProperty(AfterBeatNames.EditorAutosave)]
        public VgdEditorAutosave Autosave { get; set; } = new();
    }

    /// <summary> Editor preferences saved with the level. </summary>
    public class VgdEditorGeneral : AfterBeatNode
    {
        /// <summary> Documented as unused and always 0. </summary>
        [JsonProperty(AfterBeatNames.EditorComplexity)]
        public int Complexity { get; set; }

        /// <summary> Documented as unused and always 0 - NOT the level's active theme. </summary>
        [JsonProperty(AfterBeatNames.EditorTheme)]
        public int Theme { get; set; }

        [JsonProperty(AfterBeatNames.EditorTestMode)]
        public int TestMode { get; set; } = (int)AfterBeatTestMode.Normal;

        [JsonProperty(AfterBeatNames.EditorTextSelectObjects)]
        public bool TextSelectObjects { get; set; }

        [JsonProperty(AfterBeatNames.EditorTextSelectBackgrounds)]
        public bool TextSelectBackgrounds { get; set; }

        [JsonProperty(AfterBeatNames.EditorOutlineMode)]
        public int OutlineMode { get; set; }

        [JsonProperty(AfterBeatNames.EditorCollapseLength)]
        public float CollapseLength { get; set; } = 0.25f;
    }

    /// <summary> The level's tempo, as far as this format records one. It exists for snapping in
    /// the editor - nothing in the file is stored in beats. </summary>
    public class VgdEditorBpm : AfterBeatNode
    {
        [JsonProperty(AfterBeatNames.EditorBpmSnap)]
        public VgdEditorBpmSnap Snap { get; set; } = new();

        [JsonProperty(AfterBeatNames.EditorBpmValue)]
        public float Value { get; set; } = DefaultBpm;

        /// <summary> Phase of the first beat, in seconds. </summary>
        [JsonProperty(AfterBeatNames.EditorBpmOffset)]
        public float Offset { get; set; }

        /// <summary> A second copy of <see cref="Value"/> the format writes and is documented as
        /// possibly unused. Kept so a round trip does not decide which of the two was right. </summary>
        [JsonProperty(AfterBeatNames.EditorBpmValueDuplicate)]
        public float ValueDuplicate { get; set; } = DefaultBpm;

        public const float DefaultBpm = 140f;
    }

    /// <summary> Which timeline items the BPM grid catches. </summary>
    public class VgdEditorBpmSnap : AfterBeatNode
    {
        [JsonProperty(AfterBeatNames.EditorBpmSnapObjects)]
        public bool Objects { get; set; }

        [JsonProperty(AfterBeatNames.EditorBpmSnapCheckpoints)]
        public bool Checkpoints { get; set; }
    }

    /// <summary> The editor viewport's own grid. </summary>
    public class VgdEditorGrid : AfterBeatNode
    {
        [JsonProperty(AfterBeatNames.EditorGridScale)]
        public VgdVector2 Scale { get; set; } = new();

        [JsonProperty(AfterBeatNames.EditorGridThickness)]
        public int Thickness { get; set; } = 2;

        [JsonProperty(AfterBeatNames.EditorGridOpacity)]
        public float Opacity { get; set; } = 0.2f;

        /// <summary> Palette index, not a colour value. </summary>
        [JsonProperty(AfterBeatNames.EditorGridColor)]
        public int Color { get; set; } = 1;
    }

    /// <summary> How far the editor preview zooms out past the real camera. </summary>
    public class VgdEditorPreview : AfterBeatNode
    {
        [JsonProperty(AfterBeatNames.EditorPreviewCamZoomOffset)]
        public float CameraZoomOffset { get; set; }

        [JsonProperty(AfterBeatNames.EditorPreviewCamZoomOffsetColor)]
        public int CameraZoomOffsetColor { get; set; } = 3;
    }

    /// <summary> Autosave policy, stored per level. </summary>
    public class VgdEditorAutosave : AfterBeatNode
    {
        [JsonProperty(AfterBeatNames.EditorAutosaveMax)]
        public int Max { get; set; } = 3;

        /// <summary> Minutes. </summary>
        [JsonProperty(AfterBeatNames.EditorAutosaveInterval)]
        public int Interval { get; set; } = 10;
    }
}
