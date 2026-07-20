using System;
using BH.SDK.Models.Enum.Values;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Values
{
    [RuleContainer]
    public class ColorMinMax : IColor, IModel<ColorMinMax>
    {
        [RuleInRange(ValueRules.MinColor, ValueRules.MaxColor)]
        [JsonProperty(Names.MinR)]
        public float MinR { get; set; }
        
        [RuleInRange(ValueRules.MinColor, ValueRules.MaxColor)]
        [JsonProperty(Names.MinG)]
        public float MinG { get; set; }
        
        [RuleInRange(ValueRules.MinColor, ValueRules.MaxColor)]
        [JsonProperty(Names.MinB)]
        public float MinB { get; set; }
        
        [RuleInRange(ValueRules.MinColor, ValueRules.MaxColor)]
        [JsonProperty(Names.MinA)]
        public float MinA { get; set; }
        
        [RuleInRange(ValueRules.MinColor, ValueRules.MaxColor)]
        [JsonProperty(Names.MaxR)]
        public float MaxR { get; set; }
        
        [RuleInRange(ValueRules.MinColor, ValueRules.MaxColor)]
        [JsonProperty(Names.MaxG)]
        public float MaxG { get; set; }
        
        [RuleInRange(ValueRules.MinColor, ValueRules.MaxColor)]
        [JsonProperty(Names.MaxB)]
        public float MaxB { get; set; }
        
        [RuleInRange(ValueRules.MinColor, ValueRules.MaxColor)]
        [JsonProperty(Names.MaxA)]
        public float MaxA { get; set; }

        public ColorMinMax()
        {
            MinR = ValueRules.MinColor;
            MinG = ValueRules.MinColor;
            MinB = ValueRules.MinColor;
            MinA = ValueRules.MinColor;
            
            MaxR = ValueRules.MaxColor;
            MaxG = ValueRules.MaxColor;
            MaxB = ValueRules.MaxColor;
            MaxA = ValueRules.MaxColor;
        }
        public ColorMinMax(float minR, float minG, float minB, float minA, 
            float maxR, float maxG, float maxB, float maxA)
        {
            MinR = minR;
            MinG = minG;
            MinB = minB;
            MinA = minA;
            
            MaxR = maxR;
            MaxG = maxG;
            MaxB = maxB;
            MaxA = maxA;
        }
        public void Reset()
        {
            MinR = ValueRules.MinColor;
            MinG = ValueRules.MinColor;
            MinB = ValueRules.MinColor;
            MinA = ValueRules.MinColor;
            
            MaxR = ValueRules.MaxColor;
            MaxG = ValueRules.MaxColor;
            MaxB = ValueRules.MaxColor;
            MaxA = ValueRules.MaxColor;
        }

        public ColorType GetModelType() => ColorType.RandomMinMax;
        
        public object Clone() => Copy();
        IColor ICopyable<IColor>.Copy() => new ColorMinMax(MinR, MinG, MinB, MinA, MaxR, MaxG, MaxB, MaxA);
        public ColorMinMax Copy() => new(MinR, MinG, MinB, MinA, MaxR, MaxG, MaxB, MaxA);

        public override bool Equals(object obj) => obj is ColorMinMax value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(MinR, MinG, MinB, MinA, MaxR, MaxG, MaxB, MaxA);
        
        public bool Equals(IColor other) => other is ColorMinMax value && Equals(value);
        public bool Equals(ColorMinMax other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = MinR.Equals(other.MinR)
                         && MinG.Equals(other.MinG)
                         && MinB.Equals(other.MinB)
                         && MinA.Equals(other.MinA)
                         && MaxR.Equals(other.MaxR)
                         && MaxG.Equals(other.MaxG)
                         && MaxB.Equals(other.MaxB)
                         && MaxA.Equals(other.MaxA);
            return result;
        }
    }
}