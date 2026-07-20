using System;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Effects
{
    [RuleContainer]
    public class EffectObjectForces : IModel<EffectObjectForces>, IUpdatable<EffectObjectForces>
    {
        [RuleNotNull]
        [JsonProperty(Names.GravityMin)]
        public IFloat StartGravityMin { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.GravityMax)]
        public IFloat StartGravityMax { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.VelocityMin)]
        public IVector2 StartVelocityMin { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.VelocityMax)]
        public IVector2 StartVelocityMax { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.AngularVelocityMin)]
        public IFloat StartAngularVelocityMin { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.AngularVelocityMax)]
        public IFloat StartAngularVelocityMax { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.LinearVelocity)]
        public IVector2 LinearVelocity { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.OrbitalVelocity)]
        public IVector3 OrbitalVelocity { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.OrbitalCenterOffset)]
        public IVector3 OrbitalCenterOffset { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.VelocitySpeed)]
        public IFloat VelocitySpeed { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.LinearForce)]
        public IVector2 LinearForce { get; set; }

        public EffectObjectForces()
        {
            StartGravityMin = new FloatValue(EffectRules.Forces.StartGravityMin_Default);
            StartGravityMax = new FloatValue(EffectRules.Forces.StartGravityMax_Default);
            StartVelocityMin = new Vector2Value(
                EffectRules.Forces.StartVelocityMin_X_Default,
                EffectRules.Forces.StartVelocityMin_Y_Default);
            StartVelocityMax = new Vector2Value(
                EffectRules.Forces.StartVelocityMax_X_Default,
                EffectRules.Forces.StartVelocityMax_Y_Default);
            StartAngularVelocityMin = new FloatValue(EffectRules.Forces.StartAngularVelocityMin_Default);
            StartAngularVelocityMax = new FloatValue(EffectRules.Forces.StartAngularVelocityMax_Default);
            LinearVelocity = new Vector2Value(EffectRules.Forces.LinearVelocity_X_Default,
                EffectRules.Forces.LinearVelocity_Y_Default);
            OrbitalVelocity = new Vector3Value(
                EffectRules.Forces.OrbitalVelocity_X_Default,
                EffectRules.Forces.OrbitalVelocity_Y_Default,
                EffectRules.Forces.OrbitalVelocity_Z_Default);
            OrbitalCenterOffset = new Vector3Value(
                EffectRules.Forces.OrbitalCenterOffset_X_Default,
                EffectRules.Forces.OrbitalCenterOffset_Y_Default,
                EffectRules.Forces.OrbitalCenterOffset_Z_Default);
            VelocitySpeed = new FloatValue(EffectRules.Forces.VelocitySpeed_Default);
            LinearForce = new Vector2Value(
                EffectRules.Forces.LinearForce_X_Default,
                EffectRules.Forces.LinearForce_Y_Default);
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
        public void Reset()
        {
            StartGravityMin = new FloatValue(EffectRules.Forces.StartGravityMin_Default);
            StartGravityMax = new FloatValue(EffectRules.Forces.StartGravityMax_Default);
            StartVelocityMin = new Vector2Value(
                EffectRules.Forces.StartVelocityMin_X_Default,
                EffectRules.Forces.StartVelocityMin_Y_Default);
            StartVelocityMax = new Vector2Value(
                EffectRules.Forces.StartVelocityMax_X_Default,
                EffectRules.Forces.StartVelocityMax_Y_Default);
            StartAngularVelocityMin = new FloatValue(EffectRules.Forces.StartAngularVelocityMin_Default);
            StartAngularVelocityMax = new FloatValue(EffectRules.Forces.StartAngularVelocityMax_Default);
            LinearVelocity = new Vector2Value(EffectRules.Forces.LinearVelocity_X_Default,
                EffectRules.Forces.LinearVelocity_Y_Default);
            OrbitalVelocity = new Vector3Value(
                EffectRules.Forces.OrbitalVelocity_X_Default,
                EffectRules.Forces.OrbitalVelocity_Y_Default,
                EffectRules.Forces.OrbitalVelocity_Z_Default);
            OrbitalCenterOffset = new Vector3Value(
                EffectRules.Forces.OrbitalCenterOffset_X_Default,
                EffectRules.Forces.OrbitalCenterOffset_Y_Default,
                EffectRules.Forces.OrbitalCenterOffset_Z_Default);
            VelocitySpeed = new FloatValue(EffectRules.Forces.VelocitySpeed_Default);
            LinearForce = new Vector2Value(
                EffectRules.Forces.LinearForce_X_Default,
                EffectRules.Forces.LinearForce_Y_Default);
        }

        public object Clone() => Copy();
        public EffectObjectForces Copy() => new(StartGravityMin.Copy(), StartGravityMax.Copy(), StartVelocityMin.Copy(),
            StartVelocityMax.Copy(), StartAngularVelocityMin.Copy(), StartAngularVelocityMax.Copy(), LinearVelocity.Copy(),
            OrbitalVelocity.Copy(), OrbitalCenterOffset.Copy(), VelocitySpeed.Copy(), LinearForce.Copy());

        public void Update(EffectObjectForces src)
        {
            StartGravityMin = src.StartGravityMin;
            StartGravityMax = src.StartGravityMax;
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

        public override bool Equals(object obj) => obj is EffectObjectForces value && Equals(value);
        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(StartGravityMin);
            hashCode.Add(StartGravityMax);
            hashCode.Add(StartVelocityMin);
            hashCode.Add(StartVelocityMax);
            hashCode.Add(StartAngularVelocityMin);
            hashCode.Add(StartAngularVelocityMax);
            hashCode.Add(LinearVelocity);
            hashCode.Add(OrbitalVelocity);
            hashCode.Add(OrbitalCenterOffset);
            hashCode.Add(VelocitySpeed);
            hashCode.Add(LinearForce);
            return hashCode.ToHashCode();
        }

        public bool Equals(EffectObjectForces other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = StartGravityMin.Equals(other.StartGravityMin)
                         && StartGravityMax.Equals(other.StartGravityMax)
                         && StartVelocityMin.Equals(other.StartVelocityMin)
                         && StartVelocityMax.Equals(other.StartVelocityMax)
                         && StartAngularVelocityMin.Equals(other.StartAngularVelocityMin)
                         && StartAngularVelocityMax.Equals(other.StartAngularVelocityMax)
                         && LinearVelocity.Equals(other.LinearVelocity)
                         && OrbitalVelocity.Equals(other.OrbitalVelocity)
                         && OrbitalCenterOffset.Equals(other.OrbitalCenterOffset)
                         && VelocitySpeed.Equals(other.VelocitySpeed)
                         && LinearForce.Equals(other.LinearForce);
            return result;
        }
    }
}