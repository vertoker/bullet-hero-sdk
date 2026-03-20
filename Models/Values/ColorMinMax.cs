using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class ColorMinMax : IColor
    {
        [JsonProperty("mnr")]
        public float MinR { get; set; }
        
        [JsonProperty("mng")]
        public float MinG { get; set; }
        
        [JsonProperty("mnb")]
        public float MinB { get; set; }
        
        [JsonProperty("mnb")]
        public float MinA { get; set; }
        
        
        [JsonProperty("mxr")]
        public float MaxR { get; set; }
        
        [JsonProperty("mxg")]
        public float MaxG { get; set; }
        
        [JsonProperty("mxb")]
        public float MaxB { get; set; }
        
        [JsonProperty("mxa")]
        public float MaxA { get; set; }

        public ColorMinMax()
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
        public ColorMinMax(float minR, float minG, float minB, float minA, 
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
        public ColorMinMax(IFloat minR, IFloat minG, IFloat minB, IFloat minA, 
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