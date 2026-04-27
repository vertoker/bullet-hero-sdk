using System.Collections.Generic;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Resources
{
    public class LevelResources
    {
        [JsonProperty(Names.Textures)]
        public List<TextureResource> Textures { get; set; }
        
        [JsonProperty(Names.Fonts)]
        public List<FontResource> Fonts { get; set; }
        
        [JsonProperty(Names.Audios)]
        public List<AudioResource> Audios { get; set; }
        
        [JsonProperty(Names.Shapes)]
        public List<CompositeColliderShape> CompositeShapes { get; set; }

        public LevelResources()
        {
            Textures = new List<TextureResource>();
            Fonts = new List<FontResource>();
            Audios = new List<AudioResource>();
            CompositeShapes = new List<CompositeColliderShape>();
        }
        public LevelResources(List<TextureResource> textures, List<FontResource> fonts,
            List<AudioResource> audios, List<CompositeColliderShape> compositeShapes)
        {
            Textures = textures;
            Fonts = fonts;
            Audios = audios;
            CompositeShapes = compositeShapes;
        }
    }
}