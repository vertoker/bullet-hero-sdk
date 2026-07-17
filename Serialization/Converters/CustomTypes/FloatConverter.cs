using System;
using BH.SDK.Models.Enum.Values;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Values;
using BH.SDK.Serialization.Converters.Base;

namespace BH.SDK.Serialization.Converters.CustomTypes
{
    public class FloatConverter : JsonConverterCustomType<IFloat, FloatType>
    {
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