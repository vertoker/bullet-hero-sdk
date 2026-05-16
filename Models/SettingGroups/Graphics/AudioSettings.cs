using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.SettingGroups.Graphics
{
    public class AudioSettings : BaseGraphicsSettings
    {
        [JsonProperty(Names.RenderEffects)]
        public bool RenderEffects { get; set; }
        
        [RuleMin(0f)]
        [JsonProperty(Names.MaxDiffTime)]
        public float MaxDiffTime { get; set; }
        
        [JsonProperty(Names.Scrub)]
        public bool UseScrub { get; set; }
        
        [RuleMin(0f)]
        [JsonProperty(Names.ScrubTime)]
        public float ScrubTime { get; set; }

        public AudioSettings()
        {
            RenderEffects = true;
            MaxDiffTime = 0.2f;
            UseScrub = true;
            ScrubTime = 0.1f;
        }
        public AudioSettings(bool render, bool renderEffects,
            float maxDiffTime, bool useScrub, float scrubTime) : base(render)
        {
            RenderEffects = renderEffects;
            MaxDiffTime = maxDiffTime;
            UseScrub = useScrub;
            ScrubTime = scrubTime;
        }
    }
}