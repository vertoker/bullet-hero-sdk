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
    public class VgdLevel : ABNode
    {
        /// <summary> How many keyframe arrays events[] always holds. </summary>
        public const int EventTrackCount = 14;

        [JsonProperty(ABNames.Editor)]
        public VgdEditor Editor { get; set; } = new();

        [JsonProperty(ABNames.Triggers)]
        public List<VgdTrigger> Triggers { get; set; } = new();

        [JsonProperty(ABNames.EditorPrefabSpawn)]
        public List<VgdPrefabSpawnSlot> PrefabSpawnSlots { get; set; } = new();

        [JsonProperty(ABNames.ParallaxSettings)]
        public VgdParallaxSettings Parallax { get; set; } = new();

        [JsonProperty(ABNames.Checkpoints)]
        public List<VgdCheckpoint> Checkpoints { get; set; } = new();

        [JsonProperty(ABNames.Objects)]
        public List<VgdObject> Objects { get; set; } = new();

        [JsonProperty(ABNames.PrefabObjects)]
        public List<VgdPrefabPlacement> PrefabPlacements { get; set; } = new();

        [JsonProperty(ABNames.Prefabs)]
        public List<VgpPrefab> Prefabs { get; set; } = new();

        [JsonProperty(ABNames.Themes)]
        public List<VgtTheme> Themes { get; set; } = new();

        [JsonProperty(ABNames.Markers)]
        public List<VgdMarker> Markers { get; set; } = new();

        /// <summary> Freehand notes the author drew over the editor's own canvas. </summary>
        [JsonProperty(ABNames.Annotations)]
        public List<VgdAnnotation> Annotations { get; set; } = new();

        /// <summary> Fourteen arrays, addressed by <see cref="ABEventTrack"/>. </summary>
        [JsonProperty(ABNames.Events)]
        public List<List<VgdEventKeyframe>> Events { get; set; } = CreateEvents();

        /// <summary> One track, or an empty list when the file is short of the full fourteen -
        /// which a hand-edited or older file can be. </summary>
        public IReadOnlyList<VgdEventKeyframe> GetEvents(ABEventTrack track)
        {
            var index = (int)track;
            if (Events == null || index < 0 || index >= Events.Count) return System.Array.Empty<VgdEventKeyframe>();
            return Events[index] ?? (IReadOnlyList<VgdEventKeyframe>)System.Array.Empty<VgdEventKeyframe>();
        }

        /// <summary> Replaces one track, growing the outer list to the full fourteen if needed -
        /// a shorter events[] is not a legal document. </summary>
        public void SetEvents(ABEventTrack track, List<VgdEventKeyframe> keyframes)
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
    public class VgdEditor : ABNode
    {
        [JsonProperty(ABNames.EditorGeneral)]
        public VgdEditorGeneral General { get; set; } = new();

        [JsonProperty(ABNames.EditorBpm)]
        public VgdEditorBpm Bpm { get; set; } = new();

        [JsonProperty(ABNames.EditorGrid)]
        public VgdEditorGrid Grid { get; set; } = new();

        [JsonProperty(ABNames.EditorPreview)]
        public VgdEditorPreview Preview { get; set; } = new();

        [JsonProperty(ABNames.EditorAutosave)]
        public VgdEditorAutosave Autosave { get; set; } = new();
    }

    /// <summary> Editor preferences saved with the level. </summary>
    public class VgdEditorGeneral : ABNode
    {
        /// <summary> Documented as unused and always 0. </summary>
        [JsonProperty(ABNames.EditorComplexity)]
        public int Complexity { get; set; }

        /// <summary> Documented as unused and always 0 - NOT the level's active theme. </summary>
        [JsonProperty(ABNames.EditorTheme)]
        public int Theme { get; set; }

        [JsonProperty(ABNames.EditorTestMode)]
        public int TestMode { get; set; } = (int)ABTestMode.Normal;

        [JsonProperty(ABNames.EditorTextSelectObjects)]
        public bool TextSelectObjects { get; set; }

        [JsonProperty(ABNames.EditorTextSelectBackgrounds)]
        public bool TextSelectBackgrounds { get; set; }

        [JsonProperty(ABNames.EditorOutlineMode)]
        public int OutlineMode { get; set; }

        [JsonProperty(ABNames.EditorCollapseLength)]
        public float CollapseLength { get; set; } = 0.25f;
    }

    /// <summary> The level's tempo, as far as this format records one. It exists for snapping in
    /// the editor - nothing in the file is stored in beats. </summary>
    public class VgdEditorBpm : ABNode
    {
        [JsonProperty(ABNames.EditorBpmSnap)]
        public VgdEditorBpmSnap Snap { get; set; } = new();

        [JsonProperty(ABNames.EditorBpmValue)]
        public float Value { get; set; } = DefaultBpm;

        /// <summary> Phase of the first beat, in seconds. </summary>
        [JsonProperty(ABNames.EditorBpmOffset)]
        public float Offset { get; set; }

        /// <summary> A second copy of <see cref="Value"/> the format writes and is documented as
        /// possibly unused. Kept so a round trip does not decide which of the two was right. </summary>
        [JsonProperty(ABNames.EditorBpmValueDuplicate)]
        public float ValueDuplicate { get; set; } = DefaultBpm;

        public const float DefaultBpm = 140f;
    }

    /// <summary> Which timeline items the BPM grid catches. </summary>
    public class VgdEditorBpmSnap : ABNode
    {
        [JsonProperty(ABNames.EditorBpmSnapObjects)]
        public bool Objects { get; set; }

        [JsonProperty(ABNames.EditorBpmSnapCheckpoints)]
        public bool Checkpoints { get; set; }
    }

    /// <summary> The editor viewport's own grid. </summary>
    public class VgdEditorGrid : ABNode
    {
        [JsonProperty(ABNames.EditorGridScale)]
        public VgdVector2 Scale { get; set; } = new();

        [JsonProperty(ABNames.EditorGridThickness)]
        public int Thickness { get; set; } = 2;

        [JsonProperty(ABNames.EditorGridOpacity)]
        public float Opacity { get; set; } = 0.2f;

        /// <summary> Palette index, not a colour value. </summary>
        [JsonProperty(ABNames.EditorGridColor)]
        public int Color { get; set; } = 1;
    }

    /// <summary> How far the editor preview zooms out past the real camera. </summary>
    public class VgdEditorPreview : ABNode
    {
        [JsonProperty(ABNames.EditorPreviewCamZoomOffset)]
        public float CameraZoomOffset { get; set; }

        [JsonProperty(ABNames.EditorPreviewCamZoomOffsetColor)]
        public int CameraZoomOffsetColor { get; set; } = 3;
    }

    /// <summary> Autosave policy, stored per level. </summary>
    public class VgdEditorAutosave : ABNode
    {
        [JsonProperty(ABNames.EditorAutosaveMax)]
        public int Max { get; set; } = 3;

        /// <summary> Minutes. </summary>
        [JsonProperty(ABNames.EditorAutosaveInterval)]
        public int Interval { get; set; } = 10;
    }
}
