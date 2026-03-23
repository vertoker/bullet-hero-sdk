using System;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Serialization.Converters.Base;
using Newtonsoft.Json;

namespace BHSDK.Serialization.Converters
{
    public class ScreenLimitConverter : JsonConverterCustomType<IScreenLimit, ScreenLimitType>
    {
        public ScreenLimitConverter(JsonSerializer serializerDefault) : base(serializerDefault)
        {
            
        }

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