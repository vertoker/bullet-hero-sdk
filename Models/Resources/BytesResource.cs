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
    public class BytesResource : Resource, ICopyable<BytesResource>, IEquatable<BytesResource>
    {
        [RuleIPrimitiveIntMax(BytesResourceId.MaxUserDefinedValue)]
        [JsonProperty(Names.ByteResourceId)]
        public BytesResourceId ByteResourceId { get; set; }

        public override ResourceType Type => ResourceType.Bytes;

        public BytesResource()
        {
            ByteResourceId = BytesResourceId.Null;
        }
        public BytesResource(BytesResourceId byteResourceId, List<ResourceKey> sources) : base(sources)
        {
            ByteResourceId = byteResourceId;
        }

        public object Clone() => Copy();
        public BytesResource Copy() => new(ByteResourceId, Sources.CopyList());

        public override bool Equals(object obj) => obj is BytesResource value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), ByteResourceId);

        public bool Equals(BytesResource other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other) 
                         && ByteResourceId.Equals(other.ByteResourceId);
            return result;
        }
    }
}