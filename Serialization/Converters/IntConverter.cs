using System;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Serialization.Converters.Base;
using Newtonsoft.Json;

namespace BHSDK.Serialization.Converters
{
    public class IntConverter : JsonConverterCustomType<IInt, IntType>
    {
        public IntConverter(JsonSerializer serializerDefault) : base(serializerDefault)
        {
            
        }

        public override IntType GetCustomType(IInt value) => value.GetModelType();
        public override Type GetType(IntType customType)
        {
            return customType switch
            {
                IntType.Value => typeof(IntValue),
                IntType.RandomMinMax => typeof(IntMinMax),
                IntType.RandomMinMaxStep => typeof(IntMinMaxStep),
                _ => throw new ArgumentOutOfRangeException(nameof(customType), customType, null)
            };
        }
    }
}