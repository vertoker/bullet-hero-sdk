using System;
using BH.SDK.Models.Enums.Meta;

namespace BH.SDK.Models.Interfaces.Values
{
    public interface ILicense : IModel<ILicense>
    {
        public LicenseType GetModelType();
    }
}