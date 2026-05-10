using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Events
{
    [RuleContainer]
    public class Marker : IFrame
    {
        [RuleLevelFrame]
        [JsonProperty(Names.FrameShort)]
        public int Frame { get; set; }
        
        [RuleNotNull, RuleStringMax(ValueRules.MaxEditorName)]
        [JsonProperty(Names.Name)]
        public string Name { get; set; }
        
        [RuleNotNull, RuleStringMax(ValueRules.MaxEditorDescription)]
        [JsonProperty(Names.Description)]
        public string Description { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Color)]
        public IColor Color { get; set; }

        public Marker()
        {
            Name = string.Empty;
            Description = string.Empty;
            Frame = 0;
            Color = new ColorValue();
        }
        public Marker(string name, string description, int frame, IColor color)
        {
            Name = name;
            Description = description;
            Frame = frame;
            Color = color;
        }
    }
}