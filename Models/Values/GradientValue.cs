using System;
using System.Collections.Generic;
using System.Linq;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class GradientValue : ICopyable<GradientValue>
    {
        public const int MaxCount = 4;
        
        [JsonProperty(Names.ColorKeys)]
        public List<GradientColorKeyValue> ColorKeys { get; set; }
        
        [JsonProperty(Names.AlphaKeys)]
        public List<GradientAlphaKeyValue> AlphaKeys { get; set; }
        
        [JsonProperty(Names.Mode)]
        public GradientInterpolationMode Mode { get; set; }
        
        [JsonProperty(Names.ColorSpace)]
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

        public GradientValue Copy() => new(ColorKeys.Select(color => color.Copy()).ToList(),
            AlphaKeys.Select(alpha => alpha.Copy()).ToList(), Mode, ColorSpace);
    }
}