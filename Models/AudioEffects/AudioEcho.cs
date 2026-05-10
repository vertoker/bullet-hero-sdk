using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.AudioEffects
{
    [RuleContainer]
    public class AudioEcho : AudioEffect
    {
        [RuleInRange(AudioRules.Echo.Delay_Min, AudioRules.Echo.Delay_Max)]
        [JsonProperty(Names.Delay)]
        public float Delay { get; set; }
        
        [RuleInRange(AudioRules.Echo.Decay_Min, AudioRules.Echo.Decay_Max)]
        [JsonProperty(Names.Decay)]
        public float Decay { get; set; }
        
        [RuleInRange(AudioRules.Echo.MaxChannels_Min, AudioRules.Echo.MaxChannels_Max)]
        [JsonProperty(Names.MaxChannels)]
        public float MaxChannels { get; set; }
        
        [RuleInRange(AudioRules.Echo.DryMix_Min, AudioRules.Echo.DryMix_Max)]
        [JsonProperty(Names.DryMix)]
        public float DryMix { get; set; }
        
        [RuleInRange(AudioRules.Echo.WetMix_Min, AudioRules.Echo.WetMix_Max)]
        [JsonProperty(Names.WetMix)]
        public float WetMix { get; set; }

        public AudioEcho()
        {
            Delay = AudioRules.Echo.Delay_Default;
            Decay = AudioRules.Echo.Decay_Default;
            MaxChannels = AudioRules.Echo.MaxChannels_Default;
            DryMix = AudioRules.Echo.DryMix_Default;
            WetMix = AudioRules.Echo.WetMix_Default;
        }
        public AudioEcho(float mixLevel, float delay, float decay,
            float maxChannels, float dryMix, float wetMix) : base(mixLevel)
        {
            Delay = delay;
            Decay = decay;
            MaxChannels = maxChannels;
            DryMix = dryMix;
            WetMix = wetMix;
        }
    }
}