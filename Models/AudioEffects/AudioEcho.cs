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
            Delay = 100f; // 1f - 5000f, 1f, ms
            Decay = 0.8f; // 0f - 1f, 0.01f, %
            MaxChannels = 0f; // 0f - 16f, 0.01f, ch
            DryMix = 1f; // 0f - 1f, 0.01f, %
            WetMix = 1f; // 0f - 1f, 0.01f, %
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