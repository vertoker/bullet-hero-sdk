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
            DryMix = AudioStatic.Flange_DryMixDefault;
            WetMix = AudioStatic.Flange_WetMixDefault;
            Depth = AudioStatic.Flange_DepthDefault;
            Rate = AudioStatic.Flange_RateDefault;
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