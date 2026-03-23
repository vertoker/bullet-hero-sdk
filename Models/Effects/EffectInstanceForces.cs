using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Effects
{
    public class EffectInstanceForces
    {
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

        public EffectInstanceForces()
        {
            StartGravityModifierMin = new FloatValue(0f);
            StartGravityModifierMax = new FloatValue(0f);
            StartVelocityMin = new Vector2Value(0f, 0f);
            StartVelocityMax = new Vector2Value(0f, 0f);
            StartAngularVelocityMin = new FloatValue(0f);
            StartAngularVelocityMax = new FloatValue(0f);
            LinearVelocity = new Vector2Value(0f, 0f);
            OrbitalVelocity = new Vector3Value(0f, 0f, 0f);
            OrbitalCenterOffset = new Vector3Value(0f, 0f, 0f);
            VelocitySpeedModifier = new FloatValue(1f);
            LinearForce = new Vector2Value(0f, 0f);
        }
        
        public EffectInstanceForces(float startGravityModifierMin, float startGravityModifierMax, 
            Vector2 startVelocityMin, Vector2 startVelocityMax, 
            float startAngularVelocityMin, float startAngularVelocityMax, 
            Vector2 linearVelocity, Vector3 orbitalVelocity, Vector3 orbitalCenterOffset, 
            float velocitySpeedModifier, Vector2 linearForce)
        {
            StartGravityModifierMin = new FloatValue(startGravityModifierMin);
            StartGravityModifierMax = new FloatValue(startGravityModifierMax);
            StartVelocityMin = new Vector2Value(startVelocityMin);
            StartVelocityMax = new Vector2Value(startVelocityMax);
            StartAngularVelocityMin = new FloatValue(startAngularVelocityMin);
            StartAngularVelocityMax = new FloatValue(startAngularVelocityMax);
            LinearVelocity = new Vector2Value(linearVelocity);
            OrbitalVelocity = new Vector3Value(orbitalVelocity);
            OrbitalCenterOffset = new Vector3Value(orbitalCenterOffset);
            VelocitySpeedModifier = new FloatValue(velocitySpeedModifier);
            LinearForce = new Vector2Value(linearForce);
        }
        
        public EffectInstanceForces(IFloat startGravityModifierMin, IFloat startGravityModifierMax, 
            IVector2 startVelocityMin, IVector2 startVelocityMax, 
            IFloat startAngularVelocityMin, IFloat startAngularVelocityMax, 
            IVector2 linearVelocity, IVector3 orbitalVelocity, IVector3 orbitalCenterOffset, 
            IFloat velocitySpeedModifier, IVector2 linearForce)
        {
            StartGravityModifierMin = startGravityModifierMin;
            StartGravityModifierMax = startGravityModifierMax;
            StartVelocityMin = startVelocityMin;
            StartVelocityMax = startVelocityMax;
            StartAngularVelocityMin = startAngularVelocityMin;
            StartAngularVelocityMax = startAngularVelocityMax;
            LinearVelocity = linearVelocity;
            OrbitalVelocity = orbitalVelocity;
            OrbitalCenterOffset = orbitalCenterOffset;
            VelocitySpeedModifier = velocitySpeedModifier;
            LinearForce = linearForce;
        }
    }
}