using System;
using System.Collections.Generic;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Values;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Resources
{
    [RuleContainer]
    public class LevelResources : ICopyable<LevelResources>, IEquatable<LevelResources>
    {
        [RuleNotNull, RuleCollectionUnique(nameof(TextureResource.TextureResourceId))]
        [JsonProperty(Names.Textures)]
        public List<TextureResource> Textures { get; set; }
        
        [RuleNotNull, RuleCollectionUnique(nameof(FontResource.FontResourceId))]
        [JsonProperty(Names.Fonts)]
        public List<FontResource> Fonts { get; set; }
        
        [RuleNotNull, RuleCollectionUnique(nameof(AudioResource.AudioResourceId))]
        [JsonProperty(Names.Audios)]
        public List<AudioResource> Audios { get; set; }
        
        [RuleNotNull, RuleCollectionUnique(nameof(CompositeCollider.ColliderId))]
        [JsonProperty(Names.Shapes)]
        public List<CompositeCollider> CompositeShapes { get; set; }

        public LevelResources()
        {
            Textures = new List<TextureResource>();
            Fonts = new List<FontResource>();
            Audios = new List<AudioResource>();
            CompositeShapes = new List<CompositeCollider>();
        }
        public LevelResources(List<TextureResource> textures, List<FontResource> fonts,
            List<AudioResource> audios, List<CompositeCollider> compositeShapes)
        {
            Textures = textures;
            Fonts = fonts;
            Audios = audios;
            CompositeShapes = compositeShapes;
        }

        public object Clone() => Copy();
        public LevelResources Copy() => new(Textures.CopyList(), Fonts.CopyList(), Audios.CopyList(), CompositeShapes.CopyList());

        public override bool Equals(object obj) => obj is LevelResources value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Textures.GetListHashCode(),
            Fonts.GetListHashCode(), Audios.GetListHashCode(), CompositeShapes.GetListHashCode());

        public bool Equals(LevelResources other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Textures.ListEquals(other.Textures)
                          && Fonts.ListEquals(other.Fonts)
                          && Audios.ListEquals(other.Audios)
                          && CompositeShapes.ListEquals(other.CompositeShapes);
            return result;
        }
    }
}