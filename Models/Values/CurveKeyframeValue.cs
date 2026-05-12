using System;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BHSDK.Models.Values
{
    [RuleContainer]
    public class CurveKeyframeValue : ICopyable<CurveKeyframeValue>, IEquatable<CurveKeyframeValue>
    {
        // TODO maybe replace FloatValue to IFloat (in editor step)
        
        [JsonProperty(Names.TimeShort)]
        public float Time { get; set; }
        
        [JsonProperty(Names.ValueShort)]
        public float Value { get; set; }
        
        [JsonProperty(Names.WeightedMode)]
        public CurveWeightedMode WeightedMode { get; set; }
        
        [JsonProperty(Names.TangentMode)]
        public CurveTangentMode TangentMode { get; set; }
        
        
        [JsonProperty(Names.InTangent)]
        public float InTangent { get; set; }
        
        [JsonProperty(Names.OutTangent)]
        public float OutTangent { get; set; }
        
        [JsonProperty(Names.InWeight)]
        public float InWeight { get; set; }
        
        [JsonProperty(Names.OutWeight)]
        public float OutWeight { get; set; }
        
        public CurveKeyframeValue()
        {
            Time = 0f;
            Value = 0f;
            WeightedMode = CurveWeightedMode.None;
            TangentMode = CurveTangentMode.Free;
            InTangent = 0f;
            OutTangent = 0f;
            InWeight = 0f;
            OutWeight = 0f;
        }
        public CurveKeyframeValue(float time, float value)
        {
            Time = time;
            Value = value;
            WeightedMode = CurveWeightedMode.None;
            TangentMode = CurveTangentMode.Free;
            InTangent = 0f;
            OutTangent = 0f;
            InWeight = 0f;
            OutWeight = 0f;
        }
        public CurveKeyframeValue(float time, float value, 
            float inTangent, float outTangent, float inWeight, float outWeight)
        {
            Time = time;
            Value = value;
            WeightedMode = CurveWeightedMode.Both;
            TangentMode = CurveTangentMode.Free;
            InTangent = inTangent;
            OutTangent = outTangent;
            InWeight = inWeight;
            OutWeight = outWeight;
        }
        public CurveKeyframeValue(float time, float value,
            CurveWeightedMode weightedMode, CurveTangentMode tangentMode,
            float inTangent, float outTangent, float inWeight, float outWeight)
        {
            Time = time;
            Value = value;
            WeightedMode = weightedMode;
            TangentMode = tangentMode;
            InTangent = inTangent;
            OutTangent = outTangent;
            InWeight = inWeight;
            OutWeight = outWeight;
        }

        public object Clone() => Copy();
        public CurveKeyframeValue Copy() => new(Time, Value,
            WeightedMode, TangentMode, InTangent, OutTangent, InWeight, OutWeight);

        public override bool Equals(object obj) => obj is CurveKeyframeValue value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Time, Value,
            (int)WeightedMode, (int)TangentMode, InTangent, OutTangent, InWeight, OutWeight);

        public bool Equals(CurveKeyframeValue other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Time.Equals(other.Time)
                         && Value.Equals(other.Value)
                         && WeightedMode == other.WeightedMode
                         && TangentMode == other.TangentMode
                         && InTangent.Equals(other.InTangent)
                         && OutTangent.Equals(other.OutTangent)
                         && InWeight.Equals(other.InWeight)
                         && OutWeight.Equals(other.OutWeight);
            return result;
        }
    }
}