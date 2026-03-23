using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class CurveKeyframeValue
    {
        [JsonProperty("t")]
        public IFloat Time { get; set; }
        
        [JsonProperty("v")]
        public IFloat Value { get; set; }
        
        [JsonProperty("it")]
        public IFloat InTangent { get; set; }
        
        [JsonProperty("ot")]
        public IFloat OutTangent { get; set; }
        
        [JsonProperty("iw")]
        public IFloat InWeight { get; set; }
        
        [JsonProperty("ow")]
        public IFloat OutWeight { get; set; }
        
        public Keyframe Get()
        {
            return new Keyframe(Time.Get(), Value.Get(), 
                InTangent.Get(), OutTangent.Get(), InWeight.Get(), OutWeight.Get());
        }

        public CurveKeyframeValue()
        {
            Time = new FloatValue(0f);
            Value = new FloatValue(0f);
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
            InTangent = new FloatValue(inTangent);
            OutTangent = new FloatValue(outTangent);
            InWeight = new FloatValue(inWeight);
            OutWeight = new FloatValue(outWeight);
        }
        public CurveKeyframeValue(IFloat time, IFloat value,
            IFloat inTangent, IFloat outTangent, IFloat inWeight, IFloat outWeight)
        {
            Time = time;
            Value = value;
            InTangent = inTangent;
            OutTangent = outTangent;
            InWeight = inWeight;
            OutWeight = outWeight;
        }
        public CurveKeyframeValue(Keyframe keyframe)
        {
            Time = new FloatValue(keyframe.time);
            Value = new FloatValue(keyframe.value);
            InTangent = new FloatValue(keyframe.inTangent);
            OutTangent = new FloatValue(keyframe.outTangent);
            InWeight = new FloatValue(keyframe.inWeight);
            OutWeight = new FloatValue(keyframe.outWeight);
        }
    }
}