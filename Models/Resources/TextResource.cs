using System.Collections.Generic;
using BHSDK.Models.Enum.Resources;
using BHSDK.Models.Interfaces;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.Resources
{
    [RuleContainer]
    public class TextResource : Resource, ICopyable<TextResource>
    {
        // id for user-defined resources, allowed only negative (started with -1, 0 is uninitialized)
        // more about resourceId and how it works, read in Resource.cs file
        
        [RuleMax(0)]
        [JsonProperty(Names.TextResourceId)]
        public int TextResourceId { get; set; }
        
        public override ResourceType Type => ResourceType.Text;

        public TextResource()
        {
            TextResourceId = 0;
        }
        public TextResource(int textResourceId, List<ResourceKey> sources) : base(sources)
        {
            TextResourceId = textResourceId;
        }

        public object Clone() => Copy();
        public TextResource Copy() => new(TextResourceId, Sources.CopyList());
    }
}