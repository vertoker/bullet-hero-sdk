using System;
using BH.SDK.Models.Enum.Settings;
using BH.SDK.Models.Interfaces;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BH.SDK.Models.SettingGroups
{
    [RuleContainer]
    public class ControlsSettings : IResetable, ICopyable<ControlsSettings>, IMoveable<ControlsSettings>, IEquatable<ControlsSettings>
    {
        [JsonProperty(Names.ClassicControlsType)]
        public ClassicControlsType ClassicControlsType { get; set; }
        
        // TODO add key bindings

        public ControlsSettings()
        {
            ClassicControlsType = ClassicControlsType.Keyboard;
        }
        public ControlsSettings(ClassicControlsType classicControlsType)
        {
            ClassicControlsType = classicControlsType;
        }
        public void Reset()
        {
            ClassicControlsType = ClassicControlsType.Keyboard;
        }

        public object Clone() => Copy();
        public ControlsSettings Copy() => new(ClassicControlsType);

        public void Pull(ControlsSettings source)
        {
            ClassicControlsType = source.ClassicControlsType;
        }

        public override bool Equals(object obj) => obj is ControlsSettings value && Equals(value);
        public override int GetHashCode() => (int)ClassicControlsType;

        public bool Equals(ControlsSettings other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return ClassicControlsType == other.ClassicControlsType;
        }
    }
}