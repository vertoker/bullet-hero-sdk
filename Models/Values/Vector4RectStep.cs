using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class Vector4RectStep : IVector4
    {
        [JsonProperty("mnx")]
        public float MinX { get; set; }
        
        [JsonProperty("mny")]
        public float MinY { get; set; }
        
        [JsonProperty("mnz")]
        public float MinZ { get; set; }
        
        [JsonProperty("mnw")]
        public float MinW { get; set; }
        
        
        [JsonProperty("mxx")]
        public float MaxX { get; set; }
        
        [JsonProperty("mxy")]
        public float MaxY { get; set; }
        
        [JsonProperty("mxz")]
        public float MaxZ { get; set; }
        
        [JsonProperty("mxw")]
        public float MaxW { get; set; }
        
        
        [JsonProperty("s")]
        public float Step { get; set; }

        public Vector4RectStep()
        {
            MinX = 0f;
            MinY = 0f;
            MinZ = 0f;
            MinW = 0f;
            
            MaxX = 1f;
            MaxY = 1f;
            MaxZ = 1f;
            MaxW = 1f;
            
            Step = 1f;
        }
        public Vector4RectStep(float minX, float minY, float minZ, float minW, 
            float maxX, float maxY, float maxZ, float maxW, float step)
        {
            MinX = minX;
            MinY = minY;
            MinZ = minZ;
            MinW = minW;
            
            MaxX = maxX;
            MaxY = maxY;
            MaxZ = maxZ;
            MaxW = maxW;
            
            Step = step;
        }
        public Vector4RectStep(IFloat minX, IFloat minY, IFloat minZ, IFloat minW, 
            IFloat maxX, IFloat maxY, IFloat maxZ, IFloat maxW, IFloat step)
        {
            MinX = minX.Get();
            MinY = minY.Get();
            MinZ = minZ.Get();
            MinW = minW.Get();
            
            MaxX = maxX.Get();
            MaxY = maxY.Get();
            MaxZ = maxZ.Get();
            MaxW = maxW.Get();
            
            Step = step.Get();
        }

        public VectorType GetModelType() => VectorType.RandomRectStep;
        public Vector4 Get()
        {
            var x = Random.Range(MinX, MaxX);
            var y = Random.Range(MinY, MaxY);
            var z = Random.Range(MinZ, MaxZ);
            var w = Random.Range(MinW, MaxW);
            if (Step == 0f) return new Vector4(x, y, z, w);
            
            x = Mathf.RoundToInt(x / Step) * Step;
            y = Mathf.RoundToInt(y / Step) * Step;
            z = Mathf.RoundToInt(z / Step) * Step;
            w = Mathf.RoundToInt(w / Step) * Step;
            return new Vector4(x, y, z, w);
        }
    }
}