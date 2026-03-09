using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using UnityEngine;

namespace BHSDK.Models.V1.Values
{
    public class VectorRectStepV1 : IVectorRectStep
    {
        public float MinX { get; set; }
        public float MinY { get; set; }
        public float MaxX { get; set; }
        public float MaxY { get; set; }
        public float Step { get; set; }

        public VectorRectStepV1()
        {
            MinX = 0f;
            MinY = 0f;
            MaxX = 1f;
            MaxY = 1f;
            Step = 1f;
        }
        public VectorRectStepV1(float minX, float minY, float maxX, float maxY, float step)
        {
            MinX = minX;
            MinY = minY;
            MaxX = maxX;
            MaxY = maxY;
            Step = step;
        }
        public VectorRectStepV1(IFloat minX, IFloat minY, IFloat maxX, IFloat maxY, IFloat step)
        {
            MinX = minX.Get();
            MinY = minY.Get();
            MaxX = maxX.Get();
            MaxY = maxY.Get();
            Step = step.Get();
        }

        public VectorType Type => VectorType.RandomRectStep;
        public Vector2 Get()
        {
            var x = Random.Range(MinX, MaxX);
            var y = Random.Range(MinY, MaxY);
            return new Vector2(x, y);
        }
    }
}