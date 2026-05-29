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
    public class Vector4Circle : IVector4, ICopyable<Vector4Circle>, IEquatable<Vector4Circle>
    {
        [RuleInRange(ValueRules.MinFloatValue, ValueRules.MaxFloatValue)]
        [JsonProperty(Names.CoordX)]
        public float X { get; set; }
        
        [RuleInRange(ValueRules.MinFloatValue, ValueRules.MaxFloatValue)]
        [JsonProperty(Names.CoordY)]
        public float Y { get; set; }
        
        [RuleInRange(ValueRules.MinFloatValue, ValueRules.MaxFloatValue)]
        [JsonProperty(Names.CoordZ)]
        public float Z { get; set; }
        
        [RuleInRange(ValueRules.MinFloatValue, ValueRules.MaxFloatValue)]
        [JsonProperty(Names.CoordW)]
        public float W { get; set; }
        
        [RuleInRange(ValueRules.ZeroFloat, ValueRules.MaxFloatValue)]
        [JsonProperty(Names.Radius)]
        public float Radius { get; set; }
        
        public Vector4Circle()
        {
            X = 0f;
            Y = 0f;
            Z = 0f;
            W = 0f;
            Radius = 1f;
        }
        public Vector4Circle(float x, float y, float z, float w, float radius)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
            Radius = radius;
        }

        public VectorType GetModelType() => VectorType.RandomCircle;

        public object Clone() => Copy();
        IVector4 ICopyable<IVector4>.Copy() => new Vector4Circle(X, Y, Z, W, Radius);
        public Vector4Circle Copy() => new(X, Y, Z, W, Radius);

        public override bool Equals(object obj) => obj is Vector4Circle value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(X, Y, Z, W, Radius);
        
        public bool Equals(IVector4 other) => other is Vector4Circle value && Equals(value);
        public bool Equals(Vector4Circle other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = X.Equals(other.X)
                         && Y.Equals(other.Y)
                         && Z.Equals(other.Z)
                         && W.Equals(other.W)
                         && Radius.Equals(other.Radius);
            return result;
        }
    }
}