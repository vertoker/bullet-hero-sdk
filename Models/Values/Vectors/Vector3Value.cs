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
    public class Vector3Value : IVector3, IModel<Vector3Value>
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
        
        public static Vector3Value Zero => new(0.0f, 0.0f, 0.0f);
        public static Vector3Value One => new(1.0f, 1.0f, 1.0f);
        
        public static Vector3Value Right => new(1.0f, 0.0f, 0.0f);
        public static Vector3Value Left => new(-1.0f, 0.0f, 0.0f);
        public static Vector3Value Up => new(0.0f, 1.0f, 0.0f);
        public static Vector3Value Down => new(0.0f, -1.0f, 0.0f);
        public static Vector3Value Forward => new(0.0f, 0.0f, 1.0f);
        public static Vector3Value Backward => new(0.0f, 0.0f, -1.0f);
        
        public Vector3Value()
        {
            X = ValueRules.FloatZero;
            Y = ValueRules.FloatZero;
            Z = ValueRules.FloatZero;
        }
        public Vector3Value(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public void Reset()
        {
            X = ValueRules.FloatZero;
            Y = ValueRules.FloatZero;
            Z = ValueRules.FloatZero;
        }

        public VectorType GetModelType() => VectorType.Value;
        
        public object Clone() => Copy();
        IVector3 ICopyable<IVector3>.Copy() => new Vector3Value(X, Y, Z);
        public Vector3Value Copy() => new(X, Y, Z);
        
        public override bool Equals(object obj) => obj is Vector3Value value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(X, Y, Z);
        
        public bool Equals(IVector3 other) => other is Vector3Value value && Equals(value);
        public bool Equals(Vector3Value other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = X.Equals(other.X)
                         && Y.Equals(other.Y)
                         && Z.Equals(other.Z);
            return result;
        }
    }
}