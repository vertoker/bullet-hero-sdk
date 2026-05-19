using System;
using System.Collections.Generic;
using System.ComponentModel;
using BHSDK.Models.Enum.Meta;
using BHSDK.Models.Enum.Resources;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Resources;
using BHSDK.Models.Values;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.Meta
{
    public class ResourceMeta : ICopyable<ResourceMeta>, IEquatable<ResourceMeta>
    {
        [JsonProperty(Names.ResourceType)]
        public ResourceType ResourceType { get; set; }
        
        // Abstract resId, it cast to dedicated ids (audioResId, texResId...)
        [JsonProperty(Names.ResourceId)]
        public int ResourceId { get; set; }
        
        [RuleNotNull, RuleStringMax(ValueRules.MaxEditorName)]
        [JsonProperty(Names.Title)]
        public string ResourceTitle { get; set; }
        
        [RuleNotNull, RuleStringMax(ValueRules.MaxEditorDescription)]
        [JsonProperty(Names.Description)]
        public string ResourceDescription { get; set; }
        
        [JsonProperty(Names.Url)]
        public string ResourceUrl { get; set; }
        
        [JsonProperty(Names.License)]
        public ILicense ResourceLicense { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxAuthors)]
        [JsonProperty(Names.Authors)]
        public List<Author> ResourceAuthors { get; set; }

        public ResourceMeta()
        {
            ResourceType = ResourceType.Bytes;
            ResourceId = Resource.UninitializedId;
            ResourceTitle = string.Empty;
            ResourceDescription = string.Empty;
            ResourceLicense = new TypicalLicense(TypicalLicenseType.CC_BY_NC_4_0);
            ResourceAuthors = new List<Author>();
        }
        public ResourceMeta(ResourceType resourceType, int resourceId, string resourceTitle,
            string resourceDescription, string resourceUrl, ILicense resourceLicense, List<Author> resourceAuthors)
        {
            ResourceType = resourceType;
            ResourceId = resourceId;
            ResourceTitle = resourceTitle;
            ResourceDescription = resourceDescription;
            ResourceUrl = resourceUrl;
            ResourceLicense = resourceLicense;
            ResourceAuthors = resourceAuthors;
        }

        public object Clone() => Copy();
        public ResourceMeta Copy() => new(ResourceType, ResourceId, ResourceTitle, ResourceDescription,
            ResourceUrl, ResourceLicense.Copy(), ResourceAuthors.CopyList());

        public bool Equals(ResourceMeta other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return ResourceType == other.ResourceType
                   && ResourceId == other.ResourceId
                   && ResourceTitle.Equals(other.ResourceTitle)
                   && ResourceDescription.Equals(other.ResourceDescription)
                   && ResourceUrl.Equals(other.ResourceUrl)
                   && ResourceLicense.Equals(other.ResourceLicense)
                   && ResourceAuthors.ListEquals(other.ResourceAuthors);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ResourceMeta)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int)ResourceType, ResourceId, ResourceTitle,
                ResourceDescription, ResourceUrl, ResourceLicense, ResourceAuthors.GetListHashCode());
        }
    }
}