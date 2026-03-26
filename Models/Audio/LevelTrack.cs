using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace BHSDK.Models.Audio
{
    public class LevelTrack
    {
        [JsonProperty(ModelNames.Title)]
        public string Title { get; set; }
        
        [JsonProperty(ModelNames.Author)]
        public string Author { get; set; }
        
        [JsonProperty(ModelNames.Start + ModelNames.Time)]
        public float StartTime { get; set; }
        
        [JsonProperty(ModelNames.End + ModelNames.Time)]
        public float EndTime { get; set; }
        
        [JsonProperty(ModelNames.Source)]
        public List<LevelTrackSource> Sources { get; set; }
        
        [JsonProperty(ModelNames.AudioEffect)] [CanBeNull]
        public LevelTrackEffects Effects { get; set; }
        
        public bool HasEffects() => Effects != null;

        public LevelTrack()
        {
            Title = string.Empty;
            Author = string.Empty;
            StartTime = 0;
            EndTime = 0;
            Sources = new List<LevelTrackSource>();
            Effects = new LevelTrackEffects();
        }
        public LevelTrack(string title, string author, float startTime, float endTime, 
            List<LevelTrackSource> sources, LevelTrackEffects effects)
        {
            Title = title;
            Author = author;
            StartTime = startTime;
            EndTime = endTime;
            Sources = sources;
            Effects = effects;
        }
    }
}