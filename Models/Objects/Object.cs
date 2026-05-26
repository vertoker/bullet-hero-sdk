using System;
using System.Collections.Generic;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Keyframes;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Objects
{
    [RuleContainer]
    public class Object : ICopyable<Object>, IEquatable<Object>, IUpdatable<Object>
    {
        public virtual ObjectType GetModelType() => ObjectType.Object;
        
        [RuleMin(IdRules.MinObjectId)]
        [JsonProperty(Names.ObjectId)]
        public int ObjectId { get; set; }
        
        [RuleMin(IdRules.MinParentObjectId)]
        [JsonProperty(Names.ParentObjectId)]
        public int ParentObjectId { get; set; }
        
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
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxObjectKeyframes)]
        [RuleCollectionSorted(nameof(Pos.Frame))]
        [RuleCollectionUnique(nameof(Pos.Frame))]
        [JsonProperty(Names.Position)]
        public List<Pos> Positions { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxObjectKeyframes)]
        [RuleCollectionSorted(nameof(Rot.Frame))]
        [RuleCollectionUnique(nameof(Rot.Frame))]
        [JsonProperty(Names.Rotation)]
        public List<Rot> Rotations { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxObjectKeyframes)]
        [RuleCollectionSorted(nameof(Sca.Frame))]
        [RuleCollectionUnique(nameof(Sca.Frame))]
        [JsonProperty(Names.Scale)]
        public List<Sca> Scales { get; set; }
        
        [RuleInRange(ObjectRules.MinLayer, ObjectRules.MaxLayer)]
        [JsonProperty(Names.LayerShort)]
        public int Layer { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.PivotShort)]
        public Alignment Pivot { get; set; }

        public Object()
        {
            ObjectId = IdRules.NullObjectId;
            ParentObjectId = IdRules.NullObjectId;
            Name = string.Empty;
            Visible = true;
            
            StartFrame = FrameRules.MinFrame;
            EndFrame = FrameRules.MinFrame;
            Positions = new List<Pos>();
            Rotations = new List<Rot>();
            Scales = new List<Sca>();
            Layer = ObjectRules.DefaultLayer;
            Pivot = Alignment.CenterMiddle;
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

        public virtual object Clone() => CopyImpl();
        public virtual Object Copy() => CopyImpl();
        
        private Object CopyImpl() => new(ObjectId, ParentObjectId, Name, Visible, StartFrame, EndFrame,
            Positions.CopyList(), Rotations.CopyList(), Scales.CopyList(), Layer, Pivot.Copy());
        
        public void Update(Object src)
        {
            ObjectId = src.ObjectId;
            ParentObjectId = src.ParentObjectId;
            Name = src.Name;
            Visible = src.Visible;
            StartFrame = src.StartFrame;
            EndFrame = src.EndFrame;
            Positions = src.Positions.CopyList();
            Rotations = src.Rotations.CopyList();
            Scales = src.Scales.CopyList();
            Layer = src.Layer;
            Pivot = src.Pivot.Copy();
        }

        public override bool Equals(object obj) => obj is Object value && Equals(value);
        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(ObjectId);
            hashCode.Add(ParentObjectId);
            hashCode.Add(Name);
            hashCode.Add(Visible);
            hashCode.Add(StartFrame);
            hashCode.Add(EndFrame);
            hashCode.Add(Positions.GetListHashCode());
            hashCode.Add(Rotations.GetListHashCode());
            hashCode.Add(Scales.GetListHashCode());
            hashCode.Add(Layer);
            hashCode.Add(Pivot);
            return hashCode.ToHashCode();
        }

        public bool Equals(Object other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = ObjectId.Equals(other.ObjectId)
                         && ParentObjectId.Equals(other.ParentObjectId)
                         && Name.Equals(other.Name)
                         && Visible == other.Visible
                         && StartFrame.Equals(other.StartFrame)
                         && EndFrame.Equals(other.EndFrame)
                         && Positions.ListEquals(other.Positions)
                         && Rotations.ListEquals(other.Rotations)
                         && Scales.ListEquals(other.Scales)
                         && Layer.Equals(other.Layer)
                         && Pivot.Equals(other.Pivot);
            return result;
        }
    }
}