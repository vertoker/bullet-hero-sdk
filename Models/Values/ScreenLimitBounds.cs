using BHSDK.Models.Enum;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Game;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class ScreenLimitBounds : IScreenLimit
    {
        [JsonProperty("mna")]
        public ScreenAspect MinAspect { get; set; }
        
        [JsonProperty("mxa")]
        public ScreenAspect MaxAspect { get; set; }
        
        public ScreenLimitType GetModelType() => ScreenLimitType.Bounds;
        public bool IsValid(float currentAspect)
        {
            var minAspect = MinAspect.Aspect;
            if (currentAspect < minAspect) return false;
            
            var maxAspect = MaxAspect.Aspect;
            if (currentAspect > maxAspect) return false;
            
            return true;
        }
        public float GetValid(float currentAspect)
        {
            var minAspect = MinAspect.Aspect;
            var maxAspect = MaxAspect.Aspect;
            return Mathf.Clamp(currentAspect, minAspect, maxAspect);
        }

        public ScreenLimitBounds()
        {
            MinAspect = new ScreenAspect();
            MaxAspect = new ScreenAspect();
        }
        public ScreenLimitBounds(ScreenAspect minAspect, ScreenAspect maxAspect)
        {
            MinAspect = minAspect;
            MaxAspect = maxAspect;
        }
    }
}