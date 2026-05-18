using System;
using System.Collections.Generic;
using System.IO;
using BHSDK.Models;
using BHSDK.Models.Interfaces.SaveData;
using BHSDK.Models.Objects;
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
                new UserSettingsDataConverter(compatibilityService),
                
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