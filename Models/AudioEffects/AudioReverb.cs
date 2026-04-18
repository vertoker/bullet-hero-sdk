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
            DryLevel = AudioStatic.Reverb_DryLevel;
            Room = AudioStatic.Reverb_Room;
            RoomHF = AudioStatic.Reverb_RoomHF;
            RoomLF = AudioStatic.Reverb_RoomLF;
            DecayTime = AudioStatic.Reverb_DecayTime;
            DecayHFRatio = AudioStatic.Reverb_DecayHFRatio;
            Reflections = AudioStatic.Reverb_Reflections;
            ReflectDelay = AudioStatic.Reverb_ReflectDelay;
            Reverb = AudioStatic.Reverb_Reverb;
            ReverbDelay = AudioStatic.Reverb_ReverbDelay;
            Diffusion = AudioStatic.Reverb_Diffusion;
            Density = AudioStatic.Reverb_Density;
            HFReference = AudioStatic.Reverb_HFReference;
            LFReference = AudioStatic.Reverb_LFReference; 
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