using System;
using System.Collections.Generic;
using BHSDK.Models.Enum.Resources;
using BHSDK.Models.Interfaces;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BHSDK.Models.Resources
{
    [RuleContainer]
    public class FontResource : Resource, ICopyable<FontResource>, IEquatable<FontResource>
    {
        // id for user-defined resources, allowed only negative (started with -1, 0 is uninitialized)
        // more about resourceId and how it works, read in Resource.cs file
        
        [RuleMax(0)]
        [JsonProperty(Names.FontResourceId)]
        public int FontResourceId { get; set; }
        
        public override ResourceType Type => ResourceType.Font;

        public FontResource()
        {
            FontResourceId = 0;
        }
        public FontResource(int fontResourceId, List<ResourceKey> sources) : base(sources)
        {
            FontResourceId = fontResourceId;
        }

        public object Clone() => Copy();
        public FontResource Copy() => new(FontResourceId, Sources.CopyList());

        public override bool Equals(object obj) => obj is FontResource value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), FontResourceId);

        public bool Equals(FontResource other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && FontResourceId.Equals(other.FontResourceId);
            return result;
        }
    }
}