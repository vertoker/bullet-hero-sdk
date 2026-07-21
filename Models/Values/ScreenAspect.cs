using System;
using BH.SDK.Models.Interfaces;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Values
{
    [RuleContainer]
    public class ScreenAspect : IModel<ScreenAspect>
    {
        [RuleInRange(ValueRules.MinAspectWidth, ValueRules.MaxAspectWidth)]
        [JsonProperty(Names.WidthShort)]
        public int Width { get; set; }
        
        [RuleInRange(ValueRules.MinAspectHeight, ValueRules.MaxAspectHeight)]
        [JsonProperty(Names.HeightShort)]
        public int Height { get; set; }
        
        // TODO add vertical/horizontal metadata (for phones and special modes)
        
        public float GetAspect() => IsValid() ? Width / (float)Height : 0f;

        public bool IsValid() => Width != 0f && Height != 0f;

        public ScreenAspect()
        {
            Width = ValueRules.DefaultAspectWidth;
            Height = ValueRules.DefaultAspectHeight;
        }
        public ScreenAspect(int width, int height)
        {
            Width = width;
            Height = height;
        }
        public void Reset()
        {
            Width = ValueRules.DefaultAspectWidth;
            Height = ValueRules.DefaultAspectHeight;
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