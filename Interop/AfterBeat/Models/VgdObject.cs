using System.Collections.Generic;
using Newtonsoft.Json;

namespace BH.SDK.Interop.AfterBeat.Models
{
    // The Object Data shape is shared by .vgd objects[] and .vgp objs[] - one class serves both,
    // which is also how the format itself describes it.
    //
    // The four keyframe tracks are an ARRAY OF FOUR, positionally Move / Scale / Rotate / Color,
    // with no name anywhere in the file saying which is which. VgdObject exposes them by name so no
    // caller has to remember that; TrackIndex holds the mapping in one place.

    /// <summary> One gameplay object of a .vgd level or a .vgp prefab. </summary>
    public class VgdObject : ABNode
    {
        /// <summary> Positional meaning of each entry of <see cref="Tracks"/>. </summary>
        public static class TrackIndex
        {
            public const int Move = 0;
            public const int Scale = 1;
            public const int Rotate = 2;
            public const int Color = 3;
            public const int Count = 4;
        }

        [JsonProperty(ABNames.ObjectId)]
        public string Id { get; set; } = string.Empty;

        /// <summary> Only on an expanded prefab instance: which prefab it came from. </summary>
        [JsonProperty(ABNames.ObjectPrefabId)]
        public string SourcePrefabId { get; set; } = string.Empty;

        /// <summary> Only on an expanded prefab instance: which placement it came from. </summary>
        [JsonProperty(ABNames.ObjectPrefabInstanceId)]
        public string SourcePlacementId { get; set; } = string.Empty;

        [JsonProperty(ABNames.ObjectName)]
        public string Name { get; set; } = string.Empty;

        /// <summary> Hit / No Hit / Empty. Default 4 (Hit). </summary>
        [JsonProperty(ABNames.ObjectType)]
        public int ObjectType { get; set; } = (int)ABObjectType.Hit;

        /// <summary> Spawn time in seconds from the start of the level. </summary>
        [JsonProperty(ABNames.ObjectStartTime)]
        public float StartTime { get; set; }

        [JsonProperty(ABNames.ObjectAutokillType)]
        public int AutokillType { get; set; } = (int)ABAutokillType.LastKeyframe;

        /// <summary> Meaning depends entirely on <see cref="AutokillType"/>. </summary>
        [JsonProperty(ABNames.ObjectAutokillOffset)]
        public float AutokillOffset { get; set; }

        [JsonProperty(ABNames.ObjectGradientType)]
        public int GradientType { get; set; }

        [JsonProperty(ABNames.ObjectGradientRotation)]
        public int GradientRotation { get; set; }

        [JsonProperty(ABNames.ObjectGradientScale)]
        public float GradientScale { get; set; } = 1f;

        /// <summary> Main shape family; pairs with <see cref="ShapeOption"/>. </summary>
        [JsonProperty(ABNames.ObjectShape)]
        public int Shape { get; set; }

        [JsonProperty(ABNames.ObjectShapeOption)]
        public int ShapeOption { get; set; }

        /// <summary> Parameters of a custom polygon, in the order the editor lists them: sides,
        /// roundness, thickness, slices, inverted. Empty on an object using a preset shape - and
        /// present but unused on one that was custom earlier and is not any more, which is why the
        /// shape option decides whether it is read. </summary>
        [JsonProperty(ABNames.ObjectCustomShape)]
        public List<float> CustomShape { get; set; }

        /// <summary> Positional meaning of <see cref="CustomShape"/>. </summary>
        public static class CustomShapeIndex
        {
            public const int Sides = 0;
            public const int Roundness = 1;
            public const int Thickness = 2;
            public const int Slices = 3;
            public const int Inverted = 4;
            public const int Count = 5;
        }

        /// <summary> Reads one custom-shape parameter, answering <paramref name="fallback"/> for a
        /// document that wrote fewer of them than this build expects. </summary>
        public float GetCustomShape(int index, float fallback = 0f)
            => CustomShape != null && index >= 0 && index < CustomShape.Count
                ? CustomShape[index]
                : fallback;

        /// <summary> Body of a text object, with TextMeshPro-style inline tags left as authored. </summary>
        [JsonProperty(ABNames.ObjectText)]
        public string Text { get; set; } = string.Empty;

        /// <summary> Render depth, 0-60, SMALLER is closer to the camera. Default 20. </summary>
        [JsonProperty(ABNames.ObjectDepth)]
        public int Depth { get; set; } = DefaultDepth;

        /// <summary> Documented default of <see cref="Depth"/>. </summary>
        public const int DefaultDepth = 20;

        /// <summary> Lowest <see cref="Depth"/> the source editor allows - drawn closest. </summary>
        public const int MinDepth = 0;

        /// <summary> Highest <see cref="Depth"/> the source editor allows - drawn furthest. </summary>
        public const int MaxDepth = 60;

        /// <summary> Which of the three render bands the object lives in, independently of its
        /// depth - see <see cref="ABRenderLayer"/>. Written only when it is not Default. </summary>
        [JsonProperty(ABNames.ObjectRenderLayer)]
        public int RenderLayer { get; set; } = (int)ABRenderLayer.Default;

        /// <summary> Parent object id, or the literal "camera". </summary>
        [JsonProperty(ABNames.ObjectParentId)]
        public string ParentId { get; set; } = string.Empty;

        /// <summary> Three characters of '1'/'0' - Position, Scale, Rotation, in that order. </summary>
        [JsonProperty(ABNames.ObjectParentType)]
        public string ParentType { get; set; } = DefaultParentType;

        /// <summary> Position + Rotation inherited, Scale not. </summary>
        public const string DefaultParentType = "101";

        /// <summary> Three seconds-valued delays, Position / Scale / Rotation. </summary>
        [JsonProperty(ABNames.ObjectParentOffsets)]
        public List<float> ParentOffsets { get; set; } = new() { 0f, 0f, 0f };

        [JsonProperty(ABNames.ObjectEditor)]
        public VgdObjectEditor Editor { get; set; } = new();

        /// <summary> Offset of the object's reference point from its centre. </summary>
        [JsonProperty(ABNames.ObjectOrigin)]
        public VgdVector2 Origin { get; set; } = new();

        /// <summary> Exactly four tracks - see <see cref="TrackIndex"/>. </summary>
        [JsonProperty(ABNames.ObjectTracks)]
        public List<VgdTrack> Tracks { get; set; } = CreateTracks();

        [JsonIgnore]
        public VgdTrack Move => GetTrack(TrackIndex.Move);
        [JsonIgnore]
        public VgdTrack Scale => GetTrack(TrackIndex.Scale);
        [JsonIgnore]
        public VgdTrack Rotate => GetTrack(TrackIndex.Rotate);
        [JsonIgnore]
        public VgdTrack Color => GetTrack(TrackIndex.Color);

        /// <summary> True when this object is parented to the camera rather than to another object. </summary>
        [JsonIgnore]
        public bool IsParentedToCamera => ParentId == CameraParentId;

        /// <summary> The one non-id value <see cref="ParentId"/> can hold. </summary>
        public const string CameraParentId = "camera";

        /// <summary> A malformed file can carry fewer than four tracks; every reader goes through
        /// here so none of them has to decide what that means separately. </summary>
        public VgdTrack GetTrack(int index)
        {
            if (Tracks == null || index < 0 || index >= Tracks.Count) return null;
            return Tracks[index];
        }

        public static List<VgdTrack> CreateTracks()
        {
            var tracks = new List<VgdTrack>(TrackIndex.Count);
            for (var i = 0; i < TrackIndex.Count; i++) tracks.Add(new VgdTrack());
            return tracks;
        }
    }

    /// <summary> Editor-only bookkeeping carried on every object. </summary>
    public class VgdObjectEditor : ABNode
    {
        [JsonProperty(ABNames.ObjectEditorLocked)]
        public bool Locked { get; set; }

        [JsonProperty(ABNames.ObjectEditorCollapsed)]
        public bool Collapsed { get; set; }

        [JsonProperty(ABNames.ObjectEditorTextColor)]
        public VgdColorFlags TextColor { get; set; } = new();

        [JsonProperty(ABNames.ObjectEditorBackgroundColor)]
        public VgdColorFlags BackgroundColor { get; set; } = new();

        [JsonProperty(ABNames.ObjectEditorBin)]
        public int Bin { get; set; }

        [JsonProperty(ABNames.ObjectEditorLayer)]
        public int Layer { get; set; }

        [JsonProperty(ABNames.ObjectEditorTimelineOrder)]
        public int TimelineOrder { get; set; }
    }

    /// <summary> Additive red/green/blue toggles - the editor's own timeline tinting. </summary>
    public class VgdColorFlags : ABNode
    {
        [JsonProperty(ABNames.ColorFlagRed)]
        public bool Red { get; set; }

        [JsonProperty(ABNames.ColorFlagGreen)]
        public bool Green { get; set; }

        [JsonProperty(ABNames.ColorFlagBlue)]
        public bool Blue { get; set; }
    }

    /// <summary> A plain {x, y} pair. </summary>
    public class VgdVector2 : ABNode
    {
        [JsonProperty(ABNames.VectorX)]
        public float X { get; set; }

        [JsonProperty(ABNames.VectorY)]
        public float Y { get; set; }

        public VgdVector2() { }
        public VgdVector2(float x, float y)
        {
            X = x;
            Y = y;
        }
    }

    /// <summary> One of an object's four keyframe tracks. </summary>
    public class VgdTrack : ABNode
    {
        [JsonProperty(ABNames.TrackKeyframes)]
        public List<VgdKeyframe> Keyframes { get; set; } = new();
    }

    /// <summary>
    /// One keyframe of an object track. <see cref="Values"/> holds 2 floats for Move and Scale,
    /// 1 for Rotate, and 3 for Color (theme colour index, opacity, gradient end colour index).
    /// </summary>
    public class VgdKeyframe : ABNode
    {
        /// <summary> Seconds, measured from the owning object's own start time. </summary>
        [JsonProperty(ABNames.KeyframeTime)]
        public float Time { get; set; }

        /// <summary> Easing NAME, not a number - "Linear", "InOutSine", ... </summary>
        [JsonProperty(ABNames.KeyframeEase)]
        public string Ease { get; set; } = ABEaseMap.DefaultEaseName;

        [JsonProperty(ABNames.KeyframeRandomType)]
        public int RandomType { get; set; }

        /// <summary> Random X, Random Y, Random Interval. </summary>
        [JsonProperty(ABNames.KeyframeRandomValues)]
        public List<float> RandomValues { get; set; } = new() { 0f, 0f, 0f };

        [JsonProperty(ABNames.KeyframeValues)]
        public List<float> Values { get; set; } = new();

        /// <summary> Reads one component, answering 0 for anything the file did not write - which
        /// is ordinary rather than exceptional, since a colour keyframe legitimately carries two
        /// values when the object has no gradient. </summary>
        public float GetValue(int index)
            => Values != null && index >= 0 && index < Values.Count ? Values[index] : 0f;

        /// <summary> Same, for the three randomization parameters. </summary>
        public float GetRandom(int index)
            => RandomValues != null && index >= 0 && index < RandomValues.Count ? RandomValues[index] : 0f;
    }
}
