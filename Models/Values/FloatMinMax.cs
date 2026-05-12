using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Values
{
    [RuleContainer]
    public class FloatMinMax : IFloat, ICopyable<FloatMinMax>
    {
        [JsonProperty(Names.Min)]
        public float Min { get; set; }
        
        [JsonProperty(Names.Max)]
        public float Max { get; set; }

        public FloatMinMax()
        {
            Min = 0f;
            Max = 1f;
        }
        public FloatMinMax(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public FloatType GetModelType() => FloatType.RandomMinMax;
        
        public object Clone() => Copy();
        IFloat ICopyable<IFloat>.Copy() => new FloatMinMax(Min, Max);
        public FloatMinMax Copy() => new(Min, Max);
    }
}