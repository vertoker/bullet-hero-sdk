using System;
using System.Collections.Generic;
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
        private static readonly List<Type> InterfaceTypes;
        private static readonly List<Type> SafeDataInterfaceTypes;
        private static int _count;

        private static void Add(Type interfaceType)
        {
            InterfaceTypes.Add(interfaceType);
            var safeDataInterface = typeof(SaveData<>).MakeGenericType(interfaceType);
            SafeDataInterfaceTypes.Add(safeDataInterface);
            _count++;
        }

        static CompatibilityService()
        {
            const int typeCapacity = 8;
            InterfaceTypes = new List<Type>(typeCapacity);
            SafeDataInterfaceTypes = new List<Type>(typeCapacity);
            
            Add(typeof(ILevel));
            Add(typeof(ILevelMeta));
            Add(typeof(IUserSettings));
            Add(typeof(IPrefab));
            Add(typeof(IEffect));
            Add(typeof(ITheme));
        }

        public Type GetSaveDataType<TValue>() where TValue : IData
        {
            for (var i = 0; i < _count; i++)
            {
                if (InterfaceTypes[i].IsAssignableFrom(typeof(TValue)))
                    return SafeDataInterfaceTypes[i];
            }
            throw new NotSupportedException($"Unsupported data type: {typeof(TValue)}");
        }
        
        public TValue Convert<TValue>(IData data) where TValue : IData
        {
            if (typeof(TValue) == typeof(IData)) return (TValue)data;
            
            if (data is ILevel level) return Convert<TValue>(level);
            if (data is ILevelMeta levelMeta) return Convert<TValue>(levelMeta);
            if (data is IUserSettings settings) return Convert<TValue>(settings);
            if (data is IPrefab prefab) return Convert<TValue>(prefab);
            if (data is IEffect effect) return Convert<TValue>(effect);
            if (data is ITheme theme) return Convert<TValue>(theme);
            
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
        public TValue Convert<TValue>(ILevelMeta data) where TValue : IData
        {
            if (typeof(TValue) == typeof(ILevelMeta)) return (TValue)data;
            if (typeof(TValue) == typeof(LevelMeta))
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
        public Type GetLevelMetaType(Version version)
        {
            if (version == LevelMeta.Version) return typeof(LevelMeta);
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