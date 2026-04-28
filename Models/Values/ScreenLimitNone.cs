using BHSDK.Models.Enum;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;

namespace BHSDK.Models.Values
{
    public class ScreenLimitNone : IScreenLimit, ICopyable<ScreenLimitNone>
    {
        public ScreenLimitType GetModelType() => ScreenLimitType.None;
        public bool IsValid(float currentAspect) => true;
        public float GetValid(float currentAspect) => currentAspect;
        
        IScreenLimit ICopyable<IScreenLimit>.Copy() => new ScreenLimitNone();
        public ScreenLimitNone Copy() => new();
    }
}