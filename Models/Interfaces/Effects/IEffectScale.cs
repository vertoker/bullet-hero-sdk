using System;
using BH.SDK.Models.Enum.Effects;

namespace BH.SDK.Models.Interfaces.Effects
{
    public interface IEffectScale : IModel<IEffectScale>
    {
        public EffectScaleType GetModelType();
    }
}