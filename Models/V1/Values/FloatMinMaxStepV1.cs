using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using UnityEngine;

namespace BHSDK.Models.V1.Values
{
    public class FloatMinMaxStepV1 : IFloatMinMaxStep
    {
        public float Min { get; set; }
        public float Max { get; set; }
        public float Step { get; set; }

        public FloatMinMaxStepV1()
        {
            Min = 0f;
            Max = 1f;
            Step = 1f;
        }
        public FloatMinMaxStepV1(float min, float max, float step)
        {
            Min = min;
            Max = max;
            Step = step;
        }

        public FloatType Type => FloatType.RandomMinMaxStep;
        public float Get()
        {
            var value = Random.Range(Min, Max);
            value = Mathf.Clamp(Mathf.Round(value / Step) * Step, Min, Max);
            return value;
        }
    }
}