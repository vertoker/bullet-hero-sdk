using System;
using BH.SDK.Models.Enum.Values;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Values
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

        public override bool Equals(object obj) => obj is Vector2RectStep value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(MinX, MinY, MaxX, MaxY, Step);
        
        public bool Equals(IVector2 other) => other is Vector2RectStep value && Equals(value);
        public bool Equals(Vector2RectStep other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = MinX.Equals(other.MinX)
                         && MinY.Equals(other.MinY)
                         && MaxX.Equals(other.MaxX)
                         && MaxY.Equals(other.MaxY)
                         && Step.Equals(other.Step);
            return result;
        }
    }
}