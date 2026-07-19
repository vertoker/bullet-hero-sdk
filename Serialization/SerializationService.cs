using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BH.SDK.Models;
using BH.SDK.Models.Interfaces.SaveData;
using BH.SDK.Serialization.Converters;
using BH.SDK.Serialization.Converters.Base;
using BH.SDK.Serialization.Converters.CustomTypes;
using BH.SDK.Serialization.Converters.Data;
using BH.SDK.Serialization.Converters.Dict;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BH.SDK.Serialization
{
    public class SerializationService
    {
        public readonly CompatibilityService CompatibilityService;
        public readonly JsonSerializer Serializer;
        
        public SerializationService() : this(new SerializationSettings(), new CompatibilityService())
        {
            
        }
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
                Formatting = serializationSettings.formatting,
                TypeNameHandling = serializationSettings.typeNameHandling,
                ContractResolver = contractResolver,
            };

            var settings = new JsonSerializerSettings
            {
                Formatting = serializationSettings.formatting,
                TypeNameHandling = serializationSettings.typeNameHandling,
                ContractResolver = contractResolver,
                Converters = GetConverters(compatibilityService, settingsDefault),
            };
            Serializer = JsonSerializer.Create(settings);
            CompatibilityService = compatibilityService;
        }
        
        public static List<JsonConverter> GetConverters(CompatibilityService compatibilityService,
            JsonSerializerSettings settingsDefault)
        {
            var converters = new List<JsonConverter>
            {
                new VersionConverter(),

                new DictionaryObjectsConverter(),
                new DictionaryAudiosConverter(),
                
                new DictionaryTextureResourcesConverter(),
                new DictionaryFontResourcesConverter(),
                new DictionaryAudioResourcesConverter(),
                new DictionaryCompositeColliderResourcesConverter(),
                new DictionaryThemesConverter(),
                new DictionaryPrefabsConverter(),

                new EffectDataConverter(compatibilityService),

                new LevelDataConverter(compatibilityService),
                new LevelMetaDataConverter(compatibilityService),
                new PrefabDataConverter(compatibilityService),
                new ThemeDataConverter(compatibilityService),
                new UserSettingsDataConverter(compatibilityService),

                new PrimitiveIntConverter(),
                new PrimitiveFloatConverter(),

                new IntConverter(),
                new FloatConverter(),
                new StringConverter(),
                new ColorConverter(),
                new Vector2Converter(),
                new Vector3Converter(),
                new Vector4Converter(),

                new EffectShapeConverter(),
                new EffectAngleConverter(),
                new EffectScaleConverter(),
                new EffectColorConverter(),
                new EffectShapeSpreadConverter(),

                new ScreenLimitConverter(),
                new Color4X4KeyConverter(),
                new ObjectConverter(),
            };

            // Some converters above resolve a concrete implementation of a polymorphic type and need a
            // private "default" JsonSerializer to populate that concrete type's own members (see
            // IRequiresDefaultSerializer for why). Wired automatically here, so adding a new converter of
            // that kind to the list above is the only step a future change needs - nothing here has to change.
            foreach (var converter in converters.OfType<IRequiresDefaultSerializer>())
            {
                var excluded = new HashSet<JsonConverter>(converter.GetExcludedConverters(converters));
                var defaultSerializer = JsonSerializer.CreateDefault(settingsDefault);
                foreach (var other in converters)
                {
                    if (!excluded.Contains(other))
                        defaultSerializer.Converters.Add(other);
                }
                converter.SetDefaultSerializer(defaultSerializer);
            }

            return converters;
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
                
                contract.MemberSerialization = _serializationSettings.memberSerialization;
        
                return contract;
            }
        }
        
        public string SerializeData<TValue>(TValue value) where TValue : IData
        {
            if (value == null) return string.Empty;
            var data = new SaveData<TValue>(value);
            
            using var textWriter = new StringWriter();
            Serializer.Serialize(textWriter, data);
            
            var json = textWriter.ToString();
            return json;
        }
        public TValue DeserializeData<TValue>(string json) where TValue : IData
        {
            using var stringReader = new StringReader(json);
            using var jsonTextReader = new JsonTextReader(stringReader);

            var saveDataType = CompatibilityService.GetSaveDataType<TValue>();
            var data = (ISaveData)Serializer.Deserialize(jsonTextReader, saveDataType);
            var value = CompatibilityService.Convert<TValue>(data.GetData());
            
            return value;
        }
        
        // TODO add BSON serialization (from Newtonsoft of course)
    }
}