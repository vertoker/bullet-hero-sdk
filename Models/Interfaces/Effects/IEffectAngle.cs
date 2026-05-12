using System;
using BHSDK.Models.Enum.Effects;

namespace BHSDK.Models.Interfaces.Effects
{
    public interface IEffectAngle : ICopyable<IEffectAngle>, IEquatable<IEffectAngle>
    {
        public EffectAngleType GetModelType();
    }
}