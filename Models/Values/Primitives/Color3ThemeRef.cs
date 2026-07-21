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
    public class Color3ThemeRef : IColor3, IModel<Color3ThemeRef>
    {
        [RuleInRange(ValueRules.MinThemeIndex, ValueRules.MaxThemeIndex)]
        [JsonProperty(Names.ThemeIndex)]
        public int ThemeColorIndex { get; set; }

        public ColorType GetModelType() => ColorType.ThemeRef;

        public Color3ThemeRef()
        {
            ThemeColorIndex = ValueRules.MinThemeIndex;
        }
        public Color3ThemeRef(int themeColorIndex)
        {
            ThemeColorIndex = themeColorIndex;
        }
        public void Reset()
        {
            ThemeColorIndex = ValueRules.MinThemeIndex;
        }

        public object Clone() => Copy();
        IColor3 ICopyable<IColor3>.Copy() => new Color3ThemeRef(ThemeColorIndex);
        public Color3ThemeRef Copy() => new(ThemeColorIndex);

        public override bool Equals(object obj) => obj is Color3ThemeRef value && Equals(value);
        public override int GetHashCode() => ThemeColorIndex;

        public bool Equals(IColor3 other) => other is Color3ThemeRef value && Equals(value);
        public bool Equals(Color3ThemeRef other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = ThemeColorIndex.Equals(other.ThemeColorIndex);
            return result;
        }
    }
}
