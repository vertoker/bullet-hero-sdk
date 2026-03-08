using BHSDK.Models.Interfaces.Values;

namespace BHSDK.Models.V1.Values
{
    public class ColorV1 : IColor
    {
        public IFloatValue R { get; set; }
        public IFloatValue G { get; set; }
        public IFloatValue B { get; set; }
        public IFloatValue A { get; set; }

        public ColorV1()
        {
            R = new FloatValueV1();
            G = new FloatValueV1();
            B = new FloatValueV1();
            A = new FloatValueV1();
        }
        public ColorV1(float r, float g, float b, float a)
        {
            R = new FloatValueV1(r);
            G = new FloatValueV1(g);
            B = new FloatValueV1(b);
            A = new FloatValueV1(a);
        }
        public ColorV1(IFloatValue r, IFloatValue g, IFloatValue b, IFloatValue a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }
        
        public UnityEngine.Color GetRandom() => new(R.GetRandom(), G.GetRandom(), B.GetRandom(), A.GetRandom());
    }
}