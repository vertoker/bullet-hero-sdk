using System;
using BH.SDK.Models.Enum.Values;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Values
{
    [RuleContainer]
    public class ScreenLimitFixed : IScreenLimit, IModel<ScreenLimitFixed>
    {
        [RuleNotNull]
        [JsonProperty(Names.Aspect)]
        public ScreenAspect Aspect { get; set; }
        
        public ScreenLimitType GetModelType() => ScreenLimitType.Fixed;
        public bool IsValid(float currentAspect) => BHSDKMath.Approximately(Aspect.GetAspect(), currentAspect);
        public float GetValid(float currentAspect) => Aspect.GetAspect();

        public ScreenLimitFixed()
        {
            Aspect = new ScreenAspect();
        }
        public ScreenLimitFixed(ScreenAspect aspect)
        {
            Aspect = aspect;
        }
        public void Reset()
        {
            Aspect.Reset();
        }

        public object Clone() => Copy();
        IScreenLimit ICopyable<IScreenLimit>.Copy() => new ScreenLimitFixed(Aspect.Copy());
        public ScreenLimitFixed Copy() => new(Aspect.Copy());
        
        public override bool Equals(object obj) => obj is ScreenLimitFixed value && Equals(value);
        public override int GetHashCode() => Aspect != null ? Aspect.GetHashCode() : 0;

        public bool Equals(IScreenLimit other) => other is ScreenLimitFixed value && Equals(value);
        public bool Equals(ScreenLimitFixed other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Aspect.Equals(other.Aspect);
            return result;
        }
    }
}