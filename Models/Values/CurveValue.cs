using System;
using System.Collections.Generic;
using BHSDK.Models.Enum.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class CurveValue
    {
        public const int MaxCount = 4;
        
        [JsonProperty(Names.Keys)]
        public List<CurveKeyframeValue> KeyFrames { get; set; }
        
        [JsonProperty(Names.PreWrapMode)]
        public CurveWrapMode PreWrapMode { get; set; }
        
        [JsonProperty(Names.PostWrapMode)]
        public CurveWrapMode PostWrapMode { get; set; }
        
        public AnimationCurve Get()
        {
            Span<Keyframe> keyframes = stackalloc Keyframe[KeyFrames.Count];
            for (var i = 0; i < KeyFrames.Count; i++) keyframes[i] = KeyFrames[i].Get();
            
            var curve = new AnimationCurve();
            curve.SetKeys(keyframes);
            curve.preWrapMode = (WrapMode)PreWrapMode;
            curve.postWrapMode = (WrapMode)PostWrapMode;
            
            return curve;
        }

        public CurveValue()
        {
            KeyFrames = new List<CurveKeyframeValue>();
            PreWrapMode = CurveWrapMode.Default;
            PostWrapMode = CurveWrapMode.Default;
        }
        public CurveValue(List<CurveKeyframeValue> keyFrames, CurveWrapMode preWrapMode, CurveWrapMode postWrapMode)
        {
            KeyFrames = keyFrames;
            PreWrapMode = preWrapMode;
            PostWrapMode = postWrapMode;
        }
        public CurveValue(AnimationCurve curve)
        {
            KeyFrames = new List<CurveKeyframeValue>(curve.length);
            foreach (var keyframe in curve.keys)
                KeyFrames.Add(new CurveKeyframeValue(keyframe));
            PreWrapMode = (CurveWrapMode)curve.preWrapMode;
            PostWrapMode = (CurveWrapMode)curve.postWrapMode;
        }
    }
}