using System;
using System.Collections.Generic;
using System.Linq;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BHSDK.Models.NoGame
{
    [RuleContainer]
    public class LevelMeta : ICopyable<LevelMeta>, IEquatable<LevelMeta>
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
        public LevelMeta(IString levelName, Version levelVersion, Guid levelGuid, List<Author> authors)
        {
            LevelName = levelName;
            LevelVersion = levelVersion;
            LevelGuid = levelGuid;
            Authors = authors;
        }

        public object Clone() => Copy();
        public LevelMeta Copy() => new(LevelName.Copy(), (Version)LevelVersion.Clone(), LevelGuid, Authors.CopyList());

        public override bool Equals(object obj) => obj is LevelMeta value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(LevelName, LevelVersion, LevelGuid, Authors.GetListHashCode());

        public bool Equals(LevelMeta other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = LevelName.Equals(other.LevelName)
                          && LevelVersion.Equals(other.LevelVersion)
                          && LevelGuid.Equals(other.LevelGuid)
                          && Authors.ListEquals(other.Authors);
            return result;
        }
    }
}