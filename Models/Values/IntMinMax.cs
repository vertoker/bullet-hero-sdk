using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class IntMinMax : IInt
    {
        [JsonProperty(ModelNames.Min)]
        public int Min { get; set; }
        
        [JsonProperty(ModelNames.Max)]
        public int Max { get; set; }

        public IntMinMax()
        {
            Min = 0;
            Max = 1;
        }
        public IntMinMax(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public IntType GetModelType() => IntType.RandomMinMax;
        public int Get() => Random.Range(Min, Max);
    }
}