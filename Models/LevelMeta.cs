using System;
using System.Collections.Generic;
using BH.SDK.Models.Enum.Meta;
using BH.SDK.Models.Enum.Resources;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.SaveData;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Meta;
using BH.SDK.Models.Resources;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using Newtonsoft.Json;

namespace BH.SDK.Models
{
    // TODO add IResetable (and tests)
    
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
        [JsonProperty(Names.Logo)]
        public ResourceKey LevelLogo { get; set; }
        
        [RuleNotNull(1, 0)]
        [JsonProperty(Names.Version)]
        public Version LevelVersion { get; set; }
        
        // Level can have any license, but typical it's a 2 choice
        // CC BY-NC or CC BY-NC-SA, they both have incompatible resources (because of ShareAlike)
        // If you don't know what to choose - choose CC BY-NC
        
        [RuleNotNull(typeof(TypicalLicense), TypicalLicenseType.CC_BY_NC_4_0)]
        [JsonProperty(Names.License)]
        public ILicense LevelLicense { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(ResourceRules.MaxAuthors)]
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
            LevelLogo = new ResourceKey(ResourceUriType.LevelPath, FileNames.LogoFileNamePng);
            LevelVersion = new Version(1, 0);
            LevelLicense = new TypicalLicense(TypicalLicenseType.CC_BY_NC_4_0);
            LevelAuthors = new List<Author>();
            ResourcesMeta = new List<ResourceMeta>();
        }
        public LevelMeta(Guid levelGuid, IString levelName, IString levelDescription, ResourceKey levelLogo,
            Version levelVersion, ILicense levelLicense, List<Author> levelAuthors, List<ResourceMeta> resourcesMeta)
        {
            LevelGuid = levelGuid;
            LevelName = levelName;
            LevelDescription = levelDescription;
            LevelLogo = levelLogo;
            LevelVersion = levelVersion;
            LevelLicense = levelLicense;
            LevelAuthors = levelAuthors;
            ResourcesMeta = resourcesMeta;
        }

        public object Clone() => Copy();
        public LevelMeta Copy() => new(LevelGuid, LevelName.Copy(), LevelDescription.Copy(),
            LevelLogo.Copy(), (Version)LevelVersion.Clone(), LevelLicense.Copy(),
            LevelAuthors.CopyList(), ResourcesMeta.CopyList());

        public override bool Equals(object obj) => obj is LevelMeta value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(LevelGuid, LevelName.Copy(), LevelDescription.Copy(),
            LevelLogo.Copy(), LevelVersion, LevelLicense.Copy(),
            LevelAuthors.GetListHashCode(), ResourcesMeta.GetListHashCode());

        public bool Equals(LevelMeta other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = LevelGuid.Equals(other.LevelGuid) 
                         && LevelName.Equals(other.LevelName)
                         && LevelDescription.Equals(other.LevelDescription)
                         && LevelLogo.Equals(other.LevelLogo)
                         && LevelVersion.Equals(other.LevelVersion)
                         && LevelLicense.Equals(other.LevelLicense)
                         && LevelAuthors.ListEquals(other.LevelAuthors)
                         && ResourcesMeta.ListEquals(other.ResourcesMeta);
            return result;
        }
    }
}