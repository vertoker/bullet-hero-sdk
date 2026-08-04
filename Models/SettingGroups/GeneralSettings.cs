using System;
using BH.SDK.Models.Interfaces;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BH.SDK.Models.SettingGroups
{
    /// <summary>
    /// Device-wide options that fit no other group - today, how aggressively a level's resources are
    /// fetched. Language is planned but not stored yet (see the TODO below).
    /// </summary>
    [RuleContainer]
    public class GeneralSettings : IModel<GeneralSettings>, IMoveable<GeneralSettings>
    {
        /// <summary> How many resources download/load at once. Higher is faster on a good
        /// connection, worse on a phone with a weak one. </summary>
        [RuleInRange(1, 8)]
        [JsonProperty(Names.ResourceParallelLoadCount)]
        public int ResourceParallelLoadCount { get; set; }

        /// <summary> Seconds before a remote resource fetch is given up on and its next fallback
        /// source is tried. </summary>
        [RuleMin(0f)]
        [JsonProperty(Names.ResourceWebTimeout)]
        public float ResourceWebTimeout { get; set; }
        
        // TODO add and integrate language with Unity Localization package (save as string like "en" or "ru")

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
        public void Reset()
        {
            ResourceParallelLoadCount = 2;
            ResourceWebTimeout = 5f;
        }

        public object Clone() => Copy();
        public GeneralSettings Copy() => new(ResourceParallelLoadCount, ResourceWebTimeout);

        public void Pull(GeneralSettings source)
        {
            ResourceParallelLoadCount = source.ResourceParallelLoadCount;
            ResourceWebTimeout = source.ResourceWebTimeout;
        }

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