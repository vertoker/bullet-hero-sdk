using System;
using BHSDK.Models.Game;
using BHSDK.Models.Interfaces.SaveData;
using BHSDK.Models.NoGame;
using Newtonsoft.Json;

namespace BHSDK.Models
{
    public class Level : ILevel
    {
        public Version GetVersion() => new(1, 0);
        
        [JsonProperty(ModelNames.Meta)]
        public LevelMeta Meta { get; set; }
        
        [JsonProperty(ModelNames.Track)]
        public LevelTrack Track { get; set; }
        
        [JsonProperty(ModelNames.Rules)]
        public LevelRules Rules { get; set; }
        
        [JsonProperty(ModelNames.Game)]
        public GameLevel Game { get; set; }

        public Level()
        {
            Meta = new LevelMeta();
            Track = new LevelTrack();
            Rules = new LevelRules();
            Game = new GameLevel();
        }
        public Level(LevelMeta meta, LevelTrack track, LevelRules rules, GameLevel game)
        {
            Meta = meta;
            Track = track;
            Rules = rules;
            Game = game;
        }
    }
}