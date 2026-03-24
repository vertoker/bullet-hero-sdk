using System.Collections.Generic;
using BHSDK.Models.Components;
using Newtonsoft.Json;

namespace BHSDK.Models.Game
{
    public class PlayerEvents
    {
        [JsonProperty(ModelNames.Velocity)]
        public List<Velocity> Velocities { get; set; }
        
        [JsonProperty(ModelNames.Velocity + ModelNames.Point)]
        public List<VelocityPoint> VelocityPoints { get; set; }
        
        [JsonProperty(ModelNames.Color)]
        public List<Clr> Clr { get; set; }
        
        [JsonProperty(ModelNames.Collision)]
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