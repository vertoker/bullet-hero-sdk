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
            DryLevel = 0f; // -10000f - 0f, 1f, mB
            Room = -10000f; // -10000f - 0f, 1f, mB
            RoomHF = 0f; // -10000f - 0f, 1f, mB
            RoomLF = 0f; // -10000f - 0f, 1f, mB
            DecayTime = 1f; // 0.1f - 20f, 0.1f, s
            DecayHFRatio = 0.5f; // 0.1f - 2f, 0.01f
            Reflections = -10000f; // -10000f - 1000f, 1f, mB
            ReflectDelay = 0.02f; // 0f - 0.3f, 0.01f, s
            Reverb = 0f; // -10000f - 2000f, 1f, mB
            ReverbDelay = 0.04f; // 0f - 0.1f, 0.01f, s
            Diffusion = 1f; // 0f - 1f, 0.01f, %
            Density = 1f; // 0f - 1f, 0.01f, %
            HFReference = 5000f; // 20f - 20000f, 1f, Hz
            LFReference = 250f; // 20f - 1000f, 1f, Hz 
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