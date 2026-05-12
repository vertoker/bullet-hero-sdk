using System;
using BHSDK.Models.Enum.Effects;

namespace BHSDK.Models.Interfaces.Effects
{
    public interface IEffectScale : ICopyable<IEffectScale>, IEquatable<IEffectScale>
    {
        public EffectScaleType GetModelType();
    }
}