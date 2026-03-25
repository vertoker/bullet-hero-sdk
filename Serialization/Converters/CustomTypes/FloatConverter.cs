using System;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Serialization.Converters.Base;
using Newtonsoft.Json;

namespace BHSDK.Serialization.Converters.CustomTypes
{
    public class FloatConverter : JsonConverterCustomType<IFloat, FloatType>
    {
        public FloatConverter(JsonSerializer serializerDefault) : base(serializerDefault)
        {
            
        }

        public override FloatType GetCustomType(IFloat value) => value.GetModelType();
        public override Type GetType(FloatType customType)
        {
            return customType switch
            {
                FloatType.Value => typeof(FloatValue),
                FloatType.RandomMinMax => typeof(FloatMinMax),
                FloatType.RandomMinMaxStep => typeof(FloatMinMaxStep),
                _ => throw new ArgumentOutOfRangeException(nameof(customType), customType, null)
            };
        }
    }
}