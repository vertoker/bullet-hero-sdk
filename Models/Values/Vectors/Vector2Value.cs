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
    public class Vector2Value : IVector2, ICopyable<Vector2Value>, IEquatable<Vector2Value>
    {
        [JsonProperty(Names.CoordX)]
        public float X { get; set; }
        
        [JsonProperty(Names.CoordY)]
        public float Y { get; set; }

        public static Vector2Value Zero => new(0.0f, 0.0f);
        public static Vector2Value One => new(1.0f, 1.0f);
        
        public static Vector2Value Right => new(1.0f, 0.0f);
        public static Vector2Value Left => new(-1.0f, 0.0f);
        public static Vector2Value Up => new(0.0f, 1.0f);
        public static Vector2Value Down => new(0.0f, -1.0f);
        
        
        public Vector2Value()
        {
            X = 0f;
            Y = 0f;
        }
        public Vector2Value(float x, float y)
        {
            X = x;
            Y = y;
        }

        public VectorType GetModelType() => VectorType.Value;
        
        public object Clone() => Copy();
        IVector2 ICopyable<IVector2>.Copy() => new Vector2Value(X, Y);
        public Vector2Value Copy() => new(X, Y);
        
        public override bool Equals(object obj) => obj is Vector2Value value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(X, Y);
        
        public bool Equals(IVector2 other) => other is Vector2Value value && Equals(value);
        public bool Equals(Vector2Value other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = X.Equals(other.X)
                         && Y.Equals(other.Y);
            return result;
        }
    }
}