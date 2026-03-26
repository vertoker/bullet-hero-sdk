using System;
using BHSDK.Models.Interfaces.SaveData;
using Newtonsoft.Json;

namespace BHSDK.Models.Settings
{
    public class EditorSettings : IEditorSettings
    {
        public Version GetVersion() => new(1, 0);
        
        [JsonProperty(ModelNames.Autosave)]
        public bool Autosave { get; set; }
        
        [JsonProperty(ModelNames.AutosaveRate)]
        public float AutosaveRate { get; set; }

        public EditorSettings()
        {
            Autosave = true;
            AutosaveRate = 60f;
        }
        public EditorSettings(bool autosave, float autosaveRate)
        {
            Autosave = autosave;
            AutosaveRate = autosaveRate;
        }
    }
}