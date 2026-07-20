using System;
using BH.SDK.Models.Enum.Values;
using BH.SDK.Models.Interfaces;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Values
{
    [RuleContainer]
    public class CurveKeyframeValue : IModel<CurveKeyframeValue>
    {
        // TODO maybe replace FloatValue to IFloat (in editor step)
        
        [RuleInRange(ValueRules.MinCurveTime, ValueRules.MaxCurveTime)]
        [JsonProperty(Names.TimeShort)]
        public float Time { get; set; }
        
        [RuleInRange(ValueRules.MinFloatValue, ValueRules.MaxFloatValue)]
        [JsonProperty(Names.ValueShort)]
        public float Value { get; set; }
        
        [JsonProperty(Names.WeightedMode)]
        public CurveWeightedMode WeightedMode { get; set; }
        
        [JsonProperty(Names.TangentMode)]
        public CurveTangentMode TangentMode { get; set; }
        
        
        [RuleInRange(ValueRules.MinFloatValue, ValueRules.MaxFloatValue)]
        [JsonProperty(Names.InTangent)]
        public float InTangent { get; set; }
        
        [RuleInRange(ValueRules.MinFloatValue, ValueRules.MaxFloatValue)]
        [JsonProperty(Names.OutTangent)]
        public float OutTangent { get; set; }
        
        [RuleInRange(ValueRules.MinFloatValue, ValueRules.MaxFloatValue)]
        [JsonProperty(Names.InWeight)]
        public float InWeight { get; set; }
        
        [RuleInRange(ValueRules.MinFloatValue, ValueRules.MaxFloatValue)]
        [JsonProperty(Names.OutWeight)]
        public float OutWeight { get; set; }
        
        public CurveKeyframeValue()
        {
            Time = ValueRules.FloatZero;
            Value = ValueRules.FloatZero;
            WeightedMode = CurveWeightedMode.None;
            TangentMode = CurveTangentMode.Free;
            InTangent = ValueRules.FloatZero;
            OutTangent = ValueRules.FloatZero;
            InWeight = ValueRules.FloatZero;
            OutWeight = ValueRules.FloatZero;
        }
        public CurveKeyframeValue(float time, float value)
        {
            Time = time;
            Value = value;
            WeightedMode = CurveWeightedMode.None;
            TangentMode = CurveTangentMode.Free;
            InTangent = ValueRules.FloatZero;
            OutTangent = ValueRules.FloatZero;
            InWeight = ValueRules.FloatZero;
            OutWeight = ValueRules.FloatZero;
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
        public void Reset()
        {
            Time = ValueRules.FloatZero;
            Value = ValueRules.FloatZero;
            WeightedMode = CurveWeightedMode.None;
            TangentMode = CurveTangentMode.Free;
            InTangent = ValueRules.FloatZero;
            OutTangent = ValueRules.FloatZero;
            InWeight = ValueRules.FloatZero;
            OutWeight = ValueRules.FloatZero;
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