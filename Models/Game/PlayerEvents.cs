using System.Collections.Generic;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Keyframes;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.Game
{
    [RuleContainer]
    public class PlayerEvents : ICopyable<PlayerEvents>
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
    }
}