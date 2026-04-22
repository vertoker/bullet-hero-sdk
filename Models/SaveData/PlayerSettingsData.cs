using System;
using BHSDK.Models.Interfaces.SaveData;
using Newtonsoft.Json;

namespace BHSDK.Models.SaveData
{
    public class PlayerSettingsData : ISaveData
    {
        [JsonProperty(Names.Version)]
        public Version Version { get; set; }
        
        [JsonProperty(Names.PlayerSettings)]
        public IPlayerSettings PlayerSettings { get; set; }

        public IData GetData() => PlayerSettings;
        public void SetData(object data) => PlayerSettings = data as IPlayerSettings;

        public PlayerSettingsData()
        {
            
        }
        public PlayerSettingsData(IPlayerSettings playerSettings)
        {
            Version = playerSettings.GetVersion();
            PlayerSettings = playerSettings;
        }
    }
}