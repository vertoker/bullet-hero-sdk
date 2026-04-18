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
            Delay = AudioStatic.Echo_Delay;
            Decay = AudioStatic.Echo_Decay;
            MaxChannels = AudioStatic.Echo_MaxChannels;
            DryMix = AudioStatic.Echo_DryMix;
            WetMix = AudioStatic.Echo_WetMix;
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