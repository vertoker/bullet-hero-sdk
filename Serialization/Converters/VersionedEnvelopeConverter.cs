using System;
using System.Collections.Generic;
using System.Reflection;
using BH.SDK.Models;
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
        // Domains currently being written/read one level up the call stack. Suppresses CanConvert
        // just for that domain so the serializer.Serialize/ToObject calls below - which re-enter this
        // same converter for the exact value being wrapped - fall through to plain member
        // serialization instead of re-wrapping it in another envelope. A differently-domained nested
        // aggregate (e.g. GameLevel's own [DataVersion] inside Level) is a different domain, so it
        // stays convertible and recurses into this converter normally - this is how "aggregated"
        // domains get every nested envelope written/upgraded without any special-casing.
        private readonly HashSet<string> _activeDomains = new();

        public override bool CanConvert(Type objectType) =>
            VersionedTypeRegistry.CanConvert(objectType) && !_activeDomains.Contains(VersionedTypeRegistry.GetDomain(objectType));

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var type = value.GetType();
            var attribute = type.GetCustomAttribute<DataVersionAttribute>();

            writer.WriteStartObject();

            // serialize version
            writer.WritePropertyName(Names.Version);
            serializer.Serialize(writer, attribute.Version);

            // serialize data
            writer.WritePropertyName(Names.Value);
            _activeDomains.Add(attribute.Domain);
            serializer.Serialize(writer, value);
            _activeDomains.Remove(attribute.Domain);

            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            if (reader.TokenType != JsonToken.StartObject)
                throw new JsonSerializationException($"Expected start of versioned envelope for '{objectType}'");

            var domain = VersionedTypeRegistry.GetDomain(objectType);
            var jObject = JObject.Load(reader);

            // load version
            var versionToken = jObject[Names.Version];
            if (versionToken == null)
                throw new JsonSerializationException($"Missing '{Names.Version}' property for domain '{domain}'");
            var version = versionToken.ToObject<Version>(serializer);

            var concreteType = VersionedTypeRegistry.Resolve(domain, version.Major, version.Minor);

            var valueToken = jObject[Names.Value];
            
            _activeDomains.Add(domain);
            var raw = valueToken?.ToObject(concreteType, serializer);
            _activeDomains.Remove(domain);

            // load and convert version type
            return VersionedTypeRegistry.UpgradeToLatest(domain, raw, version.Major, version.Minor);
        }
    }
}
