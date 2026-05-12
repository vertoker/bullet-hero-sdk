using System.Collections.Generic;
using BHSDK.Models.Interfaces;
using BHSDK.Models.PostProcessing;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.Game
{
    [RuleContainer]
    public class PostProcessingEvents : ICopyable<PostProcessingEvents>
    {
        // General
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxPostProcessingEvents)]
        [RuleCollectionSorted(nameof(Bloom.Frame))]
        [RuleCollectionUnique(nameof(Bloom.Frame))]
        [JsonProperty(Names.BloomShort)]
        public List<Bloom> Blooms { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxPostProcessingEvents)]
        [RuleCollectionSorted(nameof(ChromaticAberration.Frame))]
        [RuleCollectionUnique(nameof(ChromaticAberration.Frame))]
        [JsonProperty(Names.ChromaShort)]
        public List<ChromaticAberration> Chromas { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxPostProcessingEvents)]
        [RuleCollectionSorted(nameof(Vignette.Frame))]
        [RuleCollectionUnique(nameof(Vignette.Frame))]
        [JsonProperty(Names.VignetteShort)]
        public List<Vignette> Vignettes { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxPostProcessingEvents)]
        [RuleCollectionSorted(nameof(LensDistortion.Frame))]
        [RuleCollectionUnique(nameof(LensDistortion.Frame))]
        [JsonProperty(Names.LensShort)]
        public List<LensDistortion> Lenses { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxPostProcessingEvents)]
        [RuleCollectionSorted(nameof(FilmGrain.Frame))]
        [RuleCollectionUnique(nameof(FilmGrain.Frame))]
        [JsonProperty(Names.GrainShort)]
        public List<FilmGrain> Grains { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxPostProcessingEvents)]
        [RuleCollectionSorted(nameof(MotionBlur.Frame))]
        [RuleCollectionUnique(nameof(MotionBlur.Frame))]
        [JsonProperty(Names.MotionBlurShort)]
        public List<MotionBlur> MotionBlurs { get; set; }
        
        // Colors
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxPostProcessingEvents)]
        [RuleCollectionSorted(nameof(ColorCurves.Frame))]
        [RuleCollectionUnique(nameof(ColorCurves.Frame))]
        [JsonProperty(Names.ColorCurvesShort)]
        public List<ColorCurves> ColorCurveses { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxPostProcessingEvents)]
        [RuleCollectionSorted(nameof(LiftGammaGain.Frame))]
        [RuleCollectionUnique(nameof(LiftGammaGain.Frame))]
        [JsonProperty(Names.LiftGammaGainShort)]
        public List<LiftGammaGain> LiftGammaGains { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxPostProcessingEvents)]
        [RuleCollectionSorted(nameof(ShadowsMidtonesHighlights.Frame))]
        [RuleCollectionUnique(nameof(ShadowsMidtonesHighlights.Frame))]
        [JsonProperty(Names.ShadowsMidtonesHighlightsShort)]
        public List<ShadowsMidtonesHighlights> ShadowsMidtonesHighlightses { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxPostProcessingEvents)]
        [RuleCollectionSorted(nameof(WhiteBalance.Frame))]
        [RuleCollectionUnique(nameof(WhiteBalance.Frame))]
        [JsonProperty(Names.WhiteBalanceShort)]
        public List<WhiteBalance> WhiteBalances { get; set; }
        
        // Glitches
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxPostProcessingEvents)]
        [RuleCollectionSorted(nameof(AnalogGlitch.Frame))]
        [RuleCollectionUnique(nameof(AnalogGlitch.Frame))]
        [JsonProperty(Names.AnalogGlitchShort)]
        public List<AnalogGlitch> AnalogGlitches { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxPostProcessingEvents)]
        [RuleCollectionSorted(nameof(DigitalGlitch.Frame))]
        [RuleCollectionUnique(nameof(DigitalGlitch.Frame))]
        [JsonProperty(Names.DigitalGlitchShort)]
        public List<DigitalGlitch> DigitalGlitches { get; set; }

        public PostProcessingEvents()
        {
            Blooms = new List<Bloom>();
            Chromas = new List<ChromaticAberration>();
            Vignettes = new List<Vignette>();
            Lenses = new List<LensDistortion>();
            Grains = new List<FilmGrain>();
            MotionBlurs = new List<MotionBlur>();
            
            ColorCurveses = new List<ColorCurves>();
            LiftGammaGains = new List<LiftGammaGain>();
            ShadowsMidtonesHighlightses = new List<ShadowsMidtonesHighlights>();
            WhiteBalances = new List<WhiteBalance>();
            
            AnalogGlitches = new List<AnalogGlitch>();
            DigitalGlitches = new List<DigitalGlitch>();
        }
        public PostProcessingEvents(List<Bloom> blooms, 
            List<ChromaticAberration> chromas, 
            List<Vignette> vignettes, 
            List<LensDistortion> lenses, 
            List<FilmGrain> grains, 
            List<MotionBlur> motionBlurs, 
            List<ColorCurves> colorCurveses, 
            List<LiftGammaGain> liftGammaGains, 
            List<ShadowsMidtonesHighlights> shadowsMidtonesHighlightses, 
            List<WhiteBalance> whiteBalances, 
            List<AnalogGlitch> analogGlitches, 
            List<DigitalGlitch> digitalGlitches)
        {
            Blooms = blooms;
            Chromas = chromas;
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

        public object Clone() => Copy();
        public PostProcessingEvents Copy() => new(Blooms.CopyList(), Chromas.CopyList(), Vignettes.CopyList(),
            Lenses.CopyList(), Grains.CopyList(), MotionBlurs.CopyList(), ColorCurveses.CopyList(),
            LiftGammaGains.CopyList(), ShadowsMidtonesHighlightses.CopyList(), WhiteBalances.CopyList(),
            AnalogGlitches.CopyList(), DigitalGlitches.CopyList());
    }
}