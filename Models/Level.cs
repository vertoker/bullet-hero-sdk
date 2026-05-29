using System;
using BH.SDK.Models.Audio;
using BH.SDK.Models.Game;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.SaveData;
using BH.SDK.Models.Resources;
using BH.SDK.Models.SettingGroups;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BH.SDK.Models
{
    // TODO allow null objects, this will save many space on serialization (don't forget about rules)
    // TODO add IResetable
    
    [RuleContainer]
    public class Level : ILevel, ICopyable<Level>, IEquatable<Level>
    {
        public static readonly Version Version = new(1, 0);
        public Version GetVersion() => Version;

        [RuleNotNull]
        [JsonProperty(Names.Settings)]
        public LevelSettings Settings { get; set; }

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
            Game = new GameLevel();
            Audio = new AudioLevel();
            Resources = new LevelResources();
        }

        public Level(LevelSettings settings, GameLevel game, AudioLevel audio, LevelResources resources)
        {
            Settings = settings;
            Game = game;
            Audio = audio;
            Resources = resources;
        }

        public object Clone() => Copy();
        public Level Copy() => new(Settings.Copy(), Game.Copy(), Audio.Copy(), Resources.Copy());

        public override bool Equals(object obj) => obj is Level value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Settings, Game, Audio, Resources);

        public bool Equals(Level other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Settings.Equals(other.Settings)
                         && Game.Equals(other.Game)
                         && Audio.Equals(other.Audio)
                         && Resources.Equals(other.Resources);
            return result;
        }
    }
}