using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Effects
{
    public class EffectObjectForces
    {
        [JsonProperty(Names.GravityMin)]
        public IFloat StartGravityMin { get; set; }
        
        [JsonProperty(Names.GravityMax)]
        public IFloat StartGravityMax { get; set; }
        
        [JsonProperty(Names.VelocityMin)]
        public IVector2 StartVelocityMin { get; set; }
        
        [JsonProperty(Names.VelocityMax)]
        public IVector2 StartVelocityMax { get; set; }
        
        [JsonProperty(Names.AngularVelocityMin)]
        public IFloat StartAngularVelocityMin { get; set; }
        
        [JsonProperty(Names.AngularVelocityMax)]
        public IFloat StartAngularVelocityMax { get; set; }
        
        [JsonProperty(Names.LinearVelocity)]
        public IVector2 LinearVelocity { get; set; }
        
        [JsonProperty(Names.OrbitalVelocity)]
        public IVector3 OrbitalVelocity { get; set; }
        
        [JsonProperty(Names.OrbitalCenterOffset)]
        public IVector3 OrbitalCenterOffset { get; set; }
        
        [JsonProperty(Names.VelocitySpeed)]
        public IFloat VelocitySpeed { get; set; }
        
        [JsonProperty(Names.LinearForce)]
        public IVector2 LinearForce { get; set; }

        public EffectObjectForces()
        {
            StartGravityMin = new FloatValue(EffectStatic.Forces_StartGravityMinDefault);
            StartGravityMax = new FloatValue(EffectStatic.Forces_StartGravityMaxDefault);
            StartVelocityMin = new Vector2Value(EffectStatic.Forces_StartVelocityMinDefault);
            StartVelocityMax = new Vector2Value(EffectStatic.Forces_StartVelocityMaxDefault);
            StartAngularVelocityMin = new FloatValue(EffectStatic.Forces_StartAngularVelocityMinDefault);
            StartAngularVelocityMax = new FloatValue(EffectStatic.Forces_StartAngularVelocityMaxDefault);
            LinearVelocity = new Vector2Value(EffectStatic.Forces_LinearVelocityDefault);
            OrbitalVelocity = new Vector3Value(EffectStatic.Forces_OrbitalVelocityDefault);
            OrbitalCenterOffset = new Vector3Value(EffectStatic.Forces_OrbitalCenterOffsetDefault);
            VelocitySpeed = new FloatValue(EffectStatic.Forces_VelocitySpeedDefault);
            LinearForce = new Vector2Value(EffectStatic.Forces_LinearForceDefault);
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