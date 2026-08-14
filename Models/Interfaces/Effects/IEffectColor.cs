using System;
using BH.SDK.Models.Enums.Effects;

namespace BH.SDK.Models.Interfaces.Effects
{
    public interface IEffectColor : IModel<IEffectColor>
    {
        public EffectColorType GetModelType();
    }
}