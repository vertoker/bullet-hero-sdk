using System;
using BHSDK.Models.Game;
using BHSDK.Serialization;
using Newtonsoft.Json;

namespace BHSDK.Models
{
    public class Level : ILevel
    {
        public Version GetVersion() => new(1, 0);
        
        [JsonProperty("m")]
        public LevelMeta Meta { get; set; }
        
        [JsonProperty("t")]
        public LevelTrack Track { get; set; }
        
        [JsonProperty("r")]
        public LevelRules Rules { get; set; }
        
        [JsonProperty("g")]
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