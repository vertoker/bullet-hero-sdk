using BHSDK.Models.Enum;
using BHSDK.Models.Enum.Values;

namespace BHSDK.Models.Interfaces.Values
{
    public interface IScreenLimit
    {
        public ScreenLimitType Type { get; }
        public bool IsValid(float currentAspect);
        public float GetValid(float currentAspect);
    }
}