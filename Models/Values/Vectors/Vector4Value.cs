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
    public class Vector4Value : IVector4, ICopyable<Vector4Value>, IEquatable<Vector4Value>
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

        public static Vector4Value Zero => new(0.0f, 0.0f, 0.0f, 0.0f);
        public static Vector4Value One => new(1.0f, 1.0f, 1.0f, 1.0f);
        
        public Vector4Value()
        {
            X = 0f;
            Y = 0f;
            Z = 0f;
            W = 0f;
        }
        public Vector4Value(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public VectorType GetModelType() => VectorType.Value;
        
        public object Clone() => Copy();
        IVector4 ICopyable<IVector4>.Copy() => new Vector4Value(X, Y, Z, W);
        public Vector4Value Copy() => new(X, Y, Z, W);

        public override bool Equals(object obj) => obj is Vector4Value value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(X, Y, Z, W);
        
        public bool Equals(IVector4 other) => other is Vector4Value value && Equals(value);
        public bool Equals(Vector4Value other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = X.Equals(other.X)
                         && Y.Equals(other.Y)
                         && Z.Equals(other.Z)
                         && W.Equals(other.W);
            return result;
        }
    }
}