using System;
using BH.SDK.Models.Enum.Effects;

namespace BH.SDK.Models.Interfaces.Effects
{
    public interface IEffectShape : ICopyable<IEffectShape>, IEquatable<IEffectShape>
    {
        public EffectShapeType GetModelType();
    }
}