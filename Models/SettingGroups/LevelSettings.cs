using System;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Values;
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
        [RuleMin(FrameRules.MinFramerate)]
        [JsonProperty(Names.Fps)]
        public int Framerate { get; set; }
        
        [RuleMin(FrameRules.MinFrameLength)]
        [JsonProperty(Names.FrameLengthShort)]
        public int FrameLength { get; set; }
        
        [RuleNotNull(typeof(ScreenLimitFixed))]
        [JsonProperty(Names.ScreenLimit)]
        public IScreenLimit ScreenLimit { get; set; }
        // limitations for screen will be chosen by mappers
        
        [RuleMin(IdRules.MinUserObjectIdCounter)]
        [JsonProperty(Names.ObjectIdCounter)]
        public int ObjectIdCounter { get; set; }
        
        // TODO add other counters (audio, resources, colliders)
        
        public int GetNextObjectId() => ObjectIdCounter++;

        public LevelSettings()
        {
            Framerate = 60;
            FrameLength = Framerate * 10;
            ScreenLimit = new ScreenLimitNone();
            ObjectIdCounter = IdRules.MinUserObjectIdCounter;
        }
        public LevelSettings(int framerate, int frameLength, IScreenLimit screenLimit, int objectIdCounter)
        {
            Framerate = framerate;
            FrameLength = frameLength;
            ScreenLimit = screenLimit;
            ObjectIdCounter = objectIdCounter;
        }

        public object Clone() => Copy();
        public LevelSettings Copy() => new(Framerate, FrameLength, ScreenLimit.Copy(), ObjectIdCounter);

        public override bool Equals(object obj) => obj is LevelSettings value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Framerate, FrameLength, ScreenLimit, ObjectIdCounter);

        public bool Equals(LevelSettings other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Framerate.Equals(other.Framerate)
                          && FrameLength.Equals(other.FrameLength)
                          && ScreenLimit.Equals(other.ScreenLimit)
                          && ObjectIdCounter.Equals(other.ObjectIdCounter);
            return result;
        }
    }
}