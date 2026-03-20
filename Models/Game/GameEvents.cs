using System.Collections.Generic;
using BHSDK.Models.Components;
using BHSDK.Models.Events;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Game
{
    public class GameEvents
    {
        [JsonProperty("m")]
        public List<Marker> Markers { get; set; }
        
        [JsonProperty("c")]
        public List<Checkpoint> Checkpoints { get; set; }
        
        [JsonProperty("b")]
        public List<Clr> Backgrounds { get; set; }

        public GameEvents()
        {
            Markers = new List<Marker>();
            Checkpoints = new List<Checkpoint>();
            Backgrounds = new List<Clr>();
        }
    }
}