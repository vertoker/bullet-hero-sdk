using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using UnityEngine;

namespace BHSDK.Models.V1.Values
{
    public class VectorRectV1 : IVectorRect
    {
        public float MinX { get; set; }
        public float MinY { get; set; }
        public float MaxX { get; set; }
        public float MaxY { get; set; }

        public VectorRectV1()
        {
            MinX = 0f;
            MinY = 0f;
            MaxX = 1f;
            MaxY = 1f;
        }
        public VectorRectV1(float minX, float minY, float maxX, float maxY)
        {
            MinX = minX;
            MinY = minY;
            MaxX = maxX;
            MaxY = maxY;
        }
        public VectorRectV1(IFloat minX, IFloat minY, IFloat maxX, IFloat maxY)
        {
            MinX = minX.Get();
            MinY = minY.Get();
            MaxX = maxX.Get();
            MaxY = maxY.Get();
        }

        public VectorType Type => VectorType.RandomRect;
        public Vector2 Get()
        {
            var x = Random.Range(MinX, MaxX);
            var y = Random.Range(MinY, MaxY);
            return new Vector2(x, y);
        }
    }
}