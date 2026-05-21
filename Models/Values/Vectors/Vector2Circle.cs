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
    public class Vector2Circle : IVector2, ICopyable<Vector2Circle>, IEquatable<Vector2Circle>
    {
        [JsonProperty(Names.CoordX)]
        public float X { get; set; }
        
        [JsonProperty(Names.CoordY)]
        public float Y { get; set; }
        
        [RuleMin(0f)]
        [JsonProperty(Names.Radius)]
        public float Radius { get; set; }
        
        public Vector2Circle()
        {
            X = 0f;
            Y = 0f;
            Radius = 1f;
        }
        public Vector2Circle(float x, float y, float radius)
        {
            X = x;
            Y = y;
            Radius = radius;
        }

        public VectorType GetModelType() => VectorType.RandomCircle;
        
        public object Clone() => Copy();
        IVector2 ICopyable<IVector2>.Copy() => new Vector2Circle(X, Y, Radius);
        public Vector2Circle Copy() => new(X, Y, Radius);

        public override bool Equals(object obj) => obj is Vector2Circle value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(X, Y, Radius);
        
        public bool Equals(IVector2 other) => other is Vector2Circle value && Equals(value);
        public bool Equals(Vector2Circle other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = X.Equals(other.X)
                         && Y.Equals(other.Y)
                         && Radius.Equals(other.Radius);
            return result;
        }
    }
}