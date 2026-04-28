using BHSDK.Models.Enum;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Game;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class ScreenLimitFixed : IScreenLimit, ICopyable<ScreenLimitFixed>
    {
        [JsonProperty(Names.Aspect)]
        public ScreenAspect Aspect { get; set; }
        
        public ScreenLimitType GetModelType() => ScreenLimitType.Fixed;
        public bool IsValid(float currentAspect) => Mathf.Approximately(Aspect.GetAspect(), currentAspect);
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