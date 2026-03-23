using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class FloatMinMax : IFloat
    {
        [JsonProperty("mn")]
        public float Min { get; set; }
        
        [JsonProperty("mx")]
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
        public float Get() => Random.Range(Min, Max);
    }
}