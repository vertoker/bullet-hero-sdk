using System;
using BHSDK.Models.Enum;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Rules.Attributes;

namespace BHSDK.Models.Values
{
    [RuleContainer]
    public class ScreenLimitNone : IScreenLimit, ICopyable<ScreenLimitNone>, IEquatable<ScreenLimitNone>
    {
        public ScreenLimitType GetModelType() => ScreenLimitType.None;
        public bool IsValid(float currentAspect) => true;
        public float GetValid(float currentAspect) => currentAspect;

        public object Clone() => Copy();
        IScreenLimit ICopyable<IScreenLimit>.Copy() => Copy();
        public ScreenLimitNone Copy() => new();

        // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
        public override int GetHashCode() => base.GetHashCode();
        public override bool Equals(object obj) => obj is ScreenLimitNone value && Equals(value);

        public bool Equals(IScreenLimit other) => other is not null && ReferenceEquals(this, other);
        public bool Equals(ScreenLimitNone other) => other is not null && ReferenceEquals(this, other);
    }
}