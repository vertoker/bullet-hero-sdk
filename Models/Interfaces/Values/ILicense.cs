using System;
using BH.SDK.Models.Enum.Meta;

namespace BH.SDK.Models.Interfaces.Values
{
    public interface ILicense : ICopyable<ILicense>, IEquatable<ILicense>
    {
        public LicenseType GetModelType();
    }
}