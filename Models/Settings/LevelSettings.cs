using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Settings
{
    public class LevelSettings
    {
        [JsonProperty(ModelNames.Seed)]
        public int Seed { get; set; }
        
        [JsonProperty(ModelNames.Framerate)]
        public int Framerate { get; set; }
        
        [JsonProperty(ModelNames.FrameLength)]
        public int FrameLength { get; set; }
        
        // limitations for screen will be chosen by mappers
        [JsonProperty(ModelNames.Screen + ModelNames.Limit)]
        public IScreenLimit ScreenLimit { get; set; }

        public LevelSettings()
        {
            Seed = Random.Range(int.MinValue, int.MaxValue);
            Framerate = 60;
            FrameLength = Framerate * 10;
            ScreenLimit = new ScreenLimitNone();
        }
        public LevelSettings(int seed, int framerate, int frameLength, IScreenLimit screenLimit)
        {
            Seed = seed;
            Framerate = framerate;
            FrameLength = frameLength;
            ScreenLimit = screenLimit;
        }
    }
}