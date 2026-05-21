using System;
using System.Collections.Generic;
using BH.SDK.Models.Enum.Values;
using BH.SDK.Models.Interfaces;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Values
{
    [RuleContainer]
    public class GradientValue : ICopyable<GradientValue>, IEquatable<GradientValue>
    {
        public const int MaxCount = 4;
        
        [RuleNotNull, RuleCollectionMaxCount(MaxCount)]
        [JsonProperty(Names.ColorKeys)]
        public List<GradientColorKeyValue> ColorKeys { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(MaxCount)]
        [JsonProperty(Names.AlphaKeys)]
        public List<GradientAlphaKeyValue> AlphaKeys { get; set; }
        
        [JsonProperty(Names.Mode)]
        public GradientInterpolationMode Mode { get; set; }
        
        [JsonProperty(Names.ColorSpace)]
        public GradientColorSpace ColorSpace { get; set; }
        
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

        public object Clone() => Copy();
        public GradientValue Copy() => new(ColorKeys.CopyList(), AlphaKeys.CopyList(), Mode, ColorSpace);

        public override bool Equals(object obj) => obj is GradientValue value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(ColorKeys.GetListHashCode(),
            AlphaKeys.GetListHashCode(), (int)Mode, (int)ColorSpace);

        public bool Equals(GradientValue other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = ColorKeys.ListEquals(other.ColorKeys)
                         && AlphaKeys.ListEquals(other.AlphaKeys)
                         && Mode == other.Mode
                         && ColorSpace == other.ColorSpace;
            return result;
        }
    }
}