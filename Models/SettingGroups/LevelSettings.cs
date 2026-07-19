using System;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Primitives;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.SettingGroups
{
    [RuleContainer]
    public class LevelSettings : ICopyable<LevelSettings>, IEquatable<LevelSettings>
    {
        [RuleInRange(FrameRules.MinFramerate, FrameRules.MaxFramerate)]
        [JsonProperty(Names.Fps)]
        public int Framerate { get; set; }
        
        [RuleMin(FrameRules.MinFrameLength)]
        [JsonProperty(Names.FrameLengthShort)]
        public int FrameLength { get; set; }
        
        [RuleMin(ObjectId.MinLevelValue)]
        [JsonProperty(Names.ObjectIdCounter)]
        public int ObjectIdCounter { get; set; }

        [RuleMin(AudioId.MinValue)]
        [JsonProperty(Names.AudioIdCounter)]
        public int AudioIdCounter { get; set; }

        public ObjectId GetNextObjectId() => new(ObjectIdCounter++);
        public AudioId GetNextAudioId() => new(AudioIdCounter++);

        public LevelSettings()
        {
            Framerate = 60;
            FrameLength = Framerate * 10;
            ObjectIdCounter = ObjectId.MinLevelValue;
            AudioIdCounter = AudioId.MinValue;
        }
        public LevelSettings(int framerate, int frameLength, int objectIdCounter, int audioIdCounter)
        {
            Framerate = framerate;
            FrameLength = frameLength;
            ObjectIdCounter = objectIdCounter;
            AudioIdCounter = audioIdCounter;
        }

        public object Clone() => Copy();
        public LevelSettings Copy() => new(Framerate, FrameLength, ObjectIdCounter, AudioIdCounter);

        public override bool Equals(object obj) => obj is LevelSettings value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Framerate, FrameLength, ObjectIdCounter, AudioIdCounter);

        public bool Equals(LevelSettings other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Framerate.Equals(other.Framerate)
                          && FrameLength.Equals(other.FrameLength)
                          && ObjectIdCounter.Equals(other.ObjectIdCounter)
                          && AudioIdCounter.Equals(other.AudioIdCounter);
            return result;
        }
    }
}