using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Effects
{
    public class EffectInstanceForces
    {
        [JsonProperty(ModelNames.Gravity + ModelNames.Min)]
        public IFloat StartGravityModifierMin { get; set; }
        
        [JsonProperty(ModelNames.Gravity + ModelNames.Max)]
        public IFloat StartGravityModifierMax { get; set; }
        
        [JsonProperty(ModelNames.Velocity + ModelNames.Min)]
        public IVector2 StartVelocityMin { get; set; }
        
        [JsonProperty(ModelNames.Velocity + ModelNames.Max)]
        public IVector2 StartVelocityMax { get; set; }
        
        [JsonProperty(ModelNames.Angular + ModelNames.Velocity + ModelNames.Min)]
        public IFloat StartAngularVelocityMin { get; set; }
        
        [JsonProperty(ModelNames.Angular + ModelNames.Velocity + ModelNames.Max)]
        public IFloat StartAngularVelocityMax { get; set; }
        
        [JsonProperty(ModelNames.Linear + ModelNames.Velocity)]
        public IVector2 LinearVelocity { get; set; }
        
        [JsonProperty(ModelNames.Orbital + ModelNames.Velocity)]
        public IVector3 OrbitalVelocity { get; set; }
        
        [JsonProperty(ModelNames.Orbital + ModelNames.Center + ModelNames.Offset)]
        public IVector3 OrbitalCenterOffset { get; set; }
        
        [JsonProperty(ModelNames.Velocity + ModelNames.Speed)]
        public IFloat VelocitySpeedModifier { get; set; }
        
        [JsonProperty(ModelNames.Linear + ModelNames.Force)]
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