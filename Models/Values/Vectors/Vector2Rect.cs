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
    public class Vector2Rect : IVector2, ICopyable<Vector2Rect>, IEquatable<Vector2Rect>
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

        public object Clone() => Copy();
        IVector2 ICopyable<IVector2>.Copy() => new Vector2Rect(MinX, MinY, MaxX, MaxY);
        public Vector2Rect Copy() => new(MinX, MinY, MaxX, MaxY);
        
        public override bool Equals(object obj) => obj is Vector2Rect value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(MinX, MinY, MaxX, MaxY);
        
        public bool Equals(IVector2 other) => other is Vector2Rect value && Equals(value);
        public bool Equals(Vector2Rect other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = MinX.Equals(other.MinX)
                         && MinY.Equals(other.MinY)
                         && MaxX.Equals(other.MaxX)
                         && MaxY.Equals(other.MaxY);
            return result;
        }
    }
}