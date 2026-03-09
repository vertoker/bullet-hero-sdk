using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using UnityEngine;

namespace BHSDK.Models.V1.Values
{
    public class IntMinMaxStepV1 : IIntMinMaxStep
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public int Step { get; set; }

        public IntMinMaxStepV1()
        {
            Min = 0;
            Max = 1;
            Step = 1;
        }
        public IntMinMaxStepV1(int min, int max, int step)
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