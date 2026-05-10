using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.AudioEffects
{
    [RuleContainer]
    public class AudioDistortion : AudioEffect
    {
        [RuleInRange(AudioRules.Distortion.Level_Min, AudioRules.Distortion.Level_Max)]
        [JsonProperty(Names.Level)]
        public float Level { get; set; }

        public AudioDistortion()
        {
            Level = AudioRules.Distortion.Level_Default;
        }
        public AudioDistortion(float mixLevel, float level) : base(mixLevel)
        {
            Level = level;
        }
    }
}