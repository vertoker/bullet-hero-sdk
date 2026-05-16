using System;
using BHSDK.Models.Audio;
using BHSDK.Models.Game;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.SaveData;
using BHSDK.Models.NoGame;
using BHSDK.Models.Resources;
using BHSDK.Models.SettingGroups;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BHSDK.Models
{
    [RuleContainer]
    public class Level : ILevel, ICopyable<Level>, IEquatable<Level>
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

        public override bool Equals(object obj) => obj is Level value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Settings, Meta, Game, Audio, Resources);

        public bool Equals(Level other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Settings.Equals(other.Settings) 
                         && Meta.Equals(other.Meta) 
                         && Game.Equals(other.Game)
                         && Audio.Equals(other.Audio)
                         && Resources.Equals(other.Resources);
            return result;
        }
    }
}