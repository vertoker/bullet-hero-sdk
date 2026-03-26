using Newtonsoft.Json;

namespace BHSDK.Models.AudioEffects
{
    public class AudioFlange : AudioEffect
    {
        [JsonProperty(ModelNames.DryMix)]
        public float DryMix { get; set; }
        
        [JsonProperty(ModelNames.WetMix)]
        public float WetMix { get; set; }
        
        [JsonProperty(ModelNames.Depth)]
        public float Depth { get; set; }
        
        [JsonProperty(ModelNames.Rate)]
        public float Rate { get; set; }

        public AudioFlange()
        {
            DryMix = 0.45f; // 0f - 1f, 0.01f, %
            WetMix = 0.55f; // 0f - 1f, 0.01f, %
            Depth = 1f; // 0f - 1f, 0.01f, -
            Rate = 0.1f; // 0f - 20f, 0.1f, Hz
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