using System.Collections.Generic;
using BHSDK.Models.Enum;
using BHSDK.Models.Keyframes;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Objects
{
    public class Object
    {
        public virtual ObjectType GetModelType() => ObjectType.Object;
        
        // Introducing to game identifiers. There are 2 types
        
        // 1. ObjectId - stable identifier for saving.
        // Unique only inside each object scope.
        // Can be referred (as pid) only in scope.
        // (but you still can rewrite pid from outside via modifications)
        
        // 2. FrameIndex - runtime temporary index (not id, use for direct indexation).
        // Changes every runtime and using for rendering instances in frame context only
        
        [JsonProperty(Names.ObjectId)]
        public int ObjectId { get; set; }
        
        [JsonProperty(Names.ParentObjectId)]
        public int ParentObjectId { get; set; }
        
        // What certain objectId's meaning
        // - 0 => undefined (for ObjectId) or null (for ParentObjectId), exists as a fallback value
        // - (1 - int.MaxValue) => user-space objects, valid for both ObjectId and ParentObjectId
        // All negative numbers (int.MinValue - -1) reserved for core-space objects
        // - -1 => camera predefined object, exists only in player runtime (for ObjectId - error),
        // can be used as a parent with unique transform (scale applied as a size, similar with RectTransform)

        public const int UndefinedId = 0;
        public const int UndefinedFrameIndex = int.MinValue;
        public const int CameraId = -1;
        
        [JsonProperty(Names.Name)]
        public string Name { get; set; }
        
        [JsonProperty(Names.VisibleShort)]
        public bool Visible { get; set; }
        
        
        [JsonProperty(Names.StartFrameShort)]
        public int StartFrame { get; set; }
        
        [JsonProperty(Names.EndFrameShort)]
        public int EndFrame { get; set; }
        
        [JsonProperty(Names.Position)]
        public List<Pos> Pos { get; set; }
        
        [JsonProperty(Names.Rotation)]
        public List<Rot> Rot { get; set; }
        
        [JsonProperty(Names.Scale)]
        public List<Sca> Sca { get; set; }
        
        [JsonProperty(Names.LayerShort)]
        public int Layer { get; set; }
        
        [JsonProperty(Names.PivotShort)]
        public Alignment Pivot { get; set; }

        public Object()
        {
            ObjectId = 0;
            ParentObjectId = 0;
            Name = string.Empty;
            Visible = true;
            
            StartFrame = 0;
            EndFrame = 0;
            Pos = new List<Pos>();
            Rot = new List<Rot>();
            Sca = new List<Sca>();
            Layer = 0;
            Pivot = Alignment.MiddleCenter;
        }
        public Object(int objectId, int parentObjectId, string name, bool visible, 
            int startFrame, int endFrame, List<Pos> pos, List<Rot> rot, List<Sca> sca, int layer, Alignment pivot)
        {
            ObjectId = objectId;
            ParentObjectId = parentObjectId;
            Name = name;
            Visible = visible;
            StartFrame = startFrame;
            EndFrame = endFrame;
            Pos = pos;
            Rot = rot;
            Sca = sca;
            Layer = layer;
            Pivot = pivot;
        }
    }
}