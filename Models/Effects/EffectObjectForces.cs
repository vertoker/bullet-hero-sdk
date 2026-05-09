using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Models.Values.Vectors;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.Effects
{
    public class EffectObjectForces : IUpdatable<EffectObjectForces>
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
            StartGravityMin = new FloatValue(EffectStatic.Forces_StartGravityMin_Default);
            StartGravityMax = new FloatValue(EffectStatic.Forces_StartGravityMax_Default);
            StartVelocityMin = new Vector2Value(
                EffectStatic.Forces_StartVelocityMin_X_Default,
                EffectStatic.Forces_StartVelocityMin_Y_Default);
            StartVelocityMax = new Vector2Value(
                EffectStatic.Forces_StartVelocityMax_X_Default,
                EffectStatic.Forces_StartVelocityMax_Y_Default);
            StartAngularVelocityMin = new FloatValue(EffectStatic.Forces_StartAngularVelocityMin_Default);
            StartAngularVelocityMax = new FloatValue(EffectStatic.Forces_StartAngularVelocityMax_Default);
            LinearVelocity = new Vector2Value(EffectStatic.Forces_LinearVelocity_X_Default,
                EffectStatic.Forces_LinearVelocity_Y_Default);
            OrbitalVelocity = new Vector3Value(
                EffectStatic.Forces_OrbitalVelocity_X_Default,
                EffectStatic.Forces_OrbitalVelocity_Y_Default,
                EffectStatic.Forces_OrbitalVelocity_Z_Default);
            OrbitalCenterOffset = new Vector3Value(
                EffectStatic.Forces_OrbitalCenterOffset_X_Default,
                EffectStatic.Forces_OrbitalCenterOffset_Y_Default,
                EffectStatic.Forces_OrbitalCenterOffset_Z_Default);
            VelocitySpeed = new FloatValue(EffectStatic.Forces_VelocitySpeed_Default);
            LinearForce = new Vector2Value(
                EffectStatic.Forces_LinearForce_X_Default,
                EffectStatic.Forces_LinearForce_Y_Default);
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

        public void Update(EffectObjectForces src)
        {
            StartGravityMax = src.StartGravityMax;
            StartGravityMin = src.StartGravityMin;
            StartVelocityMin = src.StartVelocityMin;
            StartVelocityMax = src.StartVelocityMax;
            StartAngularVelocityMin = src.StartAngularVelocityMin;
            StartAngularVelocityMax = src.StartAngularVelocityMax;
            LinearVelocity = src.LinearVelocity;
            OrbitalVelocity = src.OrbitalVelocity;
            OrbitalCenterOffset = src.OrbitalCenterOffset;
            VelocitySpeed = src.VelocitySpeed;
            LinearForce = src.LinearForce;
        }
    }
}