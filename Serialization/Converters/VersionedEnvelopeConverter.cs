using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BH.SDK.Models;
using BH.SDK.Serialization.Converters.CustomTypes;
using BH.SDK.Versions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BH.SDK.Serialization.Converters
{
    // Single converter for every SaveData kind and any internal aggregate that opts into
    // versioning via [DataVersion]. Replaces the old per-kind JsonConverterData<T> subclasses and
    // CompatibilityService entirely - see VERSION-UPDATE.md. CanConvert is gated purely on the
    // attribute being present, so this recurses correctly into nested aggregates without any
    // special-casing for "aggregated vs non-aggregated" models.
    public class VersionedEnvelopeConverter : JsonConverter
    {
        private readonly Dictionary<string, JsonSerializer> _valueSerializers = new();

        public override bool CanConvert(Type objectType) => VersionedTypeRegistry.CanConvert(objectType);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var type = value.GetType();
            var attribute = type.GetCustomAttribute<DataVersionAttribute>();
            var valueSerializer = GetValueSerializer(attribute.Domain, serializer);

            writer.WriteStartObject();

            writer.WritePropertyName(Names.Version);
            serializer.Serialize(writer, attribute.Version);

            writer.WritePropertyName(Names.Value);
            valueSerializer.Serialize(writer, value);

            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            if (reader.TokenType != JsonToken.StartObject)
                throw new JsonSerializationException($"Expected start of versioned envelope for '{objectType}'");

            var domain = VersionedTypeRegistry.GetDomain(objectType);
            var jObject = JObject.Load(reader);

            var versionToken = jObject[Names.Version];
            if (versionToken == null)
                throw new JsonSerializationException($"Missing '{Names.Version}' property for domain '{domain}'");
            var version = versionToken.ToObject<Version>(serializer);

            var concreteType = VersionedTypeRegistry.Resolve(domain, version.Major, version.Minor);
            var valueSerializer = GetValueSerializer(domain, serializer);

            var valueToken = jObject[Names.Value];
            var raw = valueToken?.ToObject(concreteType, valueSerializer);

            return VersionedTypeRegistry.UpgradeToLatest(domain, raw, version.Major, version.Minor);
        }

        private JsonSerializer GetValueSerializer(string domain, JsonSerializer serializer)
        {
            if (_valueSerializers.TryGetValue(domain, out var cached))
                return cached;

            var settings = new JsonSerializerSettings
            {
                Formatting = serializer.Formatting,
                TypeNameHandling = serializer.TypeNameHandling,
                ContractResolver = serializer.ContractResolver,
            };
            var valueSerializer = JsonSerializer.Create(settings);
            foreach (var converter in serializer.Converters)
            {
                if (converter == this) continue;
                valueSerializer.Converters.Add(converter);
            }

            _valueSerializers[domain] = valueSerializer;
            return valueSerializer;
        }
    }
}
