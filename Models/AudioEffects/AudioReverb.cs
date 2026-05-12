using System;
using BHSDK.Models.Interfaces;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BHSDK.Models.AudioEffects
{
    [RuleContainer]
    public class AudioReverb : AudioEffect, ICopyable<AudioReverb>, IEquatable<AudioReverb>
    {
        [RuleInRange(AudioRules.Reverb.DryLevel_Min, AudioRules.Reverb.DryLevel_Max)]
        [JsonProperty(Names.DryLevel)]
        public float DryLevel { get; set; }
        
        [RuleInRange(AudioRules.Reverb.Room_Min, AudioRules.Reverb.Room_Max)]
        [JsonProperty(Names.Room)]
        public float Room { get; set; }
        
        [RuleInRange(AudioRules.Reverb.RoomHF_Min, AudioRules.Reverb.RoomHF_Max)]
        [JsonProperty(Names.RoomHF)]
        public float RoomHF { get; set; }
        
        [RuleInRange(AudioRules.Reverb.RoomLF_Min, AudioRules.Reverb.RoomLF_Max)]
        [JsonProperty(Names.RoomLF)]
        public float RoomLF { get; set; }
        
        [RuleInRange(AudioRules.Reverb.DecayTime_Min, AudioRules.Reverb.DecayTime_Max)]
        [JsonProperty(Names.DecayTime)]
        public float DecayTime { get; set; }
        
        [RuleInRange(AudioRules.Reverb.DecayHFRatio_Min, AudioRules.Reverb.DecayHFRatio_Max)]
        [JsonProperty(Names.DecayHFRatio)]
        public float DecayHFRatio { get; set; }
        
        [RuleInRange(AudioRules.Reverb.Reflections_Min, AudioRules.Reverb.Reflections_Max)]
        [JsonProperty(Names.Reflections)]
        public float Reflections { get; set; }
        
        [RuleInRange(AudioRules.Reverb.ReflectDelay_Min, AudioRules.Reverb.ReflectDelay_Max)]
        [JsonProperty(Names.ReflectDelay)]
        public float ReflectDelay { get; set; }
        
        [RuleInRange(AudioRules.Reverb.Reverb_Min, AudioRules.Reverb.Reverb_Max)]
        [JsonProperty(Names.Reverb)]
        public float Reverb { get; set; }
        
        [RuleInRange(AudioRules.Reverb.ReverbDelay_Min, AudioRules.Reverb.ReverbDelay_Max)]
        [JsonProperty(Names.ReverbDelay)]
        public float ReverbDelay { get; set; }
        
        [RuleInRange(AudioRules.Reverb.Diffusion_Min, AudioRules.Reverb.Diffusion_Max)]
        [JsonProperty(Names.Diffusion)]
        public float Diffusion { get; set; }
        
        [RuleInRange(AudioRules.Reverb.Density_Min, AudioRules.Reverb.Density_Max)]
        [JsonProperty(Names.Density)]
        public float Density { get; set; }
        
        [RuleInRange(AudioRules.Reverb.HFReference_Min, AudioRules.Reverb.HFReference_Max)]
        [JsonProperty(Names.HFRef)]
        public float HFReference { get; set; }
        
        [RuleInRange(AudioRules.Reverb.LFReference_Min, AudioRules.Reverb.LFReference_Max)]
        [JsonProperty(Names.LFRef)]
        public float LFReference { get; set; }

        public AudioReverb()
        {
            DryLevel = AudioRules.Reverb.DryLevel_Default;
            Room = AudioRules.Reverb.Room_Default;
            RoomHF = AudioRules.Reverb.RoomHF_Default;
            RoomLF = AudioRules.Reverb.RoomLF_Default;
            DecayTime = AudioRules.Reverb.DecayTime_Default;
            DecayHFRatio = AudioRules.Reverb.DecayHFRatio_Default;
            Reflections = AudioRules.Reverb.Reflections_Default;
            ReflectDelay = AudioRules.Reverb.ReflectDelay_Default;
            Reverb = AudioRules.Reverb.Reverb_Default;
            ReverbDelay = AudioRules.Reverb.ReverbDelay_Default;
            Diffusion = AudioRules.Reverb.Diffusion_Default;
            Density = AudioRules.Reverb.Density_Default;
            HFReference = AudioRules.Reverb.HFReference_Default;
            LFReference = AudioRules.Reverb.LFReference_Default; 
        }
        public AudioReverb(float mixLevel, float dryLevel, float room, float roomHF, float roomLF, 
            float decayTime, float decayHFRatio, float reflections, float reflectDelay, float reverb,
            float reverbDelay, float diffusion, float density, float hfReference, float lfReference)
            : base(mixLevel)
        {
            DryLevel = dryLevel;
            Room = room;
            RoomHF = roomHF;
            RoomLF = roomLF;
            DecayTime = decayTime;
            DecayHFRatio = decayHFRatio;
            Reflections = reflections;
            ReflectDelay = reflectDelay;
            Reverb = reverb;
            ReverbDelay = reverbDelay;
            Diffusion = diffusion;
            Density = density;
            HFReference = hfReference;
            LFReference = lfReference;
        }

        public new object Clone() => Copy();
        public new AudioReverb Copy() => new(MixLevel, DryLevel, Room, RoomHF, RoomLF,
            DecayTime, DecayHFRatio, Reflections, ReflectDelay, Reverb,
            ReverbDelay, Diffusion, Density, HFReference, LFReference);

        public override bool Equals(object obj) => obj is AudioReverb value && Equals(value);
        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(base.GetHashCode());
            hashCode.Add(DryLevel);
            hashCode.Add(Room);
            hashCode.Add(RoomHF);
            hashCode.Add(RoomLF);
            hashCode.Add(DecayTime);
            hashCode.Add(DecayHFRatio);
            hashCode.Add(Reflections);
            hashCode.Add(ReflectDelay);
            hashCode.Add(Reverb);
            hashCode.Add(ReverbDelay);
            hashCode.Add(Diffusion);
            hashCode.Add(Density);
            hashCode.Add(HFReference);
            hashCode.Add(LFReference);
            return hashCode.ToHashCode();
        }

        public bool Equals(AudioReverb other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && DryLevel.Equals(other.DryLevel)
                         && Room.Equals(other.Room)
                         && RoomHF.Equals(other.RoomHF)
                         && RoomLF.Equals(other.RoomLF)
                         && DecayTime.Equals(other.DecayTime)
                         && DecayHFRatio.Equals(other.DecayHFRatio)
                         && Reflections.Equals(other.Reflections)
                         && ReflectDelay.Equals(other.ReflectDelay)
                         && Reverb.Equals(other.Reverb)
                         && ReverbDelay.Equals(other.ReverbDelay)
                         && Diffusion.Equals(other.Diffusion)
                         && Density.Equals(other.Density)
                         && HFReference.Equals(other.HFReference)
                         && LFReference.Equals(other.LFReference);
            return result;
        }
    }
}