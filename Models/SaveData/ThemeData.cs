using System;
using BHSDK.Models.Interfaces.SaveData;
using Newtonsoft.Json;

namespace BHSDK.Models.SaveData
{
    public class ThemeData : ISaveData
    {
        [JsonProperty(ModelNames.Version)]
        public Version Version { get; set; }
        
        [JsonProperty(ModelNames.Theme)]
        public ITheme Theme { get; set; }

        public IData GetData() => Theme;
        public void SetData(object data) => Theme = data as ITheme;

        public ThemeData()
        {
            
        }
        public ThemeData(ITheme theme)
        {
            Version = theme.GetVersion();
            Theme = theme;
        }
    }
}