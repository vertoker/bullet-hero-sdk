using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using UnityEngine;

namespace BHSDK.Models.V1.Values
{
    public class VectorValueV1 : IVectorValue
    {
        public float X { get; set; }
        public float Y { get; set; }

        public VectorValueV1()
        {
            X = 0f;
            Y = 0f;
        }
        public VectorValueV1(float x, float y)
        {
            X = x;
            Y = y;
        }
        public VectorValueV1(IFloat x, IFloat y)
        {
            X = x.Get();
            Y = y.Get();
        }

        public VectorType Type => VectorType.Value;
        public Vector2 Get() => new(X, Y);
    }
}