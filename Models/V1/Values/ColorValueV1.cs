using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;

namespace BHSDK.Models.V1.Values
{
    public class ColorValueV1 : IColorValue
    {
        public float R { get; set; }
        public float G { get; set; }
        public float B { get; set; }
        public float A { get; set; }

        public ColorValueV1()
        {
            R = 1f;
            G = 1f;
            B = 1f;
            A = 1f;
        }
        public ColorValueV1(float r, float g, float b, float a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }
        public ColorValueV1(IFloat r, IFloat g, IFloat b, IFloat a)
        {
            R = r.Get();
            G = g.Get();
            B = b.Get();
            A = a.Get();
        }

        public ColorType Type => ColorType.Value;
        public UnityEngine.Color Get() => new(R, G, B, A);
    }
}