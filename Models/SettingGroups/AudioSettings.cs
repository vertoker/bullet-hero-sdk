using System;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Values;
using BHSDK.Rules.Attributes;

namespace BHSDK.Models.SettingGroups
{
    [RuleContainer]
    public class AudioSettings : ICopyable<AudioSettings>, IEquatable<AudioSettings>
    {
        [RuleInRange(0f, 1f)]
        public float Volume { get; set; }
        
        [RuleInRange(0f, 1f)]
        public float Game { get; set; }
        
        [RuleInRange(0f, 1f)]
        public float UI { get; set; }

        public AudioSettings()
        {
            Volume = 1f;
            Game = 1f;
            UI = 1f;
        }
        public AudioSettings(float volume, float game, float ui)
        {
            Volume = volume;
            Game = game;
            UI = ui;
        }

        public object Clone() => Copy();
        public AudioSettings Copy() => new(Volume, Game, UI);

        public override int GetHashCode() => HashCode.Combine(Volume, Game, UI);
        public override bool Equals(object obj) => obj is AudioSettings value && Equals(value);

        public bool Equals(AudioSettings other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Volume.Equals(other.Volume)
                   && Game.Equals(other.Game)
                   && UI.Equals(other.UI);
        }
    }
}