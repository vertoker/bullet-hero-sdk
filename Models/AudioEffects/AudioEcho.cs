using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.AudioEffects
{
    public class AudioEcho : AudioEffect
    {
        [JsonProperty(Names.Delay)]
        public float Delay { get; set; }
        
        [JsonProperty(Names.Decay)]
        public float Decay { get; set; }
        
        [JsonProperty(Names.MaxChannels)]
        public float MaxChannels { get; set; }
        
        [JsonProperty(Names.DryMix)]
        public float DryMix { get; set; }
        
        [JsonProperty(Names.WetMix)]
        public float WetMix { get; set; }

        public AudioEcho()
        {
            Delay = AudioStatic.Echo_DelayDefault;
            Decay = AudioStatic.Echo_DecayDefault;
            MaxChannels = AudioStatic.Echo_MaxChannelsDefault;
            DryMix = AudioStatic.Echo_DryMixDefault;
            WetMix = AudioStatic.Echo_WetMixDefault;
        }
        public AudioEcho(float mixLevel, float delay, float decay,
            float maxChannels, float dryMix, float wetMix) : base(mixLevel)
        {
            Delay = delay;
            Decay = decay;
            MaxChannels = maxChannels;
            DryMix = dryMix;
            WetMix = wetMix;
        }
    }
}