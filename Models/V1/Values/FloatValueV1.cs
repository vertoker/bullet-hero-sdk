using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;

namespace BHSDK.Models.V1.Values
{
    public class FloatValueV1 : IFloatValue
    {
        public float Value { get; set; }

        public FloatValueV1()
        {
            Value = 0f;
        }
        public FloatValueV1(float value)
        {
            Value = value;
        }

        public FloatType Type => FloatType.Value;
        public float Get() => Value;
    }
}