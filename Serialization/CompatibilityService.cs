using System;
using BHSDK.Models;
using BHSDK.Models.Interfaces.SaveData;
using BHSDK.Models.Objects;
using BHSDK.Models.Values;

namespace BHSDK.Serialization
{
    // TODO refactor this class, add MORE type checks, add at least 1 real converter
    public class CompatibilityService
    {
        public TValue Convert<TValue>(IData data) where TValue : IData
        {
            if (data is ILevel level) return (TValue)level;
            if (data is IUserSettings userSettings) return (TValue)userSettings;
            if (data is IPrefab prefab) return (TValue)prefab;
            if (data is IEffect effect) return (TValue)effect;
            if (data is ITheme theme) return (TValue)theme;
            
            throw new ArgumentException(typeof(TValue).Name);
        }
        
        public Level Convert(ILevel level)
        {
            return (Level)level;
        }
        public UserSettings Convert(IUserSettings userSettings)
        {
            return (UserSettings)userSettings;
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
        public Type GetUserSettingsType(Version version)
        {
            return typeof(UserSettings);
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