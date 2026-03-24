using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class Vector4Rect : IVector4
    {
        [JsonProperty(ModelNames.Min + ModelNames.CoordX)]
        public float MinX { get; set; }
        
        [JsonProperty(ModelNames.Min + ModelNames.CoordY)]
        public float MinY { get; set; }
        
        [JsonProperty(ModelNames.Min + ModelNames.CoordZ)]
        public float MinZ { get; set; }
        
        [JsonProperty(ModelNames.Min + ModelNames.CoordW)]
        public float MinW { get; set; }
        
        
        [JsonProperty(ModelNames.Max + ModelNames.CoordX)]
        public float MaxX { get; set; }
        
        [JsonProperty(ModelNames.Max + ModelNames.CoordY)]
        public float MaxY { get; set; }
        
        [JsonProperty(ModelNames.Max + ModelNames.CoordZ)]
        public float MaxZ { get; set; }
        
        [JsonProperty(ModelNames.Max + ModelNames.CoordW)]
        public float MaxW { get; set; }

        public Vector4Rect()
        {
            MinX = 0f;
            MinY = 0f;
            MinZ = 0f;
            MinW = 0f;
            
            MaxX = 1f;
            MaxY = 1f;
            MaxZ = 1f;
            MaxW = 1f;
        }
        public Vector4Rect(float minX, float minY, float minZ, float minW, 
            float maxX, float maxY, float maxZ, float maxW)
        {
            MinX = minX;
            MinY = minY;
            MinZ = minZ;
            MinW = minW;
            
            MaxX = maxX;
            MaxY = maxY;
            MaxZ = maxZ;
            MaxW = maxW;
        }
        public Vector4Rect(IFloat minX, IFloat minY, IFloat minZ, IFloat minW, 
            IFloat maxX, IFloat maxY, IFloat maxZ, IFloat maxW)
        {
            MinX = minX.Get();
            MinY = minY.Get();
            MinZ = minZ.Get();
            MinW = minW.Get();
            
            MaxX = maxX.Get();
            MaxY = maxY.Get();
            MaxZ = maxZ.Get();
            MaxW = maxW.Get();
        }

        public VectorType GetModelType() => VectorType.RandomRect;
        public Vector4 Get()
        {
            var x = Random.Range(MinX, MaxX);
            var y = Random.Range(MinY, MaxY);
            var z = Random.Range(MinZ, MaxZ);
            var w = Random.Range(MinW, MaxW);
            return new Vector4(x, y, z, w);
        }
    }
}