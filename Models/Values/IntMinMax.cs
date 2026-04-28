using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class IntMinMax : IInt, ICopyable<IntMinMax>
    {
        [JsonProperty(Names.Min)]
        public int Min { get; set; }
        
        [JsonProperty(Names.Max)]
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
        
        IInt ICopyable<IInt>.Copy() => new IntMinMax(Min, Max);
        public IntMinMax Copy() => new(Min, Max);
    }
}