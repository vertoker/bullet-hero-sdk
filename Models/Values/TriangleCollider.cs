using System;
using BH.SDK.Models.Interfaces;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Values
{
    [RuleContainer]
    public class TriangleCollider : IModel<TriangleCollider>
    {
        [RuleNotNull, RuleIVector2InRange(ValueRules.MinPos, ValueRules.MaxPos)]
        [JsonProperty(Names.Point1)]
        public Vector2Value Point1 { get; set; }
        
        [RuleNotNull, RuleIVector2InRange(ValueRules.MinPos, ValueRules.MaxPos)]
        [JsonProperty(Names.Point2)]
        public Vector2Value Point2 { get; set; }
        
        [RuleNotNull, RuleIVector2InRange(ValueRules.MinPos, ValueRules.MaxPos)]
        [JsonProperty(Names.Point3)]
        public Vector2Value Point3 { get; set; }
        
        public TriangleCollider()
        {
            Point1 = new Vector2Value(-0.5f, -0.5f);
            Point2 = new Vector2Value(0.5f, -0.5f);
            Point3 = new Vector2Value(0.5f, 0.5f);
        }
        public TriangleCollider(float x1, float y1, float x2, float y2, float x3, float y3)
        {
            Point1 = new Vector2Value(x1, y1);
            Point2 = new Vector2Value(x2, y2);
            Point3 = new Vector2Value(x3, y3);
        }
        public TriangleCollider(Vector2Value point1, Vector2Value point2, Vector2Value point3)
        {
            Point1 = point1;
            Point2 = point2;
            Point3 = point3;
        }
        public void Reset()
        {
            Point1 = new Vector2Value(-0.5f, -0.5f);
            Point2 = new Vector2Value(0.5f, -0.5f);
            Point3 = new Vector2Value(0.5f, 0.5f);
        }

        public object Clone() => Copy();
        public TriangleCollider Copy() => new(Point1.Copy(), Point2.Copy(), Point3.Copy());

        public bool Equals(TriangleCollider other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Point1.Equals(other.Point1)
                         && Point2.Equals(other.Point2)
                         && Point3.Equals(other.Point3);
            return result;
        }

        public override bool Equals(object obj) => obj is TriangleCollider value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Point1, Point2, Point3);
    }
}