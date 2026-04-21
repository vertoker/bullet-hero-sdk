using System.Collections.Generic;
using Newtonsoft.Json;

namespace BHSDK.Models.Resources
{
    public class LevelResources
    {
        [JsonProperty(ModelNames.Texture)]
        public List<TextureResource> Textures { get; set; }
        
        [JsonProperty(ModelNames.Font)]
        public List<FontResource> Fonts { get; set; }
        
        [JsonProperty(ModelNames.Audio)]
        public List<AudioResource> Audios { get; set; }

        public LevelResources()
        {
            Textures = new List<TextureResource>();
            Fonts = new List<FontResource>();
            Audios = new List<AudioResource>();
        }
        public LevelResources(List<TextureResource> textures, List<FontResource> fonts, List<AudioResource> audios)
        {
            Textures = textures;
            Fonts = fonts;
            Audios = audios;
        }
    }
}