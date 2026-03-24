using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class Vector3RectStep : IVector3
    {
        [JsonProperty(ModelNames.Min + ModelNames.CoordX)]
        public float MinX { get; set; }
        
        [JsonProperty(ModelNames.Min + ModelNames.CoordY)]
        public float MinY { get; set; }
        
        [JsonProperty(ModelNames.Min + ModelNames.CoordZ)]
        public float MinZ { get; set; }
        
        
        [JsonProperty(ModelNames.Max + ModelNames.CoordX)]
        public float MaxX { get; set; }
        
        [JsonProperty(ModelNames.Max + ModelNames.CoordY)]
        public float MaxY { get; set; }
        
        [JsonProperty(ModelNames.Max + ModelNames.CoordZ)]
        public float MaxZ { get; set; }
        
        
        [JsonProperty(ModelNames.Step)]
        public float Step { get; set; }

        public Vector3RectStep()
        {
            MinX = 0f;
            MinY = 0f;
            MinZ = 0f;
            
            MaxX = 1f;
            MaxY = 1f;
            MaxZ = 1f;
            
            Step = 1f;
        }
        public Vector3RectStep(float minX, float minY, float minZ, float maxX, float maxY, float maxZ, float step)
        {
            MinX = minX;
            MinY = minY;
            MinZ = minZ;
            
            MaxX = maxX;
            MaxY = maxY;
            MaxZ = maxZ;
            
            Step = step;
        }
        public Vector3RectStep(IFloat minX, IFloat minY, IFloat minZ, IFloat maxX, IFloat maxY, IFloat maxZ, IFloat step)
        {
            MinX = minX.Get();
            MinY = minY.Get();
            MinZ = minZ.Get();
            
            MaxX = maxX.Get();
            MaxY = maxY.Get();
            MaxZ = maxZ.Get();
            
            Step = step.Get();
        }

        public VectorType GetModelType() => VectorType.RandomRectStep;
        public Vector3 Get()
        {
            var x = Random.Range(MinX, MaxX);
            var y = Random.Range(MinY, MaxY);
            var z = Random.Range(MinZ, MaxZ);
            if (Step == 0f) return new Vector3(x, y, z);
            
            x = Mathf.RoundToInt(x / Step) * Step;
            y = Mathf.RoundToInt(y / Step) * Step;
            z = Mathf.RoundToInt(z / Step) * Step;
            return new Vector3(x, y, z);
        }
    }
}