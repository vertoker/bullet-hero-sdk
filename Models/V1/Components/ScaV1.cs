using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Components;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.V1.Values;

namespace BHSDK.Models.V1.Components
{
    public class ScaV1 : ISca
    {
        public int Frame { get; set; }
        public IVector Vector { get; set; }
        public EaseType Ease { get; set; }

        public ScaV1()
        {
            Frame = 0;
            Vector = new VectorCircleV1();
            Ease = EaseType.Linear;
        }
        public ScaV1(int frame, IVector vector, EaseType ease)
        {
            Frame = frame;
            Vector = vector;
            Ease = ease;
        }
    }
}