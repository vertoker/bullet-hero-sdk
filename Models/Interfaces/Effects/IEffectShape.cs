using System;
using BHSDK.Models.Enum.Effects;

namespace BHSDK.Models.Interfaces.Effects
{
    public interface IEffectShape : ICopyable<IEffectShape>, IEquatable<IEffectShape>
    {
        public EffectShapeType GetModelType();
    }
}