using Newtonsoft.Json;

namespace BHSDK.Models.SettingGroups
{
    public class GameEditorSettings
    {
        // Savings
        
        [JsonProperty(Names.Autosave)]
        public bool Autosave { get; set; }
        
        [JsonProperty(Names.AutosaveRate)]
        public float AutosaveRate { get; set; }
        
        [JsonProperty(Names.MaxAutosaveFiles)]
        public int MaxAutosaveFiles { get; set; }
        
        // Editor Camera
        
        [JsonProperty(Names.CameraMinSize)]
        public float CameraMinSize { get; set; }
        
        [JsonProperty(Names.CameraMaxSize)]
        public float CameraMaxSize { get; set; }

        public GameEditorSettings()
        {
            Autosave = true;
            AutosaveRate = 60f;
            MaxAutosaveFiles = 25;
            CameraMinSize = 0.1f;
            CameraMaxSize = 100f;
        }
        public GameEditorSettings(bool autosave, float autosaveRate, int maxAutosaveFiles,
            float cameraMinSize, float cameraMaxSize)
        {
            Autosave = autosave;
            AutosaveRate = autosaveRate;
            MaxAutosaveFiles = maxAutosaveFiles;
            CameraMinSize = cameraMinSize;
            CameraMaxSize = cameraMaxSize;
        }
    }
}