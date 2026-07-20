using System;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Keyframes;
using BH.SDK.Models.Values;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.PostProcessing
{
    [RuleContainer]
    public class LiftGammaGainKey : PostProcessingKeyframe, IModel<LiftGammaGainKey>
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

        public LiftGammaGainKey()
        {
            Lift = false;
            LiftColor = ColorValue.white;
            Gamma = false;
            GammaColor = ColorValue.white;
            Gain = false;
            GainColor = ColorValue.white;
        }
        public LiftGammaGainKey(
            bool lift, IColor liftColor,
            bool gamma, IColor gammaColor,
            bool gain, IColor gainColor,
            bool active, int frame, EaseType ease = Keyframe.DefaultEase) : base(active, frame, ease)
        {
            Lift = lift;
            LiftColor = liftColor;
            Gamma = gamma;
            GammaColor = gammaColor;
            Gain = gain;
            GainColor = gainColor;
        }
        public override void Reset()
        {
            base.Reset();
            Lift = false;
            LiftColor = ColorValue.white;
            Gamma = false;
            GammaColor = ColorValue.white;
            Gain = false;
            GainColor = ColorValue.white;
        }

        public override object Clone() => CopyImpl();
        public override PostProcessingKeyframe Copy() => CopyImpl();
        LiftGammaGainKey ICopyable<LiftGammaGainKey>.Copy() => CopyImpl();
        
        private LiftGammaGainKey CopyImpl() => new(Lift, LiftColor.Copy(), Gamma, GammaColor.Copy(), Gain, GainColor.Copy(), Active, Frame, Ease);

        public override bool Equals(object obj) => obj is LiftGammaGainKey value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(),
            Lift, LiftColor, Gamma, GammaColor, Gain, GainColor);

        public bool Equals(LiftGammaGainKey other)
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