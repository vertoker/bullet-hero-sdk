using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Effects
{
    public class EffectObjectForces
    {
        [JsonProperty(ModelNames.Gravity + ModelNames.Min)]
        public IFloat StartGravityMin { get; set; }
        
        [JsonProperty(ModelNames.Gravity + ModelNames.Max)]
        public IFloat StartGravityMax { get; set; }
        
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
        public IFloat VelocitySpeed { get; set; }
        
        [JsonProperty(ModelNames.Linear + ModelNames.Force)]
        public IVector2 LinearForce { get; set; }

        public EffectObjectForces()
        {
            StartGravityMin = new FloatValue(EffectStatic.StartGravityMinDefault);
            StartGravityMax = new FloatValue(EffectStatic.StartGravityMaxDefault);
            StartVelocityMin = new Vector2Value(EffectStatic.StartVelocityMinDefault);
            StartVelocityMax = new Vector2Value(EffectStatic.StartVelocityMaxDefault);
            StartAngularVelocityMin = new FloatValue(EffectStatic.StartAngularVelocityMinDefault);
            StartAngularVelocityMax = new FloatValue(EffectStatic.StartAngularVelocityMaxDefault);
            LinearVelocity = new Vector2Value(EffectStatic.LinearVelocityDefault);
            OrbitalVelocity = new Vector3Value(EffectStatic.OrbitalVelocityDefault);
            OrbitalCenterOffset = new Vector3Value(EffectStatic.OrbitalCenterOffsetDefault);
            VelocitySpeed = new FloatValue(EffectStatic.VelocitySpeedDefault);
            LinearForce = new Vector2Value(EffectStatic.LinearForceDefault);
        }
        
        public EffectObjectForces(float startGravityMin, float startGravityMax, 
            Vector2 startVelocityMin, Vector2 startVelocityMax, 
            float startAngularVelocityMin, float startAngularVelocityMax, 
            Vector2 linearVelocity, Vector3 orbitalVelocity, Vector3 orbitalCenterOffset, 
            float velocitySpeed, Vector2 linearForce)
        {
            StartGravityMin = new FloatValue(startGravityMin);
            StartGravityMax = new FloatValue(startGravityMax);
            StartVelocityMin = new Vector2Value(startVelocityMin);
            StartVelocityMax = new Vector2Value(startVelocityMax);
            StartAngularVelocityMin = new FloatValue(startAngularVelocityMin);
            StartAngularVelocityMax = new FloatValue(startAngularVelocityMax);
            LinearVelocity = new Vector2Value(linearVelocity);
            OrbitalVelocity = new Vector3Value(orbitalVelocity);
            OrbitalCenterOffset = new Vector3Value(orbitalCenterOffset);
            VelocitySpeed = new FloatValue(velocitySpeed);
            LinearForce = new Vector2Value(linearForce);
        }
        
        public EffectObjectForces(IFloat startGravityMin, IFloat startGravityMax, 
            IVector2 startVelocityMin, IVector2 startVelocityMax, 
            IFloat startAngularVelocityMin, IFloat startAngularVelocityMax, 
            IVector2 linearVelocity, IVector3 orbitalVelocity, IVector3 orbitalCenterOffset, 
            IFloat velocitySpeed, IVector2 linearForce)
        {
            StartGravityMin = startGravityMin;
            StartGravityMax = startGravityMax;
            StartVelocityMin = startVelocityMin;
            StartVelocityMax = startVelocityMax;
            StartAngularVelocityMin = startAngularVelocityMin;
            StartAngularVelocityMax = startAngularVelocityMax;
            LinearVelocity = linearVelocity;
            OrbitalVelocity = orbitalVelocity;
            OrbitalCenterOffset = orbitalCenterOffset;
            VelocitySpeed = velocitySpeed;
            LinearForce = linearForce;
        }
    }
}