using System;
using System.Collections.Generic;
using BH.SDK.Models.Enum.Resources;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Primitives.Resources;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Resources
{
    [RuleContainer]
    public class TextResource : Resource, IModel<TextResource>
    {
        [RuleIPrimitiveIntMax(TextResourceId.MaxUserDefinedValue)]
        [JsonProperty(Names.TextResourceId)]
        public TextResourceId TextResourceId { get; set; }

        public override ResourceType Type => ResourceType.Text;

        public TextResource()
        {
            TextResourceId = TextResourceId.Null;
        }
        public TextResource(TextResourceId textResourceId, List<ResourceKey> sources) : base(sources)
        {
            TextResourceId = textResourceId;
        }
        public override void Reset()
        {
            base.Reset();
            TextResourceId.Reset();
        }
        
        public override object Clone() => CopyImpl();
        public override Resource Copy() => CopyImpl();
        TextResource ICopyable<TextResource>.Copy() => CopyImpl();
        
        private TextResource CopyImpl() => new(TextResourceId, Sources.CopyList());

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