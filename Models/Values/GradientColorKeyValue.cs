using System;
using BHSDK.Models.Interfaces;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BHSDK.Models.Values
{
    [RuleContainer]
    public class GradientColorKeyValue : ICopyable<GradientColorKeyValue>, IEquatable<GradientColorKeyValue>
    {
        // TODO maybe replace FloatValue to IFloat (color too) (in editor step)
        
        [RuleNotNull]
        [JsonProperty(Names.Color)]
        public ColorValue ColorHDR { get; set; }
        
        [JsonProperty(Names.TimeShort)]
        public float Time { get; set; }
        
        public GradientColorKeyValue()
        {
            ColorHDR = ColorValue.white;
            Time = 0f;
        }
        public GradientColorKeyValue(ColorValue colorHDR, float time)
        {
            ColorHDR = colorHDR;
            Time = time;
        }

        public object Clone() => Copy();
        public GradientColorKeyValue Copy() => new(ColorHDR.Copy(), Time);

        public override bool Equals(object obj) => obj is GradientColorKeyValue value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(ColorHDR, Time);

        public bool Equals(GradientColorKeyValue other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = ColorHDR.Equals(other.ColorHDR)
                         && Time.Equals(other.Time);
            return result;
        }
    }
}