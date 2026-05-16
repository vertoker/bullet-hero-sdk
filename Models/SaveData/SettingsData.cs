using System;
using BHSDK.Models.Interfaces.SaveData;
using Newtonsoft.Json;

namespace BHSDK.Models.SaveData
{
    public class SettingsData : ISaveData
    {
        [JsonProperty(Names.Version)]
        public Version Version { get; set; }
        
        [JsonProperty(Names.Settings)]
        public ISettings Settings { get; set; }

        public IData GetData() => Settings;
        public void SetData(object data) => Settings = data as ISettings;

        public SettingsData()
        {
            
        }
        public SettingsData(ISettings settings)
        {
            Version = settings.GetVersion();
            Settings = settings;
        }
    }
}