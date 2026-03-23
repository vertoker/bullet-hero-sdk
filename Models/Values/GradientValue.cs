using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class GradientValue
    {
        [JsonProperty("ak")]
        public List<GradientAlphaKeyValue> AlphaKeys { get; set; }
        
        [JsonProperty("ck")]
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
    }
}