using System;
using System.Collections.Generic;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Keyframes;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using BH.SDK.Versions;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Game
{
    /// <summary>
    /// Level-driven overrides of the player's own state. Three independent switch tracks, so a level
    /// can e.g. keep the player visible but take control away during a cutscene section.
    /// </summary>
    [RuleContainer]
    [DataVersion(DataDomains.PlayerEvents, 1, 0)]
    public class PlayerEvents : IModel<PlayerEvents>
    {
        // TODO add in the future with events
        // [JsonProperty(ModelNames.Velocity)]
        // public List<Velocity> Velocities { get; set; }
        // [JsonProperty(ModelNames.Velocity + ModelNames.Point)]
        // public List<VelocityPoint> VelocityPoints { get; set; }
        
        /// <summary> Whether the player avatar is drawn. Hiding it does not make it safe - see
        /// Collisions. </summary>
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxPlayerKeys)]
        [RuleCollectionUnique(nameof(BoolKey.Frame))]
        [JsonProperty(Names.Visibles)]
        public List<BoolKey> Visibles { get; set; } // player can see himself

        /// <summary> Whether input moves the player. Off freezes them in place while the level keeps
        /// running. </summary>
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxPlayerKeys)]
        [RuleCollectionUnique(nameof(BoolKey.Frame))]
        [JsonProperty(Names.Controls)]
        public List<BoolKey> Controls { get; set; } // player can control himself

        /// <summary> Whether the player can be hit at all - the authored equivalent of invulnerability
        /// during a transition. </summary>
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxPlayerKeys)]
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
        public void Reset()
        {
            Visibles.Clear();
            Controls.Clear();
            Collisions.Clear();
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