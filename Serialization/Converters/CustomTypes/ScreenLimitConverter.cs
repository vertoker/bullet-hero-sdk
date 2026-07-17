using System;
using BH.SDK.Models.Enum.Values;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Values;
using BH.SDK.Serialization.Converters.Base;

namespace BH.SDK.Serialization.Converters.CustomTypes
{
    public class ScreenLimitConverter : JsonConverterCustomType<IScreenLimit, ScreenLimitType>
    {
        public override ScreenLimitType GetCustomType(IScreenLimit value) => value.GetModelType();
        public override Type GetType(ScreenLimitType customType)
        {
            return customType switch
            {
                ScreenLimitType.None => typeof(ScreenLimitNone),
                ScreenLimitType.Fixed => typeof(ScreenLimitFixed),
                ScreenLimitType.Bounds => typeof(ScreenLimitBounds),
                _ => throw new ArgumentOutOfRangeException(nameof(customType), customType, null)
            };
        }
    }
}