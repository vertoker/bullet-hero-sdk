using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Values
{
    [RuleContainer]
    public class ColorMinMax : IColor, ICopyable<ColorMinMax>
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
            MinR = 0f;
            MinG = 0f;
            MinB = 0f;
            MinA = 0f;
            MaxR = 1f;
            MaxG = 1f;
            MaxB = 1f;
            MaxA = 1f;
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

        public ColorType GetModelType() => ColorType.RandomMinMax;
        
        public object Clone() => Copy();
        IColor ICopyable<IColor>.Copy() => new ColorMinMax(MinR, MinG, MinB, MinA, MaxR, MaxG, MaxB, MaxA);
        public ColorMinMax Copy() => new(MinR, MinG, MinB, MinA, MaxR, MaxG, MaxB, MaxA);
    }
}