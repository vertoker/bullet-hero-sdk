using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.AudioEffects
{
    [RuleContainer]
    public class AudioFlange : AudioEffect
    {
        [RuleInRange(AudioRules.Flange.DryMix_Min, AudioRules.Flange.DryMix_Max)]
        [JsonProperty(Names.DryMix)]
        public float DryMix { get; set; }
        
        [RuleInRange(AudioRules.Flange.WetMix_Min, AudioRules.Flange.WetMix_Max)]
        [JsonProperty(Names.WetMix)]
        public float WetMix { get; set; }
        
        [RuleInRange(AudioRules.Flange.Depth_Min, AudioRules.Flange.Depth_Max)]
        [JsonProperty(Names.Depth)]
        public float Depth { get; set; }
        
        [RuleInRange(AudioRules.Flange.Rate_Min, AudioRules.Flange.Rate_Max)]
        [JsonProperty(Names.Rate)]
        public float Rate { get; set; }

        public AudioFlange()
        {
            DryMix = AudioRules.Flange.DryMix_Default;
            WetMix = AudioRules.Flange.WetMix_Default;
            Depth = AudioRules.Flange.Depth_Default;
            Rate = AudioRules.Flange.Rate_Default;
        }
        public AudioFlange(float mixLevel, float dryMix, float wetMix,
            float depth, float rate) : base(mixLevel)
        {
            DryMix = dryMix;
            WetMix = wetMix;
            Depth = depth;
            Rate = rate;
        }
    }
}