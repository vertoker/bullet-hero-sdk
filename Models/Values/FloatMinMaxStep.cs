using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class FloatMinMaxStep : IFloat
    {
        [JsonProperty(Names.Min)]
        public float Min { get; set; }
        
        [JsonProperty(Names.Max)]
        public float Max { get; set; }
        
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
        public float Get()
        {
            var value = Random.Range(Min, Max);
            value = Mathf.Clamp(Mathf.Round(value / Step) * Step, Min, Max);
            return value;
        }
    }
}