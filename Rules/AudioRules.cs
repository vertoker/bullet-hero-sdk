// ReSharper disable InconsistentNaming
namespace BHSDK.Rules
{
    public static class AudioRules
    {
        // public static readonly float Core_PitchDefault = 1f; // 0.01f - 10f, 0.01f, %

        // -80f - 0f, 0.1f, dB;
        public const float MixLevel_Enabled = 0f;
        public const float MixLevel_Disabled = -80f;
        public const float MixLevel_Default = MixLevel_Disabled;

        public static bool IsActiveMixLevel(float mixLevel) => mixLevel > MixLevel_Disabled;

        public const float VolumeDefault = 1f; // 0f - 1f, 0.01f
        public const float StereoPanDefault = 0f; // -1f - 1f, 0.01f
        public const bool ActiveDefault = false;
        
        public static class Lowpass
        {
            // 10f - 22000f, 1f, Hz
            public const float CutoffFreq_Min = 10f;
            public const float CutoffFreq_Max = 22000f;
            public const float CutoffFreq_Default = 5000f;
        }
        public static class Highpass
        {
            // 10f - 22000f, 1f, Hz
            public const float CutoffFreq_Min = 10f;
            public const float CutoffFreq_Max = 22000f;
            public const float CutoffFreq_Default = 1000f;
        }
        public static class Echo
        {
            // 1f - 5000f, 1f, ms
            public const float Delay_Min = 1f;
            public const float Delay_Max = 5000f;
            public const float Delay_Default = 100f;
            
            // 0f - 1f, 0.01f, %
            public const float Decay_Min = 0f;
            public const float Decay_Max = 1f;
            public const float Decay_Default = 0.8f;
            
            // 0f - 16f, 0.01f, ch
            public const float MaxChannels_Min = 0f;
            public const float MaxChannels_Max = 16f;
            public const float MaxChannels_Default = 0f;
            
            // 0f - 1f, 0.01f, %
            public const float DryMix_Min = 0f;
            public const float DryMix_Max = 1f;
            public const float DryMix_Default = 1f;
            
            // 0f - 1f, 0.01f, %
            public const float WetMix_Min = 0f;
            public const float WetMix_Max = 1f;
            public const float WetMix_Default = 1f;
        }
        public static class Reverb
        {
            // -10000f - 0f, 1f, mB
            public const float DryLevel_Min = -10000f;
            public const float DryLevel_Max = 0f;
            public const float DryLevel_Default = 0f;
            
            // -10000f - 0f, 1f, mB
            public const float Room_Min = -10000f;
            public const float Room_Max = 0f;
            public const float Room_Default = -10000f;
            
            // -10000f - 0f, 1f, mB
            public const float RoomHF_Min = -10000f;
            public const float RoomHF_Max = 0f;
            public const float RoomHF_Default = 0f;
            
            // -10000f - 0f, 1f, mB
            public const float RoomLF_Min = -10000f;
            public const float RoomLF_Max = 0f;
            public const float RoomLF_Default = 0f;
            
            // 0.1f - 20f, 0.1f, s
            public const float DecayTime_Min = 0.1f;
            public const float DecayTime_Max = 20f;
            public const float DecayTime_Default = 1f;
            
            // 0.1f - 2f, 0.01f
            public const float DecayHFRatio_Min = 0.1f;
            public const float DecayHFRatio_Max = 2f;
            public const float DecayHFRatio_Default = 0.5f;
            
            // -10000f - 1000f, 1f, mB
            public const float Reflections_Min = -10000f;
            public const float Reflections_Max = 1000f;
            public const float Reflections_Default = -10000f;
            
            // 0f - 0.3f, 0.01f, s
            public const float ReflectDelay_Min = 0f;
            public const float ReflectDelay_Max = 0.3f;
            public const float ReflectDelay_Default = 0.02f;
            
            // -10000f - 2000f, 1f, mB
            public const float Reverb_Min = -10000f;
            public const float Reverb_Max = 2000f;
            public const float Reverb_Default = 0f;
            
            // 0f - 0.1f, 0.01f, s
            public const float ReverbDelay_Min = 0f;
            public const float ReverbDelay_Max = 0.1f;
            public const float ReverbDelay_Default = 0.04f;
            
            // 0f - 1f, 0.01f, %
            public const float Diffusion_Min = 0f;
            public const float Diffusion_Max = 1f;
            public const float Diffusion_Default = 1f;
            
            // 0f - 1f, 0.01f, %
            public const float Density_Min = 0f;
            public const float Density_Max = 1f;
            public const float Density_Default = 1f;
            
            // 20f - 20000f, 1f, Hz
            public const float HFReference_Min = 20f;
            public const float HFReference_Max = 20000f;
            public const float HFReference_Default = 5000f;
            
            // 20f - 1000f, 1f, Hz
            public const float LFReference_Min = 20f;
            public const float LFReference_Max = 1000f;
            public const float LFReference_Default = 250f;
        }
        public static class Chorus
        {
            // 0f - 1f, 0.01f, -
            public const float DryMix_Min = 0f;
            public const float DryMix_Max = 1f;
            public const float DryMix_Default = 0.5f;
            
            // 0f - 1f, 0.01f, -
            public const float WetMixTap1_Min = 0f;
            public const float WetMixTap1_Max = 1f;
            public const float WetMixTap1_Default = 0.5f;
            
            // 0f - 1f, 0.01f, -
            public const float WetMixTap2_Min = 0f;
            public const float WetMixTap2_Max = 1f;
            public const float WetMixTap2_Default = 0.5f;
            
            // 0f - 1f, 0.01f, -
            public const float WetMixTap3_Min = 0f;
            public const float WetMixTap3_Max = 1f;
            public const float WetMixTap3_Default = 0.5f;
            
            // 0f - 100f, 0.1f, ms
            public const float Delay_Min = 0f;
            public const float Delay_Max = 100f;
            public const float Delay_Default = 40f;
            
            // 0f - 20f, 0.1f, Hz
            public const float Rate_Min = 0f;
            public const float Rate_Max = 20f;
            public const float Rate_Default = 0.8f;
            
            // 0f - 1f, 0.01f, -
            public const float Depth_Min = 0f;
            public const float Depth_Max = 1f;
            public const float Depth_Default = 0.03f;
            
            // -1f - 1f, 0.01f, -
            public const float Feedback_Min = -1f;
            public const float Feedback_Max = 1f;
            public const float Feedback_Default = 0f;
        }
        public static class PitchShifter
        {
            // 0.5f - 2f, 0.01f, x
            public const float Pitch_Min = 0.5f;
            public const float Pitch_Max = 2f;
            public const float Pitch_Default = 1f;
            
            // 256f - 4096f, 1f, -
            public const float FFTSize_Min = 256f;
            public const float FFTSize_Max = 4096f;
            public const float FFTSize_Default = 1024f;
            
            // 1f - 32f, 0.1f, -
            public const float Overlap_Min = 1f;
            public const float Overlap_Max = 32f;
            public const float Overlap_Default = 4f;
            
            // 0f - 16f, 0.01f, ch
            public const float MaxChannels_Min = 0f;
            public const float MaxChannels_Max = 16f;
            public const float MaxChannels_Default = 0f;
        }
        public static class Distortion
        {
            // 0f - 1f, 0.01f, -
            public const float Level_Min = 0f;
            public const float Level_Max = 1f;
            public const float Level_Default = 0.5f;
        }
        public static class Flange
        {
            // 0f - 1f, 0.01f, %
            public const float DryMix_Min = 0f;
            public const float DryMix_Max = 1f;
            public const float DryMix_Default = 0.45f;
            
            // 0f - 1f, 0.01f, %
            public const float WetMix_Min = 0f;
            public const float WetMix_Max = 1f;
            public const float WetMix_Default = 0.55f;
            
            // 0f - 1f, 0.01f, -
            public const float Depth_Min = 0f;
            public const float Depth_Max = 1f;
            public const float Depth_Default = 1f;
            
            // 0f - 20f, 0.1f, Hz
            public const float Rate_Min = 0f;
            public const float Rate_Max = 20f;
            public const float Rate_Default = 0.1f;
        }
        public static class Compressor
        {
            // -60f - 0f, 0.1f, dB
            public const float Threshold_Min = -60f;
            public const float Threshold_Max = 0f;
            public const float Threshold_Default = 0f;
            
            // 10f - 200f, 1f, ms
            public const float Attack_Min = 10f;
            public const float Attack_Max = 200f;
            public const float Attack_Default = 50f;
            
            // 20f - 1000f, 1f, ms
            public const float Release_Min = 20f;
            public const float Release_Max = 1000f;
            public const float Release_Default = 50f;
            
            // 0f - 30f, 0.1f, dB
            public const float MakeUpGain_Min = 0f;
            public const float MakeUpGain_Max = 30f;
            public const float MakeUpGain_Default = 0f;
        }
        public static class Normalize
        {
            // 0f - 20000f, 1f, ms
            public const float FadeInTime_Min = 0f;
            public const float FadeInTime_Max = 20000f;
            public const float FadeInTime_Default = 5000f;
            
            // 0f - 1f, 0.01f, -
            public const float LowestVolume_Min = 0f;
            public const float LowestVolume_Max = 1f;
            public const float LowestVolume_Default = 0.1f;
            
            // 0f - 100000f, 1f, x
            public const float MaximumAmp_Min = 0f;
            public const float MaximumAmp_Max = 100000f;
            public const float MaximumAmp_Default = 20f;
        }
        public static class ParamEQ
        {
            // 20f - 22000f, 1f, Hz
            public const float CenterFreq_Min = 20f;
            public const float CenterFreq_Max = 22000f;
            public const float CenterFreq_Default = 5000f;
            
            // 0.2f - 5f, 0.01f, oct
            public const float OctaveRange_Min = 0.2f;
            public const float OctaveRange_Max = 5f;
            public const float OctaveRange_Default = 1f;
            
            // 0.05f - 3f, 0.01f, -
            public const float FrequencyGain_Min = 0.05f;
            public const float FrequencyGain_Max = 3f;
            public const float FrequencyGain_Default = 2f;
        }
    }
}