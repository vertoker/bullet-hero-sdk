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
            CenterFreq = AudioStatic.ParamEQ_CenterFreq;
            OctaveRange = AudioStatic.ParamEQ_OctaveRange;
            FrequencyGain = AudioStatic.ParamEQ_FrequencyGain;
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