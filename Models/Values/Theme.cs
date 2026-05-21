using System;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.SaveData;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Values
{
    [RuleContainer]
    public class Theme : ITheme, ICopyable<Theme>, IEquatable<Theme>
    {
        public static readonly Version Version = new(1, 0);
        public Version GetVersion() => Version;
        
        [RuleNotNull, RuleStringMax(ValueRules.MaxEditorName)]
        [JsonProperty(Names.Name)]
        public string Name { get; set; }
        
        [RuleNotNull, RuleCollectionCount(ValueRules.ThemeCount)]
        [JsonProperty(Names.Matrix)]
        public ColorValue[] Matrix { get; set; }
        
        // Theme - is map of colors, level can refer to color via index
        // Theme is a predefined array in runtime
        // Now it's 64 or 8x8 grid. If you see PA and this game, what indexes means (starts with 1)
        // 1 - fallback color, if index is not founded
        // 2 - GUI (PA)
        // 3 - Background (PA)
        // 4-7 - Players (PA)
        // 8 - Tail (PA)
        // 9-16 - free space
        // 17-25 - objects (PA)
        // 26-32 - free
        // 33-41 - parallax (PA)
        // 42-48 - free
        // 49-57 - effects (PA)
        // 58-64 - free
        
        public Theme()
        {
            Name = string.Empty;
            Matrix = new ColorValue[ValueRules.ThemeCount];
            Array.Fill(Matrix, ColorValue.white);
        }
        public Theme(string name, ColorValue[] matrix)
        {
            Name = name;
            Matrix = matrix;
        }

        public object Clone() => Copy();
        public Theme Copy() => new(Name, Matrix.CopyArray());

        public override bool Equals(object obj) => obj is Theme value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Name, Matrix.GetArrayHashCode());

        public bool Equals(Theme other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Name.Equals(other.Name)
                         && Matrix.ArrayEquals(other.Matrix);
            return result;
        }
    }
}