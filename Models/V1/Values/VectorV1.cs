using BHSDK.Models.Interfaces.Values;
using UnityEngine;

namespace BHSDK.Models.V1.Values
{
    public class VectorV1 : IVector
    {
        public IFloatValue X { get; set; }
        public IFloatValue Y { get; set; }

        public VectorV1()
        {
            X = new FloatValueV1();
            Y = new FloatValueV1();
        }
        public VectorV1(float x, float y)
        {
            X = new FloatValueV1(x);
            Y = new FloatValueV1(y);
        }
        public VectorV1(IFloatValue x, IFloatValue y)
        {
            X = x;
            Y = y;
        }
        
        public Vector2 GetRandom() => new(X.GetRandom(), Y.GetRandom());
    }
}