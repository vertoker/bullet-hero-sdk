using System;
using BH.SDK.Models.Enum.Values;

namespace BH.SDK.Models.Interfaces.Values
{
    public interface IScreenLimit : IModel<IScreenLimit>
    {
        public ScreenLimitType GetModelType();
        public bool IsValid(float currentAspect);
        public float GetValid(float currentAspect);
    }
}