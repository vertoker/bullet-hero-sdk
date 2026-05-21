using System;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Interfaces;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Keyframes
{
    [RuleContainer]
    public class Shake : Keyframe, ICopyable<Shake>, IEquatable<Shake>
    {
        [JsonProperty(Names.Intensity)]
        public float Intensity { get; set; }
        
        [JsonProperty(Names.Speed)]
        public float Speed { get; set; }
        
        [JsonProperty(Names.CoordX)]
        public float IntensityX { get; set; }
        
        [JsonProperty(Names.CoordY)]
        public float IntensityY { get; set; }

        public Shake()
        {
            Intensity = 1f;
            Speed = 1f;
            IntensityX = 1f;
            IntensityY = 1f;
        }
        public Shake(float intensity, float speed,
            int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Intensity = intensity;
            Speed = speed;
            IntensityX = 1f;
            IntensityY = 1f;
        }
        public Shake(float intensity, float speed, float intensityX, float intensityY,
            int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Intensity = intensity;
            Speed = speed;
            IntensityX = intensityX;
            IntensityY = intensityY;
        }

        public object Clone() => Copy();
        public Shake Copy() => new(Intensity, Speed, IntensityX, IntensityY, Frame, Ease);

        public override bool Equals(object obj) => obj is Shake value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(),
            Intensity, Speed, IntensityX, IntensityY);

        public bool Equals(Shake other)
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