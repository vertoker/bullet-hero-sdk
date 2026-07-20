using System;
using BH.SDK.Models.Enum.Values;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Values
{
    [RuleContainer]
    public class Vector2Rect : IVector2, IModel<Vector2Rect>
    {
        // TODO add rule check for Min and Max, must be always Min < Max
        
        [RuleInRange(ValueRules.MinFloatValue, ValueRules.MaxFloatValue)]
        [JsonProperty(Names.MinX)]
        public float MinX { get; set; }
        
        [RuleInRange(ValueRules.MinFloatValue, ValueRules.MaxFloatValue)]
        [JsonProperty(Names.MinY)]
        public float MinY { get; set; }
        
        [RuleInRange(ValueRules.MinFloatValue, ValueRules.MaxFloatValue)]
        [JsonProperty(Names.MaxX)]
        public float MaxX { get; set; }
        
        [RuleInRange(ValueRules.MinFloatValue, ValueRules.MaxFloatValue)]
        [JsonProperty(Names.MaxY)]
        public float MaxY { get; set; }

        public Vector2Rect()
        {
            MinX = ValueRules.FloatZero;
            MinY = ValueRules.FloatZero;
            
            MaxX = ValueRules.FloatOne;
            MaxY = ValueRules.FloatOne;
        }
        public Vector2Rect(float minX, float minY, float maxX, float maxY)
        {
            MinX = minX;
            MinY = minY;
            
            MaxX = maxX;
            MaxY = maxY;
        }
        public void Reset()
        {
            MinX = ValueRules.FloatZero;
            MinY = ValueRules.FloatZero;
            
            MaxX = ValueRules.FloatOne;
            MaxY = ValueRules.FloatOne;
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