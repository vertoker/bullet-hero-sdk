using Newtonsoft.Json;

namespace BHSDK.Models.AudioEffects
{
    public class AudioParamEQ : AudioEffect
    {
        [JsonProperty(ModelNames.CenterFreq)]
        public float CenterFreq { get; set; }
        
        [JsonProperty(ModelNames.OctaveRange)]
        public float OctaveRange { get; set; }
        
        [JsonProperty(ModelNames.FrequencyGain)]
        public float FrequencyGain { get; set; }

        public AudioParamEQ()
        {
            CenterFreq = 5000f; // 20f - 22000f, 1f, Hz
            OctaveRange = 1f; // 0.2f - 5f, 0.01f, oct
            FrequencyGain = 2f; // 0.05f - 3f, 0.01f, -
        }
        public AudioParamEQ(float mixLevel, float centerFreq,
            float octaveRange, float frequencyGain) : base(mixLevel)
        {
            CenterFreq = centerFreq;
            OctaveRange = octaveRange;
            FrequencyGain = frequencyGain;
        }
    }
}