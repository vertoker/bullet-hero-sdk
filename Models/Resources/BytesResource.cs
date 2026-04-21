using System.Collections.Generic;
using BHSDK.Models.Enum.Resources;
using Newtonsoft.Json;

namespace BHSDK.Models.Resources
{
    public class BytesResource : Resource
    {
        // id for user-defined resources, allowed only negative (started with -1, 0 is uninitialized)
        // more about resourceId and how it works, read in Resource.cs file
        
        [JsonProperty(ModelNames.Byte + ModelNames.Resource + ModelNames.Id)]
        public int ByteResourceId { get; set; }
        
        public override ResourceType Type => ResourceType.Bytes;

        public BytesResource()
        {
            ByteResourceId = 0;
        }
        public BytesResource(int byteResourceId, List<ResourceKey> sources) : base(sources)
        {
            ByteResourceId = byteResourceId;
        }
    }
}