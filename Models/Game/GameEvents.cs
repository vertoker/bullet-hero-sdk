using System.Collections.Generic;
using BHSDK.Models.Events;
using BHSDK.Models.Keyframes;
using Newtonsoft.Json;

namespace BHSDK.Models.Game
{
    public class GameEvents
    {
        [JsonProperty(Names.Markers)]
        public List<Marker> Markers { get; set; }
        
        [JsonProperty(Names.Checkpoints)]
        public List<Checkpoint> Checkpoints { get; set; }
        
        [JsonProperty(Names.Backgrounds)]
        public List<Clr> Backgrounds { get; set; }
        
        [JsonProperty(Names.Themes)]
        public List<ThemeKeyframe> Themes { get; set; }

        public GameEvents()
        {
            Markers = new List<Marker>();
            Checkpoints = new List<Checkpoint>();
            Backgrounds = new List<Clr>();
            Themes = new List<ThemeKeyframe>();
        }
        public GameEvents(List<Marker> markers, List<Checkpoint> checkpoints, 
            List<Clr> backgrounds, List<ThemeKeyframe> themes)
        {
            Markers = markers;
            Checkpoints = checkpoints;
            Backgrounds = backgrounds;
            Themes = themes;
        }
    }
}