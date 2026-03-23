using System;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Serialization.Converters.Base;
using Newtonsoft.Json;

namespace BHSDK.Serialization.Converters
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
                ColorType.RandomMinMax => typeof(ColorMinMax),
                _ => throw new ArgumentOutOfRangeException(nameof(customType), customType, null)
            };
        }
    }
}