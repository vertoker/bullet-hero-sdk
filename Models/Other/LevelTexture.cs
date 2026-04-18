using System.Collections.Generic;
using BHSDK.Models.Resources;
using Newtonsoft.Json;

namespace BHSDK.Models.Other
{
    public class LevelTexture
    {
        public const int MaxSourcesCount = 4;
        
        [JsonProperty(ModelNames.Source)]
        public List<TextureResourceKey> Sources { get; set; }

        public LevelTexture()
        {
            Sources = new List<TextureResourceKey>();
        }
        public LevelTexture(List<TextureResourceKey> sources)
        {
            Sources = sources;
        }
    }
}