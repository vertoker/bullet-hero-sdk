using System.Collections.Generic;
using BHSDK.Models.Components;
using BHSDK.Models.Enum;
using Newtonsoft.Json;

namespace BHSDK.Models.Base
{
    public class Object
    {
        public virtual ObjectType GetModelType() => ObjectType.Object;
        
        // Introducing to game identifiers. There are 2 types
        
        // 1. ObjectId - stable identifier for saving.
        // Unique only inside each object scope.
        // Can be referred (as pid) only in scope.
        // (but you still can rewrite pid from outside via modifications)
        
        // 2. in-game indexes - runtime temporary identifier (not id).
        // Changes every runtime and using for game player only.
        // This is not Unity's Object.GetInstanceID()
        
        [JsonProperty(ModelNames.ObjectId)]
        public int ObjectId { get; set; }
        
        [JsonProperty(ModelNames.ParentObjectId)]
        public int ParentObjectId { get; set; }
        
        [JsonProperty(ModelNames.Name)]
        public string Name { get; set; }
        
        [JsonProperty(ModelNames.Visible)]
        public bool Visible { get; set; }
        
        
        [JsonProperty(ModelNames.Start + ModelNames.Frame)]
        public int StartFrame { get; set; }
        
        [JsonProperty(ModelNames.End + ModelNames.Frame)]
        public int EndFrame { get; set; }
        
        [JsonProperty(ModelNames.Position)]
        public List<Pos> Pos { get; set; }
        
        [JsonProperty(ModelNames.Rotation)]
        public List<Rot> Rot { get; set; }
        
        [JsonProperty(ModelNames.Scale)]
        public List<Sca> Sca { get; set; }
        
        [JsonProperty(ModelNames.Layer)]
        public int Layer { get; set; }
        
        [JsonProperty(ModelNames.Pivot)]
        public Anchor Pivot { get; set; }

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
            Pivot = Anchor.Center_Middle;
        }
        public Object(int objectId, int parentObjectId, string name, bool visible, 
            int startFrame, int endFrame, List<Pos> pos, List<Rot> rot, List<Sca> sca, int layer, Anchor pivot)
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