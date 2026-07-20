using System;
using BH.SDK.Models.Enum.Effects;

namespace BH.SDK.Models.Interfaces.Effects
{
    public interface IEffectAngle : IModel<IEffectAngle>
    {
        public EffectAngleType GetModelType();
    }
}