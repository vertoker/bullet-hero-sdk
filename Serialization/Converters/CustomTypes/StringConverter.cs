using System;
using BH.SDK.Models.Enum.Values;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Values;
using BH.SDK.Serialization.Converters.Base;
using Newtonsoft.Json;

namespace BH.SDK.Serialization.Converters.CustomTypes
{
    public class StringConverter : JsonConverterCustomType<IString, StringType>
    {
        public StringConverter(JsonSerializer serializerDefault) : base(serializerDefault)
        {
            
        }

        public override StringType GetCustomType(IString value) => value.GetModelType();
        public override Type GetType(StringType customType)
        {
            return customType switch
            {
                StringType.Value => typeof(StringValue),
                StringType.Localized => typeof(StringLocalized),
                _ => throw new ArgumentOutOfRangeException(nameof(customType), customType, null)
            };
        }
    }
}