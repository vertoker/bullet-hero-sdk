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
    public class Vector3Circle : IVector3, ICopyable<Vector3Circle>, IEquatable<Vector3Circle>
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
        
        [RuleInRange(ValueRules.ZeroFloat, ValueRules.MaxFloatValue)]
        [JsonProperty(Names.Radius)]
        public float Radius { get; set; }
        
        public Vector3Circle()
        {
            X = 0f;
            Y = 0f;
            Z = 0f;
            Radius = 1f;
        }
        public Vector3Circle(float x, float y, float z, float radius)
        {
            X = x;
            Y = y;
            Z = z;
            Radius = radius;
        }

        public VectorType GetModelType() => VectorType.RandomCircle;

        public object Clone() => Copy();
        IVector3 ICopyable<IVector3>.Copy() => new Vector3Circle(X, Y, Z, Radius);
        public Vector3Circle Copy() => new(X, Y, Z, Radius);

        public override bool Equals(object obj) => obj is Vector3Circle value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(X, Y, Z, Radius);
        
        public bool Equals(IVector3 other) => other is Vector3Circle value && Equals(value);
        public bool Equals(Vector3Circle other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = X.Equals(other.X)
                         && Y.Equals(other.Y)
                         && Z.Equals(other.Z)
                         && Radius.Equals(other.Radius);
            return result;
        }
    }
}