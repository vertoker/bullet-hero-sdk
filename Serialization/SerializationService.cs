using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BH.SDK.Models.Primitives;
using BH.SDK.Serialization.Converters;
using BH.SDK.Serialization.Converters.Base;
using BH.SDK.Serialization.Converters.CustomTypes;
using BH.SDK.Serialization.Converters.Dict;
using BH.SDK.Serialization.Serializers;
using BH.SDK.Versions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BH.SDK.Serialization
{
    public class SerializationService
    {
        public readonly JsonSerializer Serializer;

        private readonly Dictionary<SerializationType, IDataSerializer> _dataSerializers = new();

        public IDataSerializer GetDataSerializer(SerializationType type)
        {
            if (_dataSerializers.TryGetValue(type, out var dataSerializer)) return dataSerializer;

            dataSerializer = type switch
            {
                SerializationType.Json => new JsonDataSerializer(Serializer),
                SerializationType.Bson => new BsonDataSerializer(Serializer),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
            };
            _dataSerializers[type] = dataSerializer;
            return dataSerializer;
        }

        public SerializationService() : this(new SerializationSettings())
        {

        }
        public SerializationService(SerializationSettings serializationSettings)
        {
            var contractResolver = new ContractResolver(serializationSettings);

            var settingsDefault = new JsonSerializerSettings
            {
                Formatting = serializationSettings.formatting,
                TypeNameHandling = serializationSettings.typeNameHandling,
                ContractResolver = contractResolver,
                // Without this, Newtonsoft populates (appends into) a non-null nested object/list
                // property left behind by the parameterless constructor instead of replacing it -
                // e.g. EffectAngleCurvesBySpeed's ctor seeds Curve with 2 default keyframes, and
                // deserializing a JSON curve with its own keyframes would append onto those instead
                // of starting fresh, breaking round-trip equality for any model with a non-empty
                // constructor default.
                ObjectCreationHandling = ObjectCreationHandling.Replace,
            };

            var settings = new JsonSerializerSettings
            {
                Formatting = serializationSettings.formatting,
                TypeNameHandling = serializationSettings.typeNameHandling,
                ContractResolver = contractResolver,
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                Converters = GetConverters(settingsDefault),
            };
            Serializer = JsonSerializer.Create(settings);
        }

        public static List<JsonConverter> GetConverters(JsonSerializerSettings settingsDefault)
        {
            var converters = new List<JsonConverter>
            {
                new VersionConverter(),
                new VersionedEnvelopeConverter(),

                new DictionaryObjectsConverter(),
                new DictionaryAudiosConverter(),

                new DictionaryTextureResourcesConverter(),
                new DictionaryFontResourcesConverter(),
                new DictionaryAudioResourcesConverter(),
                new DictionaryCompositeColliderResourcesConverter(),
                new DictionaryThemesConverter(),
                new DictionaryEffectsConverter(),
                new DictionaryPrefabsConverter(),
                new DictionaryModificationsConverter(),
                new DictionaryAsPairListConverter<ObjectId, ObjectId>(),

                new PrimitiveIntConverter(),
                new PrimitiveGuidConverter(),
                new PrimitiveFloatConverter(),
                new FrameSpanConverter(),

                new IntConverter(),
                new FloatConverter(),
                new StringConverter(),
                new LicenseConverter(),
                new ColorConverter(),
                new Color3Converter(),
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
        
        public string SerializeData<TValue>(TValue value)
        {
            if (value == null) return string.Empty;
            
            if (!VersionedTypeRegistry.CanConvert(value.GetType()))
            {
                throw new ArgumentException(CantConvertMessage<TValue>(nameof(SerializeData)), typeof(TValue).Name);
            }

            using var textWriter = new StringWriter();
            Serializer.Serialize(textWriter, value);

            var json = textWriter.ToString();
            return json;
        }
        public TValue DeserializeData<TValue>(string json)
        {
            if (!VersionedTypeRegistry.CanConvert(typeof(TValue)))
            {
                throw new ArgumentException(CantConvertMessage<TValue>(nameof(DeserializeData)), typeof(TValue).Name);
            }

            using var stringReader = new StringReader(json);
            using var jsonTextReader = new JsonTextReader(stringReader);

            return (TValue)Serializer.Deserialize(jsonTextReader, typeof(TValue));
        }

        private static string CantConvertMessage<TValue>(string methodName)
            => $"Type '{typeof(TValue)}' has no [DataVersion] attribute and cannot be used with {methodName}";

        // TODO add BSON serialization (from Newtonsoft of course)
    }
}