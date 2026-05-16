using System;
using BHSDK.Models;
using BHSDK.Models.Interfaces.SaveData;
using BHSDK.Models.Objects;
using BHSDK.Models.Values;

namespace BHSDK.Serialization
{
    public class CompatibilityService
    {
        public Level Convert(ILevel level)
        {
            return (Level)level;
        }
        public Settings Convert(ISettings settings)
        {
            return (Settings)settings;
        }
        public Prefab Convert(IPrefab prefab)
        {
            return (Prefab)prefab;
        }
        public EffectObject Convert(IEffect effect)
        {
            return (EffectObject)effect;
        }
        public Theme Convert(ITheme theme)
        {
            return (Theme)theme;
        }
        
        public Type GetLevelType(Version version)
        {
            return typeof(Level);
        }
        public Type GetSettingsType(Version version)
        {
            return typeof(Settings);
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
    }
}