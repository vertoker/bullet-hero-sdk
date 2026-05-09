using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Values.Vectors
{
    public class Vector4RectStep : IVector4, ICopyable<Vector4RectStep>
    {
        [JsonProperty(Names.MinX)]
        public float MinX { get; set; }
        
        [JsonProperty(Names.MinY)]
        public float MinY { get; set; }
        
        [JsonProperty(Names.MinZ)]
        public float MinZ { get; set; }
        
        [JsonProperty(Names.MinW)]
        public float MinW { get; set; }
        
        
        [JsonProperty(Names.MaxX)]
        public float MaxX { get; set; }
        
        [JsonProperty(Names.MaxY)]
        public float MaxY { get; set; }
        
        [JsonProperty(Names.MaxZ)]
        public float MaxZ { get; set; }
        
        [JsonProperty(Names.MaxW)]
        public float MaxW { get; set; }
        
        
        [JsonProperty(Names.Step)]
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

        public VectorType GetModelType() => VectorType.RandomRectStep;

        IVector4 ICopyable<IVector4>.Copy() => new Vector4RectStep(MinX, MinY, MinZ, MinW, MaxX, MaxY, MaxZ, MaxW, Step);
        public Vector4RectStep Copy() => new(MinX, MinY, MinZ, MinW, MaxX, MaxY, MaxZ, MaxW, Step);
    }
}