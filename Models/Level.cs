using System;
using BH.SDK.Models.Audio;
using BH.SDK.Models.Game;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Resources;
using BH.SDK.Models.SettingGroups;
using BH.SDK.Rules.Attributes;
using BH.SDK.Versions;
using Newtonsoft.Json;

namespace BH.SDK.Models
{
    // TODO allow null objects, this will save many space on serialization
    // TODO add MORE rules

    /// <summary>
    /// The level itself - the root of level.json/.bson. Four independent aggregates, each its own
    /// versioned domain. Note what is NOT here: the level's name, authors and licensing live in a
    /// separate LevelMeta file, so listing levels never means loading them.
    /// </summary>
    [RuleContainer]
    [DataVersion(DataDomains.Level, 1, 0)]
    public class Level : IModel<Level>
    {
        /// <summary> Timeline shape and id counters - framerate, length, and the next free object
        /// and audio id. </summary>
        [RuleNotNull]
        [JsonProperty(Names.Settings)]
        public LevelSettings Settings { get; set; }

        /// <summary> Objects and the level-global event tracks: what is seen and what happens. </summary>
        [RuleNotNull]
        [JsonProperty(Names.Game)]
        public GameLevel Game { get; set; }

        /// <summary> Scheduled audio tracks - separate from Game because sound is placed on the
        /// timeline, not in the scene. </summary>
        [RuleNotNull]
        [JsonProperty(Names.Audio)]
        public AudioLevel Audio { get; set; }

        /// <summary> Everything the above two reference by id: textures, fonts, clips, colliders,
        /// themes, effects, prefabs. </summary>
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
        public void Reset()
        {
            Settings.Reset();
            Game.Reset();
            Audio.Reset();
            Resources.Reset();
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