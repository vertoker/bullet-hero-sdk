using System.Collections.Generic;
using BHSDK.Models.Keyframes;
using Newtonsoft.Json;

namespace BHSDK.Models.Game
{
    public class PlayerEvents
    {
        // TODO add in the future with events
        // [JsonProperty(ModelNames.Velocity)]
        // public List<Velocity> Velocities { get; set; }
        // [JsonProperty(ModelNames.Velocity + ModelNames.Point)]
        // public List<VelocityPoint> VelocityPoints { get; set; }
        
        [JsonProperty(Names.Visibles)]
        public List<BoolKey> Visibles { get; set; } // player can see himself
        
        [JsonProperty(Names.Controls)]
        public List<BoolKey> Controls { get; set; } // player can control himself
        
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
    }
}