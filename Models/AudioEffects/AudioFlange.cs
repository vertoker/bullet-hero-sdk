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
            DryMix = AudioStatic.Flange_DryMix;
            WetMix = AudioStatic.Flange_WetMix;
            Depth = AudioStatic.Flange_Depth;
            Rate = AudioStatic.Flange_Rate;
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