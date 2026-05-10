using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Values
{
    [RuleContainer]
    public class FloatMinMaxStep : IFloat, ICopyable<FloatMinMaxStep>
    {
        [JsonProperty(Names.Min)]
        public float Min { get; set; }
        
        [JsonProperty(Names.Max)]
        public float Max { get; set; }
        
        [RuleMin(0f)]
        [JsonProperty(Names.Step)]
        public float Step { get; set; }

        public FloatMinMaxStep()
        {
            Min = 0f;
            Max = 1f;
            Step = 1f;
        }
        public FloatMinMaxStep(float min, float max, float step)
        {
            Min = min;
            Max = max;
            Step = step;
        }

        public FloatType GetModelType() => FloatType.RandomMinMaxStep;

        IFloat ICopyable<IFloat>.Copy() => new FloatMinMaxStep(Min, Max, Step);
        public FloatMinMaxStep Copy() => new(Min, Max, Step);
    }
}