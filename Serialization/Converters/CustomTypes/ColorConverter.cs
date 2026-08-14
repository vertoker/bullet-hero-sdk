using System;
using BH.SDK.Models.Enums.Values;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Values;
using BH.SDK.Serialization.Converters.Base;

namespace BH.SDK.Serialization.Converters.CustomTypes
{
    public class ColorConverter : JsonConverterCustomType<IColor4, ColorType>
    {
        public override ColorType GetCustomType(IColor4 value) => value.GetModelType();
        public override Type GetType(ColorType customType)
        {
            return customType switch
            {
                ColorType.Value => typeof(Color4Value),
                ColorType.ThemeRef => typeof(Color4ThemeRef),
                ColorType.RandomMinMax => typeof(Color4MinMax),
                _ => throw new ArgumentOutOfRangeException(nameof(customType), customType, null)
            };
        }
    }
}