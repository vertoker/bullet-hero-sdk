using System;
using System.Collections.Generic;
using System.Linq;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BHSDK.Models.Values
{
    [RuleContainer]
    public class CurveValue : ICopyable<CurveValue>, IEquatable<CurveValue>
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
        public CurveValue Copy() => new(KeyFrames.CopyList(), PostWrapMode, PreWrapMode);

        public override bool Equals(object obj) => obj is CurveValue value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(KeyFrames.GetListHashCode(), (int)PreWrapMode, (int)PostWrapMode);

        public bool Equals(CurveValue other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = KeyFrames.ListEquals(other.KeyFrames)
                         && PreWrapMode == other.PreWrapMode
                         && PostWrapMode == other.PostWrapMode;
            return result;
        }
    }
}