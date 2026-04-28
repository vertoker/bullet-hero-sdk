using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class ColorThemeRef : IColor, ICopyable<ColorThemeRef>
    {
        [JsonProperty(Names.ThemeIndex)]
        public int ThemeIndex { get; set; }
        
        public ColorType GetModelType() => ColorType.ThemeRef;
        public Color Get()
        {
            throw new System.NotImplementedException();
        }

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