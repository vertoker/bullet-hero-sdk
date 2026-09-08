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

        /// <summary> The source editor's own workspace settings, carried in the level file. </summary>
        [JsonProperty(ABNames.Editor)]
        public VgdEditor Editor { get; set; } = new();

        /// <summary> Event triggers - the source's scripting layer, which this project has no counterpart for. </summary>
        [JsonProperty(ABNames.Triggers)]
        public List<VgdTrigger> Triggers { get; set; } = new();

        /// <summary> The editor's prefab hotkey slots, not level content. </summary>
        [JsonProperty(ABNames.EditorPrefabSpawn)]
        public List<VgdPrefabSpawnSlot> PrefabSpawnSlots { get; set; } = new();

        /// <summary> The background layers, which have no keyframes of their own. </summary>
        [JsonProperty(ABNames.ParallaxSettings)]
        public VgdParallaxSettings Parallax { get; set; } = new();

        /// <summary> Where a lost run comes back to. </summary>
        [JsonProperty(ABNames.Checkpoints)]
        public List<VgdCheckpoint> Checkpoints { get; set; } = new();

        /// <summary> The gameplay objects - the bulk of any level. </summary>
        [JsonProperty(ABNames.Objects)]
        public List<VgdObject> Objects { get; set; } = new();

        /// <summary> Prefab placements, each naming one of the prefabs below. </summary>
        [JsonProperty(ABNames.PrefabObjects)]
        public List<VgdPrefabPlacement> PrefabPlacements { get; set; } = new();

        /// <summary> Prefab templates carried inside the level rather than as separate .vgp files. </summary>
        [JsonProperty(ABNames.Prefabs)]
        public List<VgpPrefab> Prefabs { get; set; } = new();

        /// <summary> Themes carried inside the level; a level may also reference the shipped ones. </summary>
        [JsonProperty(ABNames.Themes)]
        public List<VgtTheme> Themes { get; set; } = new();

        /// <summary> Author-placed timeline markers. </summary>
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

        /// <summary> One empty track per event channel, which is the shape a written level must have. </summary>
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
        /// <summary> Assorted editor preferences. </summary>
        [JsonProperty(ABNames.EditorGeneral)]
        public VgdEditorGeneral General { get; set; } = new();

        /// <summary> The tempo the editor's grid and snapping are built on. </summary>
        [JsonProperty(ABNames.EditorBpm)]
        public VgdEditorBpm Bpm { get; set; } = new();

        /// <summary> The viewport grid's own look. </summary>
        [JsonProperty(ABNames.EditorGrid)]
        public VgdEditorGrid Grid { get; set; } = new();

        /// <summary> How the editor frames its preview. </summary>
        [JsonProperty(ABNames.EditorPreview)]
        public VgdEditorPreview Preview { get; set; } = new();

        /// <summary> How many autosaves the editor keeps. </summary>
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

        /// <summary> Which mode the editor's play test runs in. </summary>
        [JsonProperty(ABNames.EditorTestMode)]
        public int TestMode { get; set; } = (int)ABTestMode.Normal;

        /// <summary> Clicking text selects the object behind it. </summary>
        [JsonProperty(ABNames.EditorTextSelectObjects)]
        public bool TextSelectObjects { get; set; }

        /// <summary> The same for background objects. </summary>
        [JsonProperty(ABNames.EditorTextSelectBackgrounds)]
        public bool TextSelectBackgrounds { get; set; }

        /// <summary> How a selected object is outlined. </summary>
        [JsonProperty(ABNames.EditorOutlineMode)]
        public int OutlineMode { get; set; }

        /// <summary> How short a folded timeline row is drawn. </summary>
        [JsonProperty(ABNames.EditorCollapseLength)]
        public float CollapseLength { get; set; } = 0.25f;
    }

    /// <summary> The level's tempo, as far as this format records one. It exists for snapping in
    /// the editor - nothing in the file is stored in beats. </summary>
    public class VgdEditorBpm : ABNode
    {
        /// <summary> What the tempo grid snaps. </summary>
        [JsonProperty(ABNames.EditorBpmSnap)]
        public VgdEditorBpmSnap Snap { get; set; } = new();

        /// <summary> The tempo itself, in beats per minute. </summary>
        [JsonProperty(ABNames.EditorBpmValue)]
        public float Value { get; set; } = DefaultBpm;

        /// <summary> Phase of the first beat, in seconds. </summary>
        [JsonProperty(ABNames.EditorBpmOffset)]
        public float Offset { get; set; }

        /// <summary> A second copy of <see cref="Value"/> the format writes and is documented as
        /// possibly unused. Kept so a round trip does not decide which of the two was right. </summary>
        [JsonProperty(ABNames.EditorBpmValueDuplicate)]
        public float ValueDuplicate { get; set; } = DefaultBpm;

        /// <summary> What the source editor starts a level at. </summary>
        public const float DefaultBpm = 140f;
    }

    /// <summary> Which timeline items the BPM grid catches. </summary>
    public class VgdEditorBpmSnap : ABNode
    {
        /// <summary> Dragging an object snaps to the beat. </summary>
        [JsonProperty(ABNames.EditorBpmSnapObjects)]
        public bool Objects { get; set; }

        /// <summary> Dragging a checkpoint snaps to the beat. </summary>
        [JsonProperty(ABNames.EditorBpmSnapCheckpoints)]
        public bool Checkpoints { get; set; }
    }

    /// <summary> The editor viewport's own grid. </summary>
    public class VgdEditorGrid : ABNode
    {
        /// <summary> Spacing of the grid lines. </summary>
        [JsonProperty(ABNames.EditorGridScale)]
        public VgdVector2 Scale { get; set; } = new();

        /// <summary> How heavy they are drawn. </summary>
        [JsonProperty(ABNames.EditorGridThickness)]
        public int Thickness { get; set; } = 2;

        /// <summary> How visible they are. </summary>
        [JsonProperty(ABNames.EditorGridOpacity)]
        public float Opacity { get; set; } = 0.2f;

        /// <summary> Palette index, not a colour value. </summary>
        [JsonProperty(ABNames.EditorGridColor)]
        public int Color { get; set; } = 1;
    }

    /// <summary> How far the editor preview zooms out past the real camera. </summary>
    public class VgdEditorPreview : ABNode
    {
        /// <summary> Zoom the preview applies on top of the level's own camera. </summary>
        [JsonProperty(ABNames.EditorPreviewCamZoomOffset)]
        public float CameraZoomOffset { get; set; }

        /// <summary> Which theme colour the preview tints that zoom's readout with. </summary>
        [JsonProperty(ABNames.EditorPreviewCamZoomOffsetColor)]
        public int CameraZoomOffsetColor { get; set; } = 3;
    }

    /// <summary> Autosave policy, stored per level. </summary>
    public class VgdEditorAutosave : ABNode
    {
        /// <summary> How many autosaves are kept before the oldest is dropped. </summary>
        [JsonProperty(ABNames.EditorAutosaveMax)]
        public int Max { get; set; } = 3;

        /// <summary> Minutes. </summary>
        [JsonProperty(ABNames.EditorAutosaveInterval)]
        public int Interval { get; set; } = 10;
    }
}
