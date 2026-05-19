using System;
using BHSDK.Models.Enum;
using BHSDK.Models.Enum.Values;

namespace BHSDK.Models.Interfaces.Values
{
    public interface IScreenLimit : ICopyable<IScreenLimit>, IEquatable<IScreenLimit>
    {
        public ScreenLimitType GetModelType();
        public bool IsValid(float currentAspect);
        public float GetValid(float currentAspect);
    }
}