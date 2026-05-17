using System;
using BHSDK.Models.Interfaces;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.SettingGroups
{
    [RuleContainer]
    public class GeneralSettings : ICopyable<GeneralSettings>, IEquatable<GeneralSettings>
    {
        [RuleInRange(1, 8)]
        [JsonProperty(Names.ResourceParallelLoadCount)]
        public int ResourceParallelLoadCount { get; set; }
        
        [RuleMin(0f)]
        [JsonProperty(Names.ResourceParallelLoadCount)]
        public float ResourceWebTimeout { get; set; }

        public GeneralSettings()
        {
            ResourceParallelLoadCount = 2;
            ResourceWebTimeout = 5f;
        }
        public GeneralSettings(int resourceParallelLoadCount, float resourceWebTimeout)
        {
            ResourceParallelLoadCount = resourceParallelLoadCount;
            ResourceWebTimeout = resourceWebTimeout;
        }

        public object Clone() => Copy();
        public GeneralSettings Copy() => new(ResourceParallelLoadCount, ResourceWebTimeout);

        public override bool Equals(object obj) => obj is GeneralSettings value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(ResourceParallelLoadCount, ResourceWebTimeout);

        public bool Equals(GeneralSettings other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return ResourceParallelLoadCount == other.ResourceParallelLoadCount
                   && ResourceWebTimeout.Equals(other.ResourceWebTimeout);
        }
    }
}