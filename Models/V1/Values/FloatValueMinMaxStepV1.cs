using BHSDK.Models.Interfaces.Values;
using UnityEngine;

namespace BHSDK.Models.V1.Values
{
    public class FloatValueMinMaxStepV1 : IFloatValue
    {
        public float Min { get; set; }
        public float Max { get; set; }
        public float Step { get; set; }

        public FloatValueMinMaxStepV1()
        {
            Min = 0f;
            Max = 1f;
            Step = 1f;
        }
        public FloatValueMinMaxStepV1(float min, float max, float step)
        {
            Min = min;
            Max = max;
            Step = step;
        }

        public float Get()
        {
            var value = Random.Range(Min, Max);
            value = Mathf.Clamp(Mathf.Floor(value / Step) * Step, Min, Max);
            return value;
        }
    }
}