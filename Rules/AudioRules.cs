// ReSharper disable InconsistentNaming
namespace BH.SDK.Rules
{
    public static class AudioRules
    {
        // public static readonly float Core_PitchDefault = 1f; // 0.01f - 10f, 0.01f, %

        public const int MaxFrameCount = 16;
        public const int MinAudioLayer = 0;
        public const int MaxAudioLayer = MaxFrameCount - 1;
        
        // How far a track's audio is shifted against the timeline, in seconds. Bounded both ways at
        // an hour: the value is a float with no other constraint, so without this a file can hold
        // NaN or 1e30 and every time-to-frame conversion downstream inherits it.
        public const float MinOffsetTime = -3600f;
        public const float MaxOffsetTime = 3600f;
        public const float OffsetTimeDefault = 0f;

        // Same numbers as FrameRules.MinSpeed/MaxSpeed, different thing entirely: this is one
        // track's own rate, that one is the whole level's playback rate. The two multiply, so the
        // real pitch a source ends up with reaches +-4 - which Unity allows, its own +-3 limit is
        // an inspector slider, not an API bound.

        // -2f - 2f, 0.01f, x
        public const float MinSpeed = -2f;
        public const float MaxSpeed = 2f;
        public const float SpeedDefault = 1f;

        // Bounds for the playback-sync settings on AudioGraphicsSettings. They are what the settings
        // screen offers and what validation checks; nothing clamps at runtime, exactly like every
        // other UserSettings value - a hand-edited file is the player's own doing.

        // How far the playhead must jump before it counts as a discontinuity rather than drift.
        // The floor is one audio buffer: below that it would fire on AudioSettings.dspTime's own
        // quantization alone and re-seek constantly, which is audible as a stutter. The ceiling is
        // where a scrub starts feeling unresponsive.

        // 0.02f - 0.5f, 0.01f, s
        public const float MinResyncJumpTime = 0.02f;
        public const float MaxResyncJumpTime = 0.5f;
        public const float ResyncJumpTimeDefault = 0.05f;

        // Drift below which the playback rate is left at exactly 1. Not a precision knob: any rate
        // other than 1 puts the engine's resampler in the signal path, so this is "how much desync
        // is worth that cost". Too small and the resampler is simply always on.

        // 0.005f - 0.1f, 0.005f, s
        public const float MinSyncDeadZone = 0.005f;
        public const float MaxSyncDeadZone = 0.1f;
        public const float SyncDeadZoneDefault = 0.02f;

        // How far the rate correction may bend playback. 0 turns it off entirely, leaving the hard
        // resync as the only correction - a legitimate choice if the resampler is more objectionable
        // than the occasional jump. The ceiling is ~0.85 of a semitone, past which it stops reading
        // as a correction and starts reading as the music being wrong.

        // 0f - 0.05f, 0.005f, x
        public const float MinPitchCorrection = 0f;
        public const float MaxPitchCorrection = 0.05f;
        public const float PitchCorrectionDefault = 0.02f;

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