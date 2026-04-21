using System;
using BHSDK.Models;
using BHSDK.Models.Objects;
using BHSDK.Models.SaveData;
using BHSDK.Models.Settings;
using BHSDK.Models.Values;

namespace BHSDK.Serialization
{
    public class CompatibilityService
    {
        public Type GetLevelType(Version version)
        {
            return typeof(Level);
        }
        public Type GetPrefabType(Version version)
        {
            return typeof(Prefab);
        }
        public Type GetEffectType(Version version)
        {
            return typeof(EffectObject);
        }
        public Type GetThemeType(Version version)
        {
            return typeof(Theme);
        }
        public Type GetPlayerSettingsType(Version version)
        {
            return typeof(PlayerSettings);
        }
        public Type GetEditorSettingsType(Version version)
        {
            return typeof(EditorSettings);
        }
    }
}