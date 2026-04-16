using BHSDK.Models.Enum.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class CurveKeyframeValue
    {
        // TODO maybe replace FloatValue to IFloat (in editor step)
        
        [JsonProperty(ModelNames.Time)]
        public FloatValue Time { get; set; }
        
        [JsonProperty(ModelNames.Value)]
        public FloatValue Value { get; set; }
        
        [JsonProperty(ModelNames.Weight + ModelNames.Mode)]
        public CurveWeightedMode WeightedMode { get; set; }
        
        [JsonProperty(ModelNames.Tangent + ModelNames.Mode)]
        public CurveTangentMode TangentMode { get; set; }
        
        
        [JsonProperty(ModelNames.In + ModelNames.Tangent)]
        public FloatValue InTangent { get; set; }
        
        [JsonProperty(ModelNames.Out + ModelNames.Tangent)]
        public FloatValue OutTangent { get; set; }
        
        [JsonProperty(ModelNames.In + ModelNames.Weight)]
        public FloatValue InWeight { get; set; }
        
        [JsonProperty(ModelNames.Out + ModelNames.Weight)]
        public FloatValue OutWeight { get; set; }
        
        public Keyframe Get()
        {
            return new Keyframe(Time.Get(), Value.Get(), 
                InTangent.Get(), OutTangent.Get(), InWeight.Get(), OutWeight.Get());
        }

        public CurveKeyframeValue()
        {
            Time = new FloatValue(0f);
            Value = new FloatValue(0f);
            WeightedMode = CurveWeightedMode.None;
            TangentMode = CurveTangentMode.Free;
            InTangent = new FloatValue(0f);
            OutTangent = new FloatValue(0f);
            InWeight = new FloatValue(0f);
            OutWeight = new FloatValue(0f);
        }
        public CurveKeyframeValue(float time, float value)
        {
            Time = new FloatValue(time);
            Value = new FloatValue(value);
            WeightedMode = CurveWeightedMode.None;
            TangentMode = CurveTangentMode.Free;
            InTangent = new FloatValue(0f);
            OutTangent = new FloatValue(0f);
            InWeight = new FloatValue(0f);
            OutWeight = new FloatValue(0f);
        }
        public CurveKeyframeValue(FloatValue time, FloatValue value)
        {
            Time = time;
            Value = value;
            WeightedMode = CurveWeightedMode.None;
            TangentMode = CurveTangentMode.Free;
            InTangent = new FloatValue(0f);
            OutTangent = new FloatValue(0f);
            InWeight = new FloatValue(0f);
            OutWeight = new FloatValue(0f);
        }
        public CurveKeyframeValue(float time, float value, 
            float inTangent, float outTangent, float inWeight, float outWeight)
        {
            Time = new FloatValue(time);
            Value = new FloatValue(value);
            WeightedMode = CurveWeightedMode.Both;
            TangentMode = CurveTangentMode.Free;
            InTangent = new FloatValue(inTangent);
            OutTangent = new FloatValue(outTangent);
            InWeight = new FloatValue(inWeight);
            OutWeight = new FloatValue(outWeight);
        }
        public CurveKeyframeValue(FloatValue time, FloatValue value,
            FloatValue inTangent, FloatValue outTangent, FloatValue inWeight, FloatValue outWeight)
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
        public CurveKeyframeValue(Keyframe keyframe)
        {
            Time = new FloatValue(keyframe.time);
            Value = new FloatValue(keyframe.value);
            WeightedMode = (CurveWeightedMode)keyframe.weightedMode;
            TangentMode = CurveTangentMode.Free;
            InTangent = new FloatValue(keyframe.inTangent);
            OutTangent = new FloatValue(keyframe.outTangent);
            InWeight = new FloatValue(keyframe.inWeight);
            OutWeight = new FloatValue(keyframe.outWeight);
        }
    }
}