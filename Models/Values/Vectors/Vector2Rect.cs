using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Values.Vectors
{
    public class Vector2Rect : IVector2, ICopyable<Vector2Rect>
    {
        // TODO add check for Min and Max, must be always Min < Max
        [JsonProperty(Names.MinX)]
        public float MinX { get; set; }
        
        [JsonProperty(Names.MinY)]
        public float MinY { get; set; }
        
        
        [JsonProperty(Names.MaxX)]
        public float MaxX { get; set; }
        
        [JsonProperty(Names.MaxY)]
        public float MaxY { get; set; }

        public Vector2Rect()
        {
            MinX = 0f;
            MinY = 0f;
            
            MaxX = 1f;
            MaxY = 1f;
        }
        public Vector2Rect(float minX, float minY, float maxX, float maxY)
        {
            MinX = minX;
            MinY = minY;
            
            MaxX = maxX;
            MaxY = maxY;
        }

        public VectorType GetModelType() => VectorType.RandomRect;

        IVector2 ICopyable<IVector2>.Copy() => new Vector2Rect(MinX, MinY, MaxX, MaxY);
        public Vector2Rect Copy() => new(MinX, MinY, MaxX, MaxY);
    }
}