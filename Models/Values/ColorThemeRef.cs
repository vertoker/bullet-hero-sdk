using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Values
{
    public class ColorThemeRef : IColor, ICopyable<ColorThemeRef>
    {
        [RuleThemeIndex]
        [JsonProperty(Names.ThemeIndex)]
        public int ThemeIndex { get; set; }
        
        public ColorType GetModelType() => ColorType.ThemeRef;

        public ColorThemeRef()
        {
            ThemeIndex = 0;
        }
        public ColorThemeRef(int themeIndex)
        {
            ThemeIndex = themeIndex;
        }
        
        IColor ICopyable<IColor>.Copy() => new ColorThemeRef(ThemeIndex);
        public ColorThemeRef Copy() => new(ThemeIndex);
    }
}