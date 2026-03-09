using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using UnityEngine;

namespace BHSDK.Models.V1.Values
{
    public class IntMinMaxV1 : IIntMinMax
    {
        public int Min { get; set; }
        public int Max { get; set; }

        public IntMinMaxV1()
        {
            Min = 0;
            Max = 1;
        }
        public IntMinMaxV1(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public IntType Type => IntType.RandomMinMax;
        public int Get() => Random.Range(Min, Max);
    }
}