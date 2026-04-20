using Newtonsoft.Json;

namespace BHSDK.Models.AudioEffects
{
    public class AudioReverb : AudioEffect
    {
        [JsonProperty(ModelNames.DryLevel)]
        public float DryLevel { get; set; }
        
        [JsonProperty(ModelNames.Room)]
        public float Room { get; set; }
        
        [JsonProperty(ModelNames.RoomHF)]
        public float RoomHF { get; set; }
        
        [JsonProperty(ModelNames.RoomLF)]
        public float RoomLF { get; set; }
        
        [JsonProperty(ModelNames.DecayTime)]
        public float DecayTime { get; set; }
        
        [JsonProperty(ModelNames.DecayHFRatio)]
        public float DecayHFRatio { get; set; }
        
        [JsonProperty(ModelNames.Reflections)]
        public float Reflections { get; set; }
        
        [JsonProperty(ModelNames.ReflectDelay)]
        public float ReflectDelay { get; set; }
        
        [JsonProperty(ModelNames.Reverb)]
        public float Reverb { get; set; }
        
        [JsonProperty(ModelNames.ReverbDelay)]
        public float ReverbDelay { get; set; }
        
        [JsonProperty(ModelNames.Diffusion)]
        public float Diffusion { get; set; }
        
        [JsonProperty(ModelNames.Density)]
        public float Density { get; set; }
        
        [JsonProperty(ModelNames.HFReference)]
        public float HFReference { get; set; }
        
        [JsonProperty(ModelNames.LFReference)]
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