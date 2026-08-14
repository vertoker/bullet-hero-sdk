using System;
using BH.SDK.Models.Enums.Effects;

namespace BH.SDK.Models.Interfaces.Effects
{
    public interface IEffectScale : IModel<IEffectScale>
    {
        public EffectScaleType GetModelType();
    }
}