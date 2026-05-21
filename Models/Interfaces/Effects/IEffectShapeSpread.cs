using System;
using BH.SDK.Models.Enum.Effects;

namespace BH.SDK.Models.Interfaces.Effects
{
    public interface IEffectShapeSpread : ICopyable<IEffectShapeSpread>, IEquatable<IEffectShapeSpread>
    {
        public EffectShapeSpreadType GetModelType();
    }
}