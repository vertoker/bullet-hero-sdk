using System;
using BH.SDK.Models.Enums.Meta;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Values;
using BH.SDK.Serialization.Converters.Base;

namespace BH.SDK.Serialization.Converters.CustomTypes
{
    public class LicenseConverter : JsonConverterCustomType<ILicense, LicenseType>
    {
        public override LicenseType GetCustomType(ILicense value) => value.GetModelType();
        public override Type GetType(LicenseType customType)
        {
            return customType switch
            {
                LicenseType.NoSpecified => typeof(NoSpecifiedLicense),
                LicenseType.Typical => typeof(TypicalLicense),
                LicenseType.Custom => typeof(CustomLicense),
                _ => throw new ArgumentOutOfRangeException(nameof(customType), customType, null)
            };
        }
    }
}
