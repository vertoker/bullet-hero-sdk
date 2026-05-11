using System;
using System.Collections.Generic;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.NoGame
{
    [RuleContainer]
    public class LevelMeta
    {
        [RuleNotNull(typeof(StringValue)), RuleIStringMax(ValueRules.MaxGameString)]
        [JsonProperty(Names.Name)]
        public IString LevelName { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Version)]
        public Version LevelVersion { get; set; }
        
        [RuleGuidNotEmpty]
        [JsonProperty(Names.LevelGuid)]
        public Guid LevelGuid { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(8)]
        [JsonProperty(Names.Authors)]
        public List<Author> Authors { get; set; }

        public LevelMeta()
        {
            LevelName = new StringValue();
            LevelVersion = new Version();
            LevelGuid = Guid.NewGuid();
            Authors = new List<Author>();
        }
        public LevelMeta(string levelName, Version levelVersion, Guid levelGuid, List<Author> authors)
        {
            LevelName = new StringValue(levelName);
            LevelVersion = levelVersion;
            LevelGuid = levelGuid;
            Authors = authors;
        }
        public LevelMeta(IString levelName, Version levelVersion, Guid levelGuid, List<Author> authors)
        {
            LevelName = levelName;
            LevelVersion = levelVersion;
            LevelGuid = levelGuid;
            Authors = authors;
        }
    }
}