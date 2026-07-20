using System;
using BH.SDK.Models.Enum.Effects;

namespace BH.SDK.Models.Interfaces.Effects
{
    public interface IEffectShape : IModel<IEffectShape>
    {
        public EffectShapeType GetModelType();
    }
}