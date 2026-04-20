using Newtonsoft.Json;

namespace BHSDK.Models.AudioEffects
{
    public class AudioEcho : AudioEffect
    {
        [JsonProperty(ModelNames.Delay)]
        public float Delay { get; set; }
        
        [JsonProperty(ModelNames.Decay)]
        public float Decay { get; set; }
        
        [JsonProperty(ModelNames.MaxChannels)]
        public float MaxChannels { get; set; }
        
        [JsonProperty(ModelNames.DryMix)]
        public float DryMix { get; set; }
        
        [JsonProperty(ModelNames.WetMix)]
        public float WetMix { get; set; }

        public AudioEcho()
        {
            Delay = AudioStatic.Echo_DelayDefault;
            Decay = AudioStatic.Echo_DecayDefault;
            MaxChannels = AudioStatic.Echo_MaxChannelsDefault;
            DryMix = AudioStatic.Echo_DryMixDefault;
            WetMix = AudioStatic.Echo_WetMixDefault;
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