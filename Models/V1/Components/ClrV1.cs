using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Components;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.V1.Values;

namespace BHSDK.Models.V1.Components
{
    public class ClrV1 : IClr
    {
        public int Frame { get; set; }
        public IColor Color { get; set; }
        public EasingType Easing { get; set; }

        public ClrV1()
        {
            Frame = 0;
            Color = new ColorV1(1f, 1f, 1f, 1f);
            Easing = EasingType.Linear;
        }
        public ClrV1(int frame, IColor color, EasingType easing)
        {
            Frame = frame;
            Color = color;
            Easing = easing;
        }
    }
}