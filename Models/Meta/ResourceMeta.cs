using System;
using System.Collections.Generic;
using BH.SDK.Models.Enum.Meta;
using BH.SDK.Models.Enum.Resources;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Resources;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using Newtonsoft.Json;

namespace BH.SDK.Models.Meta
{
    [RuleContainer]
    public class ResourceMeta : ICopyable<ResourceMeta>, IEquatable<ResourceMeta>
    {
        [JsonProperty(Names.ResourceType)]
        public ResourceType ResourceType { get; set; }
        
        // Abstract resId, it cast to dedicated ids (audioResId, texResId...)
        [JsonProperty(Names.ResourceId)]
        public int ResourceId { get; set; }
        
        [RuleNotNull(typeof(StringValue)), RuleIStringMax(ValueRules.MaxEditorName)]
        [JsonProperty(Names.Title)]
        public IString ResourceTitle { get; set; }
        
        [RuleNotNull(typeof(StringValue)), RuleIStringMax(ValueRules.MaxEditorDescription)]
        [JsonProperty(Names.Description)]
        public IString ResourceDescription { get; set; }
        
        [RuleNotNull, RuleStringMax(ValueRules.MaxUrl)]
        [JsonProperty(Names.Url)]
        public string ResourceUrl { get; set; }
        
        [RuleNotNull(typeof(NoSpecifiedLicense))]
        [JsonProperty(Names.License)]
        public ILicense ResourceLicense { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxSources)]
        [JsonProperty(Names.Sources)]
        public List<IString> ResourceSources { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxAuthors)]
        [JsonProperty(Names.Authors)]
        public List<Author> ResourceAuthors { get; set; }
        
        // TODO add method for author permission to use resource (for whole BH or several levels / unlimited or time limit)

        public ResourceMeta()
        {
            ResourceType = ResourceType.Bytes;
            ResourceId = Resource.UninitializedId;
            ResourceTitle = new StringValue();
            ResourceDescription = new StringValue();
            ResourceLicense = new TypicalLicense(TypicalLicenseType.CC_BY_NC_4_0);
            ResourceSources = new List<IString>();
            ResourceAuthors = new List<Author>();
        }
        public ResourceMeta(ResourceType resourceType, int resourceId, IString resourceTitle,
            IString resourceDescription, string resourceUrl, ILicense resourceLicense,
            List<IString> resourceSources, List<Author> resourceAuthors)
        {
            ResourceType = resourceType;
            ResourceId = resourceId;
            ResourceTitle = resourceTitle;
            ResourceDescription = resourceDescription;
            ResourceUrl = resourceUrl;
            ResourceLicense = resourceLicense;
            ResourceSources = resourceSources;
            ResourceAuthors = resourceAuthors;
        }

        public object Clone() => Copy();
        public ResourceMeta Copy() => new(ResourceType, ResourceId, ResourceTitle.Copy(), 
            ResourceDescription.Copy(), ResourceUrl, ResourceLicense.Copy(),
            ResourceSources.CopyList(), ResourceAuthors.CopyList());

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
                ResourceDescription, ResourceUrl, ResourceLicense, 
                ResourceSources.GetListHashCode(), ResourceAuthors.GetListHashCode());
        }

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
                   && ResourceSources.ListEquals(other.ResourceSources)
                   && ResourceAuthors.ListEquals(other.ResourceAuthors);
        }
    }
}