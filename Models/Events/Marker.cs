using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Events
{
    [RuleContainer]
    public class Marker : IFrame, ICopyable<Marker>
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
        
        [RuleNotNull(typeof(ColorValue))]
        [JsonProperty(Names.Color)]
        public IColor Color { get; set; }

        public Marker()
        {
            Frame = 0;
            
            Name = string.Empty;
            Description = string.Empty;
            Color = new ColorValue();
        }
        public Marker(string name, string description, IColor color, int frame)
        {
            Frame = frame;
            
            Name = name;
            Description = description;
            Color = color;
        }

        public object Clone() => Copy();
        public Marker Copy() => new(Name, Description, Color.Copy(), Frame);
    }
}