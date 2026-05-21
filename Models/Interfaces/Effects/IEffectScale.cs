using System;
using BH.SDK.Models.Enum.Effects;

namespace BH.SDK.Models.Interfaces.Effects
{
    public interface IEffectScale : ICopyable<IEffectScale>, IEquatable<IEffectScale>
    {
        public EffectScaleType GetModelType();
    }
}