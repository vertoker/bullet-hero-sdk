using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Values
{
    [RuleContainer]
    public class IntMinMaxStep : IInt, ICopyable<IntMinMaxStep>
    {
        [JsonProperty(Names.Min)]
        public int Min { get; set; }
        
        [JsonProperty(Names.Max)]
        public int Max { get; set; }
        
        [RuleMin(0)]
        [JsonProperty(Names.Step)]
        public int Step { get; set; }

        public IntMinMaxStep()
        {
            Min = 0;
            Max = 1;
            Step = 1;
        }
        public IntMinMaxStep(int min, int max, int step)
        {
            Min = min;
            Max = max;
            Step = step;
        }

        public IntType GetModelType() => IntType.RandomMinMaxStep;

        IInt ICopyable<IInt>.Copy() => new IntMinMaxStep(Min, Max, Step);
        public IntMinMaxStep Copy() => new(Min, Max, Step);
    }
}