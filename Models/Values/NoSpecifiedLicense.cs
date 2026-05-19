using System;
using BHSDK.Models.Enum.Meta;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;

namespace BHSDK.Models.Values
{
    public class NoSpecifiedLicense : ILicense, ICopyable<NoSpecifiedLicense>, IEquatable<NoSpecifiedLicense>
    {
        public LicenseType GetModelType() => LicenseType.NoSpecified;

        public object Clone() => Copy();
        ILicense ICopyable<ILicense>.Copy() => Copy();
        public NoSpecifiedLicense Copy() => new();
        
        // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
        public override int GetHashCode() => base.GetHashCode();
        public override bool Equals(object obj) => obj is NoSpecifiedLicense value && Equals(value);
        
        public bool Equals(ILicense other) => other is not null && ReferenceEquals(this, other);
        public bool Equals(NoSpecifiedLicense other) => other is not null && ReferenceEquals(this, other);
    }
}