using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using UnityEngine;

namespace BHSDK.Models.V1.Values
{
    public class ColorMinMaxV1 : IColorMinMax
    {
        public float MinR { get; set; }
        public float MinG { get; set; }
        public float MinB { get; set; }
        public float MinA { get; set; }
        public float MaxR { get; set; }
        public float MaxG { get; set; }
        public float MaxB { get; set; }
        public float MaxA { get; set; }

        public ColorMinMaxV1()
        {
            MinR = 0f;
            MinG = 0f;
            MinB = 0f;
            MinA = 0f;
            MaxR = 1f;
            MaxG = 1f;
            MaxB = 1f;
            MaxA = 1f;
        }
        public ColorMinMaxV1(float minR, float minG, float minB, float minA, 
            float maxR, float maxG, float maxB, float maxA)
        {
            MinR = minR;
            MinG = minG;
            MinB = minB;
            MinA = minA;
            MaxR = maxR;
            MaxG = maxG;
            MaxB = maxB;
            MaxA = maxA;
        }
        public ColorMinMaxV1(IFloat minR, IFloat minG, IFloat minB, IFloat minA, 
            IFloat maxR, IFloat maxG, IFloat maxB, IFloat maxA)
        {
            MinR = minR.Get();
            MinG = minG.Get();
            MinB = minB.Get();
            MinA = minA.Get();
            MaxR = maxR.Get();
            MaxG = maxG.Get();
            MaxB = maxB.Get();
            MaxA = maxA.Get();
        }

        public ColorType Type => ColorType.RandomMinMax;
        public Color Get()
        {
            var r = Random.Range(MinR, MaxR);
            var g = Random.Range(MinG, MaxG);
            var b = Random.Range(MinB, MaxB);
            var a = Random.Range(MinA, MaxA);
            return new Color(r, g, b, a);
        }
    }
}