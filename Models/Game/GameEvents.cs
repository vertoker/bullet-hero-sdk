using System.Collections.Generic;
using BHSDK.Models.Components;
using BHSDK.Models.Events;
using Newtonsoft.Json;

namespace BHSDK.Models.Game
{
    public class GameEvents
    {
        [JsonProperty(ModelNames.Marker)]
        public List<Marker> Markers { get; set; }
        
        [JsonProperty(ModelNames.Checkpoint)]
        public List<Checkpoint> Checkpoints { get; set; }
        
        [JsonProperty(ModelNames.Background)]
        public List<Clr> Backgrounds { get; set; }
        
        [JsonProperty(ModelNames.Theme)]
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