using System;
using BHSDK.Models.Enum.Effects;

namespace BHSDK.Models.Interfaces.Effects
{
    public interface IEffectColor : ICopyable<IEffectColor>, IEquatable<IEffectColor>
    {
        public EffectColorType GetModelType();
    }
}