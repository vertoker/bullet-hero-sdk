using System;
using System.Collections.Generic;
using BH.SDK.Models.Interfaces;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Audio
{
    [RuleContainer]
    public class AudioLevel : ICopyable<AudioLevel>, IEquatable<AudioLevel>
    {
        // because master channels created only in Unity
        public const int FrameTrackLimit = 16;
        
        [RuleNotNull]
        [JsonProperty(Names.Tracks)]
        public List<LevelTrack> Tracks { get; set; }

        public AudioLevel()
        {
            Tracks = new List<LevelTrack>();
        }
        public AudioLevel(List<LevelTrack> tracks)
        {
            Tracks = tracks;
        }

        public object Clone() => Copy();
        public AudioLevel Copy() => new(Tracks.CopyList());

        public bool Equals(AudioLevel other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Tracks.ListEquals(other.Tracks);
            return result;
        }

        public override bool Equals(object obj) => obj is AudioLevel value && Equals(value);
        public override int GetHashCode() => Tracks != null ? Tracks.GetListHashCode() : 0;
    }
}