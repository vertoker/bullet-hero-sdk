using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BHSDK.Models.NoGame
{
    public class LevelMeta
    {
        [JsonProperty(ModelNames.Name)]
        public string LevelName { get; set; }
        
        [JsonProperty(ModelNames.Version)]
        public Version LevelVersion { get; set; }
        
        [JsonProperty(ModelNames.Author)]
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