using System;
using BH.SDK.Models.Enum.Meta;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BH.SDK.Models.Values
{
    [RuleContainer]
    public class CustomLicense : ILicense, IModel<CustomLicense>
    {
        [RuleNotNull]
        [JsonProperty(Names.Name)]
        public string LicenseName { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Url)]
        public string LicenseUrl { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Text)]
        public string LicenseText { get; set; }
        
        /// <summary>
        /// If true - "Copyleft", if false - "Permissive"
        /// </summary>
        [JsonProperty(Names.Aggressive)]
        public bool Aggressive { get; set; }
        
        [JsonProperty(Names.AllowsDistribution)]
        public bool AllowsDistribution { get; set; }
        
        [JsonProperty(Names.AllowsModification)]
        public bool AllowsModification { get; set; }
        
        [JsonProperty(Names.AllowsCommercialUse)]
        public bool AllowsCommercialUse { get; set; }
        
        [JsonProperty(Names.RequiresAttribution)]
        public bool RequiresAttribution { get; set; }
        
        [JsonProperty(Names.RequiresSourceDisclosure)]
        public bool RequiresSourceDisclosure { get; set; }
        
        [JsonProperty(Names.RequiresSameLicense)]
        public bool RequiresSameLicense { get; set; }

        public CustomLicense()
        {
            LicenseName = string.Empty;
            LicenseUrl = string.Empty;
            LicenseText = string.Empty;
            Aggressive = false;
            AllowsDistribution = false;
            AllowsModification = false;
            AllowsCommercialUse = false;
            RequiresAttribution = false;
            RequiresSourceDisclosure = false;
            RequiresSameLicense = false;
        }
        public CustomLicense(string licenseName, string licenseUrl, string licenseText,
            bool aggressive, bool allowsDistribution, bool allowsModification, bool allowsCommercialUse,
            bool requiresAttribution, bool requiresSourceDisclosure, bool requiresSameLicense)
        {
            LicenseName = licenseName;
            LicenseUrl = licenseUrl;
            LicenseText = licenseText;
            Aggressive = aggressive;
            AllowsDistribution = allowsDistribution;
            AllowsModification = allowsModification;
            AllowsCommercialUse = allowsCommercialUse;
            RequiresAttribution = requiresAttribution;
            RequiresSourceDisclosure = requiresSourceDisclosure;
            RequiresSameLicense = requiresSameLicense;
        }
        public void Reset()
        {
            LicenseName = string.Empty;
            LicenseUrl = string.Empty;
            LicenseText = string.Empty;
            Aggressive = false;
            AllowsDistribution = false;
            AllowsModification = false;
            AllowsCommercialUse = false;
            RequiresAttribution = false;
            RequiresSourceDisclosure = false;
            RequiresSameLicense = false;
        }
        
        public LicenseType GetModelType() => LicenseType.Custom;

        public object Clone() => Copy();
        ILicense ICopyable<ILicense>.Copy() => Copy();
        public CustomLicense Copy() => new(LicenseName, LicenseUrl, LicenseText,
            Aggressive, AllowsDistribution, AllowsModification, AllowsCommercialUse,
            RequiresAttribution, RequiresSourceDisclosure, RequiresSameLicense);



        public override bool Equals(object obj) => obj is CustomLicense value && Equals(value);
        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(LicenseName);
            hashCode.Add(LicenseUrl);
            hashCode.Add(Aggressive);
            hashCode.Add(AllowsDistribution);
            hashCode.Add(AllowsModification);
            hashCode.Add(AllowsCommercialUse);
            hashCode.Add(RequiresAttribution);
            hashCode.Add(RequiresSourceDisclosure);
            hashCode.Add(RequiresSameLicense);
            return hashCode.ToHashCode();
        }
        
        public bool Equals(ILicense other) => other is CustomLicense value && Equals(value);
        public bool Equals(CustomLicense other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return LicenseName.Equals(other.LicenseName)
                   && LicenseUrl.Equals(other.LicenseUrl)
                   && LicenseText.Equals(other.LicenseText)
                   && Aggressive == other.Aggressive
                   && AllowsDistribution == other.AllowsDistribution
                   && AllowsModification == other.AllowsModification
                   && AllowsCommercialUse == other.AllowsCommercialUse
                   && RequiresAttribution == other.RequiresAttribution
                   && RequiresSourceDisclosure == other.RequiresSourceDisclosure
                   && RequiresSameLicense == other.RequiresSameLicense;
        }
    }
}