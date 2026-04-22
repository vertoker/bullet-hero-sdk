using System.Collections.Generic;
using BHSDK.Models.AudioEffects;
using BHSDK.Models.Keyframes;
using Newtonsoft.Json;

namespace BHSDK.Models.Audio
{
    public class LevelTrackEffects
    {
        [JsonProperty(Names.Volume)]
        public List<FloatKey> Volume { get; set; }
        
        [JsonProperty(Names.StereoPan)]
        public List<FloatKey> StereoPan { get; set; }
        
        // TODO add Inverse, play track in reverse
        
        [JsonProperty(Names.Active)]
        public bool Active { get; set; }
        
        [JsonProperty(Names.Lowpass)]
        public AudioLowpass Lowpass { get; set; }
        
        [JsonProperty(Names.Highpass)]
        public AudioHighpass Highpass { get; set; }
        
        [JsonProperty(Names.Echo)]
        public AudioEcho Echo { get; set; }
        
        [JsonProperty(Names.Reverb)]
        public AudioReverb Reverb { get; set; }
        
        [JsonProperty(Names.Chorus)]
        public AudioChorus Chorus { get; set; }
        
        [JsonProperty(Names.PitchShifter)]
        public AudioPitchShifter PitchShifter { get; set; }
        
        [JsonProperty(Names.Distortion)]
        public AudioDistortion Distortion { get; set; }
        
        [JsonProperty(Names.Flange)]
        public AudioFlange Flange { get; set; }
        
        [JsonProperty(Names.Compressor)]
        public AudioCompressor Compressor { get; set; }
        
        [JsonProperty(Names.Normalize)]
        public AudioNormalize Normalize { get; set; }
        
        [JsonProperty(Names.ParamEQ)]
        public AudioParamEQ ParamEQ { get; set; }

        public LevelTrackEffects()
        {
            Volume = new List<FloatKey>();
            StereoPan = new List<FloatKey>();
            Active = AudioStatic.ActiveDefault;
            
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

        public LevelTrackEffects(List<FloatKey> volume, List<FloatKey> stereoPan, bool active, 
            AudioLowpass lowpass, AudioHighpass highpass, AudioEcho echo, AudioReverb reverb, 
            AudioChorus chorus, AudioPitchShifter pitchShifter, AudioDistortion distortion, 
            AudioFlange flange, AudioCompressor compressor, AudioNormalize normalize, AudioParamEQ paramEQ)
        {
            Volume = volume;
            StereoPan = stereoPan;
            Active = active;
            
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