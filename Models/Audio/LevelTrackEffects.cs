using System;
using System.Collections.Generic;
using BH.SDK.Models.AudioEffects;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Keyframes;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Audio
{
    [RuleContainer]
    public class LevelTrackEffects : IModel<LevelTrackEffects>
    {
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxAudioKeys)]
        [RuleCollectionUnique(nameof(FloatKey.Frame))]
        [JsonProperty(Names.Volume)]
        public List<FloatKey> Volumes { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxAudioKeys)]
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
        public void Reset()
        {
            Volumes.Clear();
            StereoPans.Clear();
            Active = AudioRules.ActiveDefault;
            
            Lowpass.Reset();
            Highpass.Reset();
            Echo.Reset();
            Reverb.Reset();
            Chorus.Reset();
            PitchShifter.Reset();
            Distortion.Reset();
            Flange.Reset();
            Compressor.Reset();
            Normalize.Reset();
            ParamEQ.Reset();
        }

        public object Clone() => Copy();
        public LevelTrackEffects Copy() => new(Volumes.CopyList(), StereoPans.CopyList(), Active,
            (AudioLowpass)Lowpass.Clone(), (AudioHighpass)Highpass.Clone(), (AudioEcho)Echo.Clone(),
            (AudioReverb)Reverb.Clone(), (AudioChorus)Chorus.Clone(), (AudioPitchShifter)PitchShifter.Clone(),
            (AudioDistortion)Distortion.Clone(), (AudioFlange)Flange.Clone(), (AudioCompressor)Compressor.Clone(),
            (AudioNormalize)Normalize.Clone(), (AudioParamEQ)ParamEQ.Clone());

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