using BHSDK.Models.Enum;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Game;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class ScreenLimitFixed : IScreenLimit
    {
        [JsonProperty("a")]
        public ScreenAspect Aspect { get; set; }
        
        public ScreenLimitType Type => ScreenLimitType.Fixed;
        public bool IsValid(float currentAspect) => Mathf.Approximately(Aspect.Aspect, currentAspect);
        public float GetValid(float currentAspect) => Aspect.Aspect;
    }
}