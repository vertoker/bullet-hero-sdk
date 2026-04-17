using System;
using System.Collections.Generic;
using BHSDK.Models.Enum.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class GradientValue
    {
        public const int MaxCount = 4;
        
        [JsonProperty(ModelNames.Color + ModelNames.Keys)]
        public List<GradientColorKeyValue> ColorKeys { get; set; }
        
        [JsonProperty(ModelNames.Alpha + ModelNames.Keys)]
        public List<GradientAlphaKeyValue> AlphaKeys { get; set; }
        
        [JsonProperty(ModelNames.Mode)]
        public GradientInterpolationMode Mode { get; set; }
        
        [JsonProperty(ModelNames.Color + ModelNames.Space)]
        public GradientColorSpace ColorSpace { get; set; }
        
        public Gradient Get()
        {
            Span<GradientColorKey> colorKeys = stackalloc GradientColorKey[ColorKeys.Count];
            Span<GradientAlphaKey> alphaKeys = stackalloc GradientAlphaKey[AlphaKeys.Count];
            
            for (var i = 0; i < colorKeys.Length; i++) colorKeys[i] = ColorKeys[i].Get();
            for (var i = 0; i < alphaKeys.Length; i++) alphaKeys[i] = AlphaKeys[i].Get();

            var gradient = new Gradient();
            gradient.SetKeys(colorKeys, alphaKeys);
            gradient.mode = (GradientMode)Mode;
            gradient.colorSpace = (ColorSpace)ColorSpace;
            
            return gradient;
        }

        public GradientValue()
        {
            ColorKeys = new List<GradientColorKeyValue>();
            AlphaKeys = new List<GradientAlphaKeyValue>();
            Mode = GradientInterpolationMode.PerceptualBlend;
            ColorSpace = GradientColorSpace.Linear;
        }
        public GradientValue(List<GradientColorKeyValue> colorKeys, List<GradientAlphaKeyValue> alphaKeys,
            GradientInterpolationMode mode, GradientColorSpace colorSpace)
        {
            ColorKeys = colorKeys;
            AlphaKeys = alphaKeys;
            Mode = mode;
            ColorSpace = colorSpace;
        }
        public GradientValue(Gradient gradient)
        {
            AlphaKeys = new List<GradientAlphaKeyValue>(gradient.alphaKeyCount);
            ColorKeys = new List<GradientColorKeyValue>(gradient.colorKeyCount);
            
            foreach (var alphaKey in gradient.alphaKeys)
                AlphaKeys.Add(new GradientAlphaKeyValue(alphaKey));
            foreach (var colorKey in gradient.colorKeys)
                ColorKeys.Add(new GradientColorKeyValue(colorKey));

            Mode = (GradientInterpolationMode)gradient.mode;
            ColorSpace = (GradientColorSpace)gradient.colorSpace;
        }
    }
}