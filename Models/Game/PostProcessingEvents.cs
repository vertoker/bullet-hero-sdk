using System.Collections.Generic;
using BHSDK.Models.PostProcessing;
using Newtonsoft.Json;

namespace BHSDK.Models.Game
{
    public class PostProcessingEvents
    {
        // General
        
        [JsonProperty("blm")]
        public List<Bloom> Bloom { get; set; }
        
        [JsonProperty("chr")]
        public List<ChromaticAberration> Chroma { get; set; }
        
        [JsonProperty("vgn")]
        public List<Vignette> Vignette { get; set; }
        
        [JsonProperty("lns")]
        public List<LensDistortion> Lens { get; set; }
        
        [JsonProperty("grn")]
        public List<FilmGrain> Grain { get; set; }
        
        [JsonProperty("mbr")]
        public List<MotionBlur> MotionBlur { get; set; }
        
        // Colors
        
        [JsonProperty("ccv")]
        public List<ColorCurves> ColorCurves { get; set; }
        
        [JsonProperty("lgg")]
        public List<LiftGammaGain> LiftGammaGain { get; set; }
        
        [JsonProperty("smh")]
        public List<ShadowsMidtonesHighlights> ShadowsMidtonesHighlights { get; set; }
        
        [JsonProperty("wbl")]
        public List<WhiteBalance> WhiteBalance { get; set; }
        
        // Glitches
        
        [JsonProperty("agl")]
        public List<AnalogGlitch> AnalogGlitch { get; set; }
        
        [JsonProperty("dgl")]
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