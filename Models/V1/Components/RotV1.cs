using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Components;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.V1.Values;

namespace BHSDK.Models.V1.Components
{
    public class RotV1 : IRot
    {
        public int Frame { get; set; }
        public IFloat Angle { get; set; }
        public EaseType Ease { get; set; }

        public RotV1()
        {
            Frame = 0;
            Angle = new FloatValueV1();
            Ease = EaseType.Linear;
        }
        public RotV1(int frame, IFloat angle, EaseType ease)
        {
            Frame = frame;
            Angle = angle;
            Ease = ease;
        }
    }
}