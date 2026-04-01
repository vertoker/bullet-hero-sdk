using System.Collections.Generic;
using BHSDK.Models.Components;
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
        public List<Bln> Visible { get; set; }
        
        [JsonProperty(ModelNames.Collision)]
        public List<Bln> Collisions { get; set; }

        public PlayerEvents()
        {
            Visible = new List<Bln>();
            Collisions = new List<Bln>();
        }

        public PlayerEvents(List<Bln> visible, List<Bln> collisions)
        {
            Visible = visible;
            Collisions = collisions;
        }
    }
}