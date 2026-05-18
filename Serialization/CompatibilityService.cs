using System;
using BHSDK.Models;
using BHSDK.Models.Interfaces.SaveData;
using BHSDK.Models.Objects;
using BHSDK.Models.Values;

namespace BHSDK.Serialization
{
    // TODO refactor this class, add MORE type checks, add at least 1 real converter
    // TODO add new attribute [DataVersion(1, 0, 0, 0)] and direct collect to dictionary
    public class CompatibilityService
    {
        public Type GetSaveDataType<TValue>() where TValue : IData
        {
            if (typeof(ILevel).IsAssignableFrom(typeof(TValue)))
                return typeof(SaveData<ILevel>);
            if (typeof(IUserSettings).IsAssignableFrom(typeof(TValue)))
                return typeof(SaveData<IUserSettings>);
            if (typeof(IPrefab).IsAssignableFrom(typeof(TValue)))
                return typeof(SaveData<IPrefab>);
            if (typeof(IEffect).IsAssignableFrom(typeof(TValue)))
                return typeof(SaveData<IEffect>);
            if (typeof(ITheme).IsAssignableFrom(typeof(TValue)))
                return typeof(SaveData<ITheme>);
            
            throw new NotSupportedException($"Unsupported data type: {typeof(TValue)}");
        }
        
        public TValue Convert<TValue>(IData data) where TValue : IData
        {
            if (typeof(TValue) == typeof(IData)) return (TValue)data;
            if (data is ILevel level)
                return Convert<TValue>(level);
            if (data is IUserSettings userSettings)
                return Convert<TValue>(userSettings);
            if (data is IPrefab prefab)
                return Convert<TValue>(prefab);
            if (data is IEffect effect)
                return Convert<TValue>(effect);
            if (data is ITheme theme)
                return Convert<TValue>(theme);
            throw new ArgumentException(typeof(TValue).Name);
        }
        
        public TValue Convert<TValue>(ILevel data) where TValue : IData
        {
            if (typeof(TValue) == typeof(ILevel)) return (TValue)data;
            if (typeof(TValue) == typeof(Level))
            {
                // var fromVersion = data.GetVersion();
                // var toVersion = Level.Version;
                return (TValue)data;
            }
            throw new ArgumentException(typeof(TValue).Name);
        }
        public TValue Convert<TValue>(IUserSettings data) where TValue : IData
        {
            if (typeof(TValue) == typeof(IUserSettings)) return (TValue)data;
            if (typeof(TValue) == typeof(UserSettings))
            {
                // var fromVersion = data.GetVersion();
                // var toVersion = UserSettings.Version;
                return (TValue)data;
            }
            throw new ArgumentException(typeof(TValue).Name);
        }
        public TValue Convert<TValue>(IPrefab data) where TValue : IData
        {
            if (typeof(TValue) == typeof(IPrefab)) return (TValue)data;
            if (typeof(TValue) == typeof(Prefab))
            {
                // var fromVersion = data.GetVersion();
                // var toVersion = Prefab.Version;
                return (TValue)data;
            }
            throw new ArgumentException(typeof(TValue).Name);
        }
        public TValue Convert<TValue>(IEffect data) where TValue : IData
        {
            if (typeof(TValue) == typeof(IEffect)) return (TValue)data;
            if (typeof(TValue) == typeof(EffectObject))
            {
                // var fromVersion = data.GetVersion();
                // var toVersion = EffectObject.Version;
                return (TValue)data;
            }
            throw new ArgumentException(typeof(TValue).Name);
        }
        public TValue Convert<TValue>(ITheme data) where TValue : IData
        {
            if (typeof(TValue) == typeof(ITheme)) return (TValue)data;
            if (typeof(TValue) == typeof(Theme))
            {
                // var fromVersion = data.GetVersion();
                // var toVersion = Theme.Version;
                return (TValue)data;
            }
            throw new ArgumentException(typeof(TValue).Name);
        }
        
        public Type GetLevelType(Version version)
        {
            if (version == Level.Version) return typeof(Level);
            throw new NotSupportedException($"Unsupported version: {version}");
        }
        public Type GetUserSettingsType(Version version)
        {
            if (version == UserSettings.Version) return typeof(UserSettings);
            throw new NotSupportedException($"Unsupported version: {version}");
        }
        public Type GetPrefabType(Version version)
        {
            if (version == Prefab.Version) return typeof(Prefab);
            throw new NotSupportedException($"Unsupported version: {version}");
        }
        public Type GetEffectType(Version version)
        {
            if (version == EffectObject.Version) return typeof(EffectObject);
            throw new NotSupportedException($"Unsupported version: {version}");
        }
        public Type GetThemeType(Version version)
        {
            if (version == Theme.Version) return typeof(Theme);
            throw new NotSupportedException($"Unsupported version: {version}");
        }
    }
}