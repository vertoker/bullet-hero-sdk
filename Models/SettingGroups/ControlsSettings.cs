using System;
using BHSDK.Models.Enum.Settings;
using BHSDK.Models.Interfaces;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.SettingGroups
{
    [RuleContainer]
    public class ControlsSettings : ICopyable<ControlsSettings>, IEquatable<ControlsSettings>
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

        public object Clone() => Copy();
        public ControlsSettings Copy() => new(ClassicControlsType);

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