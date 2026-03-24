using System.Collections.Generic;
using Newtonsoft.Json;

namespace BHSDK.Models.NoGame
{
    public class LevelTrack
    {
        [JsonProperty(ModelNames.Title)]
        public string Title { get; set; }
        
        [JsonProperty(ModelNames.Author)]
        public string Author { get; set; }
        
        [JsonProperty(ModelNames.Source)]
        public List<LevelTrackSource> Sources { get; set; }

        public LevelTrack()
        {
            Title = string.Empty;
            Author = string.Empty;
            Sources = new List<LevelTrackSource>();
        }
        public LevelTrack(string title, string author, List<LevelTrackSource> sources)
        {
            Title = title;
            Author = author;
            Sources = sources;
        }
    }
}