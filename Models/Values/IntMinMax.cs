using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class IntMinMax : IInt
    {
        [JsonProperty("mn")]
        public int Min { get; set; }
        
        [JsonProperty("mx")]
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

        public IntType Type => IntType.RandomMinMax;
        public int Get() => Random.Range(Min, Max);
    }
}