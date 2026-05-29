using System;
using System.Collections.Generic;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Keyframes;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using Newtonsoft.Json;

namespace BH.SDK.Models.Objects
{
    public class RectObject : Object, ICopyable<RectObject>, IEquatable<RectObject>, IUpdatable<RectObject>
    {
        public override ObjectType GetModelType() => ObjectType.RectObject;
        
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
            : base(objectId, parentObjectId, name, visible, startFrame, endFrame)
        {
            Positions = positions;
            Layers = layers;
            Rotations = rotations;
            Scales = scales;
            Sizes = sizes;
            AnchorsMin = anchorsMin;
            AnchorsMax = anchorsMax;
            Pivots = pivots;
        }
        
        public override object Clone() => CopyImpl();
        public override Object Copy() => CopyImpl();
        
        RectObject ICopyable<RectObject>.Copy() => CopyImpl();
        
        private RectObject CopyImpl() => new(ObjectId, ParentObjectId, Name, Visible, StartFrame, EndFrame,
            Positions.CopyList(), Layers.CopyList(), Rotations.CopyList(), Scales.CopyList(), Sizes.CopyList(),
            AnchorsMin.CopyList(), AnchorsMax.CopyList(), Pivots.CopyList());

        public void Update(RectObject src)
        {
            base.Update(src);
            
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
            hashCode.Add(base.GetHashCode());
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

        public bool Equals(RectObject other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
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