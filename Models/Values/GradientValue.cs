using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class GradientValue
    {
        public const int MaxCount = 4;
        
        [JsonProperty(ModelNames.Alpha + ModelNames.Keys)]
        public List<GradientAlphaKeyValue> AlphaKeys { get; set; }
        
        [JsonProperty(ModelNames.Color + ModelNames.Keys)]
        public List<GradientColorKeyValue> ColorKeys { get; set; }
        
        public Gradient Get()
        {
            Span<GradientAlphaKey> alphaKeys = stackalloc GradientAlphaKey[AlphaKeys.Count];
            Span<GradientColorKey> colorKeys = stackalloc GradientColorKey[ColorKeys.Count];
            
            for (var i = 0; i < alphaKeys.Length; i++) alphaKeys[i] = AlphaKeys[i].Get();
            for (var i = 0; i < colorKeys.Length; i++) colorKeys[i] = ColorKeys[i].Get();
            
            var gradient = new Gradient();
            gradient.SetAlphaKeys(alphaKeys);
            gradient.SetColorKeys(colorKeys);
            
            return gradient;
        }

        public GradientValue()
        {
            AlphaKeys = new List<GradientAlphaKeyValue>();
            ColorKeys = new List<GradientColorKeyValue>();
        }
        public GradientValue(List<GradientAlphaKeyValue> alphaKeys, List<GradientColorKeyValue> colorKeys)
        {
            AlphaKeys = alphaKeys;
            ColorKeys = colorKeys;
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