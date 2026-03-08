using BHSDK.Models.Interfaces.Values;
using BHSDK.Utility;
using UnityEngine;

namespace BHSDK.Models.V1.Values
{
    public class VectorMinMaxStep : IVector
    {
        public float MinX { get; set; }
        public float MinY { get; set; }
        public float MaxX { get; set; }
        public float MaxY { get; set; }
        public float Step { get; set; }

        public Vector2 GetRandom()
        {
            var x = Random.Range(MinX, MaxX);
            var y = Random.Range(MinY, MaxY);
            x = MathUtility.ClampStep(x, MinX, MaxX, Step);
            y = MathUtility.ClampStep(y, MinY, MaxY, Step);
            return new Vector2(x, y);
        }
    }
}