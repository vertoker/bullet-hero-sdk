using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class Vector3Rect : IVector3
    {
        [JsonProperty("mnx")]
        public float MinX { get; set; }
        
        [JsonProperty("mny")]
        public float MinY { get; set; }
        
        [JsonProperty("mnz")]
        public float MinZ { get; set; }
        
        
        [JsonProperty("mxx")]
        public float MaxX { get; set; }
        
        [JsonProperty("mxy")]
        public float MaxY { get; set; }
        
        [JsonProperty("mxz")]
        public float MaxZ { get; set; }

        public Vector3Rect()
        {
            MinX = 0f;
            MinY = 0f;
            MinZ = 0f;
            
            MaxX = 1f;
            MaxY = 1f;
            MaxZ = 1f;
        }
        public Vector3Rect(float minX, float minY, float minZ, float maxX, float maxY, float maxZ)
        {
            MinX = minX;
            MinY = minY;
            MinZ = minZ;
            
            MaxX = maxX;
            MaxY = maxY;
            MaxZ = maxZ;
        }
        public Vector3Rect(IFloat minX, IFloat minY, IFloat minZ, IFloat maxX, IFloat maxY, IFloat maxZ)
        {
            MinX = minX.Get();
            MinY = minY.Get();
            MinZ = minZ.Get();
            
            MaxX = maxX.Get();
            MaxY = maxY.Get();
            MaxZ = maxZ.Get();
        }

        public VectorType GetModelType() => VectorType.RandomRect;
        public Vector3 Get()
        {
            var x = Random.Range(MinX, MaxX);
            var y = Random.Range(MinY, MaxY);
            var z = Random.Range(MinZ, MaxZ);
            return new Vector3(x, y, z);
        }
    }
}