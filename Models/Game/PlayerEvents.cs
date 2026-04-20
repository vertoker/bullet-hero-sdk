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
        public List<Bln> Visibles { get; set; } // player can see himself
        
        [JsonProperty(ModelNames.Control)]
        public List<Bln> Controls { get; set; } // player can control himself
        
        [JsonProperty(ModelNames.Collision)]
        public List<Bln> Collisions { get; set; } // active collision detection system for player

        public PlayerEvents()
        {
            Visibles = new List<Bln>();
            Controls = new List<Bln>();
            Collisions = new List<Bln>();
        }
        public PlayerEvents(List<Bln> visibles, List<Bln> controls, List<Bln> collisions)
        {
            Visibles = visibles;
            Controls = controls;
            Collisions = collisions;
        }
    }
}