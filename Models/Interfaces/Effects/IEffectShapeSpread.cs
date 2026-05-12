using System;
using BHSDK.Models.Enum.Effects;

namespace BHSDK.Models.Interfaces.Effects
{
    public interface IEffectShapeSpread : ICopyable<IEffectShapeSpread>, IEquatable<IEffectShapeSpread>
    {
        public EffectShapeSpreadType GetModelType();
    }
}