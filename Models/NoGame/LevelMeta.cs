using System;
using System.Collections.Generic;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.NoGame
{
    public class LevelMeta
    {
        [JsonProperty(Names.Name)]
        public IString LevelName { get; set; }
        
        [JsonProperty(Names.Version)]
        public Version LevelVersion { get; set; }
        
        [JsonProperty(Names.Authors)]
        public List<Author> Authors { get; set; }

        public LevelMeta()
        {
            LevelName = new StringValue();
            LevelVersion = new Version();
            Authors = new List<Author>();
        }
        public LevelMeta(string levelName, Version levelVersion, List<Author> authors)
        {
            LevelName = new StringValue(levelName);
            LevelVersion = levelVersion;
            Authors = authors;
        }
        public LevelMeta(IString levelName, Version levelVersion, List<Author> authors)
        {
            LevelName = levelName;
            LevelVersion = levelVersion;
            Authors = authors;
        }
    }
}