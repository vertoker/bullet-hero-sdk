using BHSDK.Models.Enum.Settings;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.SettingGroups
{
    [RuleContainer]
    public class ControlsSettings
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
    }
}