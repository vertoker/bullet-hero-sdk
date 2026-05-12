using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Events
{
    [RuleContainer]
    public class Checkpoint : IFrame
    {
        [RuleLevelFrame]
        [JsonProperty(Names.FrameShort)]
        public int Frame { get; set; }
        
        [RuleNotNull, RuleStringMax(ValueRules.MaxEditorName)]
        [JsonProperty(Names.Name)]
        public string Name { get; set; }
        
        [JsonProperty(Names.ActiveShort)]
        public bool Active { get; set; }
        
        [RuleNotNull(typeof(ColorValue))]
        [JsonProperty(Names.Color)]
        public IColor Color { get; set; }

        public Checkpoint()
        {
            Frame = 0;
            
            Name = string.Empty;
            Active = true;
            Color = ColorValue.white;
        }
        public Checkpoint(string name, bool active, IColor color, int frame)
        {
            Frame = frame;
            
            Name = name;
            Active = active;
            Color = color;
        }
    }
}