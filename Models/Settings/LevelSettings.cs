using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Settings
{
    [RuleContainer]
    public class LevelSettings : ICopyable<LevelSettings>
    {
        [RuleMin(FrameRules.MinFramerate)]
        [JsonProperty(Names.Fps)]
        public int Framerate { get; set; }
        
        [RuleMin(FrameRules.MinFrameLength)]
        [JsonProperty(Names.FrameLengthShort)]
        public int FrameLength { get; set; }
        
        [RuleNotNull(typeof(ScreenLimitFixed))]
        [JsonProperty(Names.ScreenLimit)]
        public IScreenLimit ScreenLimit { get; set; }
        // limitations for screen will be chosen by mappers

        public LevelSettings()
        {
            Framerate = 60;
            FrameLength = Framerate * 10;
            ScreenLimit = new ScreenLimitNone();
        }
        public LevelSettings(int framerate, int frameLength, IScreenLimit screenLimit)
        {
            Framerate = framerate;
            FrameLength = frameLength;
            ScreenLimit = screenLimit;
        }

        public object Clone() => Copy();
        public LevelSettings Copy() => new(Framerate, FrameLength, ScreenLimit.Copy());
    }
}