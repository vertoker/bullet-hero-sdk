using Newtonsoft.Json;

namespace BHSDK.Models.AudioEffects
{
    public class AudioLowpass : AudioEffect
    {
        [JsonProperty(ModelNames.CutoffFreq)]
        public float CutoffFreq { get; set; }

        public AudioLowpass()
        {
            CutoffFreq = AudioStatic.Lowpass_CutoffFreq;
        }
        public AudioLowpass(float mixLevel, float cutoffFreq) : base(mixLevel)
        {
            CutoffFreq = cutoffFreq;
        }
    }
}