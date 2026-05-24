using System;
using System.Collections.Generic;
using BH.SDK.Models.Enum.Resources;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Resources
{
    [RuleContainer]
    public abstract class Resource : IEquatable<Resource>
    {
        public const int MaxSourcesCount = 4;
        
        [RuleNotNull, RuleCollectionMaxCount(MaxSourcesCount)]
        [JsonProperty(Names.Src)]
        public List<ResourceKey> Sources { get; set; }
        
        public abstract ResourceType Type { get; }

        protected Resource()
        {
            Sources = new List<ResourceKey>();
        }
        protected Resource(List<ResourceKey> sources)
        {
            Sources = sources;
        }

        public bool Equals(Resource other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Sources.ListEquals(other.Sources)
                         && Type == other.Type;
            return result;
        }

        public override bool Equals(object obj) => obj is Resource value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Sources.GetListHashCode(), (int)Type);
    }
}