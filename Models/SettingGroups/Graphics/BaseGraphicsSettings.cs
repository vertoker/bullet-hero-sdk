using System;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BH.SDK.Models.SettingGroups.Graphics
{
    [RuleContainer]
    public abstract class BaseGraphicsSettings : IEquatable<BaseGraphicsSettings>
    {
        [JsonProperty(Names.Render)]
        public bool Render { get; set; }

        protected BaseGraphicsSettings()
        {
            Render = true;
        }
        protected BaseGraphicsSettings(bool render)
        {
            Render = render;
        }

        public override bool Equals(object obj) => obj is BaseGraphicsSettings value && Equals(value);
        public override int GetHashCode() => Render.GetHashCode();

        public bool Equals(BaseGraphicsSettings other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Render == other.Render;
        }
    }
}