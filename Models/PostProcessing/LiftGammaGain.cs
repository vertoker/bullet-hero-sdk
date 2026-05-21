using System;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Values;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;
using Keyframes_Keyframe = BH.SDK.Models.Keyframes.Keyframe;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.PostProcessing
{
    [RuleContainer]
    public class LiftGammaGain : Keyframes_Keyframe,
        ICopyable<LiftGammaGain>, IEquatable<LiftGammaGain>
    {
        [JsonProperty(Names.Lift)]
        public bool Lift { get; set; }
        
        [RuleNotNull(typeof(ColorValue))] // TODO add color hdr support for alpha rule (0f-2f)
        [JsonProperty(Names.LiftColor)]
        public IColor LiftColor { get; set; }
        
        [JsonProperty(Names.Gamma)]
        public bool Gamma { get; set; }
        
        [RuleNotNull(typeof(ColorValue))] // TODO add color hdr support for alpha rule (0f-2f)
        [JsonProperty(Names.GammaColor)]
        public IColor GammaColor { get; set; }
        
        [JsonProperty(Names.Gain)]
        public bool Gain { get; set; }
        
        [RuleNotNull(typeof(ColorValue))] // TODO add color hdr support for alpha rule (0f-2f)
        [JsonProperty(Names.GainColor)]
        public IColor GainColor { get; set; }

        public LiftGammaGain()
        {
            Lift = false;
            LiftColor = ColorValue.white;
            Gamma = false;
            GammaColor = ColorValue.white;
            Gain = false;
            GainColor = ColorValue.white;
        }
        public LiftGammaGain(
            bool lift, IColor liftColor,
            bool gamma, IColor gammaColor,
            bool gain, IColor gainColor,
            int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Lift = lift;
            LiftColor = liftColor;
            Gamma = gamma;
            GammaColor = gammaColor;
            Gain = gain;
            GainColor = gainColor;
        }

        public object Clone() => Copy();
        public LiftGammaGain Copy() => new(Lift, LiftColor.Copy(), Gamma, GammaColor.Copy(), Gain, GainColor.Copy(), Frame, Ease);

        public override bool Equals(object obj) => obj is LiftGammaGain value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(),
            Lift, LiftColor, Gamma, GammaColor, Gain, GainColor);

        public bool Equals(LiftGammaGain other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && Lift == other.Lift
                         && LiftColor.Equals(other.LiftColor)
                         && Gamma == other.Gamma
                         && GammaColor.Equals(other.GammaColor)
                         && Gain == other.Gain
                         && GainColor.Equals(other.GainColor);
            return result;
        }
    }
}