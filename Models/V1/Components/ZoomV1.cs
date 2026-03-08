using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Components;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.V1.Values;

namespace BHSDK.Models.V1.Components
{
    public class ZoomV1 : IZoom
    {
        public int Frame { get; set; }
        public IFloatValue Size { get; set; }
        public EasingType Easing { get; set; }

        public ZoomV1()
        {
            Frame = 0;
            Size = new FloatValueV1();
            Easing = EasingType.Linear;
        }
        public ZoomV1(int frame, IFloatValue size, EasingType easing)
        {
            Frame = frame;
            Size = size;
            Easing = easing;
        }
    }
}