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
        public EaseType Ease { get; set; }

        public ClrV1()
        {
            Frame = 0;
            Color = new ColorValueV1(1f, 1f, 1f, 1f);
            Ease = EaseType.Linear;
        }
        public ClrV1(int frame, IColor color, EaseType ease)
        {
            Frame = frame;
            Color = color;
            Ease = ease;
        }
    }
}