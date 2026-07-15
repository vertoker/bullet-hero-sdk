using System;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Primitives;
using BH.SDK.Models.Primitives.Resources;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Audio
{
    [RuleContainer]
    public class LevelTrack : ICopyable<LevelTrack>, IEquatable<LevelTrack>
    {
        // Same logic as RectObject.ObjectId, but only for audio and much simpler
        // 0 - undefined
        // 1, 2, 3... - user-defined audio
        // negative space IS BANNED for consistency
        
        [RuleIPrimitiveIntMin(AudioId.MinValue)]
        [JsonProperty(Names.AudioId)]
        public AudioId AudioId { get; set; }
        
        public const int UndefinedAudioId = 0;
        
        [RuleLevelFrame]
        [JsonProperty(Names.StartFrameShort)]
        public int StartFrame { get; set; }
        
        [RuleLevelFrame]
        [JsonProperty(Names.EndFrameShort)]
        public int EndFrame { get; set; }
        
        [JsonProperty(Names.OffsetLocalTime)]
        public float OffsetLocalTime { get; set; }
        
        // 0 - Null (no audio resource assigned), 1+ - game-defined, negative - user-defined
        // more about resourceId and how it works, read in TypedResourceId.cs file
        
        [JsonProperty(Names.AudioResourceId)]
        public AudioResourceId AudioResourceId { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Effects)]
        public LevelTrackEffects Effects { get; set; }
        
        public LevelTrack()
        {
            AudioId = AudioId.Null;
            StartFrame = 0;
            EndFrame = 0;
            OffsetLocalTime = 0f;
            AudioResourceId = AudioResourceId.Null;
            Effects = new LevelTrackEffects();
        }
        public LevelTrack(AudioId audioId, int startFrame, int endFrame,
            float offsetLocalTime, AudioResourceId audioResourceId, LevelTrackEffects effects)
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