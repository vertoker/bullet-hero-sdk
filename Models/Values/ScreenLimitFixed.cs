using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.Values
{
    [RuleContainer]
    public class ScreenLimitFixed : IScreenLimit, ICopyable<ScreenLimitFixed>
    {
        [RuleNotNull]
        [JsonProperty(Names.Aspect)]
        public ScreenAspect Aspect { get; set; }
        
        public ScreenLimitType GetModelType() => ScreenLimitType.Fixed;
        public bool IsValid(float currentAspect) => MathUtils.Approximately(Aspect.GetAspect(), currentAspect);
        public float GetValid(float currentAspect) => Aspect.GetAspect();

        public ScreenLimitFixed()
        {
            Aspect = new ScreenAspect();
        }
        public ScreenLimitFixed(ScreenAspect aspect)
        {
            Aspect = aspect;
        }

        IScreenLimit ICopyable<IScreenLimit>.Copy() => new ScreenLimitFixed(Aspect.Copy());
        public ScreenLimitFixed Copy() => new(Aspect.Copy());
    }
}