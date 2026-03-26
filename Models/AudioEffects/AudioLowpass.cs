using Newtonsoft.Json;

namespace BHSDK.Models.AudioEffects
{
    public class AudioLowpass : AudioEffect
    {
        [JsonProperty(ModelNames.CutoffFreq)]
        public float CutoffFreq { get; set; }

        public AudioLowpass()
        {
            CutoffFreq = 5000f; // 10f - 22000f, 1f, Hz
        }
        public AudioLowpass(float mixLevel, float cutoffFreq) : base(mixLevel)
        {
            CutoffFreq = cutoffFreq;
        }
    }
}