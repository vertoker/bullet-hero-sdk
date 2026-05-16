using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.SettingGroups.Graphics
{
    [RuleContainer]
    public abstract class BaseGraphicsSettings
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
    }
}