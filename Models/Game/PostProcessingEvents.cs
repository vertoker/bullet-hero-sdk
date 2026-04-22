using System.Collections.Generic;
using BHSDK.Models.PostProcessing;
using Newtonsoft.Json;

namespace BHSDK.Models.Game
{
    public class PostProcessingEvents
    {
        // General
        
        [JsonProperty(Names.BloomShort)]
        public List<Bloom> Bloom { get; set; }
        
        [JsonProperty(Names.ChromaShort)]
        public List<ChromaticAberration> Chroma { get; set; }
        
        [JsonProperty(Names.VignetteShort)]
        public List<Vignette> Vignette { get; set; }
        
        [JsonProperty(Names.LensShort)]
        public List<LensDistortion> Lens { get; set; }
        
        [JsonProperty(Names.GrainShort)]
        public List<FilmGrain> Grain { get; set; }
        
        [JsonProperty(Names.MotionBlurShort)]
        public List<MotionBlur> MotionBlur { get; set; }
        
        // Colors
        
        [JsonProperty(Names.ColorCurvesShort)]
        public List<ColorCurves> ColorCurves { get; set; }
        
        [JsonProperty(Names.LiftGammaGainShort)]
        public List<LiftGammaGain> LiftGammaGain { get; set; }
        
        [JsonProperty(Names.ShadowsMidtonesHighlightsShort)]
        public List<ShadowsMidtonesHighlights> ShadowsMidtonesHighlights { get; set; }
        
        [JsonProperty(Names.WhiteBalanceShort)]
        public List<WhiteBalance> WhiteBalance { get; set; }
        
        // Glitches
        
        [JsonProperty(Names.AnalogGlitchShort)]
        public List<AnalogGlitch> AnalogGlitch { get; set; }
        
        [JsonProperty(Names.DigitalGlitchShort)]
        public List<DigitalGlitch> DigitalGlitch { get; set; }

        public PostProcessingEvents()
        {
            Bloom = new List<Bloom>();
            Chroma = new List<ChromaticAberration>();
            Vignette = new List<Vignette>();
            Lens = new List<LensDistortion>();
            Grain = new List<FilmGrain>();
            MotionBlur = new List<MotionBlur>();
            
            ColorCurves = new List<ColorCurves>();
            LiftGammaGain = new List<LiftGammaGain>();
            ShadowsMidtonesHighlights = new List<ShadowsMidtonesHighlights>();
            WhiteBalance = new List<WhiteBalance>();
            
            AnalogGlitch = new List<AnalogGlitch>();
            DigitalGlitch = new List<DigitalGlitch>();
        }
        public PostProcessingEvents(List<Bloom> bloom, 
            List<ChromaticAberration> chroma, 
            List<Vignette> vignette, 
            List<LensDistortion> lens, 
            List<FilmGrain> grain, 
            List<MotionBlur> motionBlur, 
            List<ColorCurves> colorCurves, 
            List<LiftGammaGain> liftGammaGain, 
            List<ShadowsMidtonesHighlights> shadowsMidtonesHighlights, 
            List<WhiteBalance> whiteBalance, 
            List<AnalogGlitch> analogGlitch, 
            List<DigitalGlitch> digitalGlitch)
        {
            Bloom = bloom;
            Chroma = chroma;
            Vignette = vignette;
            Lens = lens;
            Grain = grain;
            MotionBlur = motionBlur;
            ColorCurves = colorCurves;
            LiftGammaGain = liftGammaGain;
            ShadowsMidtonesHighlights = shadowsMidtonesHighlights;
            WhiteBalance = whiteBalance;
            AnalogGlitch = analogGlitch;
            DigitalGlitch = digitalGlitch;
        }
    }
}