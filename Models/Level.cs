using System;
using BHSDK.Models.Audio;
using BHSDK.Models.Game;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.SaveData;
using BHSDK.Models.NoGame;
using BHSDK.Models.Resources;
using BHSDK.Models.Settings;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models
{
    [RuleContainer]
    public class Level : ILevel, ICopyable<Level>
    {
        public Version GetVersion() => new(1, 0);
        
        [RuleNotNull]
        [JsonProperty(Names.Settings)]
        public LevelSettings Settings { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Meta)]
        public LevelMeta Meta { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Game)]
        public GameLevel Game { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Audio)]
        public AudioLevel Audio { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Resources)]
        public LevelResources Resources { get; set; }

        public Level()
        {
            Settings = new LevelSettings();
            Meta = new LevelMeta();
            Game = new GameLevel();
            Audio = new AudioLevel();
            Resources = new LevelResources();
        }
        public Level(LevelSettings settings, LevelMeta meta, GameLevel game, AudioLevel audio, LevelResources resources)
        {
            Settings = settings;
            Meta = meta;
            Game = game;
            Audio = audio;
            Resources = resources;
        }

        public object Clone() => Copy();
        public Level Copy() => new(Settings.Copy(), Meta.Copy(), Game.Copy(), Audio.Copy(), Resources.Copy());
    }
}