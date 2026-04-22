using System;
using BHSDK.Models.Interfaces.SaveData;
using Newtonsoft.Json;

namespace BHSDK.Models.SaveData
{
    public class LevelData : ISaveData
    {
        [JsonProperty(Names.Version)]
        public Version Version { get; set; }
        
        [JsonProperty(Names.Level)]
        public ILevel Level { get; set; }

        public IData GetData() => Level;
        public void SetData(object data) => Level = data as ILevel;

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