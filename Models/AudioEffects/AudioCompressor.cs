using Newtonsoft.Json;

namespace BHSDK.Models.AudioEffects
{
    public class AudioCompressor : AudioEffect
    {
        [JsonProperty(ModelNames.Threshold)]
        public float Threshold { get; set; }
        
        [JsonProperty(ModelNames.Attack)]
        public float Attack { get; set; }
        
        [JsonProperty(ModelNames.Release)]
        public float Release { get; set; }
        
        [JsonProperty(ModelNames.MakeUpGain)]
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