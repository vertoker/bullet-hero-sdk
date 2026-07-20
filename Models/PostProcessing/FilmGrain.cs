using System;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Keyframes;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.PostProcessing
{
    [RuleContainer]
    public class FilmGrain : Keyframe, IModel<FilmGrain>
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
        public override void Reset()
        {
            base.Reset();
            Type = FilmGrainType.Medium1;
            Intensity = 1.0f;
        }
        
        public override object Clone() => CopyImpl();
        public override Keyframe Copy() => CopyImpl();
        FilmGrain ICopyable<FilmGrain>.Copy() => CopyImpl();
        
        private FilmGrain CopyImpl() => new(Type, Intensity, Frame, Ease);

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