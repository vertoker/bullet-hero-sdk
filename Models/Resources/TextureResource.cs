using System.Collections.Generic;
using BHSDK.Models.Enum.Resources;
using Newtonsoft.Json;

namespace BHSDK.Models.Resources
{
    public class TextureResource : Resource
    {
        // id for user-defined resources, allowed only negative (started with -1, 0 is uninitialized)
        // more about resourceId and how it works, read in Resource.cs file
        
        [JsonProperty(Names.TextureResourceId)]
        public int TextureResourceId { get; set; }
        
        public override ResourceType Type => ResourceType.Texture;

        public TextureResource()
        {
            TextureResourceId = 0;
        }
        public TextureResource(int textureResourceId, List<ResourceKey> sources) : base(sources)
        {
            TextureResourceId = textureResourceId;
        }
    }
}