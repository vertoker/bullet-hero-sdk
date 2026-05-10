using System.Collections.Generic;
using BHSDK.Models.Values;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Resources
{
    [RuleContainer]
    public class LevelResources
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
    }
}