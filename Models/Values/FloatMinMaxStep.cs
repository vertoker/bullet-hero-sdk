using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class FloatMinMaxStep : IFloat
    {
        [JsonProperty("mn")]
        public float Min { get; set; }
        
        [JsonProperty("mx")]
        public float Max { get; set; }
        
        [JsonProperty("s")]
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