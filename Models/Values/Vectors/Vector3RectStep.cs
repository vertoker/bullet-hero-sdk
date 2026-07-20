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
    public class Vector3RectStep : IVector3, IModel<Vector3RectStep>
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
        [JsonProperty(Names.MaxX)]
        public float MaxX { get; set; }
        
        [RuleInRange(ValueRules.MinFloatValue, ValueRules.MaxFloatValue)]
        [JsonProperty(Names.MaxY)]
        public float MaxY { get; set; }
        
        [RuleInRange(ValueRules.MinFloatValue, ValueRules.MaxFloatValue)]
        [JsonProperty(Names.MaxZ)]
        public float MaxZ { get; set; }
        
        [RuleInRange(ValueRules.FloatZero, ValueRules.MaxFloatValue)]
        [JsonProperty(Names.Step)]
        public float Step { get; set; }

        public Vector3RectStep()
        {
            MinX = ValueRules.FloatZero;
            MinY = ValueRules.FloatZero;
            MinZ = ValueRules.FloatZero;
            
            MaxX = ValueRules.FloatOne;
            MaxY = ValueRules.FloatOne;
            MaxZ = ValueRules.FloatOne;
            
            Step = ValueRules.FloatOne;
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
        public void Reset()
        {
            MinX = ValueRules.FloatZero;
            MinY = ValueRules.FloatZero;
            MinZ = ValueRules.FloatZero;
            
            MaxX = ValueRules.FloatOne;
            MaxY = ValueRules.FloatOne;
            MaxZ = ValueRules.FloatOne;
            
            Step = ValueRules.FloatOne;
        }

        public VectorType GetModelType() => VectorType.RandomRectStep;

        public object Clone() => Copy();
        IVector3 ICopyable<IVector3>.Copy() => new Vector3RectStep(MinX, MinY, MinZ, MaxX, MaxY, MaxZ, Step);
        public Vector3RectStep Copy() => new(MinX, MinY, MinZ, MaxX, MaxY, MaxZ, Step);

        public override bool Equals(object obj) => obj is Vector3RectStep value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(MinX, MinY, MinZ, MaxX, MaxY, MaxZ, Step);
        
        public bool Equals(IVector3 other) => other is Vector3RectStep value && Equals(value);
        public bool Equals(Vector3RectStep other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = MinX.Equals(other.MinX)
                         && MinY.Equals(other.MinY)
                         && MinZ.Equals(other.MinZ)
                         && MaxX.Equals(other.MaxX)
                         && MaxY.Equals(other.MaxY)
                         && MaxZ.Equals(other.MaxZ)
                         && Step.Equals(other.Step);
            return result;
        }
    }
}