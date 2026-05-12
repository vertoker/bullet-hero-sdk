using System;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Keyframes;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BHSDK.Models.PostProcessing
{
    [RuleContainer]
    public class FilmGrain : Keyframe,
        ICopyable<FilmGrain>, IEquatable<FilmGrain>
    {
        [JsonProperty(Names.Type)]
        public FilmGrainType Type { get; set; }
        
        [RuleInRange(PostProcessingRules.FilmGrain.IntensityMin,
            PostProcessingRules.FilmGrain.IntensityMax)]
        [JsonProperty(Names.Intensity)]
        public float Intensity { get; set; }

        public FilmGrain()
        {
            Type = FilmGrainType.Medium1;
            Intensity = 1.0f;
        }
        public FilmGrain(FilmGrainType type, float intensity,
            int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Type = type;
            Intensity = intensity;
        }

        public object Clone() => Copy();
        public FilmGrain Copy() => new(Type, Intensity, Frame, Ease);

        public override bool Equals(object obj) => obj is FilmGrain value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), (int)Type, Intensity);

        public bool Equals(FilmGrain other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && Type == other.Type
                         && Intensity.Equals(other.Intensity);
            return result;
        }
    }
}