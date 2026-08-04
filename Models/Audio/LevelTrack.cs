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
    /// <summary>
    /// One scheduled playback of an audio resource inside a level - the audio counterpart of a
    /// RectObject: the clip lives in Level.Resources.Audios, this only says when/where/how it plays.
    /// Stored in AudioLevel.Tracks; several tracks may overlap, separated by AudioLayer.
    /// </summary>
    [RuleContainer]
    public class LevelTrack : IFrameBounds, INameable, IModel<LevelTrack>
    {
        // Same logic as RectObject.ObjectId, but only for audio and much simpler
        // 0 - undefined
        // 1, 2, 3... - user-defined audio
        // negative space IS BANNED for consistency

        /// <summary> Identity of this track inside the level, handed out by LevelSettings.GetNextAudioId
        /// and used as the key of AudioLevel.Tracks. </summary>
        [RuleIPrimitiveIntMin(AudioId.MinValue)]
        [JsonProperty(Names.AudioId)]
        public AudioId AudioId { get; set; }

        // 0 - Null (no audio resource assigned), 1+ - game-defined, negative - user-defined
        // more about resourceId and how it works, read in TypedResourceId.cs file

        /// <summary> Which clip to play - points at an AudioResource, either a game-defined one or a
        /// user-defined entry of Level.Resources.Audios. Null means the track is silent. </summary>
        [RuleIPrimitiveIntNotNull]
        [JsonProperty(Names.AudioResourceId)]
        public AudioResourceId AudioResourceId { get; set; }

        /// <summary> Level frame the clip starts sounding at. </summary>
        [RuleLevelFrame]
        [JsonProperty(Names.StartFrameShort)]
        public int StartFrame { get; set; }

        /// <summary> Level frame the clip is cut off at, even if the clip itself is longer. </summary>
        [RuleLevelFrame]
        [JsonProperty(Names.EndFrameShort)]
        public int EndFrame { get; set; }

        // Offset for audio clip itself. Frames tells where boundaries of track in level,
        // OffsetTime tells from which time starts clip itself

        /// <summary> Seconds skipped inside the clip at StartFrame. Frames place the track on the level
        /// timeline, this places the playhead inside the clip - the two are independent. </summary>
        [JsonProperty(Names.OffsetTime)]
        public float OffsetTime { get; set; }

        // TODO integrate pitch shift, Unity not limit it, -2f - 2f should be enough for shitty atmospheric remixes (slowed/nightcore)
        // TODO also slow down original speed limits for whole level moving (-2f - 2f)

        /// <summary> Mixing slot this track occupies, so simultaneous tracks stay separable
        /// (music / sfx / voice ...). Not a render layer - unrelated to RectObject.Layer. </summary>
        [RuleInRange(AudioRules.MinAudioLayer, AudioRules.MaxAudioLayer)]
        [JsonProperty(Names.AudioLayer)]
        public int AudioLayer { get; set; }

        /// <summary> Human-readable label shown in the editor timeline. Purely cosmetic, never an
        /// identity - AudioId is. </summary>
        [RuleNotNull, RuleStringMax(ValueRules.MaxEditorName)]
        [JsonProperty(Names.Name)]
        public string Name { get; set; }

        /// <summary> Per-track DSP chain (lowpass, echo, reverb, ...). Always present as an object;
        /// whether it does anything is decided by LevelTrackEffects.Active. </summary>
        [RuleNotNull]
        [JsonProperty(Names.Effects)]
        public LevelTrackEffects Effects { get; set; }
        
        public LevelTrack()
        {
            AudioId = AudioId.Null;
            AudioResourceId = AudioResourceId.Null;
            StartFrame = FrameRules.MinFrame;
            EndFrame = FrameRules.MinFrame;
            OffsetTime = AudioRules.OffsetTimeDefault;
            AudioLayer = AudioRules.MinAudioLayer;
            Name = string.Empty;
            Effects = new LevelTrackEffects();
        }
        public LevelTrack(AudioId audioId, AudioResourceId audioResourceId, int startFrame, int endFrame,
            float offsetTime, int audioLayer, string name, LevelTrackEffects effects)
        {
            AudioId = audioId;
            AudioResourceId = audioResourceId;
            StartFrame = startFrame;
            EndFrame = endFrame;
            OffsetTime = offsetTime;
            AudioLayer = audioLayer;
            Name = name;
            Effects = effects;
        }
        public void Reset()
        {
            AudioId = AudioId.Null;
            AudioResourceId = AudioResourceId.Null;
            StartFrame = FrameRules.MinFrame;
            EndFrame = FrameRules.MinFrame;
            OffsetTime = AudioRules.OffsetTimeDefault;
            AudioLayer = AudioRules.MinAudioLayer;
            Name = string.Empty;
            Effects.Reset();
        }

        public object Clone() => Copy();
        public LevelTrack Copy() => new(AudioId, AudioResourceId, StartFrame, EndFrame,
            OffsetTime, AudioLayer, Name, Effects.Copy());

        public override bool Equals(object obj) => obj is LevelTrack value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(AudioId,
            StartFrame, EndFrame, OffsetTime, AudioResourceId, AudioLayer, Name, Effects);

        public bool Equals(LevelTrack other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = AudioId.Equals(other.AudioId)
                         && StartFrame.Equals(other.StartFrame)
                         && EndFrame.Equals(other.EndFrame)
                         && OffsetTime.Equals(other.OffsetTime)
                         && AudioResourceId.Equals(other.AudioResourceId)
                         && AudioLayer.Equals(other.AudioLayer)
                         && Name.Equals(other.Name)
                         && Effects.Equals(other.Effects);
            return result;
        }
    }
}