using System;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Interfaces;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Keyframes
{
    [RuleContainer]
    public class ShakeKey : Keyframe, IModel<ShakeKey>
    {
        [RuleInRange(ValueRules.MinFloatValue, ValueRules.MaxFloatValue)]
        [JsonProperty(Names.Intensity)]
        public float Intensity { get; set; }
        
        [RuleInRange(ValueRules.MinFloatValue, ValueRules.MaxFloatValue)]
        [JsonProperty(Names.Speed)]
        public float Speed { get; set; }
        
        [RuleInRange(ValueRules.MinFloatValue, ValueRules.MaxFloatValue)]
        [JsonProperty(Names.CoordX)]
        public float IntensityX { get; set; }
        
        [RuleInRange(ValueRules.MinFloatValue, ValueRules.MaxFloatValue)]
        [JsonProperty(Names.CoordY)]
        public float IntensityY { get; set; }

        public ShakeKey()
        {
            Intensity = 1f;
            Speed = 1f;
            IntensityX = 1f;
            IntensityY = 1f;
        }
        public ShakeKey(float intensity, float speed,
            int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Intensity = intensity;
            Speed = speed;
            IntensityX = 1f;
            IntensityY = 1f;
        }
        public ShakeKey(float intensity, float speed, float intensityX, float intensityY,
            int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Intensity = intensity;
            Speed = speed;
            IntensityX = intensityX;
            IntensityY = intensityY;
        }
        public override void Reset()
        {
            base.Reset();
            Intensity = 1f;
            Speed = 1f;
            IntensityX = 1f;
            IntensityY = 1f;
        }
        
        public override object Clone() => CopyImpl();
        public override Keyframe Copy() => CopyImpl();
        ShakeKey ICopyable<ShakeKey>.Copy() => CopyImpl();
        
        private ShakeKey CopyImpl() => new(Intensity, Speed, IntensityX, IntensityY, Frame, Ease);

        public override bool Equals(object obj) => obj is ShakeKey value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(),
            Intensity, Speed, IntensityX, IntensityY);

        public bool Equals(ShakeKey other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && Intensity.Equals(other.Intensity)
                         && Speed.Equals(other.Speed)
                         && IntensityX.Equals(other.IntensityX)
                         && IntensityY.Equals(other.IntensityY);
            return result;
        }
    }
}