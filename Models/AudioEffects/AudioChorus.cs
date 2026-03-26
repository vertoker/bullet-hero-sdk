using Newtonsoft.Json;

namespace BHSDK.Models.AudioEffects
{
    public class AudioChorus : AudioEffect
    {
        [JsonProperty(ModelNames.DryMix)]
        public float DryMix { get; set; }
        
        [JsonProperty(ModelNames.WetMixTap1)]
        public float WetMixTap1 { get; set; }
        
        [JsonProperty(ModelNames.WetMixTap2)]
        public float WetMixTap2 { get; set; }
        
        [JsonProperty(ModelNames.WetMixTap3)]
        public float WetMixTap3 { get; set; }
        
        [JsonProperty(ModelNames.Delay)]
        public float Delay { get; set; }
        
        [JsonProperty(ModelNames.Rate)]
        public float Rate { get; set; }
        
        [JsonProperty(ModelNames.Depth)]
        public float Depth { get; set; }
        
        [JsonProperty(ModelNames.Feedback)]
        public float Feedback { get; set; }

        public AudioChorus()
        {
            DryMix = 0.5f; // 0f - 1f, 0.01f, -
            WetMixTap1 = 0.5f; // 0f - 1f, 0.01f, -
            WetMixTap2 = 0.5f; // 0f - 1f, 0.01f, -
            WetMixTap3 = 0.5f; // 0f - 1f, 0.01f, -
            Delay = 40f; // 0f - 100f, 0.1f, ms
            Rate = 0.8f; // 0f - 20f, 0.1f, Hz
            Depth = 0.03f; // 0f - 1f, 0.01f, -
            Feedback = 0f; // -1f - 1f, 0.01f, -
        }
        public AudioChorus(float mixLevel, float dryMix,
            float wetMixTap1, float wetMixTap2, float wetMixTap3,
            float delay, float rate, float depth, float feedback) 
            : base(mixLevel)
        {
            DryMix = dryMix;
            WetMixTap1 = wetMixTap1;
            WetMixTap2 = wetMixTap2;
            WetMixTap3 = wetMixTap3;
            Delay = delay;
            Rate = rate;
            Depth = depth;
            Feedback = feedback;
        }
    }
}