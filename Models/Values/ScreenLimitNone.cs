using System;
using BH.SDK.Models.Enum.Values;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Rules.Attributes;

namespace BH.SDK.Models.Values
{
    [RuleContainer]
    public class ScreenLimitNone : IScreenLimit, IModel<ScreenLimitNone>
    {
        public ScreenLimitType GetModelType() => ScreenLimitType.None;
        public bool IsValid(float currentAspect) => true;
        public float GetValid(float currentAspect) => currentAspect;

        public void Reset() { }
        
        public object Clone() => Copy();
        IScreenLimit ICopyable<IScreenLimit>.Copy() => Copy();
        public ScreenLimitNone Copy() => new();

        // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
        public override int GetHashCode() => base.GetHashCode();
        public override bool Equals(object obj) => obj is ScreenLimitNone value && Equals(value);

        public bool Equals(IScreenLimit other) => other is not null && ReferenceEquals(this, other);
        public bool Equals(ScreenLimitNone other) => other is not null;
    }
}