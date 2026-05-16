using System;
using BHSDK.Models.Interfaces.SaveData;
using Newtonsoft.Json;

namespace BHSDK.Models.SaveData
{
    public class UserSettingsData : ISaveData
    {
        [JsonProperty(Names.Version)]
        public Version Version { get; set; }
        
        [JsonProperty(Names.Settings)]
        public IUserSettings UserSettings { get; set; }

        public IData GetData() => UserSettings;
        public void SetData(object data) => UserSettings = data as IUserSettings;

        public UserSettingsData()
        {
            
        }
        public UserSettingsData(IUserSettings userSettings)
        {
            Version = userSettings.GetVersion();
            UserSettings = userSettings;
        }
    }
}