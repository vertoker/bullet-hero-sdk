using System;
using BHSDK.Models.Audio;
using BHSDK.Models.Game;
using BHSDK.Models.Interfaces.SaveData;
using BHSDK.Models.NoGame;
using BHSDK.Models.Settings;
using Newtonsoft.Json;

namespace BHSDK.Models
{
    public class Level : ILevel
    {
        public Version GetVersion() => new(1, 0);
        
        [JsonProperty(ModelNames.Settings)]
        public LevelSettings Settings { get; set; }
        
        [JsonProperty(ModelNames.Meta)]
        public LevelMeta Meta { get; set; }
        
        [JsonProperty(ModelNames.Game)]
        public GameLevel Game { get; set; }
        
        [JsonProperty(ModelNames.Audio)]
        public AudioLevel Audio { get; set; }

        public Level()
        {
            Settings = new LevelSettings();
            Meta = new LevelMeta();
            Game = new GameLevel();
            Audio = new AudioLevel();
        }
        public Level(LevelSettings settings, LevelMeta meta, GameLevel game, AudioLevel audio)
        {
            Settings = settings;
            Meta = meta;
            Game = game;
            Audio = audio;
        }
    }
}