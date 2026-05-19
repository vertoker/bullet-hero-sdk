using System;
using BHSDK.Models.Enum.Meta;

namespace BHSDK.Models.Interfaces.Values
{
    public interface ILicense : ICopyable<ILicense>, IEquatable<ILicense>
    {
        public LicenseType GetModelType();
    }
}