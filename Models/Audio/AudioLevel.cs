using System;
using System.Collections.Generic;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Primitives;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Audio
{
    [RuleContainer]
    public class AudioLevel : ICopyable<AudioLevel>, IEquatable<AudioLevel>
    {
        [RuleNotNull]
        [JsonProperty(Names.Tracks)]
        public Dictionary<AudioId, LevelTrack> Tracks { get; set; }

        public AudioLevel()
        {
            Tracks = new Dictionary<AudioId, LevelTrack>();
        }
        public AudioLevel(Dictionary<AudioId, LevelTrack> tracks)
        {
            Tracks = tracks;
        }

        public object Clone() => Copy();
        public AudioLevel Copy() => new(Tracks.CopyDictionary());

        public bool Equals(AudioLevel other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Tracks.DictionaryEquals(other.Tracks);
            return result;
        }

        public override bool Equals(object obj) => obj is AudioLevel value && Equals(value);
        public override int GetHashCode() => Tracks != null ? Tracks.GetDictionaryHashCode() : 0;
    }
}