using System;
using System.Collections.Generic;
using BH.SDK.Models.Enum.Resources;
using BH.SDK.Models.Interfaces;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Resources
{
    [RuleContainer]
    public class TextureResource : Resource, ICopyable<TextureResource>, IEquatable<TextureResource>
    {
        // id for user-defined resources, allowed only negative (started with -1, 0 is uninitialized)
        // more about resourceId and how it works, read in Resource.cs file
        
        [RuleMax(0)]
        [JsonProperty(Names.TextureResourceId)]
        public int TextureResourceId { get; set; }
        
        public override ResourceType Type => ResourceType.Texture;

        public TextureResource()
        {
            TextureResourceId = UninitializedId;
        }
        public TextureResource(int textureResourceId, List<ResourceKey> sources) : base(sources)
        {
            TextureResourceId = textureResourceId;
        }

        public object Clone() => Copy();
        public TextureResource Copy() => new(TextureResourceId, Sources.CopyList());

        public override bool Equals(object obj) => obj is TextureResource value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), TextureResourceId);

        public bool Equals(TextureResource other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && TextureResourceId.Equals(other.TextureResourceId);
            return result;
        }
    }
}