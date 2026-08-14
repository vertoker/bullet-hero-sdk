using System;
using System.Collections.Generic;
using BH.SDK.Models.Enums.Resources;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Primitives.Resources;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Resources
{
    /// <summary>
    /// An image the level brings with it. The only Resource subtype with extra data of its own -
    /// a sub-rect, so one shipped atlas can back many different sprites.
    /// </summary>
    [RuleContainer]
    public class TextureResource : Resource, IModel<TextureResource>
    {
        /// <summary> Identity of this image within the level. </summary>
        [RuleIPrimitiveIntMax(TextureResourceId.MaxUserDefinedValue)]
        [JsonProperty(Names.TextureResourceId)]
        public TextureResourceId TextureResourceId { get; set; }

        /// <summary> Region of the source image this resource actually is, as tiling+offset. Applies
        /// to the resource itself, unlike UVKey which animates a single object's mapping. </summary>
        [RuleNotNull]
        [JsonProperty(Names.TextureResourceUV)]
        public Vector4Value TextureResourceUV { get; set; }

        public override ResourceType Type => ResourceType.Texture;

        public TextureResource()
        {
            TextureResourceId = TextureResourceId.Null;
            TextureResourceUV = new Vector4Value(ValueRules.DefaultUvX,
                ValueRules.DefaultUvY, ValueRules.DefaultUvZ, ValueRules.DefaultUvW);
        }
        public TextureResource(TextureResourceId textureResourceId, List<ResourceKey> sources) : base(sources)
        {
            TextureResourceId = textureResourceId;
            TextureResourceUV = new Vector4Value(ValueRules.DefaultUvX,
                ValueRules.DefaultUvY, ValueRules.DefaultUvZ, ValueRules.DefaultUvW);
        }
        public TextureResource(TextureResourceId textureResourceId, Vector4Value textureResourceUV, List<ResourceKey> sources) : base(sources)
        {
            TextureResourceId = textureResourceId;
            TextureResourceUV = textureResourceUV;
        }
        public override void Reset()
        {
            base.Reset();
            TextureResourceId = TextureResourceId.Null;
            TextureResourceUV = new Vector4Value(ValueRules.DefaultUvX,
                ValueRules.DefaultUvY, ValueRules.DefaultUvZ, ValueRules.DefaultUvW);
        }
        
        public override object Clone() => CopyImpl();
        public override Resource Copy() => CopyImpl();
        TextureResource ICopyable<TextureResource>.Copy() => CopyImpl();
        
        private TextureResource CopyImpl() => new(TextureResourceId, TextureResourceUV.Copy(), Sources.CopyList());

        public override bool Equals(object obj) => obj is TextureResource value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), TextureResourceId, TextureResourceUV);

        public bool Equals(TextureResource other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && TextureResourceId.Equals(other.TextureResourceId)
                         && TextureResourceUV.Equals(other.TextureResourceUV);
            return result;
        }
    }
}