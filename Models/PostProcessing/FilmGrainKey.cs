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
    public class FilmGrainKey : PostProcessingKeyframe, IModel<FilmGrainKey>
    {
        [JsonProperty(Names.Type)]
        public FilmGrainType Type { get; set; }
        
        [RuleInRange(PostProcessingRules.FilmGrain.IntensityMin,
            PostProcessingRules.FilmGrain.IntensityMax)]
        [JsonProperty(Names.Intensity)]
        public float Intensity { get; set; }

        public FilmGrainKey()
        {
            Type = FilmGrainType.Medium1;
            Intensity = 1.0f;
        }
        public FilmGrainKey(FilmGrainType type, float intensity,
            bool active, int frame, EaseType ease = Keyframe.DefaultEase) : base(active, frame, ease)
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
        public override PostProcessingKeyframe Copy() => CopyImpl();
        FilmGrainKey ICopyable<FilmGrainKey>.Copy() => CopyImpl();
        
        private FilmGrainKey CopyImpl() => new(Type, Intensity, Active, Frame, Ease);

        public override bool Equals(object obj) => obj is FilmGrainKey value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), (int)Type, Intensity);

        public bool Equals(FilmGrainKey other)
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