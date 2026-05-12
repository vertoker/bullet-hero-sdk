using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Values
{
    [RuleContainer]
    public class FloatValue : IFloat, ICopyable<FloatValue>
    {
        [JsonProperty(Names.ValueShort)]
        public float Value { get; set; }

        public FloatValue()
        {
            Value = 0f;
        }
        public FloatValue(float value)
        {
            Value = value;
        }

        public FloatType GetModelType() => FloatType.Value;
        
        public object Clone() => Copy();
        IFloat ICopyable<IFloat>.Copy() => new FloatValue(Value);
        public FloatValue Copy() => new(Value);
    }
}