using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class VectorRectStep : IVector
    {
        [JsonProperty("mnx")]
        public float MinX { get; set; }
        
        [JsonProperty("mny")]
        public float MinY { get; set; }
        
        [JsonProperty("mxx")]
        public float MaxX { get; set; }
        
        [JsonProperty("mxy")]
        public float MaxY { get; set; }
        
        [JsonProperty("s")]
        public float Step { get; set; }

        public VectorRectStep()
        {
            MinX = 0f;
            MinY = 0f;
            MaxX = 1f;
            MaxY = 1f;
            Step = 1f;
        }
        public VectorRectStep(float minX, float minY, float maxX, float maxY, float step)
        {
            MinX = minX;
            MinY = minY;
            MaxX = maxX;
            MaxY = maxY;
            Step = step;
        }
        public VectorRectStep(IFloat minX, IFloat minY, IFloat maxX, IFloat maxY, IFloat step)
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