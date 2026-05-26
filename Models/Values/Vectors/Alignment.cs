using System;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Values
{
    [RuleContainer]
    public class Alignment : ICopyable<Alignment>, IEquatable<Alignment>
    {
        [RuleNotNull(typeof(Vector2Value)), RuleIVector2InRange(ValueRules.MinSca, ValueRules.MaxSca)]
        [JsonProperty(Names.ValueShort)]
        public IVector2 Value { get; set; }

        public static Vector2Value LeftBottomValue =>   new(0.0f, 0.0f);
        public static Vector2Value LeftMiddleValue =>   new(0.0f, 0.5f);
        public static Vector2Value LeftTopValue =>      new(0.0f, 1.0f);
        public static Vector2Value CenterBottomValue => new(0.5f, 0.0f);
        public static Vector2Value CenterMiddleValue => new(0.5f, 0.5f);
        public static Vector2Value CenterTopValue =>    new(0.5f, 1.0f);
        public static Vector2Value RightBottomValue =>  new(1.0f, 0.0f);
        public static Vector2Value RightMiddleValue =>  new(1.0f, 0.5f);
        public static Vector2Value RightTopValue =>     new(1.0f, 1.0f);
        
        public static Vector2Value Equilateral3Value => new(0.5f, 0.355570f); // 1f / cos(-30°) / 2f = 0.577350269189625764
        public static Vector2Value Equilateral5Value => new(0.5f, 0.449739f); // 1f / cos(18°) / 2f = 0.525731112119133606
        public static Vector2Value Equilateral7Value => new(0.5f, 0.474765f); // 1f / cos((90-360/7*2)°) / 2f = 0.512858431636276949
        public static Vector2Value Equilateral9Value => new(0.5f, 0.484907f); // 1f / cos((90-360/9*2)°) / 2f = 0.507713305942872492
        
        public static Alignment LeftBottom => new(LeftBottomValue);
        public static Alignment LeftMiddle => new(LeftMiddleValue);
        public static Alignment LeftTop => new(LeftTopValue);
        public static Alignment CenterBottom => new(CenterBottomValue);
        public static Alignment CenterMiddle => new(CenterMiddleValue);
        public static Alignment CenterTop => new(CenterTopValue);
        public static Alignment RightBottom => new(RightBottomValue);
        public static Alignment RightMiddle => new(RightMiddleValue);
        public static Alignment RightTop => new(RightTopValue);
        
        public static Alignment Equilateral3 => new(Equilateral3Value);
        public static Alignment Equilateral5 => new(Equilateral5Value);
        public static Alignment Equilateral7 => new(Equilateral7Value);
        public static Alignment Equilateral9 => new(Equilateral9Value);
        
        public Alignment()
        {
            Value = CenterMiddleValue;
        }
        public Alignment(IVector2 value)
        {
            Value = value;
        }

        public object Clone() => Copy();
        public Alignment Copy() => new(Value.Copy());

        public override bool Equals(object obj) => obj is Vector4Value value && Equals(value);
        public override int GetHashCode() => Value != null ? Value.GetHashCode() : 0;

        public bool Equals(Alignment other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Value.Equals(other.Value);
            return result;
        }
    }
}