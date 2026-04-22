using Newtonsoft.Json;

namespace BHSDK.Models.AudioEffects
{
    public class AudioHighpass : AudioEffect
    {
        [JsonProperty(Names.CutoffFreq)]
        public float CutoffFreq { get; set; }

        public AudioHighpass()
        {
            CutoffFreq = AudioStatic.Highpass_CutoffFreqDefault;
        }
        public AudioHighpass(float mixLevel, float cutoffFreq) : base(mixLevel)
        {
            CutoffFreq = cutoffFreq;
        }
    }
}