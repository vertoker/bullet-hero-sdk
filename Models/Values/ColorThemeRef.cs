using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class ColorThemeRef : IColor
    {
        [JsonProperty(ModelNames.Theme + ModelNames.Index)]
        public IntValue ThemeIndex { get; set; }
        
        public ColorType GetModelType() => ColorType.ThemeRef;
        public Color Get()
        {
            // TODO решить проблему или удалить метод, нужна внешняя ссылка на темы
            throw new System.NotImplementedException();
        }
    }
}