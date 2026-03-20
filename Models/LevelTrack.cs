using System.Collections.Generic;
using Newtonsoft.Json;

namespace BHSDK.Models
{
    public class LevelTrack
    {
        [JsonProperty("t")]
        public string Title { get; set; }
        
        [JsonProperty("a")]
        public string Author { get; set; }
        
        [JsonProperty("ss")]
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