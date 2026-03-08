using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Components;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.V1.Values;

namespace BHSDK.Models.V1.Components
{
    public class RotV1 : IRot
    {
        public int Frame { get; set; }
        public IFloatValue Angle { get; set; }
        public EasingType Easing { get; set; }

        public RotV1()
        {
            Frame = 0;
            Angle = new FloatValueV1();
            Easing = EasingType.Linear;
        }
        public RotV1(int frame, IFloatValue angle, EasingType easing)
        {
            Frame = frame;
            Angle = angle;
            Easing = easing;
        }
    }
}