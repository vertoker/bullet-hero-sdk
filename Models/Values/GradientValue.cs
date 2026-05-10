using System.Collections.Generic;
using System.Linq;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Values
{
    [RuleContainer]
    public class GradientValue : ICopyable<GradientValue>
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

        public GradientValue Copy() => new(ColorKeys.Select(color => color.Copy()).ToList(),
            AlphaKeys.Select(alpha => alpha.Copy()).ToList(), Mode, ColorSpace);
    }
}