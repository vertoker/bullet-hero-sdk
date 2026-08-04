using System;
using System.Collections.Generic;
using BH.SDK.Models.Enum.Meta;
using BH.SDK.Models.Enum.Resources;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Primitives.Resources;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using Newtonsoft.Json;

namespace BH.SDK.Models.Meta
{
    /// <summary>
    /// Legal/attribution record for one user-defined resource of a level - who made it, under what
    /// license, where it came from. Lives in LevelMeta (metadata.json), never inside Level itself:
    /// the level file says how a resource is used, this says whether it may be used at all.
    /// </summary>
    [RuleContainer]
    public class ResourceMeta : IModel<ResourceMeta>
    {
        /// <summary> Which resource family ResourceId belongs to - the only thing that disambiguates
        /// an otherwise untyped id. </summary>
        [JsonProperty(Names.ResourceType)]
        public ResourceType ResourceType { get; set; }

        // Abstract resId, it cast to dedicated ids (audioResId, texResId...)
        /// <summary> The described resource, kept untyped so one list can cover every category.
        /// Capped to the user-defined (negative) range - game resources need no UGC metadata. </summary>
        [RuleIPrimitiveIntMax(TypedResourceId.MaxUserDefinedValue)]
        [JsonProperty(Names.ResourceId)]
        public TypedResourceId ResourceId { get; set; }

        /// <summary> Display name of the original work, localizable (IString). </summary>
        [RuleNotNull(typeof(StringValue)), RuleIStringMax(ValueRules.MaxEditorName)]
        [JsonProperty(Names.Title)]
        public IString ResourceTitle { get; set; }

        /// <summary> Free-form description of the work, localizable (IString). </summary>
        [RuleNotNull(typeof(StringValue)), RuleIStringMax(ValueRules.MaxEditorDescription)]
        [JsonProperty(Names.Description)]
        public IString ResourceDescription { get; set; }

        /// <summary> Canonical page of the work (the artist's post/store page), for attribution.
        /// Not a download link - those live in Resource.Sources on the level side. </summary>
        [RuleNotNull, RuleStringMax(ValueRules.MaxUrl)]
        [JsonProperty(Names.Url)]
        public string ResourceUrl { get; set; }

        /// <summary> Terms the work is distributed under. Defaults to a typical CC license rather
        /// than "unspecified", so an unfilled record still states something. </summary>
        [RuleNotNull(typeof(NoSpecifiedLicense))]
        [JsonProperty(Names.License)]
        public ILicense ResourceLicense { get; set; }

        /// <summary> Human-readable provenance strings (where it was taken from), localizable. Free
        /// text for a reader, unlike the machine-usable URIs in Resource.Sources. </summary>
        [RuleNotNull, RuleCollectionMaxCount(ResourceRules.MaxSources)]
        [JsonProperty(Names.Sources)]
        public List<IString> ResourceSources { get; set; }

        /// <summary> Everyone who must be credited for this single resource - separate from the
        /// level's own LevelMeta.Authors. </summary>
        [RuleNotNull, RuleCollectionMaxCount(ResourceRules.MaxAuthors)]
        [JsonProperty(Names.Authors)]
        public List<Author> ResourceAuthors { get; set; }
        
        // TODO add method for author permission to use resource (for whole BH or several levels / unlimited or time limit)

        public ResourceMeta()
        {
            ResourceType = ResourceType.Bytes;
            ResourceId = TypedResourceId.Null;
            ResourceTitle = new StringValue();
            ResourceDescription = new StringValue();
            ResourceUrl = string.Empty;
            ResourceLicense = new TypicalLicense(TypicalLicenseType.CC_BY_NC_4_0);
            ResourceSources = new List<IString>();
            ResourceAuthors = new List<Author>();
        }
        public ResourceMeta(ResourceType resourceType, TypedResourceId resourceId, IString resourceTitle,
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
        public void Reset()
        {
            ResourceType = ResourceType.Bytes;
            ResourceId = TypedResourceId.Null;
            ResourceTitle = new StringValue();
            ResourceDescription = new StringValue();
            ResourceUrl = string.Empty;
            ResourceLicense = new TypicalLicense(TypicalLicenseType.CC_BY_NC_4_0);
            ResourceSources.Clear();
            ResourceAuthors.Clear();
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