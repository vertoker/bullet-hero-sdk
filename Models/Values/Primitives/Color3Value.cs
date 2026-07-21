using System;
using BH.SDK.Models.Enum.Values;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable InconsistentNaming

namespace BH.SDK.Models.Values
{
    // Alpha is not stored: any IColor3 is implicitly fully opaque (alpha = 1).
    [RuleContainer]
    public class Color3Value : IColor3, IModel<Color3Value>
    {
        [RuleInRange(ValueRules.MinColor, ValueRules.MaxColor)]
        [JsonProperty(Names.ChannelR)]
        public float R { get; set; }

        [RuleInRange(ValueRules.MinColor, ValueRules.MaxColor)]
        [JsonProperty(Names.ChannelG)]
        public float G { get; set; }

        [RuleInRange(ValueRules.MinColor, ValueRules.MaxColor)]
        [JsonProperty(Names.ChannelB)]
        public float B { get; set; }

        public Color3Value()
        {
            R = ValueRules.MaxColor;
            G = ValueRules.MaxColor;
            B = ValueRules.MaxColor;
        }
        public Color3Value(float r, float g, float b)
        {
            R = r;
            G = g;
            B = b;
        }
        public void Reset()
        {
            R = ValueRules.MaxColor;
            G = ValueRules.MaxColor;
            B = ValueRules.MaxColor;
        }

        public ColorType GetModelType() => ColorType.Value;

        public object Clone() => Copy();
        IColor3 ICopyable<IColor3>.Copy() => new Color3Value(R, G, B);
        public Color3Value Copy() => new(R, G, B);

        public override bool Equals(object obj) => obj is Color3Value value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(R, G, B);

        public bool Equals(IColor3 other) => other is Color3Value value && Equals(value);
        public bool Equals(Color3Value other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = R.Equals(other.R)
                         && G.Equals(other.G)
                         && B.Equals(other.B);
            return result;
        }

        public static Color3Value white => new(1f, 1f, 1f);
        public static Color3Value black => new(0f, 0f, 0f);
    }
}
