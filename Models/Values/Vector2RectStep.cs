using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class Vector2RectStep : IVector2
    {
        [JsonProperty(Names.MinX)]
        public float MinX { get; set; }
        
        [JsonProperty(Names.MinY)]
        public float MinY { get; set; }
        
        
        [JsonProperty(Names.MaxX)]
        public float MaxX { get; set; }
        
        [JsonProperty(Names.MaxY)]
        public float MaxY { get; set; }
        
        
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
        public Vector2RectStep(IFloat minX, IFloat minY, IFloat maxX, IFloat maxY, IFloat step)
        {
            MinX = minX.Get();
            MinY = minY.Get();
            
            MaxX = maxX.Get();
            MaxY = maxY.Get();
            
            Step = step.Get();
        }

        public VectorType GetModelType() => VectorType.RandomRectStep;
        public Vector2 Get()
        {
            var x = Random.Range(MinX, MaxX);
            var y = Random.Range(MinY, MaxY);
            if (Step == 0f) return new Vector2(x, y);
            
            x = Mathf.RoundToInt(x / Step) * Step;
            y = Mathf.RoundToInt(y / Step) * Step;
            return new Vector2(x, y);
        }
    }
}