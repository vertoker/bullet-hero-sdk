using System;
using BHSDK.Models.Enum.Resources;
using BHSDK.Models.Interfaces;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BHSDK.Models.Resources
{
    [RuleContainer]
    public class ResourceKey : ICopyable<ResourceKey>, IEquatable<ResourceKey>
    {
        // URI - Universal Resource Identifier, either for paths, urls or keys
        
        [JsonProperty(Names.UriType)]
        public ResourceUriType UriType { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Uri)]
        public string Uri { get; set; }
        
        public ResourceKey()
        {
            UriType = ResourceUriType.Undefined;
            Uri = string.Empty;
        }
        public ResourceKey(ResourceUriType uriType, string uri)
        {
            UriType = uriType;
            Uri = uri;
        }

        public object Clone() => Copy();
        public ResourceKey Copy() => new(UriType, Uri);

        public bool Equals(ResourceKey other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = UriType == other.UriType && Uri.Equals(other.Uri);
            return result;
        }

        public override bool Equals(object obj) => obj is Resource value && Equals(value);
        public override int GetHashCode() => HashCode.Combine((int)UriType, Uri);
    }
}