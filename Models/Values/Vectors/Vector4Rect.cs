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
    public class Vector4Rect : IVector4, IModel<Vector4Rect>
    {
        [RuleInRange(ValueRules.MinFloatValue, ValueRules.MaxFloatValue)]
        [JsonProperty(Names.MinX)]
        public float MinX { get; set; }
        
        [RuleInRange(ValueRules.MinFloatValue, ValueRules.MaxFloatValue)]
        [JsonProperty(Names.MinY)]
        public float MinY { get; set; }
        
        [RuleInRange(ValueRules.MinFloatValue, ValueRules.MaxFloatValue)]
        [JsonProperty(Names.MinZ)]
        public float MinZ { get; set; }
        
        [RuleInRange(ValueRules.MinFloatValue, ValueRules.MaxFloatValue)]
        [JsonProperty(Names.MinW)]
        public float MinW { get; set; }
        
        [RuleInRange(ValueRules.MinFloatValue, ValueRules.MaxFloatValue)]
        [JsonProperty(Names.MaxX)]
        public float MaxX { get; set; }
        
        [RuleInRange(ValueRules.MinFloatValue, ValueRules.MaxFloatValue)]
        [JsonProperty(Names.MaxY)]
        public float MaxY { get; set; }
        
        [RuleInRange(ValueRules.MinFloatValue, ValueRules.MaxFloatValue)]
        [JsonProperty(Names.MaxZ)]
        public float MaxZ { get; set; }
        
        [RuleInRange(ValueRules.MinFloatValue, ValueRules.MaxFloatValue)]
        [JsonProperty(Names.MaxW)]
        public float MaxW { get; set; }

        public Vector4Rect()
        {
            MinX = ValueRules.FloatZero;
            MinY = ValueRules.FloatZero;
            MinZ = ValueRules.FloatZero;
            MinW = ValueRules.FloatZero;
            
            MaxX = ValueRules.FloatOne;
            MaxY = ValueRules.FloatOne;
            MaxZ = ValueRules.FloatOne;
            MaxW = ValueRules.FloatOne;
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
        public void Reset()
        {
            MinX = ValueRules.FloatZero;
            MinY = ValueRules.FloatZero;
            MinZ = ValueRules.FloatZero;
            MinW = ValueRules.FloatZero;
            
            MaxX = ValueRules.FloatOne;
            MaxY = ValueRules.FloatOne;
            MaxZ = ValueRules.FloatOne;
            MaxW = ValueRules.FloatOne;
        }

        public VectorType GetModelType() => VectorType.RandomRect;

        public object Clone() => Copy();
        IVector4 ICopyable<IVector4>.Copy() => new Vector4Rect(MinX, MinY, MinZ, MinW, MaxX, MaxY, MaxZ, MaxW);
        public Vector4Rect Copy() => new(MinX, MinY, MinZ, MinW, MaxX, MaxY, MaxZ, MaxW);

        public override bool Equals(object obj) => obj is Vector4Rect value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(MinX, MinY, MinZ, MinW, MaxX, MaxY, MaxZ, MaxW);
        
        public bool Equals(IVector4 other) => other is Vector4Rect value && Equals(value);
        public bool Equals(Vector4Rect other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = MinX.Equals(other.MinX)
                         && MinY.Equals(other.MinY)
                         && MinZ.Equals(other.MinZ)
                         && MinW.Equals(other.MinW)
                         && MaxX.Equals(other.MaxX)
                         && MaxY.Equals(other.MaxY)
                         && MaxZ.Equals(other.MaxZ)
                         && MaxW.Equals(other.MaxW);
            return result;
        }
    }
}