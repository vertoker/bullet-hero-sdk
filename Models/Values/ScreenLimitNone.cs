using BHSDK.Models.Enum;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;

namespace BHSDK.Models.Values
{
    public class ScreenLimitNone : IScreenLimit
    {
        public ScreenLimitType GetModelType() => ScreenLimitType.None;
        public bool IsValid(float currentAspect) => true;
        public float GetValid(float currentAspect) => currentAspect;
    }
}