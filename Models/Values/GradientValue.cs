using System;
using System.Collections.Generic;
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
        
        public Gradient Get()
        {
            Span<GradientColorKey> colorKeys = stackalloc GradientColorKey[ColorKeys.Count];
            Span<GradientAlphaKey> alphaKeys = stackalloc GradientAlphaKey[AlphaKeys.Count];
            
            for (var i = 0; i < colorKeys.Length; i++) colorKeys[i] = ColorKeys[i].Get();
            for (var i = 0; i < alphaKeys.Length; i++) alphaKeys[i] = AlphaKeys[i].Get();
            
            var gradient = new Gradient();
            gradient.SetKeys(colorKeys, alphaKeys);
            
            return gradient;
        }

        public GradientValue()
        {
            ColorKeys = new List<GradientColorKeyValue>();
            AlphaKeys = new List<GradientAlphaKeyValue>();
        }
        public GradientValue(List<GradientColorKeyValue> colorKeys, List<GradientAlphaKeyValue> alphaKeys)
        {
            ColorKeys = colorKeys;
            AlphaKeys = alphaKeys;
        }
        public GradientValue(Gradient gradient)
        {
            AlphaKeys = new List<GradientAlphaKeyValue>(gradient.alphaKeyCount);
            ColorKeys = new List<GradientColorKeyValue>(gradient.colorKeyCount);
            
            foreach (var alphaKey in gradient.alphaKeys)
                AlphaKeys.Add(new GradientAlphaKeyValue(alphaKey));
            foreach (var colorKey in gradient.colorKeys)
                ColorKeys.Add(new GradientColorKeyValue(colorKey));
        }
    }
}