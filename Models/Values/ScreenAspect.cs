using System;
using BHSDK.Models.Interfaces;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BHSDK.Models.Values
{
    [RuleContainer]
    public class ScreenAspect : ICopyable<ScreenAspect>, IEquatable<ScreenAspect>
    {
        [RuleMin(1)]
        [JsonProperty(Names.WidthShort)]
        public int Width { get; set; }
        
        [RuleMin(1)]
        [JsonProperty(Names.HeightShort)]
        public int Height { get; set; }
        
        // TODO add vertical/horizontal metadata (for phones and special modes)
        
        public float GetAspect() => Width / (float)Height;

        public ScreenAspect()
        {
            Width = 16;
            Height = 9;
        }
        public ScreenAspect(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public object Clone() => Copy();
        public ScreenAspect Copy() => new(Width, Height);

        public override bool Equals(object obj) => obj is ScreenAspect value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Width, Height);

        public bool Equals(ScreenAspect other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Width.Equals(other.Width)
                         && Height.Equals(other.Height);
            return result;
        }
    }
}