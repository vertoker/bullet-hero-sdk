using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class CurveValue
    {
        public const int MaxCount = 4;
        
        [JsonProperty(ModelNames.Keys)]
        public List<CurveKeyframeValue> KeyFrames { get; set; }
        
        public AnimationCurve Get()
        {
            Span<Keyframe> keyframes = stackalloc Keyframe[KeyFrames.Count];
            for (var i = 0; i < KeyFrames.Count; i++) keyframes[i] = KeyFrames[i].Get();
            
            var curve = new AnimationCurve();
            curve.SetKeys(keyframes);
            
            return curve;
        }

        public CurveValue()
        {
            KeyFrames = new List<CurveKeyframeValue>();
        }
        public CurveValue(List<CurveKeyframeValue> keyFrames)
        {
            KeyFrames = keyFrames;
        }
        public CurveValue(AnimationCurve curve)
        {
            KeyFrames = new List<CurveKeyframeValue>(curve.length);
            foreach (var keyframe in curve.keys)
                KeyFrames.Add(new CurveKeyframeValue(keyframe));
        }
    }
}