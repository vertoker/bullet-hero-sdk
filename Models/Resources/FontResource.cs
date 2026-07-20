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
    public class FontResource : Resource, IModel<FontResource>
    {
        [RuleIPrimitiveIntMax(FontResourceId.MaxUserDefinedValue)]
        [JsonProperty(Names.FontResourceId)]
        public FontResourceId FontResourceId { get; set; }

        public override ResourceType Type => ResourceType.Font;

        public FontResource()
        {
            FontResourceId = FontResourceId.Null;
        }
        public FontResource(FontResourceId fontResourceId, List<ResourceKey> sources) : base(sources)
        {
            FontResourceId = fontResourceId;
        }
        public override void Reset()
        {
            base.Reset();
            FontResourceId.Reset();
        }
        
        public override object Clone() => CopyImpl();
        public override Resource Copy() => CopyImpl();
        FontResource ICopyable<FontResource>.Copy() => CopyImpl();
        
        private FontResource CopyImpl() => new(FontResourceId, Sources.CopyList());

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