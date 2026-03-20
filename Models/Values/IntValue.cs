using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Values
{
    public class IntValue : IInt
    {
        [JsonProperty("v")]
        public int Value { get; set; }

        public IntValue()
        {
            Value = 0;
        }
        public IntValue(int value)
        {
            Value = value;
        }

        public IntType Type => IntType.Value;
        public int Get() => Value;
    }
}