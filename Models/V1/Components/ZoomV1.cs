using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Components;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.V1.Values;

namespace BHSDK.Models.V1.Components
{
    public class ZoomV1 : IZoom
    {
        public int Frame { get; set; }
        public IFloat Size { get; set; }
        public EaseType Ease { get; set; }

        public ZoomV1()
        {
            Frame = 0;
            Size = new FloatValueV1();
            Ease = EaseType.Linear;
        }
        public ZoomV1(int frame, IFloat size, EaseType ease)
        {
            Frame = frame;
            Size = size;
            Ease = ease;
        }
    }
}