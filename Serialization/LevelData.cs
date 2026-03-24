using System;
using BHSDK.Models;
using Newtonsoft.Json;

namespace BHSDK.Serialization
{
    public class LevelData
    {
        [JsonProperty(ModelNames.Version)]
        public Version Version;
        
        [JsonProperty(ModelNames.Level)]
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