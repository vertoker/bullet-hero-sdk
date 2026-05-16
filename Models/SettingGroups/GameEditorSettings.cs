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
        
        [JsonProperty(Names.EditorCameraMinSize)]
        public float EditorCameraMinSize { get; set; }
        
        [JsonProperty(Names.EditorCameraMaxSize)]
        public float EditorCameraMaxSize { get; set; }

        public GameEditorSettings()
        {
            Autosave = true;
            AutosaveRate = 60f;
            MaxAutosaveFiles = 25;
            EditorCameraMinSize = 0.1f;
            EditorCameraMaxSize = 100f;
        }
        public GameEditorSettings(bool autosave, float autosaveRate, int maxAutosaveFiles,
            float editorCameraMinSize, float editorCameraMaxSize)
        {
            Autosave = autosave;
            AutosaveRate = autosaveRate;
            MaxAutosaveFiles = maxAutosaveFiles;
            EditorCameraMinSize = editorCameraMinSize;
            EditorCameraMaxSize = editorCameraMaxSize;
        }
    }
}