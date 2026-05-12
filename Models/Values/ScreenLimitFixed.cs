using System;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BHSDK.Models.Values
{
    [RuleContainer]
    public class ScreenLimitFixed : IScreenLimit, ICopyable<ScreenLimitFixed>, IEquatable<ScreenLimitFixed>
    {
        [RuleNotNull]
        [JsonProperty(Names.Aspect)]
        public ScreenAspect Aspect { get; set; }
        
        public ScreenLimitType GetModelType() => ScreenLimitType.Fixed;
        public bool IsValid(float currentAspect) => MathUtils.Approximately(Aspect.GetAspect(), currentAspect);
        public float GetValid(float currentAspect) => Aspect.GetAspect();

        public ScreenLimitFixed()
        {
            Aspect = new ScreenAspect();
        }
        public ScreenLimitFixed(ScreenAspect aspect)
        {
            Aspect = aspect;
        }

        public object Clone() => Copy();
        IScreenLimit ICopyable<IScreenLimit>.Copy() => new ScreenLimitFixed(Aspect.Copy());
        public ScreenLimitFixed Copy() => new(Aspect.Copy());

        public override bool Equals(object obj) => obj is ScreenLimitFixed value && Equals(value);
        public override int GetHashCode() => Aspect != null ? Aspect.GetHashCode() : 0;

        public bool Equals(ScreenLimitFixed other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Aspect.Equals(other.Aspect);
            return result;
        }
    }
}