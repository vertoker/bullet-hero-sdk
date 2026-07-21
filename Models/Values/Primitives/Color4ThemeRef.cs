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
    public class Color4ThemeRef : IColor4, IModel<Color4ThemeRef>
    {
        [RuleInRange(ValueRules.MinThemeIndex, ValueRules.MaxThemeIndex)]
        [JsonProperty(Names.ThemeIndex)]
        public int ThemeColorIndex { get; set; }
        
        public ColorType GetModelType() => ColorType.ThemeRef;

        public Color4ThemeRef()
        {
            ThemeColorIndex = ValueRules.MinThemeIndex;
        }
        public Color4ThemeRef(int themeColorIndex)
        {
            ThemeColorIndex = themeColorIndex;
        }
        public void Reset()
        {
            ThemeColorIndex = ValueRules.MinThemeIndex;
        }
        
        public object Clone() => Copy();
        IColor4 ICopyable<IColor4>.Copy() => new Color4ThemeRef(ThemeColorIndex);
        public Color4ThemeRef Copy() => new(ThemeColorIndex);

        public override bool Equals(object obj) => obj is Color4ThemeRef value && Equals(value);
        public override int GetHashCode() => ThemeColorIndex;
        
        public bool Equals(IColor4 other) => other is Color4ThemeRef value && Equals(value);
        public bool Equals(Color4ThemeRef other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = ThemeColorIndex.Equals(other.ThemeColorIndex);
            return result;
        }
    }
}