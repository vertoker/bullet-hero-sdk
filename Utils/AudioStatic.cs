// ReSharper disable InconsistentNaming
namespace BHSDK.Utils
{
    public static class AudioStatic
    {
        // -----------------------------------------------------------------------------
        // Defaults
        // -----------------------------------------------------------------------------
        
        public static readonly float VolumeDefault = 1f; // 0f - 1f, 0.01f
        public static readonly float StereoPanDefault = 0f; // -1f - 1f, 0.01f
        public static readonly bool ActiveDefault = false;
        
        // public static readonly float Core_PitchDefault = 1f; // 0.01f - 10f, 0.01f, %
        public static readonly float MixLevelDefault = -80f; // -80f - 0f, 0.1f, dB;
        public static bool IsActiveMixLevel(float mixLevel) => mixLevel > MixLevelDefault;
        public static float MixLevelEnabled = 0f;
        public static float MixLevelDisabled = -80f;
        
        public static readonly float Lowpass_CutoffFreqDefault = 5000f; // 10f - 22000f, 1f, Hz
        public static readonly float Highpass_CutoffFreqDefault = 1000f; // 10f - 22000f, 1f, Hz
        
        public static readonly float Echo_DelayDefault = 100f; // 1f - 5000f, 1f, ms
        public static readonly float Echo_DecayDefault = 0.8f; // 0f - 1f, 0.01f, %
        public static readonly float Echo_MaxChannelsDefault = 0f; // 0f - 16f, 0.01f, ch
        public static readonly float Echo_DryMixDefault = 1f; // 0f - 1f, 0.01f, %
        public static readonly float Echo_WetMixDefault = 1f; // 0f - 1f, 0.01f, %
        
        public static readonly float Reverb_DryLevelDefault = 0f; // -10000f - 0f, 1f, mB
        public static readonly float Reverb_RoomDefault = -10000f; // -10000f - 0f, 1f, mB
        public static readonly float Reverb_RoomHFDefault = 0f; // -10000f - 0f, 1f, mB
        public static readonly float Reverb_RoomLFDefault = 0f; // -10000f - 0f, 1f, mB
        public static readonly float Reverb_DecayTimeDefault = 1f; // 0.1f - 20f, 0.1f, s
        public static readonly float Reverb_DecayHFRatioDefault = 0.5f; // 0.1f - 2f, 0.01f
        public static readonly float Reverb_ReflectionsDefault = -10000f; // -10000f - 1000f, 1f, mB
        public static readonly float Reverb_ReflectDelayDefault = 0.02f; // 0f - 0.3f, 0.01f, s
        public static readonly float Reverb_ReverbDefault = 0f; // -10000f - 2000f, 1f, mB
        public static readonly float Reverb_ReverbDelayDefault = 0.04f; // 0f - 0.1f, 0.01f, s
        public static readonly float Reverb_DiffusionDefault = 1f; // 0f - 1f, 0.01f, %
        public static readonly float Reverb_DensityDefault = 1f; // 0f - 1f, 0.01f, %
        public static readonly float Reverb_HFReferenceDefault = 5000f; // 20f - 20000f, 1f, Hz
        public static readonly float Reverb_LFReferenceDefault = 250f; // 20f - 1000f, 1f, Hz
        
        public static readonly float Chorus_DryMixDefault = 0.5f; // 0f - 1f, 0.01f, -
        public static readonly float Chorus_WetMixTap1Default = 0.5f; // 0f - 1f, 0.01f, -
        public static readonly float Chorus_WetMixTap2Default = 0.5f; // 0f - 1f, 0.01f, -
        public static readonly float Chorus_WetMixTap3Default = 0.5f; // 0f - 1f, 0.01f, -
        public static readonly float Chorus_DelayDefault = 40f; // 0f - 100f, 0.1f, ms
        public static readonly float Chorus_RateDefault = 0.8f; // 0f - 20f, 0.1f, Hz
        public static readonly float Chorus_DepthDefault = 0.03f; // 0f - 1f, 0.01f, -
        public static readonly float Chorus_FeedbackDefault = 0f; // -1f - 1f, 0.01f, -
        
        public static readonly float PitchShifter_PitchDefault = 1f; // 0.5f - 2f, 0.01f, x
        public static readonly float PitchShifter_FFTSizeDefault = 1024f; // 256f - 4096f, 1f, -
        public static readonly float PitchShifter_OverlapDefault = 4f; // 1f - 32f, 0.1f, -
        public static readonly float PitchShifter_MaxChannelsDefault = 0f; // 0f - 16f, 0.01f, ch
        
        public static readonly float Distortion_LevelDefault = 0.5f;  // 0f - 1f, 0.01f, -
        
        public static readonly float Flange_DryMixDefault = 0.45f; // 0f - 1f, 0.01f, %
        public static readonly float Flange_WetMixDefault = 0.55f; // 0f - 1f, 0.01f, %
        public static readonly float Flange_DepthDefault = 1f; // 0f - 1f, 0.01f, -
        public static readonly float Flange_RateDefault = 0.1f; // 0f - 20f, 0.1f, Hz
        
        public static readonly float Compressor_ThresholdDefault = 0f; // -60f - 0f, 0.1f, dB
        public static readonly float Compressor_AttackDefault = 50f; // 10f - 200f, 1f, ms
        public static readonly float Compressor_ReleaseDefault = 50f; // 20f - 1000f, 1f, ms
        public static readonly float Compressor_MakeUpGainDefault = 0f; // 0f - 30f, 0.1f, dB
        
        public static readonly float Normalize_FadeInTimeDefault = 5000f; // 0f - 20000f, 1f, ms
        public static readonly float Normalize_LowestVolumeDefault = 0.1f; // 0f - 1f, 0.01f, -
        public static readonly float Normalize_MaximumAmpDefault = 20f; // 0f - 100000f, 1f, x
        
        public static readonly float ParamEQ_CenterFreqDefault = 5000f; // 20f - 22000f, 1f, Hz
        public static readonly float ParamEQ_OctaveRangeDefault = 1f; // 0.2f - 5f, 0.01f, oct
        public static readonly float ParamEQ_FrequencyGainDefault = 2f; // 0.05f - 3f, 0.01f, -
    }
}