using BHSDK.Models.AudioEffects;
using Newtonsoft.Json;

namespace BHSDK.Models.Audio
{
    public class LevelTrackEffects
    {
        [JsonProperty(ModelNames.Volume)]
        public float Volume { get; set; }
        
        [JsonProperty(ModelNames.StereoPan)]
        public float StereoPan { get; set; }
        
        [JsonProperty(ModelNames.Mixer)]
        public bool UseMixer { get; set; }
        
        [JsonProperty(ModelNames.Pitch)]
        public float Pitch { get; set; }
        
        [JsonProperty(ModelNames.Lowpass)]
        public AudioLowpass Lowpass { get; set; }
        
        [JsonProperty(ModelNames.Highpass)]
        public AudioHighpass Highpass { get; set; }
        
        [JsonProperty(ModelNames.Echo)]
        public AudioEcho Echo { get; set; }
        
        [JsonProperty(ModelNames.Reverb)]
        public AudioReverb Reverb { get; set; }
        
        [JsonProperty(ModelNames.Chorus)]
        public AudioChorus Chorus { get; set; }
        
        [JsonProperty(ModelNames.PitchShifter)]
        public AudioPitchShifter PitchShifter { get; set; }
        
        [JsonProperty(ModelNames.Distortion)]
        public AudioDistortion Distortion { get; set; }
        
        [JsonProperty(ModelNames.Flange)]
        public AudioFlange Flange { get; set; }
        
        [JsonProperty(ModelNames.Compressor)]
        public AudioCompressor Compressor { get; set; }
        
        [JsonProperty(ModelNames.Normalize)]
        public AudioNormalize Normalize { get; set; }
        
        [JsonProperty(ModelNames.ParamEQ)]
        public AudioParamEQ ParamEQ { get; set; }

        public LevelTrackEffects()
        {
            Volume = AudioStatic.VolumeDefault;
            StereoPan = AudioStatic.StereoPanDefault;
            UseMixer = AudioStatic.UseMixerDefault;
            
            Pitch = AudioStatic.Core_PitchDefault;
            Lowpass = new AudioLowpass();
            Highpass = new AudioHighpass();
            Echo = new AudioEcho();
            Reverb = new AudioReverb();
            Chorus = new AudioChorus();
            PitchShifter = new AudioPitchShifter();
            Distortion = new AudioDistortion();
            Flange = new AudioFlange();
            Compressor = new AudioCompressor();
            Normalize = new AudioNormalize();
            ParamEQ = new AudioParamEQ();
        }

        public LevelTrackEffects(float volume, float stereoPan, bool useMixer, float pitch, 
            AudioLowpass lowpass, AudioHighpass highpass, AudioEcho echo, AudioReverb reverb, 
            AudioChorus chorus, AudioPitchShifter pitchShifter, AudioDistortion distortion, 
            AudioFlange flange, AudioCompressor compressor, AudioNormalize normalize, AudioParamEQ paramEQ)
        {
            Volume = volume;
            StereoPan = stereoPan;
            UseMixer = useMixer;
            Pitch = pitch;
            
            Lowpass = lowpass;
            Highpass = highpass;
            Echo = echo;
            Reverb = reverb;
            Chorus = chorus;
            PitchShifter = pitchShifter;
            Distortion = distortion;
            Flange = flange;
            Compressor = compressor;
            Normalize = normalize;
            ParamEQ = paramEQ;
        }
    }
}