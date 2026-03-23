using System;
using System.Collections.Generic;
using System.IO;
using BHSDK.Serialization.Converters;
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
        public SerializationService(SerializationSettings serializationSettings, CompatibilityService compatibilityService)
        {
            var contractResolver = new ContractResolver(serializationSettings);

            var settingsDefault = new JsonSerializerSettings
            {
                Formatting = serializationSettings.Formatting,
                TypeNameHandling = serializationSettings.TypeNameHandling,
                ContractResolver = contractResolver,
                Error = SerializationUtils.SerializationErrorHandling,
            };
            var innerSerializer = JsonSerializer.CreateDefault(settingsDefault);
            
            var settings = new JsonSerializerSettings
            {
                Formatting = serializationSettings.Formatting,
                TypeNameHandling = serializationSettings.TypeNameHandling,
                ContractResolver = contractResolver,
                Converters = GetConverters(compatibilityService, innerSerializer),
                Error = SerializationUtils.SerializationErrorHandling,
            };
            Serializer = JsonSerializer.Create(settings);
            CompatibilityService = compatibilityService;
        }

        public static List<JsonConverter> GetConverters(CompatibilityService compatibilityService,
            JsonSerializer innerSerializer)
        {
            return new List<JsonConverter>
            {
                new LevelDataConverter(compatibilityService),
                new VersionConverter(),

                new IntConverter(innerSerializer),
                new FloatConverter(innerSerializer),
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
                new InstanceConverter(innerSerializer),
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
        
        public const string SerializedDataMimeType = "application/unity.bullethero ";
        
        public string Serialize(ILevel level)
        {
            throw new NotImplementedException();
            
            if (level == null) return string.Empty;
            
            using var textWriter = new StringWriter();
            textWriter.Write(SerializedDataMimeType);
            Serializer.Serialize(textWriter, level);
            
            var json = textWriter.ToString();
            // if serializer doesn't write anything - header can't be added, see deserialize realization
            return json.Length == SerializedDataMimeType.Length ? string.Empty : json;

            // var json = JsonConvert.SerializeObject(level, _settings);
            // if (string.IsNullOrEmpty(json)) return json;
            // return SerializedDataMimeType + " " + json;
        }
        
        public bool CanDeserialize(string json)
        {
            throw new NotImplementedException();
            return json != null && json.StartsWith(SerializedDataMimeType);
        }
        public ILevel Deserialize(string json, bool autoUpdate = true)
        {
            throw new NotImplementedException();
            using var stringReader = new StringReader(json);
            
            if (json.StartsWith(SerializedDataMimeType))
                for (var i = 0; i < SerializedDataMimeType.Length; i++)
                    stringReader.Read();
            
            using var jsonTextReader = new JsonTextReader(stringReader);
            var level = (ILevel)Serializer.Deserialize(jsonTextReader, typeof(ILevel));
            // var level = JsonConvert.DeserializeObject<ILevel>(json, _settings);
            
            //if (autoUpdate) level = _compatibilityService.UpdateModel(level);
            return level;
        }
    }
}