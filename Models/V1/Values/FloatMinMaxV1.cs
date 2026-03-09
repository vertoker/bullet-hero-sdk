using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using UnityEngine;

namespace BHSDK.Models.V1.Values
{
    public class FloatMinMaxV1 : IFloatMinMax
    {
        public float Min { get; set; }
        public float Max { get; set; }

        public FloatMinMaxV1()
        {
            Min = 0f;
            Max = 1f;
        }
        public FloatMinMaxV1(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public FloatType Type => FloatType.RandomMinMax;
        public float Get() => Random.Range(Min, Max);
    }
}