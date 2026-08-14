using System;
using BH.SDK.Models.Enums.Effects;

namespace BH.SDK.Models.Interfaces.Effects
{
    public interface IEffectShape : IModel<IEffectShape>
    {
        public EffectShapeType GetModelType();
    }
}