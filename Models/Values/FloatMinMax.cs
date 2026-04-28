using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
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
        public float Get() => Random.Range(Min, Max);
        
        IFloat ICopyable<IFloat>.Copy() => new FloatMinMax(Min, Max);
        public FloatMinMax Copy() => new(Min, Max);
    }
}