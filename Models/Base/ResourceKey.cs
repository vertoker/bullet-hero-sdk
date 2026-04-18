using System;
using BHSDK.Models.Enum;
using BHSDK.Models.Enum.Resources;
using BHSDK.Models.Resources;
using Newtonsoft.Json;

namespace BHSDK.Models.Base
{
    public class ResourceKey
    {
        // URI - Universal Resource Identifier, either for paths, urls or keys
        
        [JsonProperty(ModelNames.Uri + ModelNames.Type)]
        public ResourceUriType UriType { get; set; }
        
        [JsonProperty(ModelNames.Uri)]
        public string Uri { get; set; }
        
        public virtual ResourceType Type => ResourceType.Bytes;
        
        public ResourceKey()
        {
            UriType = ResourceUriType.Undefined;
            Uri = string.Empty;
        }
        public ResourceKey(ResourceUriType uriType, string uri)
        {
            UriType = uriType;
            Uri = uri;
        }
        
        public static ResourceKey Construct(ResourceType type, ResourceUriType uriType, string uri)
        {
            return type switch
            {
                ResourceType.Bytes => new ResourceKey { UriType = uriType, Uri = uri },
                ResourceType.Text => new TextResourceKey { UriType = uriType, Uri = uri },
                ResourceType.Texture => new TextureResourceKey { UriType = uriType, Uri = uri },
                ResourceType.Audio => new AudioResourceKey { UriType = uriType, Uri = uri },
                ResourceType.Font => new FontResourceKey { UriType = uriType, Uri = uri },
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}