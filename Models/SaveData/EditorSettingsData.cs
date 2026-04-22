using System;
using BHSDK.Models.Interfaces.SaveData;
using Newtonsoft.Json;

namespace BHSDK.Models.SaveData
{
    public class EditorSettingsData : ISaveData
    {
        [JsonProperty(Names.Version)]
        public Version Version { get; set; }
        
        [JsonProperty(Names.EditorSettings)]
        public IEditorSettings EditorSettings { get; set; }

        public IData GetData() => EditorSettings;
        public void SetData(object data) => EditorSettings = data as IEditorSettings;

        public EditorSettingsData()
        {
            
        }
        public EditorSettingsData(IEditorSettings editorSettings)
        {
            Version = editorSettings.GetVersion();
            EditorSettings = editorSettings;
        }
    }
}