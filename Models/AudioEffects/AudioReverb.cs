using Newtonsoft.Json;

namespace BHSDK.Models.AudioEffects
{
    public class AudioReverb : AudioEffect
    {
        [JsonProperty(Names.DryLevel)]
        public float DryLevel { get; set; }
        
        [JsonProperty(Names.Room)]
        public float Room { get; set; }
        
        [JsonProperty(Names.RoomHF)]
        public float RoomHF { get; set; }
        
        [JsonProperty(Names.RoomLF)]
        public float RoomLF { get; set; }
        
        [JsonProperty(Names.DecayTime)]
        public float DecayTime { get; set; }
        
        [JsonProperty(Names.DecayHFRatio)]
        public float DecayHFRatio { get; set; }
        
        [JsonProperty(Names.Reflections)]
        public float Reflections { get; set; }
        
        [JsonProperty(Names.ReflectDelay)]
        public float ReflectDelay { get; set; }
        
        [JsonProperty(Names.Reverb)]
        public float Reverb { get; set; }
        
        [JsonProperty(Names.ReverbDelay)]
        public float ReverbDelay { get; set; }
        
        [JsonProperty(Names.Diffusion)]
        public float Diffusion { get; set; }
        
        [JsonProperty(Names.Density)]
        public float Density { get; set; }
        
        [JsonProperty(Names.HFRef)]
        public float HFReference { get; set; }
        
        [JsonProperty(Names.LFRef)]
        public float LFReference { get; set; }

        public AudioReverb()
        {
            DryLevel = AudioStatic.Reverb_DryLevelDefault;
            Room = AudioStatic.Reverb_RoomDefault;
            RoomHF = AudioStatic.Reverb_RoomHFDefault;
            RoomLF = AudioStatic.Reverb_RoomLFDefault;
            DecayTime = AudioStatic.Reverb_DecayTimeDefault;
            DecayHFRatio = AudioStatic.Reverb_DecayHFRatioDefault;
            Reflections = AudioStatic.Reverb_ReflectionsDefault;
            ReflectDelay = AudioStatic.Reverb_ReflectDelayDefault;
            Reverb = AudioStatic.Reverb_ReverbDefault;
            ReverbDelay = AudioStatic.Reverb_ReverbDelayDefault;
            Diffusion = AudioStatic.Reverb_DiffusionDefault;
            Density = AudioStatic.Reverb_DensityDefault;
            HFReference = AudioStatic.Reverb_HFReferenceDefault;
            LFReference = AudioStatic.Reverb_LFReferenceDefault; 
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
    }
}