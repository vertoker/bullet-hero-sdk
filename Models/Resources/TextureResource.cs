using System.Collections.Generic;
using BHSDK.Models.Enum.Resources;
using BHSDK.Models.Interfaces;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.Resources
{
    [RuleContainer]
    public class TextureResource : Resource, ICopyable<TextureResource>
    {
        // id for user-defined resources, allowed only negative (started with -1, 0 is uninitialized)
        // more about resourceId and how it works, read in Resource.cs file
        
        [RuleMax(0)]
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

        public object Clone() => Copy();
        public TextureResource Copy() => new(TextureResourceId, Sources.CopyList());
    }
}