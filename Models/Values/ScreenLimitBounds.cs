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
    public class ScreenLimitBounds : IScreenLimit, ICopyable<ScreenLimitBounds>, IEquatable<ScreenLimitBounds>
    {
        [RuleNotNull]
        [JsonProperty(Names.MinAspect)]
        public ScreenAspect MinAspect { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.MaxAspect)]
        public ScreenAspect MaxAspect { get; set; }
        
        public ScreenLimitType GetModelType() => ScreenLimitType.Bounds;
        public bool IsValid(float currentAspect)
        {
            var minAspect = MinAspect.GetAspect();
            if (currentAspect < minAspect) return false;
            
            var maxAspect = MaxAspect.GetAspect();
            if (currentAspect > maxAspect) return false;
            
            return true;
        }
        public float GetValid(float currentAspect)
        {
            var minAspect = MinAspect.GetAspect();
            var maxAspect = MaxAspect.GetAspect();
            return MathUtils.Clamp(currentAspect, minAspect, maxAspect);
        }

        public ScreenLimitBounds()
        {
            MinAspect = new ScreenAspect();
            MaxAspect = new ScreenAspect();
        }
        public ScreenLimitBounds(ScreenAspect minAspect, ScreenAspect maxAspect)
        {
            MinAspect = minAspect;
            MaxAspect = maxAspect;
        }

        public object Clone() => Copy();
        IScreenLimit ICopyable<IScreenLimit>.Copy() => new ScreenLimitBounds(MinAspect.Copy(), MaxAspect.Copy());
        public ScreenLimitBounds Copy() => new(MinAspect.Copy(), MaxAspect.Copy());

        public override bool Equals(object obj) => obj is ScreenLimitBounds value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(MinAspect, MaxAspect);

        public bool Equals(IScreenLimit other) => other is ScreenLimitBounds value && Equals(value);
        public bool Equals(ScreenLimitBounds other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = MinAspect.Equals(other.MinAspect)
                         && MaxAspect.Equals(other.MaxAspect);
            return result;
        }
    }
}