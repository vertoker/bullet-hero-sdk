using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class ColorValue : IColor
    {
        [JsonProperty("r")]
        public float R { get; set; }
        
        [JsonProperty("g")]
        public float G { get; set; }
        
        [JsonProperty("b")]
        public float B { get; set; }
        
        [JsonProperty("a")]
        public float A { get; set; }

        public ColorValue()
        {
            R = 1f;
            G = 1f;
            B = 1f;
            A = 1f;
        }
        public ColorValue(float r, float g, float b, float a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }
        public ColorValue(Color color)
        {
            R = color.r;
            G = color.g;
            B = color.b;
            A = color.a;
        }
        public ColorValue(IFloat r, IFloat g, IFloat b, IFloat a)
        {
            R = r.Get();
            G = g.Get();
            B = b.Get();
            A = a.Get();
        }

        public ColorType Type => ColorType.Value;
        public UnityEngine.Color Get() => new(R, G, B, A);
    }
}