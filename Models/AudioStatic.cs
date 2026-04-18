// ReSharper disable InconsistentNaming
namespace BHSDK.Models
{
    public static class AudioStatic
    {
        // -----------------------------------------------------------------------------
        // Defaults
        // -----------------------------------------------------------------------------
        
        public static readonly float VolumeDefault = 1f; // 0f - 1f, 0.01f
        public static readonly float StereoPanDefault = 0f; // -1f - 1f, 0.01f
        public static readonly bool UseMixerDefault = false;
        
        public static readonly float Core_PitchDefault = 1f; // 0.01f - 10f, 0.01f, %
        
        public static readonly float Lowpass_CutoffFreq = 5000f; // 10f - 22000f, 1f, Hz
        public static readonly float Highpass_CutoffFreq = 1000f; // 10f - 22000f, 1f, Hz
        
        public static readonly float Echo_Delay = 100f; // 1f - 5000f, 1f, ms
        public static readonly float Echo_Decay = 0.8f; // 0f - 1f, 0.01f, %
        public static readonly float Echo_MaxChannels = 0f; // 0f - 16f, 0.01f, ch
        public static readonly float Echo_DryMix = 1f; // 0f - 1f, 0.01f, %
        public static readonly float Echo_WetMix = 1f; // 0f - 1f, 0.01f, %
        
        public static readonly float Reverb_DryLevel = 0f; // -10000f - 0f, 1f, mB
        public static readonly float Reverb_Room = -10000f; // -10000f - 0f, 1f, mB
        public static readonly float Reverb_RoomHF = 0f; // -10000f - 0f, 1f, mB
        public static readonly float Reverb_RoomLF = 0f; // -10000f - 0f, 1f, mB
        public static readonly float Reverb_DecayTime = 1f; // 0.1f - 20f, 0.1f, s
        public static readonly float Reverb_DecayHFRatio = 0.5f; // 0.1f - 2f, 0.01f
        public static readonly float Reverb_Reflections = -10000f; // -10000f - 1000f, 1f, mB
        public static readonly float Reverb_ReflectDelay = 0.02f; // 0f - 0.3f, 0.01f, s
        public static readonly float Reverb_Reverb = 0f; // -10000f - 2000f, 1f, mB
        public static readonly float Reverb_ReverbDelay = 0.04f; // 0f - 0.1f, 0.01f, s
        public static readonly float Reverb_Diffusion = 1f; // 0f - 1f, 0.01f, %
        public static readonly float Reverb_Density = 1f; // 0f - 1f, 0.01f, %
        public static readonly float Reverb_HFReference = 5000f; // 20f - 20000f, 1f, Hz
        public static readonly float Reverb_LFReference = 250f; // 20f - 1000f, 1f, Hz
        
        public static readonly float Chorus_DryMix = 0.5f; // 0f - 1f, 0.01f, -
        public static readonly float Chorus_WetMixTap1 = 0.5f; // 0f - 1f, 0.01f, -
        public static readonly float Chorus_WetMixTap2 = 0.5f; // 0f - 1f, 0.01f, -
        public static readonly float Chorus_WetMixTap3 = 0.5f; // 0f - 1f, 0.01f, -
        public static readonly float Chorus_Delay = 40f; // 0f - 100f, 0.1f, ms
        public static readonly float Chorus_Rate = 0.8f; // 0f - 20f, 0.1f, Hz
        public static readonly float Chorus_Depth = 0.03f; // 0f - 1f, 0.01f, -
        public static readonly float Chorus_Feedback = 0f; // -1f - 1f, 0.01f, -
        
        public static readonly float PitchShifter_Pitch = 1f; // 0.5f - 2f, 0.01f, x
        public static readonly float PitchShifter_FFTSize = 1024f; // 256f - 4096f, 1f, -
        public static readonly float PitchShifter_Overlap = 4f; // 1f - 32f, 0.1f, -
        public static readonly float PitchShifter_MaxChannels = 0f; // 0f - 16f, 0.01f, ch
        
        public static readonly float Distortion_Level = 0.5f;  // 0f - 1f, 0.01f, -
        
        public static readonly float Flange_DryMix = 0.45f; // 0f - 1f, 0.01f, %
        public static readonly float Flange_WetMix = 0.55f; // 0f - 1f, 0.01f, %
        public static readonly float Flange_Depth = 1f; // 0f - 1f, 0.01f, -
        public static readonly float Flange_Rate = 0.1f; // 0f - 20f, 0.1f, Hz
        
        public static readonly float Compressor_Threshold = 0f; // -60f - 0f, 0.1f, dB
        public static readonly float Compressor_Attack = 50f; // 10f - 200f, 1f, ms
        public static readonly float Compressor_Release = 50f; // 20f - 1000f, 1f, ms
        public static readonly float Compressor_MakeUpGain = 0f; // 0f - 30f, 0.1f, dB
        
        public static readonly float Normalize_FadeInTime = 5000f; // 0f - 20000f, 1f, ms
        public static readonly float Normalize_LowestVolume = 0.1f; // 0f - 1f, 0.01f, -
        public static readonly float Normalize_MaximumAmp = 20f; // 0f - 100000f, 1f, x
        
        public static readonly float ParamEQ_CenterFreq = 5000f; // 20f - 22000f, 1f, Hz
        public static readonly float ParamEQ_OctaveRange = 1f; // 0.2f - 5f, 0.01f, oct
        public static readonly float ParamEQ_FrequencyGain = 2f; // 0.05f - 3f, 0.01f, -
    }
}