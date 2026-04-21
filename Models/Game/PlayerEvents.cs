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
        
        [JsonProperty(ModelNames.Visible)]
        public List<BoolKey> Visibles { get; set; } // player can see himself
        
        [JsonProperty(ModelNames.Control)]
        public List<BoolKey> Controls { get; set; } // player can control himself
        
        [JsonProperty(ModelNames.Collision)]
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