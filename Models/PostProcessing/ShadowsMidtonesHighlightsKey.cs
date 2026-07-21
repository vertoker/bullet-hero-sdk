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
        
        [RuleNotNull(typeof(Color4Value))] // TODO add color hdr support for alpha rule (0f-2f)
        [JsonProperty(Names.ShadowColor)]
        public IColor4 ShadowsColor4 { get; set; }
        
        
        [JsonProperty(Names.Midtone)]
        public bool Midtones { get; set; }
        
        [RuleNotNull(typeof(Color4Value))] // TODO add color hdr support for alpha rule (0f-2f)
        [JsonProperty(Names.MidtoneColor)]
        public IColor4 MidtonesColor4 { get; set; }
        
        
        [JsonProperty(Names.Highlight)]
        public bool Highlights { get; set; }
        
        [RuleNotNull(typeof(Color4Value))] // TODO add color hdr support for alpha rule (0f-2f)
        [JsonProperty(Names.HighlightColor)]
        public IColor4 HighlightsColor4 { get; set; }
        
        
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
            ShadowsColor4 = Color4Value.white;
            Midtones = false;
            MidtonesColor4 = Color4Value.white;
            Highlights = false;
            HighlightsColor4 = Color4Value.white;
            
            ShadowLimits = new Vector2Value(0f, 0.3f);
            HighlightLimits = new Vector2Value(0.55f, 1f);
        }
        public ShadowsMidtonesHighlightsKey(
            bool shadows, IColor4 shadowsColor4,
            bool midtones, IColor4 midtonesColor4, 
            bool highlights, IColor4 highlightsColor4, 
            IVector2 shadowLimits, IVector2 highlightLimits,
            bool active, int frame, EaseType ease = Keyframe.DefaultEase) : base(active, frame, ease)
        {
            Shadows = shadows;
            ShadowsColor4 = shadowsColor4;
            Midtones = midtones;
            MidtonesColor4 = midtonesColor4;
            Highlights = highlights;
            HighlightsColor4 = highlightsColor4;
            ShadowLimits = shadowLimits;
            HighlightLimits = highlightLimits;
        }
        public override void Reset()
        {
            base.Reset();
            Shadows = false;
            ShadowsColor4 = Color4Value.white;
            Midtones = false;
            MidtonesColor4 = Color4Value.white;
            Highlights = false;
            HighlightsColor4 = Color4Value.white;
            
            ShadowLimits = new Vector2Value(0f, 0.3f);
            HighlightLimits = new Vector2Value(0.55f, 1f);
        }
        
        public override object Clone() => CopyImpl();
        public override PostProcessingKeyframe Copy() => CopyImpl();
        ShadowsMidtonesHighlightsKey ICopyable<ShadowsMidtonesHighlightsKey>.Copy() => CopyImpl();
        
        private ShadowsMidtonesHighlightsKey CopyImpl() => new(Shadows, ShadowsColor4.Copy(), Midtones, MidtonesColor4.Copy(),
            Highlights, HighlightsColor4.Copy(), ShadowLimits.Copy(), HighlightLimits.Copy(), Active, Frame, Ease);
        
        public override bool Equals(object obj) => obj is ShadowsMidtonesHighlightsKey value && Equals(value);
        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(base.GetHashCode());
            hashCode.Add(Shadows);
            hashCode.Add(ShadowsColor4);
            hashCode.Add(Midtones);
            hashCode.Add(MidtonesColor4);
            hashCode.Add(Highlights);
            hashCode.Add(HighlightsColor4);
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
                         && ShadowsColor4.Equals(other.ShadowsColor4)
                         && Midtones == other.Midtones
                         && MidtonesColor4.Equals(other.MidtonesColor4)
                         && Highlights == other.Highlights
                         && HighlightsColor4.Equals(other.HighlightsColor4)
                         && ShadowLimits.Equals(other.ShadowLimits)
                         && HighlightLimits.Equals(other.HighlightLimits);
            return result;
        }
    }
}