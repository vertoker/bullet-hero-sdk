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
    public class WhiteBalanceKey : PostProcessingKeyframe, IModel<WhiteBalanceKey>
    {
        [RuleInRange(PostProcessingRules.WhiteBalance.TemperatureMin,
            PostProcessingRules.WhiteBalance.TemperatureMax)]
        [JsonProperty(Names.Temperature)]
        public float Temperature { get; set; }
        
        [RuleInRange(PostProcessingRules.WhiteBalance.TintMin,
            PostProcessingRules.WhiteBalance.TintMax)]
        [JsonProperty(Names.Tint)]
        public float Tint { get; set; }

        public WhiteBalanceKey()
        {
            Temperature = 0f;
            Tint = 0f;
        }
        public WhiteBalanceKey(float temperature, float tint,
            bool active, int frame, EaseType ease = Keyframe.DefaultEase) : base(active, frame, ease)
        {
            Temperature = temperature;
            Tint = tint;
        }
        public override void Reset()
        {
            base.Reset();
            Temperature = 0f;
            Tint = 0f;
        }
        
        public override object Clone() => CopyImpl();
        public override PostProcessingKeyframe Copy() => CopyImpl();
        WhiteBalanceKey ICopyable<WhiteBalanceKey>.Copy() => CopyImpl();
        
        private WhiteBalanceKey CopyImpl() => new(Temperature, Tint, Active, Frame, Ease);

        public override bool Equals(object obj) => obj is WhiteBalanceKey value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Temperature, Tint);

        public bool Equals(WhiteBalanceKey other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && Temperature.Equals(other.Temperature)
                         && Tint.Equals(other.Tint);
            return result;
        }
    }
}