using System.Collections.Generic;
using BHSDK.Models.Enum.Resources;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Resources
{
    [RuleContainer]
    public class AudioResource : Resource
    {
        // id for user-defined resources, allowed only negative (started with -1, 0 is uninitialized)
        // more about resourceId and how it works, read in Resource.cs file
        
        [RuleMax(0)]
        [JsonProperty(Names.AudioResourceId)]
        public int AudioResourceId { get; set; }
        
        public override ResourceType Type => ResourceType.Audio;

        public AudioResource()
        {
            AudioResourceId = 0;
        }
        public AudioResource(int audioResourceId, List<ResourceKey> sources) : base(sources)
        {
            AudioResourceId = audioResourceId;
        }
    }
}