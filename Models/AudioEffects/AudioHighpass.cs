using Newtonsoft.Json;

namespace BHSDK.Models.AudioEffects
{
    public class AudioHighpass : AudioEffect
    {
        [JsonProperty(ModelNames.CutoffFreq)]
        public float CutoffFreq { get; set; }

        public AudioHighpass()
        {
            CutoffFreq = AudioStatic.Highpass_CutoffFreq;
        }
        public AudioHighpass(float mixLevel, float cutoffFreq) : base(mixLevel)
        {
            CutoffFreq = cutoffFreq;
        }
    }
}