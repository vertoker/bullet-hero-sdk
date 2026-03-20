using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class IntMinMaxStep : IInt
    {
        [JsonProperty("mn")]
        public int Min { get; set; }
        
        [JsonProperty("mx")]
        public int Max { get; set; }
        
        [JsonProperty("s")]
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

        public IntType Type => IntType.RandomMinMaxStep;
        public int Get()
        {
            var value = Random.Range(Min, Max);
            value = Mathf.Clamp(Mathf.RoundToInt(value / (float)Step) * Step, Min, Max);
            return value;
        }
    }
}