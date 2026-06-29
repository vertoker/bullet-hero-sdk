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
    public class RectObject : ICopyable<RectObject>, IEquatable<RectObject>, IUpdatable<RectObject>
    {
        public virtual ObjectType GetModelType() => ObjectType.RectObject;
        
        [RuleObjectIdValid]
        [JsonProperty(Names.ObjectId)]
        public int ObjectId { get; set; }
        
        [RuleMin(IdRules.MinParentObjectId, IdRules.NullObjectId)]
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
        
        // Rect content
        
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxObjectKeys)]
        [RuleCollectionSorted(nameof(PosKey.Frame))]
        [RuleCollectionUnique(nameof(PosKey.Frame))]
        [JsonProperty(Names.Position)]
        public List<PosKey> Positions { get; set; }
        
        [RuleCollectionSorted(nameof(LayerKey.Frame))]
        [RuleCollectionUnique(nameof(LayerKey.Frame))]
        [JsonProperty(Names.Layer)]
        public List<LayerKey> Layers { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxObjectKeys)]
        [RuleCollectionSorted(nameof(AngleKey.Frame))]
        [RuleCollectionUnique(nameof(AngleKey.Frame))]
        [JsonProperty(Names.Rotation)]
        public List<AngleKey> Rotations { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxObjectKeys)]
        [RuleCollectionSorted(nameof(ScaKey.Frame))]
        [RuleCollectionUnique(nameof(ScaKey.Frame))]
        [JsonProperty(Names.Scale)]
        public List<ScaKey> Scales { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxObjectKeys)]
        [RuleCollectionSorted(nameof(ScaKey.Frame))]
        [RuleCollectionUnique(nameof(ScaKey.Frame))]
        [JsonProperty(Names.Size)]
        public List<ScaKey> Sizes { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxObjectKeys)]
        [RuleCollectionSorted(nameof(AlignmentKey.Frame))]
        [RuleCollectionUnique(nameof(AlignmentKey.Frame))]
        [JsonProperty(Names.AnchorMin)]
        public List<AlignmentKey> AnchorsMin { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxObjectKeys)]
        [RuleCollectionSorted(nameof(AlignmentKey.Frame))]
        [RuleCollectionUnique(nameof(AlignmentKey.Frame))]
        [JsonProperty(Names.AnchorMax)]
        public List<AlignmentKey> AnchorsMax { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxObjectKeys)]
        [RuleCollectionSorted(nameof(AlignmentKey.Frame))]
        [RuleCollectionUnique(nameof(AlignmentKey.Frame))]
        [JsonProperty(Names.Pivot)]
        public List<AlignmentKey> Pivots { get; set; }

        public RectObject()
        {
            ObjectId = IdRules.NullObjectId;
            ParentObjectId = IdRules.NullObjectId;
            Name = string.Empty;
            Visible = true;
            StartFrame = FrameRules.MinFrame;
            EndFrame = FrameRules.MinFrame;
            
            Positions = new List<PosKey>();
            Layers = new List<LayerKey>();
            Rotations = new List<AngleKey>();
            Scales = new List<ScaKey>();
            Sizes = new List<ScaKey>();
            AnchorsMin = new List<AlignmentKey>();
            AnchorsMax = new List<AlignmentKey>();
            Pivots = new List<AlignmentKey>();
        }
        public RectObject(int objectId, int parentObjectId, string name, bool visible, int startFrame, int endFrame,
            List<PosKey> positions, List<LayerKey> layers, List<AngleKey> rotations, List<ScaKey> scales, List<ScaKey> sizes,
            List<AlignmentKey> anchorsMin, List<AlignmentKey> anchorsMax, List<AlignmentKey> pivots)
        {
            ObjectId = objectId;
            ParentObjectId = parentObjectId;
            Name = name;
            Visible = visible;
            StartFrame = startFrame;
            EndFrame = endFrame;
            
            Positions = positions;
            Layers = layers;
            Rotations = rotations;
            Scales = scales;
            Sizes = sizes;
            AnchorsMin = anchorsMin;
            AnchorsMax = anchorsMax;
            Pivots = pivots;
        }

        public virtual object Clone() => CopyImpl();
        public virtual RectObject Copy() => CopyImpl();
        
        private RectObject CopyImpl() => new(ObjectId, ParentObjectId, Name, Visible, StartFrame, EndFrame,
            Positions.CopyList(), Layers.CopyList(), Rotations.CopyList(), Scales.CopyList(), Sizes.CopyList(),
            AnchorsMin.CopyList(), AnchorsMax.CopyList(), Pivots.CopyList());
        
        public void Update(RectObject src)
        {
            ObjectId = src.ObjectId;
            ParentObjectId = src.ParentObjectId;
            Name = src.Name;
            Visible = src.Visible;
            StartFrame = src.StartFrame;
            EndFrame = src.EndFrame;
            
            Positions = src.Positions.CopyList();
            Layers = src.Layers.CopyList();
            Rotations = src.Rotations.CopyList();
            Scales = src.Scales.CopyList();
            Sizes = src.Sizes.CopyList();
            AnchorsMin = src.AnchorsMin.CopyList();
            AnchorsMax = src.AnchorsMax.CopyList();
            Pivots = src.Pivots.CopyList();
        }

        public override bool Equals(object obj) => obj is RectObject value && Equals(value);
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
            hashCode.Add(Layers.GetListHashCode());
            hashCode.Add(Rotations.GetListHashCode());
            hashCode.Add(Scales.GetListHashCode());
            hashCode.Add(Sizes.GetListHashCode());
            hashCode.Add(AnchorsMin.GetListHashCode());
            hashCode.Add(AnchorsMax.GetListHashCode());
            hashCode.Add(Pivots.GetListHashCode());
            
            return hashCode.ToHashCode();
        }

        public virtual bool Equals(RectObject other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            
            var result = EqualsObject(other);
            return result;
        }
        
        protected bool EqualsObject(RectObject other)
        {
            var result = ObjectId.Equals(other.ObjectId)
                         && ParentObjectId.Equals(other.ParentObjectId)
                         && Name.Equals(other.Name)
                         && Visible == other.Visible
                         && StartFrame.Equals(other.StartFrame)
                         && EndFrame.Equals(other.EndFrame)
                         // rect content
                         && Positions.ListEquals(other.Positions)
                         && Layers.ListEquals(other.Layers)
                         && Rotations.ListEquals(other.Rotations)
                         && Scales.ListEquals(other.Scales)
                         && Sizes.ListEquals(other.Sizes)
                         && AnchorsMin.ListEquals(other.AnchorsMin)
                         && AnchorsMax.ListEquals(other.AnchorsMax)
                         && Pivots.ListEquals(other.Pivots);
            return result;
        }
    }
}