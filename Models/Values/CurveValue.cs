using System.Collections.Generic;
using System.Linq;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Values
{
    [RuleContainer]
    public class CurveValue : ICopyable<CurveValue>
    {
        public const int MaxCount = 4;
        
        [RuleNotNull, RuleCollectionMaxCount(MaxCount)]
        [JsonProperty(Names.Keys)]
        public List<CurveKeyframeValue> KeyFrames { get; set; }
        
        [JsonProperty(Names.PreWrapMode)]
        public CurveWrapMode PreWrapMode { get; set; }
        
        [JsonProperty(Names.PostWrapMode)]
        public CurveWrapMode PostWrapMode { get; set; }
        
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
        
        public object Clone() => Copy();
        public CurveValue Copy() => new(KeyFrames.Select(keyframe => keyframe.Copy()).ToList(), PostWrapMode, PreWrapMode);
    }
}