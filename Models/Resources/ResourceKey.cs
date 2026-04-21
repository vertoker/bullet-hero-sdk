using System;
using BHSDK.Models.Enum.Resources;
using Newtonsoft.Json;

namespace BHSDK.Models.Resources
{
    public class ResourceKey
    {
        // URI - Universal Resource Identifier, either for paths, urls or keys
        
        [JsonProperty(ModelNames.Uri + ModelNames.Type)]
        public ResourceUriType UriType { get; set; }
        
        [JsonProperty(ModelNames.Uri)]
        public string Uri { get; set; }
        
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
    }
}