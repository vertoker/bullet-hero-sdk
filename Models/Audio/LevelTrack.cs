using System.Collections.Generic;
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

        public LevelTrack()
        {
            Title = string.Empty;
            Author = string.Empty;
            StartTime = 0;
            EndTime = 0;
            Sources = new List<LevelTrackSource>();
        }
        public LevelTrack(string title, string author, float startTime, float endTime, List<LevelTrackSource> sources)
        {
            Title = title;
            Author = author;
            StartTime = startTime;
            EndTime = endTime;
            Sources = sources;
        }
    }
}