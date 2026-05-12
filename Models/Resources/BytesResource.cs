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
    public class BytesResource : Resource, ICopyable<BytesResource>, IEquatable<BytesResource>
    {
        // id for user-defined resources, allowed only negative (started with -1, 0 is uninitialized)
        // more about resourceId and how it works, read in Resource.cs file
        
        [RuleMax(0)]
        [JsonProperty(Names.ByteResourceId)]
        public int ByteResourceId { get; set; }
        
        public override ResourceType Type => ResourceType.Bytes;

        public BytesResource()
        {
            ByteResourceId = 0;
        }
        public BytesResource(int byteResourceId, List<ResourceKey> sources) : base(sources)
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