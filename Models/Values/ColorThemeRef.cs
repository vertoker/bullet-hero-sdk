using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Values
{
    [RuleContainer]
    public class ColorThemeRef : IColor, ICopyable<ColorThemeRef>
    {
        [RuleInRange(0, ValueRules.ThemeCount - 1)]
        [JsonProperty(Names.ThemeIndex)]
        public int ThemeColorIndex { get; set; }
        
        public ColorType GetModelType() => ColorType.ThemeRef;

        public ColorThemeRef()
        {
            ThemeColorIndex = 0;
        }
        public ColorThemeRef(int themeColorIndex)
        {
            ThemeColorIndex = themeColorIndex;
        }
        
        public object Clone() => Copy();
        IColor ICopyable<IColor>.Copy() => new ColorThemeRef(ThemeColorIndex);
        public ColorThemeRef Copy() => new(ThemeColorIndex);
    }
}