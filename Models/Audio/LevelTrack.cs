using System;
using System.Collections.Generic;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Resources;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BHSDK.Models.Audio
{
    [RuleContainer]
    public class LevelTrack : ICopyable<LevelTrack>, IEquatable<LevelTrack>
    {
        // Same logic as Object.ObjectId, but only for audio and much simpler
        // 0 - undefined
        // 1, 2, 3... - user-defined audio
        // negative space IS BANNED for consistency
        
        [RuleMin(1)]
        [JsonProperty(Names.AudioId)]
        public int AudioId { get; set; }
        
        public const int UndefinedAudioId = 0;
        
        [RuleLevelFrame]
        [JsonProperty(Names.StartFrameShort)]
        public int StartFrame { get; set; }
        
        [RuleLevelFrame]
        [JsonProperty(Names.EndFrameShort)]
        public int EndFrame { get; set; }
        
        [JsonProperty(Names.OffsetLocalTime)]
        public float OffsetLocalTime { get; set; }
        
        // positive with 0 - game-defined (0 is silence), negative - user-defined
        // more about resourceId and how it works, read in Resource.cs file
        
        [JsonProperty(Names.AudioResourceId)]
        public int AudioResourceId { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Effects)]
        public LevelTrackEffects Effects { get; set; }
        
        public LevelTrack()
        {
            AudioId = 0;
            StartFrame = 0;
            EndFrame = 0;
            OffsetLocalTime = 0f;
            AudioResourceId = 0;
            Effects = new LevelTrackEffects();
        }
        public LevelTrack(int audioId, int startFrame, int endFrame,
            float offsetLocalTime, int audioResourceId, LevelTrackEffects effects)
        {
            AudioId = audioId;
            StartFrame = startFrame;
            EndFrame = endFrame;
            OffsetLocalTime = offsetLocalTime;
            AudioResourceId = audioResourceId;
            Effects = effects;
        }

        public object Clone() => Copy();
        public LevelTrack Copy() => new(AudioId, StartFrame, EndFrame, OffsetLocalTime, AudioResourceId, Effects.Copy());

        public override bool Equals(object obj) => obj is LevelTrack value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(AudioId,
            StartFrame, EndFrame, OffsetLocalTime, AudioResourceId, Effects);

        public bool Equals(LevelTrack other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = AudioId.Equals(other.AudioId)
                         && StartFrame.Equals(other.StartFrame)
                         && EndFrame.Equals(other.EndFrame)
                         && OffsetLocalTime.Equals(other.OffsetLocalTime)
                         && AudioResourceId.Equals(other.AudioResourceId)
                         && Effects.Equals(other.Effects);
            return result;
        }
    }
}