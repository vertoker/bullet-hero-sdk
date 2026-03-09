using BHSDK.Models.Interfaces.Values;
using UnityEngine;

namespace BHSDK.Models.V1.Values
{
    public class IntValueMinMaxV1 : IIntValue
    {
        public int Min { get; set; }
        public int Max { get; set; }

        public IntValueMinMaxV1()
        {
            Min = 0;
            Max = 1;
        }
        public IntValueMinMaxV1(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public int Get() => Random.Range(Min, Max);
    }
}