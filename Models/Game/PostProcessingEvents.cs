using System;
using System.Collections.Generic;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.PostProcessing;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Game
{
    [RuleContainer]
    public class PostProcessingEvents : IModel<PostProcessingEvents>
    {
        [JsonProperty(Names.ActiveShort)]
        public bool Active { get; set; }
        
        // General
        
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxPostProcessingKeys)]
        [RuleCollectionUnique(nameof(BloomKey.Frame))]
        [JsonProperty(Names.BloomShort)]
        public List<BloomKey> Blooms { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxPostProcessingKeys)]
        [RuleCollectionUnique(nameof(ChromaticAberrationKey.Frame))]
        [JsonProperty(Names.ChromaticShort)]
        public List<ChromaticAberrationKey> Chromatics { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxPostProcessingKeys)]
        [RuleCollectionUnique(nameof(VignetteKey.Frame))]
        [JsonProperty(Names.VignetteShort)]
        public List<VignetteKey> Vignettes { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxPostProcessingKeys)]
        [RuleCollectionUnique(nameof(LensDistortionKey.Frame))]
        [JsonProperty(Names.LensShort)]
        public List<LensDistortionKey> Lenses { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxPostProcessingKeys)]
        [RuleCollectionUnique(nameof(FilmGrainKey.Frame))]
        [JsonProperty(Names.GrainShort)]
        public List<FilmGrainKey> Grains { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxPostProcessingKeys)]
        [RuleCollectionUnique(nameof(MotionBlurKey.Frame))]
        [JsonProperty(Names.MotionBlurShort)]
        public List<MotionBlurKey> MotionBlurs { get; set; }
        
        // Colors
        
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxPostProcessingKeys)]
        [RuleCollectionUnique(nameof(ColorCurvesKey.Frame))]
        [JsonProperty(Names.ColorCurvesShort)]
        public List<ColorCurvesKey> ColorCurveses { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxPostProcessingKeys)]
        [RuleCollectionUnique(nameof(LiftGammaGainKey.Frame))]
        [JsonProperty(Names.LiftGammaGainShort)]
        public List<LiftGammaGainKey> LiftGammaGains { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxPostProcessingKeys)]
        [RuleCollectionUnique(nameof(ShadowsMidtonesHighlightsKey.Frame))]
        [JsonProperty(Names.ShadowsMidtonesHighlightsShort)]
        public List<ShadowsMidtonesHighlightsKey> ShadowsMidtonesHighlightses { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxPostProcessingKeys)]
        [RuleCollectionUnique(nameof(WhiteBalanceKey.Frame))]
        [JsonProperty(Names.WhiteBalanceShort)]
        public List<WhiteBalanceKey> WhiteBalances { get; set; }
        
        // Glitches
        
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxPostProcessingKeys)]
        [RuleCollectionUnique(nameof(AnalogGlitchKey.Frame))]
        [JsonProperty(Names.AnalogGlitchShort)]
        public List<AnalogGlitchKey> AnalogGlitches { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxPostProcessingKeys)]
        [RuleCollectionUnique(nameof(DigitalGlitchKey.Frame))]
        [JsonProperty(Names.DigitalGlitchShort)]
        public List<DigitalGlitchKey> DigitalGlitches { get; set; }

        public PostProcessingEvents()
        {
            Active = PostProcessingRules.ActiveDefault;
            Blooms = new List<BloomKey>();
            Chromatics = new List<ChromaticAberrationKey>();
            Vignettes = new List<VignetteKey>();
            Lenses = new List<LensDistortionKey>();
            Grains = new List<FilmGrainKey>();
            MotionBlurs = new List<MotionBlurKey>();
            ColorCurveses = new List<ColorCurvesKey>();
            LiftGammaGains = new List<LiftGammaGainKey>();
            ShadowsMidtonesHighlightses = new List<ShadowsMidtonesHighlightsKey>();
            WhiteBalances = new List<WhiteBalanceKey>();
            AnalogGlitches = new List<AnalogGlitchKey>();
            DigitalGlitches = new List<DigitalGlitchKey>();
        }
        public PostProcessingEvents(bool active,
            List<BloomKey> blooms, 
            List<ChromaticAberrationKey> chromatics, 
            List<VignetteKey> vignettes, 
            List<LensDistortionKey> lenses, 
            List<FilmGrainKey> grains, 
            List<MotionBlurKey> motionBlurs, 
            List<ColorCurvesKey> colorCurveses, 
            List<LiftGammaGainKey> liftGammaGains, 
            List<ShadowsMidtonesHighlightsKey> shadowsMidtonesHighlightses, 
            List<WhiteBalanceKey> whiteBalances, 
            List<AnalogGlitchKey> analogGlitches, 
            List<DigitalGlitchKey> digitalGlitches)
        {
            Active = active;
            Blooms = blooms;
            Chromatics = chromatics;
            Vignettes = vignettes;
            Lenses = lenses;
            Grains = grains;
            MotionBlurs = motionBlurs;
            ColorCurveses = colorCurveses;
            LiftGammaGains = liftGammaGains;
            ShadowsMidtonesHighlightses = shadowsMidtonesHighlightses;
            WhiteBalances = whiteBalances;
            AnalogGlitches = analogGlitches;
            DigitalGlitches = digitalGlitches;
        }
        public void Reset()
        {
            Active = PostProcessingRules.ActiveDefault;
            Blooms.Clear();
            Chromatics.Clear();
            Vignettes.Clear();
            Lenses.Clear();
            Grains.Clear();
            MotionBlurs.Clear();
            ColorCurveses.Clear();
            LiftGammaGains.Clear();
            ShadowsMidtonesHighlightses.Clear();
            WhiteBalances.Clear();
            AnalogGlitches.Clear();
            DigitalGlitches.Clear();
        }

        public object Clone() => Copy();
        public PostProcessingEvents Copy() => new(Active, Blooms.CopyList(), Chromatics.CopyList(), Vignettes.CopyList(),
            Lenses.CopyList(), Grains.CopyList(), MotionBlurs.CopyList(), ColorCurveses.CopyList(),
            LiftGammaGains.CopyList(), ShadowsMidtonesHighlightses.CopyList(), WhiteBalances.CopyList(),
            AnalogGlitches.CopyList(), DigitalGlitches.CopyList());

        public override bool Equals(object obj) => obj is PostProcessingEvents value && Equals(value);
        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(Active.GetHashCode());
            hashCode.Add(Blooms.GetListHashCode());
            hashCode.Add(Chromatics.GetListHashCode());
            hashCode.Add(Vignettes.GetListHashCode());
            hashCode.Add(Lenses.GetListHashCode());
            hashCode.Add(Grains.GetListHashCode());
            hashCode.Add(MotionBlurs.GetListHashCode());
            hashCode.Add(ColorCurveses.GetListHashCode());
            hashCode.Add(LiftGammaGains.GetListHashCode());
            hashCode.Add(ShadowsMidtonesHighlightses.GetListHashCode());
            hashCode.Add(WhiteBalances.GetListHashCode());
            hashCode.Add(AnalogGlitches.GetListHashCode());
            hashCode.Add(DigitalGlitches.GetListHashCode());
            return hashCode.ToHashCode();
        }

        public bool Equals(PostProcessingEvents other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Active == other.Active
                         && Blooms.ListEquals(other.Blooms)
                         && Chromatics.ListEquals(other.Chromatics)
                         && Vignettes.ListEquals(other.Vignettes)
                         && Lenses.ListEquals(other.Lenses)
                         && Grains.ListEquals(other.Grains)
                         && MotionBlurs.ListEquals(other.MotionBlurs)
                         && ColorCurveses.ListEquals(other.ColorCurveses)
                         && LiftGammaGains.ListEquals(other.LiftGammaGains)
                         && ShadowsMidtonesHighlightses.ListEquals(other.ShadowsMidtonesHighlightses)
                         && WhiteBalances.ListEquals(other.WhiteBalances)
                         && AnalogGlitches.ListEquals(other.AnalogGlitches)
                         && DigitalGlitches.ListEquals(other.DigitalGlitches);
            return result;
        }
    }
}