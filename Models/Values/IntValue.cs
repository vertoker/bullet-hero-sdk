using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Values
{
    [RuleContainer]
    public class IntValue : IInt, ICopyable<IntValue>
    {
        [JsonProperty(Names.ValueShort)]
        public int Value { get; set; }

        public IntValue()
        {
            Value = 0;
        }
        public IntValue(int value)
        {
            Value = value;
        }

        public IntType GetModelType() => IntType.Value;
        
        IInt ICopyable<IInt>.Copy() => new IntValue(Value);
        public IntValue Copy() => new(Value);
    }
}