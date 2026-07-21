using System;
using BH.SDK.Models.Enum.Values;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Values;
using BH.SDK.Serialization.Converters.Base;

namespace BH.SDK.Serialization.Converters.CustomTypes
{
    public class Color3Converter : JsonConverterCustomType<IColor3, ColorType>
    {
        public override ColorType GetCustomType(IColor3 value) => value.GetModelType();
        public override Type GetType(ColorType customType)
        {
            return customType switch
            {
                ColorType.Value => typeof(Color3Value),
                ColorType.ThemeRef => typeof(Color3ThemeRef),
                ColorType.RandomMinMax => typeof(Color3MinMax),
                _ => throw new ArgumentOutOfRangeException(nameof(customType), customType, null)
            };
        }
    }
}
