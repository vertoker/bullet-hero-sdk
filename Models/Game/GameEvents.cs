using System;
using System.Collections.Generic;
using BH.SDK.Models.Events;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Keyframes;
using BH.SDK.Models.Keyframes;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Game
{
    [RuleContainer]
    public class GameEvents : IModel<GameEvents>
    {
        // only for editor
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxMarkerEvents)]
        [RuleCollectionUnique(nameof(Checkpoint.Frame))]
        [JsonProperty(Names.Markers)]
        public List<Marker> Markers { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxCheckpointEvents)]
        [RuleCollectionUnique(nameof(Checkpoint.Frame))]
        [JsonProperty(Names.Checkpoints)]
        public List<Checkpoint> Checkpoints { get; set; }
        
        // limitations for screen will be chosen by mappers
        [RuleNotNull(typeof(ScreenLimitFixed))]
        [JsonProperty(Names.ScreenLimits)]
        public List<ScreenLimitKey> ScreenLimits { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxBackgroundEvents)]
        [RuleCollectionUnique(nameof(Color4Key.Frame))]
        [JsonProperty(Names.Backgrounds)]
        public List<Color4Key> Backgrounds { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxThemeEvents)]
        [RuleCollectionUnique(nameof(ThemeKeyframe.Frame))]
        [JsonProperty(Names.Themes)]
        public List<ThemeKeyframe> Themes { get; set; }

        public GameEvents()
        {
            Markers = new List<Marker>();
            Checkpoints = new List<Checkpoint>();
            ScreenLimits = new List<ScreenLimitKey>();
            Backgrounds = new List<Color4Key>();
            Themes = new List<ThemeKeyframe>();
        }
        public GameEvents(List<Marker> markers, List<Checkpoint> checkpoints, 
            List<ScreenLimitKey> screenLimits, List<Color4Key> backgrounds, List<ThemeKeyframe> themes)
        {
            Markers = markers;
            Checkpoints = checkpoints;
            ScreenLimits = screenLimits;
            Backgrounds = backgrounds;
            Themes = themes;
        }
        public void Reset()
        {
            Markers.Clear();
            Checkpoints.Clear();
            ScreenLimits.Clear();
            Backgrounds.Clear();
            Themes.Clear();
        }

        public object Clone() => Copy();
        public GameEvents Copy() => new(Markers.CopyList(), Checkpoints.CopyList(),
            ScreenLimits.CopyList(), Backgrounds.CopyList(), Themes.CopyList());

        public override bool Equals(object obj) => obj is CameraEvents value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Markers.GetListHashCode(),
            Checkpoints.GetListHashCode(), Backgrounds.GetListHashCode(), Themes.GetListHashCode());

        public bool Equals(GameEvents other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Markers.ListEquals(other.Markers)
                         && Checkpoints.ListEquals(other.Checkpoints)
                         && Backgrounds.ListEquals(other.Backgrounds)
                         && Themes.ListEquals(other.Themes);
            return result;
        }
    }
}