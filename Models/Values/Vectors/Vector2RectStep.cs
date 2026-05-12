using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Values
{
    [RuleContainer]
    public class Vector2RectStep : IVector2, ICopyable<Vector2RectStep>
    {
        [JsonProperty(Names.MinX)]
        public float MinX { get; set; }
        
        [JsonProperty(Names.MinY)]
        public float MinY { get; set; }
        
        [JsonProperty(Names.MaxX)]
        public float MaxX { get; set; }
        
        [JsonProperty(Names.MaxY)]
        public float MaxY { get; set; }
        
        [RuleMin(0f)]
        [JsonProperty(Names.Step)]
        public float Step { get; set; }

        public Vector2RectStep()
        {
            MinX = 0f;
            MinY = 0f;
            
            MaxX = 1f;
            MaxY = 1f;
            
            Step = 1f;
        }
        public Vector2RectStep(float minX, float minY, float maxX, float maxY, float step)
        {
            MinX = minX;
            MinY = minY;
            
            MaxX = maxX;
            MaxY = maxY;
            
            Step = step;
        }

        public VectorType GetModelType() => VectorType.RandomRectStep;

        public object Clone() => Copy();
        IVector2 ICopyable<IVector2>.Copy() => new Vector2RectStep(MinX, MinY, MaxX, MaxY, Step);
        public Vector2RectStep Copy() => new(MinX, MinY, MaxX, MaxY, Step);
    }
}