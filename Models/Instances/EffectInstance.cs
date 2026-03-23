using BHSDK.Models.Base;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Instances
{
    public class EffectInstance : Instance
    {
        // Core
        
        [JsonProperty("l")]
        public bool Loop { get; set; }
        
        [JsonProperty("c")]
        public int ParticleCount { get; set; }
        
        [JsonProperty("lb")]
        public IVector2 LifetimeBounds { get; set; }
        
        [JsonProperty("p")]
        public IVector2 ParticlePivot { get; set; }
        
        [JsonProperty("pti")]
        public int ParticleTextureIndex { get; set; }
        
        [JsonProperty("c")]
        public bool HasCollider { get; set; }
        
        // Forces
        
        [JsonProperty("gmn")]
        public IFloat StartGravityModifierMin { get; set; }
        
        [JsonProperty("gmx")]
        public IFloat StartGravityModifierMax { get; set; }
        
        [JsonProperty("vmn")]
        public IVector2 StartVelocityMin { get; set; }
        
        [JsonProperty("vmx")]
        public IVector2 StartVelocityMax { get; set; }
        
        [JsonProperty("avmn")]
        public IFloat StartAngularVelocityMin { get; set; }
        
        [JsonProperty("avmx")]
        public IFloat StartAngularVelocityMax { get; set; }
        
        [JsonProperty("lv")]
        public IVector2 LinearVelocity { get; set; }
        
        [JsonProperty("ov")]
        public IVector3 OrbitalVelocity { get; set; }
        
        [JsonProperty("oco")]
        public IVector3 OrbitalCenterOffset { get; set; }
        
        [JsonProperty("vs")]
        public IFloat VelocitySpeedModifier { get; set; }
        
        [JsonProperty("lf")]
        public IVector2 LinearForce { get; set; }
        
        // Types
        
        [JsonProperty("eshp")]
        public IEffectShape Shape { get; set; }
        
        [JsonProperty("eang")]
        public IEffectAngle Angle { get; set; }
        
        [JsonProperty("esca")]
        public IEffectScale Scale { get; set; }
        
        [JsonProperty("eclr")]
        public IEffectColor Color { get; set; }
        
        // [JsonProperty("pti")]
        // public int ParticleTextureIndex { get; set; }
    }
}