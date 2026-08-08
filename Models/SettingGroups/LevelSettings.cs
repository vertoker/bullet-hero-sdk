using System;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Primitives;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using BH.SDK.Versions;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.SettingGroups
{
    /// <summary>
    /// The level's timeline shape and its id counters. Also the level-scope IObjectIdCounter, while
    /// the objects those ids belong to live on GameLevel - the two halves a Prefab carries on one
    /// class are split here, so anything creating objects must hold both.
    /// </summary>
    [RuleContainer]
    [DataVersion(DataDomains.LevelSettings, 1, 0)]
    public class LevelSettings : IObjectIdCounter, IFrameDuration, IModel<LevelSettings>
    {
        /// <summary> Frames per second the level is authored in - fixes what one frame means, and so
        /// what every keyframe's Frame refers to. Not a rendering framerate. </summary>
        [RuleInRange(FrameRules.MinFramerate, FrameRules.MaxFramerate)]
        [JsonProperty(Names.Fps)]
        public int Framerate { get; set; }

        /// <summary> Total length of the level in frames; every keyframe is validated against it. </summary>
        [RuleInRange(FrameRules.MinFrameDuration, FrameRules.MaxFrameDuration)]
        [JsonProperty(Names.FrameDurationShort)]
        public int FrameDuration { get; set; }

        /// <summary> Next free object id. Only ever grows - ids of deleted objects are never reused,
        /// so a stale reference can't silently point at a different object. </summary>
        [RuleInRange(ObjectId.MinLevelValue, LevelRules.MaxObjects)]
        [JsonProperty(Names.ObjectIdCounter)]
        public int ObjectIdCounter { get; set; }

        /// <summary> Next free audio track id, same never-reused rule. </summary>
        [RuleMin(AudioId.MinValue)]
        [JsonProperty(Names.AudioIdCounter)]
        public int AudioIdCounter { get; set; }

        /// <summary> Advisory per-frame capacity measurement, refreshed on every editor save - see
        /// LevelCapacityHint. Optional: a level written before this field existed simply has none. </summary>
        [RuleNotNull]
        [JsonProperty(Names.LimitHints)]
        public LevelLimitHints LimitHints { get; set; }

        public ObjectId GetNextObjectId() => new(ObjectIdCounter++);
        public AudioId GetNextAudioId() => new(AudioIdCounter++);

        public LevelSettings()
        {
            Framerate = 60;
            FrameDuration = Framerate * 10;
            ObjectIdCounter = ObjectId.MinLevelValue;
            AudioIdCounter = AudioId.MinValue;
            LimitHints = new LevelLimitHints();
        }
        public LevelSettings(int framerate, int frameDuration, int objectIdCounter, int audioIdCounter)
            : this(framerate, frameDuration, objectIdCounter, audioIdCounter, new LevelLimitHints()) { }
        
        public LevelSettings(int framerate, int frameDuration, int objectIdCounter, int audioIdCounter,
            LevelLimitHints limitHints)
        {
            Framerate = framerate;
            FrameDuration = frameDuration;
            ObjectIdCounter = objectIdCounter;
            AudioIdCounter = audioIdCounter;
            LimitHints = limitHints;
        }

        public object Clone() => Copy();
        public LevelSettings Copy() => new(Framerate, FrameDuration, ObjectIdCounter, AudioIdCounter, LimitHints?.Copy());

        public override bool Equals(object obj) => obj is LevelSettings value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Framerate, FrameDuration, ObjectIdCounter, AudioIdCounter, LimitHints);

        public void Reset()
        {
            Framerate = 60;
            FrameDuration = Framerate * 10;
            ObjectIdCounter = ObjectId.MinLevelValue;
            AudioIdCounter = AudioId.MinValue;
            LimitHints = new LevelLimitHints();
        }

        public bool Equals(LevelSettings other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Framerate.Equals(other.Framerate)
                          && FrameDuration.Equals(other.FrameDuration)
                          && ObjectIdCounter.Equals(other.ObjectIdCounter)
                          && AudioIdCounter.Equals(other.AudioIdCounter)
                          && Equals(LimitHints, other.LimitHints);
            return result;
        }
    }
}