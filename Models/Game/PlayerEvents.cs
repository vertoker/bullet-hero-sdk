using System.Collections.Generic;
using BHSDK.Models.Components;
using Newtonsoft.Json;

namespace BHSDK.Models.Game
{
    public class PlayerEvents
    {
        [JsonProperty("v")]
        public List<Velocity> Velocities { get; set; }
        
        [JsonProperty("vp")]
        public List<VelocityPoint> VelocityPoints { get; set; }
        
        [JsonProperty("clr")]
        public List<Clr> Clr { get; set; }
        
        [JsonProperty("clsn")]
        public List<Bln> Collisions { get; set; }

        public PlayerEvents()
        {
            Velocities = new List<Velocity>();
            VelocityPoints = new List<VelocityPoint>();
            Clr = new List<Clr>();
            Collisions = new List<Bln>();
        }
        public PlayerEvents(List<Velocity> velocities, 
            List<VelocityPoint> velocityPoints, List<Clr> clr, List<Bln> collisions)
        {
            Velocities = velocities;
            VelocityPoints = velocityPoints;
            Clr = clr;
            Collisions = collisions;
        }
    }
}