using System;
using System.Collections.Generic;
using System.IO;
using BHSDK.Models;
using BHSDK.Models.Interfaces.SaveData;
using BHSDK.Models.Objects;
using BHSDK.Models.SaveData;
using BHSDK.Models.Values;
using BHSDK.Serialization.Converters;
using BHSDK.Serialization.Converters.CustomTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BHSDK.Serialization
{
    public class SerializationService
    {
        public readonly CompatibilityService CompatibilityService;
        public readonly JsonSerializer Serializer;
        
        public SerializationService(SerializationSettings serializationSettings)
            : this(serializationSettings, new CompatibilityService())
        {
            
        }
        public SerializationService(SerializationSettings serializationSettings,
            CompatibilityService compatibilityService)
        {
            var contractResolver = new ContractResolver(serializationSettings);

            var settingsDefault = new JsonSerializerSettings
            {
                Formatting = serializationSettings.Formatting,
                TypeNameHandling = serializationSettings.TypeNameHandling,
                ContractResolver = contractResolver,
            };
            var innerSerializer = JsonSerializer.CreateDefault(settingsDefault);
            
            var settings = new JsonSerializerSettings
            {
                Formatting = serializationSettings.Formatting,
                TypeNameHandling = serializationSettings.TypeNameHandling,
                ContractResolver = contractResolver,
                Converters = GetConverters(compatibilityService, innerSerializer),
            };
            Serializer = JsonSerializer.Create(settings);
            CompatibilityService = compatibilityService;
        }

        public static List<JsonConverter> GetConverters(CompatibilityService compatibilityService,
            JsonSerializer innerSerializer)
        {
            return new List<JsonConverter>
            {
                new VersionConverter(),
                
                // Effect is also serialized by ObjectConverter.
                // We already know the type (EffectObject), parse it via default serializer 
                new EffectDataConverter(compatibilityService, innerSerializer),
                
                new LevelDataConverter(compatibilityService),
                new PrefabDataConverter(compatibilityService),
                new ThemeDataConverter(compatibilityService),
                new SettingsDataConverter(compatibilityService),
                
                new IntConverter(innerSerializer),
                new FloatConverter(innerSerializer),
                new StringConverter(innerSerializer),
                new ColorConverter(innerSerializer),
                new Vector2Converter(innerSerializer),
                new Vector3Converter(innerSerializer),
                new Vector4Converter(innerSerializer),

                new EffectShapeConverter(innerSerializer),
                new EffectAngleConverter(innerSerializer),
                new EffectScaleConverter(innerSerializer),
                new EffectColorConverter(innerSerializer),
                new EffectShapeSpreadConverter(innerSerializer),

                new ScreenLimitConverter(innerSerializer),
                new ObjectConverter(innerSerializer),
            };
        }
        
        public class ContractResolver : DefaultContractResolver
        {
            private readonly SerializationSettings _serializationSettings;

            public ContractResolver(SerializationSettings serializationSettings)
            {
                _serializationSettings = serializationSettings;
            }
            protected override JsonObjectContract CreateObjectContract(Type objectType)
            {
                var contract = base.CreateObjectContract(objectType);
                
                contract.MemberSerialization = _serializationSettings.MemberSerialization;
        
                return contract;
            }
        }
        
        public string SerializeLevel(ILevel level)
        {
            if (level == null) return string.Empty;
            var data = new LevelData(level);
            
            using var textWriter = new StringWriter();
            Serializer.Serialize(textWriter, data);
            
            var json = textWriter.ToString();
            return json;
        }
        public string SerializeSettings(ISettings settings)
        {
            if (settings == null) return string.Empty;
            var data = new SettingsData(settings);
            
            using var textWriter = new StringWriter();
            Serializer.Serialize(textWriter, data);
            
            var json = textWriter.ToString();
            return json;
        }
        public string SerializePrefab(IPrefab prefab)
        {
            if (prefab == null) return string.Empty;
            var data = new PrefabData(prefab);
            
            using var textWriter = new StringWriter();
            Serializer.Serialize(textWriter, data);
            
            var json = textWriter.ToString();
            return json;
        }
        public string SerializeEffect(IEffect effect)
        {
            if (effect == null) return string.Empty;
            var data = new EffectData(effect);
            
            using var textWriter = new StringWriter();
            Serializer.Serialize(textWriter, data);
            
            var json = textWriter.ToString();
            return json;
        }
        public string SerializeTheme(ITheme theme)
        {
            if (theme == null) return string.Empty;
            var data = new ThemeData(theme);
            
            using var textWriter = new StringWriter();
            Serializer.Serialize(textWriter, data);
            
            var json = textWriter.ToString();
            return json;
        }
        
        public Level DeserializeLevel(string json)
        {
            using var stringReader = new StringReader(json);
            using var jsonTextReader = new JsonTextReader(stringReader);
            
            var data = Serializer.Deserialize<LevelData>(jsonTextReader);
            var obj = CompatibilityService.Convert(data.Level);
            
            return obj;
        }
        public Settings DeserializeSettings(string json)
        {
            using var stringReader = new StringReader(json);
            using var jsonTextReader = new JsonTextReader(stringReader);
            
            var data = Serializer.Deserialize<SettingsData>(jsonTextReader);
            var obj = CompatibilityService.Convert(data.Settings);
            
            return obj;
        }
        public Prefab DeserializePrefab(string json)
        {
            using var stringReader = new StringReader(json);
            using var jsonTextReader = new JsonTextReader(stringReader);
            
            var data = Serializer.Deserialize<PrefabData>(jsonTextReader);
            var obj = CompatibilityService.Convert(data.Prefab);
            
            return obj;
        }
        public EffectObject DeserializeEffect(string json)
        {
            using var stringReader = new StringReader(json);
            using var jsonTextReader = new JsonTextReader(stringReader);
            
            var data = Serializer.Deserialize<EffectData>(jsonTextReader);
            var obj = CompatibilityService.Convert(data.Effect);
            
            return obj;
        }
        public Theme DeserializeTheme(string json)
        {
            using var stringReader = new StringReader(json);
            using var jsonTextReader = new JsonTextReader(stringReader);
            
            var data = Serializer.Deserialize<ThemeData>(jsonTextReader);
            var obj = CompatibilityService.Convert(data.Theme);
            
            return obj;
        }
        
        // TODO add BSON serialization (from Newtonsoft of course)
    }
}