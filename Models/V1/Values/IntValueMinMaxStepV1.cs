using BHSDK.Models.Interfaces.Values;
using UnityEngine;

namespace BHSDK.Models.V1.Values
{
    public class IntValueMinMaxStepV1 : IIntValue
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public int Step { get; set; }

        public IntValueMinMaxStepV1()
        {
            Min = 0;
            Max = 1;
            Step = 1;
        }
        public IntValueMinMaxStepV1(int min, int max, int step)
        {
            Min = min;
            Max = max;
            Step = step;
        }

        public int Get()
        {
            var value = Random.Range(Min, Max);
            value = Mathf.Clamp(Mathf.RoundToInt(value / (float)Step) * Step, Min, Max);
            return value;
        }
    }
}