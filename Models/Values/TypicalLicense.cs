using System;
using BH.SDK.Models.Enum.Meta;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Values;
using Newtonsoft.Json;

namespace BH.SDK.Models.Values
{
    public class TypicalLicense : ILicense, ICopyable<TypicalLicense>, IEquatable<TypicalLicense>
    {
        [JsonProperty(Names.LicenseType)]
        public TypicalLicenseType Type { get; set; }

        public TypicalLicense()
        {
            Type = TypicalLicenseType.CC_BY_NC_4_0;
        }
        public TypicalLicense(TypicalLicenseType type)
        {
            Type = type;
        }

        public LicenseType GetModelType() => LicenseType.Typical;
        

        public object Clone() => Copy();
        ILicense ICopyable<ILicense>.Copy() => Copy();
        public TypicalLicense Copy() => new(Type);
        
        public override bool Equals(object obj) => obj is TypicalLicense value && Equals(value);
        public override int GetHashCode() => (int)Type;
        
        public bool Equals(ILicense other) => other is TypicalLicense value && Equals(value);
        public bool Equals(TypicalLicense other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Type == other.Type;
            return result;
        }
    }
}