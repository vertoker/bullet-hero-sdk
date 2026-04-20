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
            CenterFreq = AudioStatic.ParamEQ_CenterFreqDefault;
            OctaveRange = AudioStatic.ParamEQ_OctaveRangeDefault;
            FrequencyGain = AudioStatic.ParamEQ_FrequencyGainDefault;
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