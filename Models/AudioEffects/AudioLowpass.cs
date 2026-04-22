using Newtonsoft.Json;

namespace BHSDK.Models.AudioEffects
{
    public class AudioLowpass : AudioEffect
    {
        [JsonProperty(Names.CutoffFreq)]
        public float CutoffFreq { get; set; }

        public AudioLowpass()
        {
            CutoffFreq = AudioStatic.Lowpass_CutoffFreqDefault;
        }
        public AudioLowpass(float mixLevel, float cutoffFreq) : base(mixLevel)
        {
            CutoffFreq = cutoffFreq;
        }
    }
}