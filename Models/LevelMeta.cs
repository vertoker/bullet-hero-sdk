using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BHSDK.Models
{
    public class LevelMeta
    {
        [JsonProperty("ln")]
        public string LevelName { get; set; }
        
        [JsonProperty("lv")]
        public Version LevelVersion { get; set; }
        
        [JsonProperty("as")]
        public List<Author> Authors { get; set; }

        public LevelMeta()
        {
            LevelName = string.Empty;
            LevelVersion = new Version();
            Authors = new List<Author>();
        }
        public LevelMeta(string levelName, Version levelVersion, List<Author> authors)
        {
            LevelName = levelName;
            LevelVersion = levelVersion;
            Authors = authors;
        }
    }
}