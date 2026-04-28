using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class CurveKeyframeValue : ICopyable<CurveKeyframeValue>
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
        
        public Keyframe Get()
        {
            var keyframe = new Keyframe(Time, Value, 
                InTangent, OutTangent, InWeight, OutWeight)
            {
                weightedMode = (WeightedMode)WeightedMode,
#pragma warning disable CS0618 // Type or member is obsolete
                tangentMode = (int)TangentMode,
#pragma warning restore CS0618 // Type or member is obsolete
            };
            return keyframe;
        }

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
        public CurveKeyframeValue(Keyframe keyframe)
        {
            Time = keyframe.time;
            Value = keyframe.value;
            WeightedMode = (CurveWeightedMode)keyframe.weightedMode;
            TangentMode = CurveTangentMode.Free;
            InTangent = keyframe.inTangent;
            OutTangent = keyframe.outTangent;
            InWeight = keyframe.inWeight;
            OutWeight = keyframe.outWeight;
        }

        public CurveKeyframeValue Copy() => new(Time, Value,
            WeightedMode, TangentMode, InTangent, OutTangent, InWeight, OutWeight);
    }
}