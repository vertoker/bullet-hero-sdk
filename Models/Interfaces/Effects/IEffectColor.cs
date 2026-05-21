using System;
using BH.SDK.Models.Enum.Effects;

namespace BH.SDK.Models.Interfaces.Effects
{
    public interface IEffectColor : ICopyable<IEffectColor>, IEquatable<IEffectColor>
    {
        public EffectColorType GetModelType();
    }
}