using System;
using System.Collections.Generic;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Keyframes;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Game
{
    [RuleContainer]
    public class PlayerEvents : ICopyable<PlayerEvents>, IEquatable<PlayerEvents>
    {
        // TODO add in the future with events
        // [JsonProperty(ModelNames.Velocity)]
        // public List<Velocity> Velocities { get; set; }
        // [JsonProperty(ModelNames.Velocity + ModelNames.Point)]
        // public List<VelocityPoint> VelocityPoints { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxPlayerEvents)]
        [RuleCollectionSorted(nameof(BoolKey.Frame))]
        [RuleCollectionUnique(nameof(BoolKey.Frame))]
        [JsonProperty(Names.Visibles)]
        public List<BoolKey> Visibles { get; set; } // player can see himself
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxPlayerEvents)]
        [RuleCollectionSorted(nameof(BoolKey.Frame))]
        [RuleCollectionUnique(nameof(BoolKey.Frame))]
        [JsonProperty(Names.Controls)]
        public List<BoolKey> Controls { get; set; } // player can control himself
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxPlayerEvents)]
        [RuleCollectionSorted(nameof(BoolKey.Frame))]
        [RuleCollectionUnique(nameof(BoolKey.Frame))]
        [JsonProperty(Names.Collisions)]
        public List<BoolKey> Collisions { get; set; } // active collision detection system for player

        public PlayerEvents()
        {
            Visibles = new List<BoolKey>();
            Controls = new List<BoolKey>();
            Collisions = new List<BoolKey>();
        }
        public PlayerEvents(List<BoolKey> visibles, List<BoolKey> controls, List<BoolKey> collisions)
        {
            Visibles = visibles;
            Controls = controls;
            Collisions = collisions;
        }

        public object Clone() => Copy();
        public PlayerEvents Copy() => new(Visibles.CopyList(), Controls.CopyList(), Collisions.CopyList());

        public override bool Equals(object obj) => obj is PlayerEvents value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Visibles.GetListHashCode(),
            Controls.GetListHashCode(), Collisions.GetListHashCode());

        public bool Equals(PlayerEvents other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Visibles.ListEquals(other.Visibles)
                         && Controls.ListEquals(other.Controls)
                         && Collisions.ListEquals(other.Collisions);
            return result;
        }
    }
}