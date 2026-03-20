using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class VectorRect : IVector
    {
        [JsonProperty("mnx")]
        public float MinX { get; set; }
        
        [JsonProperty("mny")]
        public float MinY { get; set; }
        
        [JsonProperty("mxx")]
        public float MaxX { get; set; }
        
        [JsonProperty("mxy")]
        public float MaxY { get; set; }

        public VectorRect()
        {
            MinX = 0f;
            MinY = 0f;
            MaxX = 1f;
            MaxY = 1f;
        }
        public VectorRect(float minX, float minY, float maxX, float maxY)
        {
            MinX = minX;
            MinY = minY;
            MaxX = maxX;
            MaxY = maxY;
        }
        public VectorRect(IFloat minX, IFloat minY, IFloat maxX, IFloat maxY)
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