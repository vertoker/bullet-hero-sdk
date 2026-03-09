using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;

namespace BHSDK.Models.V1.Values
{
    public class IntValueV1 : IIntValue
    {
        public int Value { get; set; }

        public IntValueV1()
        {
            Value = 0;
        }
        public IntValueV1(int value)
        {
            Value = value;
        }

        public IntType Type => IntType.Value;
        public int Get() => Value;
    }
}