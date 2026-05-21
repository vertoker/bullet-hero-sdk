using System;
using BH.SDK.Models.Enum.Values;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Values;
using BH.SDK.Serialization.Converters.Base;
using Newtonsoft.Json;

namespace BH.SDK.Serialization.Converters.CustomTypes
{
    public class ColorConverter : JsonConverterCustomType<IColor, ColorType>
    {
        public ColorConverter(JsonSerializer serializerDefault) : base(serializerDefault)
        {
            
        }
        
        public override ColorType GetCustomType(IColor value) => value.GetModelType();
        public override Type GetType(ColorType customType)
        {
            return customType switch
            {
                ColorType.Value => typeof(ColorValue),
                ColorType.ThemeRef => typeof(ColorThemeRef),
                ColorType.RandomMinMax => typeof(ColorMinMax),
                _ => throw new ArgumentOutOfRangeException(nameof(customType), customType, null)
            };
        }
    }
}