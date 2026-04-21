using System.Collections.Generic;
using BHSDK.Models.Enum.Resources;
using Newtonsoft.Json;

namespace BHSDK.Models.Resources
{
    public class FontResource : Resource
    {
        // id for user-defined resources, allowed only negative (started with -1, 0 is uninitialized)
        // more about resourceId and how it works, read in Resource.cs file
        
        [JsonProperty(ModelNames.Font + ModelNames.Resource + ModelNames.Id)]
        public int FontResourceId { get; set; }
        
        public override ResourceType Type => ResourceType.Font;

        public FontResource()
        {
            FontResourceId = 0;
        }
        public FontResource(int fontResourceId, List<ResourceKey> sources) : base(sources)
        {
            FontResourceId = fontResourceId;
        }
    }
}