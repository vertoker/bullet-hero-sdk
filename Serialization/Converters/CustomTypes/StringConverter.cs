using System;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Serialization.Converters.Base;
using Newtonsoft.Json;

namespace BHSDK.Serialization.Converters.CustomTypes
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