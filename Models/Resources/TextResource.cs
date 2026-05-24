using System;
using System.Collections.Generic;
using BH.SDK.Models.Enum.Resources;
using BH.SDK.Models.Interfaces;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Resources
{
    [RuleContainer]
    public class TextResource : Resource, ICopyable<TextResource>, IEquatable<TextResource>
    {
        [RuleMax(IdRules.MaxUserTypedResourceId)]
        [JsonProperty(Names.TextResourceId)]
        public int TextResourceId { get; set; }
        
        public override ResourceType Type => ResourceType.Text;

        public TextResource()
        {
            TextResourceId = IdRules.UninitializedUserTypedResourceId;
        }
        public TextResource(int textResourceId, List<ResourceKey> sources) : base(sources)
        {
            TextResourceId = textResourceId;
        }

        public object Clone() => Copy();
        public TextResource Copy() => new(TextResourceId, Sources.CopyList());

        public override bool Equals(object obj) => obj is TextResource value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), TextResourceId);

        public bool Equals(TextResource other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && TextResourceId.Equals(other.TextResourceId);
            return result;
        }
    }
}