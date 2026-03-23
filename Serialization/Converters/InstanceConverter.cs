using System;
using BHSDK.Models.Base;
using BHSDK.Models.Enum;
using BHSDK.Models.Instances;
using BHSDK.Serialization.Converters.Base;
using Newtonsoft.Json;

namespace BHSDK.Serialization.Converters
{
    public class InstanceConverter : JsonConverterCustomType<Instance, InstanceType>
    {
        public InstanceConverter(JsonSerializer serializerDefault) : base(serializerDefault)
        {
            
        }

        public override InstanceType GetCustomType(Instance value) => value.GetModelType();
        public override Type GetType(InstanceType customType)
        {
            return customType switch
            {
                InstanceType.Base => typeof(Instance),
                InstanceType.Texture => typeof(TextureInstance),
                InstanceType.Text => typeof(TextInstance),
                InstanceType.Effect => typeof(EffectInstance),
                _ => throw new ArgumentOutOfRangeException(nameof(customType), customType, null)
            };
        }
    }
}