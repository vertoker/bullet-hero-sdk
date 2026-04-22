using BHSDK.Models.Enum;
using BHSDK.Models.Keyframes;
using Newtonsoft.Json;

namespace BHSDK.Models.PostProcessing
{
    public class ChromaticAberration : Keyframe
    {
        [JsonProperty(Names.Intensity)]
        public float Intensity { get; set; }

        public ChromaticAberration()
        {
            Intensity = 1.0f;
        }
        public ChromaticAberration(int frame, 
            EaseType ease, float intensity)
            : base(frame, ease)
        {
            Intensity = intensity;
        }
    }
}