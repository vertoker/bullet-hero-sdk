using System;
using System.Collections.Generic;
using BHSDK.Models.AudioEffects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Keyframes;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BHSDK.Models.Audio
{
    [RuleContainer]
    public class LevelTrackEffects : ICopyable<LevelTrackEffects>, IEquatable<LevelTrackEffects>
    {
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxAudioVolumes)]
        [RuleCollectionSorted(nameof(FloatKey.Frame))]
        [RuleCollectionUnique(nameof(FloatKey.Frame))]
        [JsonProperty(Names.Volume)]
        public List<FloatKey> Volumes { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxAudioStereoPans)]
        [RuleCollectionSorted(nameof(FloatKey.Frame))]
        [RuleCollectionUnique(nameof(FloatKey.Frame))]
        [JsonProperty(Names.StereoPan)]
        public List<FloatKey> StereoPans { get; set; }
        
        // TODO add Inverse, play track in reverse
        
        [JsonProperty(Names.Active)]
        public bool Active { get; set; }
        
        // TODO replace float to IFloat
        
        [RuleNotNull]
        [JsonProperty(Names.Lowpass)]
        public AudioLowpass Lowpass { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Highpass)]
        public AudioHighpass Highpass { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Echo)]
        public AudioEcho Echo { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Reverb)]
        public AudioReverb Reverb { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Chorus)]
        public AudioChorus Chorus { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.PitchShifter)]
        public AudioPitchShifter PitchShifter { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Distortion)]
        public AudioDistortion Distortion { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Flange)]
        public AudioFlange Flange { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Compressor)]
        public AudioCompressor Compressor { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Normalize)]
        public AudioNormalize Normalize { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.ParamEQ)]
        public AudioParamEQ ParamEQ { get; set; }

        public LevelTrackEffects()
        {
            Volumes = new List<FloatKey>();
            StereoPans = new List<FloatKey>();
            Active = AudioRules.ActiveDefault;
            
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

        public LevelTrackEffects(List<FloatKey> volumes, List<FloatKey> stereoPans, bool active, 
            AudioLowpass lowpass, AudioHighpass highpass, AudioEcho echo, AudioReverb reverb, 
            AudioChorus chorus, AudioPitchShifter pitchShifter, AudioDistortion distortion, 
            AudioFlange flange, AudioCompressor compressor, AudioNormalize normalize, AudioParamEQ paramEQ)
        {
            Volumes = volumes;
            StereoPans = stereoPans;
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

        public object Clone() => Copy();
        public LevelTrackEffects Copy() => new(Volumes.CopyList(), StereoPans.CopyList(), Active,
            Lowpass.Copy(), Highpass.Copy(), Echo.Copy(), Reverb.Copy(),
            Chorus.Copy(), PitchShifter.Copy(), Distortion.Copy(),
            Flange.Copy(), Compressor.Copy(), Normalize.Copy(), ParamEQ.Copy());

        public override bool Equals(object obj) => obj is LevelTrackEffects value && Equals(value);
        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(Volumes.GetListHashCode());
            hashCode.Add(StereoPans.GetListHashCode());
            hashCode.Add(Active);
            hashCode.Add(Lowpass);
            hashCode.Add(Highpass);
            hashCode.Add(Echo);
            hashCode.Add(Reverb);
            hashCode.Add(Chorus);
            hashCode.Add(PitchShifter);
            hashCode.Add(Distortion);
            hashCode.Add(Flange);
            hashCode.Add(Compressor);
            hashCode.Add(Normalize);
            hashCode.Add(ParamEQ);
            return hashCode.ToHashCode();
        }

        public bool Equals(LevelTrackEffects other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Volumes.ListEquals(other.Volumes)
                         && StereoPans.ListEquals(other.StereoPans)
                         && Active == other.Active
                         && Lowpass.Equals(other.Lowpass)
                         && Highpass.Equals(other.Highpass)
                         && Echo.Equals(other.Echo)
                         && Reverb.Equals(other.Reverb)
                         && Chorus.Equals(other.Chorus)
                         && PitchShifter.Equals(other.PitchShifter)
                         && Distortion.Equals(other.Distortion)
                         && Flange.Equals(other.Flange)
                         && Compressor.Equals(other.Compressor)
                         && Normalize.Equals(other.Normalize)
                         && ParamEQ.Equals(other.ParamEQ);
            return result;
        }
    }
}