using System;
using System.Collections.Generic;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.SaveData;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Meta;
using BHSDK.Models.Values;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models
{
    [RuleContainer]
    public class LevelMeta : ILevelMeta, ICopyable<LevelMeta>, IEquatable<LevelMeta>
    {
        public static readonly Version Version = new(1, 0);
        public Version GetVersion() => Version;
        
        [RuleGuidNotEmpty]
        [JsonProperty(Names.LevelGuid)]
        public Guid LevelGuid { get; set; }
        
        [RuleNotNull(typeof(StringValue)), RuleIStringMax(ValueRules.MaxGameString)]
        [JsonProperty(Names.Name)]
        public IString LevelName { get; set; }
        
        [RuleNotNull(typeof(StringValue)), RuleIStringMax(ValueRules.MaxEditorDescription)]
        [JsonProperty(Names.Description)]
        public IString LevelDescription { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Version)]
        public Version LevelVersion { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxAuthors)]
        [JsonProperty(Names.Authors)]
        public List<Author> LevelAuthors { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.ResourcesMeta)]
        public List<ResourceMeta> ResourcesMeta { get; set; }

        public LevelMeta()
        {
            LevelGuid = Guid.NewGuid();
            LevelName = new StringValue();
            LevelDescription = new StringValue();
            LevelVersion = new Version();
            LevelAuthors = new List<Author>();
            ResourcesMeta = new List<ResourceMeta>();
        }
        public LevelMeta(Guid levelGuid, IString levelName, IString levelDescription,
            Version levelVersion, List<Author> levelAuthors, List<ResourceMeta> resourcesMeta)
        {
            LevelGuid = levelGuid;
            LevelName = levelName;
            LevelDescription = levelDescription;
            LevelVersion = levelVersion;
            LevelAuthors = levelAuthors;
            ResourcesMeta = resourcesMeta;
        }

        public object Clone() => Copy();
        public LevelMeta Copy() => new(LevelGuid, LevelName.Copy(), LevelDescription.Copy(),
            (Version)LevelVersion.Clone(), LevelAuthors.CopyList(), ResourcesMeta.CopyList());

        public override bool Equals(object obj) => obj is LevelMeta value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(LevelGuid, LevelName, LevelDescription,
            LevelVersion, LevelAuthors.GetListHashCode(), ResourcesMeta.GetListHashCode());

        public bool Equals(LevelMeta other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = LevelGuid.Equals(other.LevelGuid) 
                         && LevelName.Equals(other.LevelName)
                         && LevelDescription.Equals(other.LevelDescription)
                         && LevelVersion.Equals(other.LevelVersion)
                         && LevelAuthors.ListEquals(other.LevelAuthors)
                         && ResourcesMeta.ListEquals(other.ResourcesMeta);
            return result;
        }
    }
}