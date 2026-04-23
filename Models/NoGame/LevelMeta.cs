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
        
        [JsonProperty(Names.LevelId)]
        public Guid LevelId { get; set; }
        
        [JsonProperty(Names.Authors)]
        public List<Author> Authors { get; set; }

        public LevelMeta()
        {
            LevelName = new StringValue();
            LevelVersion = new Version();
            LevelId = Guid.NewGuid();
            Authors = new List<Author>();
        }
        public LevelMeta(string levelName, Version levelVersion, Guid levelId, List<Author> authors)
        {
            LevelName = new StringValue(levelName);
            LevelVersion = levelVersion;
            LevelId = levelId;
            Authors = authors;
        }
        public LevelMeta(IString levelName, Version levelVersion, Guid levelId, List<Author> authors)
        {
            LevelName = levelName;
            LevelVersion = levelVersion;
            LevelId = levelId;
            Authors = authors;
        }
    }
}