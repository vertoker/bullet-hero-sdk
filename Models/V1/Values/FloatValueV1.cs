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

        public float GetRandom() => Value;

        public static FloatValueV1 Zero => new(0f);
        public static FloatValueV1 One = new(1f);
    }
}