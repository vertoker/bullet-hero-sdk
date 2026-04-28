using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.AudioEffects
{
    public class AudioCompressor : AudioEffect
    {
        [JsonProperty(Names.Threshold)]
        public float Threshold { get; set; }
        
        [JsonProperty(Names.Attack)]
        public float Attack { get; set; }
        
        [JsonProperty(Names.Release)]
        public float Release { get; set; }
        
        [JsonProperty(Names.MakeUpGain)]
        public float MakeUpGain { get; set; }

        public AudioCompressor()
        {
            Threshold = AudioStatic.Compressor_ThresholdDefault;
            Attack = AudioStatic.Compressor_AttackDefault;
            Release = AudioStatic.Compressor_ReleaseDefault;
            MakeUpGain = AudioStatic.Compressor_MakeUpGainDefault;
        }
        public AudioCompressor(float mixLevel, float threshold,
            float attack, float release, float makeUpGain) : base(mixLevel)
        {
            Threshold = threshold;
            Attack = attack;
            Release = release;
            MakeUpGain = makeUpGain;
        }
    }
}