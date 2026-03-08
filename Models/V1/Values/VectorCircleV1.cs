using System;
using BHSDK.Models.Interfaces.Values;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BHSDK.Models.V1.Values
{
    public class VectorCircleV1 : IVector
    {
        public IFloatValue X { get; set; }
        public IFloatValue Y { get; set; }
        public float Radius { get; set; }
        
        public VectorCircleV1()
        {
            X = new FloatValueV1();
            Y = new FloatValueV1();
            Radius = 1f;
        }
        public VectorCircleV1(float x, float y, float radius)
        {
            X = new FloatValueV1(x);
            Y = new FloatValueV1(y);
            Radius = radius;
        }
        public VectorCircleV1(IFloatValue x, IFloatValue y, float radius)
        {
            X = x;
            Y = y;
            Radius = radius;
        }

        public Vector2 GetRandom()
        {
            // https://www.youtube.com/watch?v=4y_nmpv-9lI
            // Use distribution through Sqrt, because it uses one number.
            // Not optimal, but random predicted
            var distance = Mathf.Sqrt(Random.value);
            var angle = Random.Range(0f, 2f * Mathf.PI);

            var x = distance * Mathf.Cos(angle) * Radius + X.GetRandom();
            var y = distance * Mathf.Sin(angle) * Radius + Y.GetRandom();
            return new Vector2(x, y);
        }
    }
}