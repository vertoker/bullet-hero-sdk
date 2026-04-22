using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Values
{
    public class FloatValue : IFloat
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
        public float Get() => Value;
    }
}