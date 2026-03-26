using Newtonsoft.Json;

namespace BHSDK.Models.AudioEffects
{
    public class AudioHighpass : AudioEffect
    {
        [JsonProperty(ModelNames.CutoffFreq)]
        public float CutoffFreq { get; set; }

        public AudioHighpass()
        {
            CutoffFreq = 1000f; // 10f - 22000f, 1f, Hz
        }
        public AudioHighpass(float mixLevel, float cutoffFreq) : base(mixLevel)
        {
            CutoffFreq = cutoffFreq;
        }
    }
}