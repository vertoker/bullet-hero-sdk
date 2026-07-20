using System;
using BH.SDK.Models.Enum.Effects;

namespace BH.SDK.Models.Interfaces.Effects
{
    public interface IEffectShapeSpread : IModel<IEffectShapeSpread>
    {
        public EffectShapeSpreadType GetModelType();
    }
}