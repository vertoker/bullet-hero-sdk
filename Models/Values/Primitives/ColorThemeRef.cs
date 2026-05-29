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
    public class ColorThemeRef : IColor, ICopyable<ColorThemeRef>, IEquatable<ColorThemeRef>
    {
        [RuleInRange(ValueRules.MinThemeIndex, ValueRules.MaxThemeIndex)]
        [JsonProperty(Names.ThemeIndex)]
        public int ThemeColorIndex { get; set; }
        
        public ColorType GetModelType() => ColorType.ThemeRef;

        public ColorThemeRef()
        {
            ThemeColorIndex = ValueRules.MinThemeIndex;
        }
        public ColorThemeRef(int themeColorIndex)
        {
            ThemeColorIndex = themeColorIndex;
        }
        
        public object Clone() => Copy();
        IColor ICopyable<IColor>.Copy() => new ColorThemeRef(ThemeColorIndex);
        public ColorThemeRef Copy() => new(ThemeColorIndex);

        public override bool Equals(object obj) => obj is ColorThemeRef value && Equals(value);
        public override int GetHashCode() => ThemeColorIndex;
        
        public bool Equals(IColor other) => other is ColorThemeRef value && Equals(value);
        public bool Equals(ColorThemeRef other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = ThemeColorIndex.Equals(other.ThemeColorIndex);
            return result;
        }
    }
}