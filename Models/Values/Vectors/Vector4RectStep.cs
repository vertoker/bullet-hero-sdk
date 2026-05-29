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
    public class Vector4RectStep : IVector4, ICopyable<Vector4RectStep>, IEquatable<Vector4RectStep>
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
        
        [RuleInRange(ValueRules.ZeroFloat, ValueRules.MaxFloatValue)]
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

        public object Clone() => Copy();
        IVector4 ICopyable<IVector4>.Copy() => new Vector4RectStep(MinX, MinY, MinZ, MinW, MaxX, MaxY, MaxZ, MaxW, Step);
        public Vector4RectStep Copy() => new(MinX, MinY, MinZ, MinW, MaxX, MaxY, MaxZ, MaxW, Step);

        public override bool Equals(object obj) => obj is Vector4RectStep value && Equals(value);
        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(MinX);
            hashCode.Add(MinY);
            hashCode.Add(MinZ);
            hashCode.Add(MinW);
            hashCode.Add(MaxX);
            hashCode.Add(MaxY);
            hashCode.Add(MaxZ);
            hashCode.Add(MaxW);
            hashCode.Add(Step);
            return hashCode.ToHashCode();
        }
        
        public bool Equals(IVector4 other) => other is Vector4RectStep value && Equals(value);
        public bool Equals(Vector4RectStep other)
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
                         && MaxW.Equals(other.MaxW)
                         && Step.Equals(other.Step);
            return result;
        }
    }
}