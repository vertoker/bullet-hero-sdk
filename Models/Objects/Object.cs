using System.Collections.Generic;
using System.Linq;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Keyframes;
using BHSDK.Models.Values;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Objects
{
    [RuleContainer]
    public class Object : IUpdatable<Object>
    {
        public virtual ObjectType GetModelType() => ObjectType.Object;
        
        // Introducing to game identifiers. There are 2 types
        
        // 1. ObjectId - stable identifier for saving.
        // Unique only inside each object scope. Can be referred (as pid) only in scope.
        // When PrefabObjects converts to regular Objects, they change all ids for each hierarchy level
        
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
        
        [RuleNotNull, RuleStringMax(ValueRules.MaxEditorName)]
        [JsonProperty(Names.Name)]
        public string Name { get; set; }
        
        [JsonProperty(Names.VisibleShort)]
        public bool Visible { get; set; }
        
        
        [RuleLevelFrame]
        [JsonProperty(Names.StartFrameShort)]
        public int StartFrame { get; set; }
        
        [RuleLevelFrame]
        [JsonProperty(Names.EndFrameShort)]
        public int EndFrame { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxObjectPositions)]
        [RuleCollectionSorted(nameof(Pos.Frame))]
        [RuleCollectionUnique(nameof(Pos.Frame))]
        [JsonProperty(Names.Position)]
        public List<Pos> Positions { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxObjectRotations)]
        [RuleCollectionSorted(nameof(Rot.Frame))]
        [RuleCollectionUnique(nameof(Rot.Frame))]
        [JsonProperty(Names.Rotation)]
        public List<Rot> Rotations { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxObjectScales)]
        [RuleCollectionSorted(nameof(Sca.Frame))]
        [RuleCollectionUnique(nameof(Sca.Frame))]
        [JsonProperty(Names.Scale)]
        public List<Sca> Scales { get; set; }
        
        [RuleInRange(ValueRules.MinLayer, ValueRules.MaxLayer)]
        [JsonProperty(Names.LayerShort)]
        public int Layer { get; set; }
        
        [RuleNotNull]
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
            Positions = new List<Pos>();
            Rotations = new List<Rot>();
            Scales = new List<Sca>();
            Layer = 0;
            Pivot = Alignment.MiddleCenter;
        }
        public Object(int objectId, int parentObjectId, string name, bool visible, 
            int startFrame, int endFrame, List<Pos> positions, List<Rot> rotations, List<Sca> scales, int layer, Alignment pivot)
        {
            ObjectId = objectId;
            ParentObjectId = parentObjectId;
            Name = name;
            Visible = visible;
            StartFrame = startFrame;
            EndFrame = endFrame;
            Positions = positions;
            Rotations = rotations;
            Scales = scales;
            Layer = layer;
            Pivot = pivot;
        }

        public void Update(Object src)
        {
            ObjectId = src.ObjectId;
            ParentObjectId = src.ParentObjectId;
            Name = src.Name;
            Visible = src.Visible;
            StartFrame = src.StartFrame;
            EndFrame = src.EndFrame;
            Positions = src.Positions.Select(p => p.Copy()).ToList();
            Rotations = src.Rotations.Select(r => r.Copy()).ToList();
            Scales = src.Scales.Select(s => s.Copy()).ToList();
            Layer = src.Layer;
            Pivot = src.Pivot.Copy();
        }
    }
}