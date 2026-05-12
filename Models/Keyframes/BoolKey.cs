using BHSDK.Models.Interfaces;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Keyframes
{
    [RuleContainer]
    public class BoolKey : IFrame, ICopyable<BoolKey>
    {
        [RuleLevelFrame]
        [JsonProperty(Names.FrameShort)]
        public int Frame { get; set; }
        
        [JsonProperty(Names.Bool)]
        public bool Value { get; set; }

        public BoolKey()
        {
            Frame = 0;
            
            Value = false;
        }
        public BoolKey(bool value, int frame)
        {
            Frame = frame;
            
            Value = value;
        }

        public object Clone() => Copy();
        public BoolKey Copy() => new(Value, Frame);
    }
}