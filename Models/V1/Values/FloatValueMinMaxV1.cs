using BHSDK.Models.Interfaces.Values;
using UnityEngine;

namespace BHSDK.Models.V1.Values
{
    public class FloatValueMinMaxV1 : IFloatValue
    {
        public float Min { get; set; }
        public float Max { get; set; }

        public FloatValueMinMaxV1()
        {
            Min = 0f;
            Max = 1f;
        }
        public FloatValueMinMaxV1(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public float Get() => Random.Range(Min, Max);
    }
}