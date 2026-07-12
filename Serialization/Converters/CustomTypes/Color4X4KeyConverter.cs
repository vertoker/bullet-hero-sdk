using System;
using BH.SDK.Models.Enum.Keyframes;
using BH.SDK.Models.Interfaces.Keyframes;
using BH.SDK.Models.Keyframes;
using BH.SDK.Serialization.Converters.Base;
using Newtonsoft.Json;

namespace BH.SDK.Serialization.Converters.CustomTypes
{
    public class Color4X4KeyConverter : JsonConverterCustomType<IColor4X4Key, Color4X4KeyType>
    {
        public Color4X4KeyConverter(JsonSerializer serializerDefault) : base(serializerDefault)
        {

        }

        public override Color4X4KeyType GetCustomType(IColor4X4Key value) => value.GetModelType();
        public override Type GetType(Color4X4KeyType customType)
        {
            return customType switch
            {
                Color4X4KeyType.Value => typeof(Color4Key),
                Color4X4KeyType.Horizontal => typeof(ColorHorizontalKey),
                Color4X4KeyType.Vertical => typeof(ColorVerticalKey),
                Color4X4KeyType.BariCentrical => typeof(Color4X4Key),
                _ => throw new ArgumentOutOfRangeException(nameof(customType), customType, null)
            };
        }
    }
}
