using System.Collections.Generic;
using BHSDK.Models.Events;
using BHSDK.Models.Keyframes;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Game
{
    [RuleContainer]
    public class GameEvents
    {
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxMarkerEvents)]
        [RuleCollectionSorted(nameof(Marker.Frame))]
        [JsonProperty(Names.Markers)]
        public List<Marker> Markers { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxCheckpointEvents)]
        [RuleCollectionSorted(nameof(Checkpoint.Frame))]
        [RuleCollectionUnique(nameof(Checkpoint.Frame))]
        [JsonProperty(Names.Checkpoints)]
        public List<Checkpoint> Checkpoints { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxBackgroundEvents)]
        [RuleCollectionSorted(nameof(Clr.Frame))]
        [RuleCollectionUnique(nameof(Clr.Frame))]
        [JsonProperty(Names.Backgrounds)]
        public List<Clr> Backgrounds { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxThemeEvents)]
        [RuleCollectionSorted(nameof(ThemeKeyframe.Frame))]
        [RuleCollectionUnique(nameof(ThemeKeyframe.Frame))]
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