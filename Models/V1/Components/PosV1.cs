using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Components;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.V1.Values;

namespace BHSDK.Models.V1.Components
{
    public class PosV1 : IPos
    {
        public int Frame { get; set; }
        public IVector Vector { get; set; }
        public Anchor Anchor { get; set; }
        public EasingType Easing { get; set; }

        public PosV1()
        {
            Frame = 0;
            Vector = new VectorV1();
            Anchor = Anchor.Center_Middle;
            Easing = EasingType.Linear;
        }
        public PosV1(int frame, IVector vector, Anchor anchor, EasingType easing)
        {
            Frame = frame;
            Vector = vector;
            Anchor = anchor;
            Easing = easing;
        }
    }
}