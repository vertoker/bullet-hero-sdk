using System;
using Newtonsoft.Json;

namespace BHSDK.Serialization
{
    public class LevelData
    {
        public const string VersionPropertyName = "v";
        public const string LevelPropertyName = "l";
        
        [JsonProperty(VersionPropertyName)]
        public Version Version;
        
        [JsonProperty(LevelPropertyName)]
        public ILevel Level;

        public LevelData()
        {
            
        }
        public LevelData(ILevel level)
        {
            Version = level.GetVersion();
            Level = level;
        }
    }
}