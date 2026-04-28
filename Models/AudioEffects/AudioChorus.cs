using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.AudioEffects
{
    public class AudioChorus : AudioEffect
    {
        [JsonProperty(Names.DryMix)]
        public float DryMix { get; set; }
        
        [JsonProperty(Names.WetMixTap1)]
        public float WetMixTap1 { get; set; }
        
        [JsonProperty(Names.WetMixTap2)]
        public float WetMixTap2 { get; set; }
        
        [JsonProperty(Names.WetMixTap3)]
        public float WetMixTap3 { get; set; }
        
        [JsonProperty(Names.Delay)]
        public float Delay { get; set; }
        
        [JsonProperty(Names.Rate)]
        public float Rate { get; set; }
        
        [JsonProperty(Names.Depth)]
        public float Depth { get; set; }
        
        [JsonProperty(Names.Feedback)]
        public float Feedback { get; set; }

        public AudioChorus()
        {
            DryMix = AudioStatic.Chorus_DryMixDefault;
            WetMixTap1 = AudioStatic.Chorus_WetMixTap1Default;
            WetMixTap2 = AudioStatic.Chorus_WetMixTap2Default;
            WetMixTap3 = AudioStatic.Chorus_WetMixTap3Default;
            Delay = AudioStatic.Chorus_DelayDefault;
            Rate = AudioStatic.Chorus_RateDefault;
            Depth = AudioStatic.Chorus_DepthDefault;
            Feedback = AudioStatic.Chorus_FeedbackDefault;
        }
        public AudioChorus(float mixLevel, float dryMix,
            float wetMixTap1, float wetMixTap2, float wetMixTap3,
            float delay, float rate, float depth, float feedback) 
            : base(mixLevel)
        {
            DryMix = dryMix;
            WetMixTap1 = wetMixTap1;
            WetMixTap2 = wetMixTap2;
            WetMixTap3 = wetMixTap3;
            Delay = delay;
            Rate = rate;
            Depth = depth;
            Feedback = feedback;
        }
    }
}