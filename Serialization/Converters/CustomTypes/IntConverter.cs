using System;
using BH.SDK.Models.Enum.Values;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Values;
using BH.SDK.Serialization.Converters.Base;

namespace BH.SDK.Serialization.Converters.CustomTypes
{
    public class IntConverter : JsonConverterCustomType<IInt, IntType>
    {
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