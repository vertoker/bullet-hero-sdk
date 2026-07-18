using System;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Primitives;
using BH.SDK.Models.Primitives.Resources;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Audio
{
    [RuleContainer]
    public class LevelTrack : IFrameBounds, INameable, ICopyable<LevelTrack>, IEquatable<LevelTrack>
    {
        // Same logic as RectObject.ObjectId, but only for audio and much simpler
        // 0 - undefined
        // 1, 2, 3... - user-defined audio
        // negative space IS BANNED for consistency
        
        [RuleIPrimitiveIntMin(AudioId.MinValue)]
        [JsonProperty(Names.AudioId)]
        public AudioId AudioId { get; set; }
        
        [RuleLevelFrame]
        [JsonProperty(Names.StartFrameShort)]
        public int StartFrame { get; set; }
        
        [RuleLevelFrame]
        [JsonProperty(Names.EndFrameShort)]
        public int EndFrame { get; set; }
        
        // Offset for audio clip itself. Frames tells where boundaries of track in level,
        // OffsetTime tells from which time starts clip itself
        [JsonProperty(Names.OffsetTime)]
        public float OffsetTime { get; set; }
        
        [RuleInRange(AudioRules.MinAudioLayer, AudioRules.MaxAudioLayer)]
        [JsonProperty(Names.AudioLayer)]
        public int AudioLayer { get; set; }
        
        [RuleNotNull, RuleStringMax(ValueRules.MaxEditorName)]
        [JsonProperty(Names.Name)]
        public string Name { get; set; }
        
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
            OffsetTime = 0f;
            AudioLayer = AudioRules.MinAudioLayer;
            Name = string.Empty;
            AudioResourceId = AudioResourceId.Null;
            Effects = new LevelTrackEffects();
        }
        public LevelTrack(AudioId audioId, int startFrame, int endFrame, float offsetTime,
            int audioLayer, string name, AudioResourceId audioResourceId, LevelTrackEffects effects)
        {
            AudioId = audioId;
            StartFrame = startFrame;
            EndFrame = endFrame;
            OffsetTime = offsetTime;
            AudioLayer = audioLayer;
            Name = name;
            AudioResourceId = audioResourceId;
            Effects = effects;
        }

        public object Clone() => Copy();
        public LevelTrack Copy() => new(AudioId, StartFrame, EndFrame,
            OffsetTime, AudioLayer, Name, AudioResourceId, Effects.Copy());

        public override bool Equals(object obj) => obj is LevelTrack value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(AudioId,
            StartFrame, EndFrame, OffsetTime, AudioResourceId, Effects);

        public bool Equals(LevelTrack other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = AudioId.Equals(other.AudioId)
                         && StartFrame.Equals(other.StartFrame)
                         && EndFrame.Equals(other.EndFrame)
                         && OffsetTime.Equals(other.OffsetTime)
                         && AudioResourceId.Equals(other.AudioResourceId)
                         && Effects.Equals(other.Effects);
            return result;
        }
    }
}