using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.AudioEffects
{
    public class AudioNormalize : AudioEffect
    {
        [RuleInRange(AudioRules.Normalize.FadeInTime_Min, AudioRules.Normalize.FadeInTime_Max)]
        [JsonProperty(Names.FadeInTime)]
        public float FadeInTime { get; set; }
        
        [RuleInRange(AudioRules.Normalize.LowestVolume_Min, AudioRules.Normalize.LowestVolume_Max)]
        [JsonProperty(Names.LowestVolume)]
        public float LowestVolume { get; set; }
        
        [RuleInRange(AudioRules.Normalize.MaximumAmp_Min, AudioRules.Normalize.MaximumAmp_Max)]
        [JsonProperty(Names.MaximumAmp)]
        public float MaximumAmp { get; set; }

        public AudioNormalize()
        {
            FadeInTime = AudioRules.Normalize.FadeInTime_Default;
            LowestVolume = AudioRules.Normalize.LowestVolume_Default;
            MaximumAmp = AudioRules.Normalize.MaximumAmp_Default;
        }
        public AudioNormalize(float mixLevel, float fadeInTime,
            float lowestVolume, float maximumAmp) : base(mixLevel)
        {
            FadeInTime = fadeInTime;
            LowestVolume = lowestVolume;
            MaximumAmp = maximumAmp;
        }
    }
}