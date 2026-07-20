using System;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Keyframes;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.PostProcessing
{
    [RuleContainer]
    public class ShadowsMidtonesHighlightsKey : PostProcessingKeyframe, IModel<ShadowsMidtonesHighlightsKey>
    {
        [JsonProperty(Names.Shadow)]
        public bool Shadows { get; set; }
        
        [RuleNotNull(typeof(ColorValue))] // TODO add color hdr support for alpha rule (0f-2f)
        [JsonProperty(Names.ShadowColor)]
        public IColor ShadowsColor { get; set; }
        
        
        [JsonProperty(Names.Midtone)]
        public bool Midtones { get; set; }
        
        [RuleNotNull(typeof(ColorValue))] // TODO add color hdr support for alpha rule (0f-2f)
        [JsonProperty(Names.MidtoneColor)]
        public IColor MidtonesColor { get; set; }
        
        
        [JsonProperty(Names.Highlight)]
        public bool Highlights { get; set; }
        
        [RuleNotNull(typeof(ColorValue))] // TODO add color hdr support for alpha rule (0f-2f)
        [JsonProperty(Names.HighlightColor)]
        public IColor HighlightsColor { get; set; }
        
        
        // TODO graph like in Post Processing menu
        
        [RuleNotNull, RuleIVector2InRange(PostProcessingRules.ShadowsMidtonesHighlights.ShadowLimitMin,
             PostProcessingRules.ShadowsMidtonesHighlights.ShadowLimitMax)]
        [JsonProperty(Names.ShadowLimit)]
        public IVector2 ShadowLimits { get; set; }
        
        [RuleNotNull, RuleIVector2InRange(PostProcessingRules.ShadowsMidtonesHighlights.HighlightLimitMin,
             PostProcessingRules.ShadowsMidtonesHighlights.HighlightLimitMax)]
        [JsonProperty(Names.HighlightLimit)]
        public IVector2 HighlightLimits { get; set; }

        public ShadowsMidtonesHighlightsKey()
        {
            Shadows = false;
            ShadowsColor = ColorValue.white;
            Midtones = false;
            MidtonesColor = ColorValue.white;
            Highlights = false;
            HighlightsColor = ColorValue.white;
            
            ShadowLimits = new Vector2Value(0f, 0.3f);
            HighlightLimits = new Vector2Value(0.55f, 1f);
        }
        public ShadowsMidtonesHighlightsKey(
            bool shadows, IColor shadowsColor,
            bool midtones, IColor midtonesColor, 
            bool highlights, IColor highlightsColor, 
            IVector2 shadowLimits, IVector2 highlightLimits,
            bool active, int frame, EaseType ease = Keyframe.DefaultEase) : base(active, frame, ease)
        {
            Shadows = shadows;
            ShadowsColor = shadowsColor;
            Midtones = midtones;
            MidtonesColor = midtonesColor;
            Highlights = highlights;
            HighlightsColor = highlightsColor;
            ShadowLimits = shadowLimits;
            HighlightLimits = highlightLimits;
        }
        public override void Reset()
        {
            base.Reset();
            Shadows = false;
            ShadowsColor = ColorValue.white;
            Midtones = false;
            MidtonesColor = ColorValue.white;
            Highlights = false;
            HighlightsColor = ColorValue.white;
            
            ShadowLimits = new Vector2Value(0f, 0.3f);
            HighlightLimits = new Vector2Value(0.55f, 1f);
        }
        
        public override object Clone() => CopyImpl();
        public override PostProcessingKeyframe Copy() => CopyImpl();
        ShadowsMidtonesHighlightsKey ICopyable<ShadowsMidtonesHighlightsKey>.Copy() => CopyImpl();
        
        private ShadowsMidtonesHighlightsKey CopyImpl() => new(Shadows, ShadowsColor.Copy(), Midtones, MidtonesColor.Copy(),
            Highlights, HighlightsColor.Copy(), ShadowLimits.Copy(), HighlightLimits.Copy(), Active, Frame, Ease);
        
        public override bool Equals(object obj) => obj is ShadowsMidtonesHighlightsKey value && Equals(value);
        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(base.GetHashCode());
            hashCode.Add(Shadows);
            hashCode.Add(ShadowsColor);
            hashCode.Add(Midtones);
            hashCode.Add(MidtonesColor);
            hashCode.Add(Highlights);
            hashCode.Add(HighlightsColor);
            hashCode.Add(ShadowLimits);
            hashCode.Add(HighlightLimits);
            return hashCode.ToHashCode();
        }

        public bool Equals(ShadowsMidtonesHighlightsKey other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && Shadows == other.Shadows
                         && ShadowsColor.Equals(other.ShadowsColor)
                         && Midtones == other.Midtones
                         && MidtonesColor.Equals(other.MidtonesColor)
                         && Highlights == other.Highlights
                         && HighlightsColor.Equals(other.HighlightsColor)
                         && ShadowLimits.Equals(other.ShadowLimits)
                         && HighlightLimits.Equals(other.HighlightLimits);
            return result;
        }
    }
}