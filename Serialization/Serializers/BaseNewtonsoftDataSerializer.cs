using System;
using System.IO;
using System.Reflection;
using BH.SDK.Models;
using BH.SDK.Versions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BH.SDK.Serialization.Serializers
{
    // Shared envelope logic for every Newtonsoft-backed IDataSerializer. None of the converters in
    // Serialization/Converters/* touch JsonTextWriter/JsonTextReader specifically - they only use
    // members of the abstract JsonWriter/JsonReader - so the same JsonSerializer (and therefore the
    // same VersionedEnvelopeConverter chain) works unchanged for both JSON and BSON. Only the raw
    // reader/writer over the byte stream differs per format, which is what subclasses supply.
    // See VERSION-UPDATE.md, "Format-agnosticism".
    public abstract class BaseNewtonsoftDataSerializer : IDataSerializer
    {
        private readonly JsonSerializer _serializer;

        protected BaseNewtonsoftDataSerializer(JsonSerializer serializer)
        {
            _serializer = serializer;
        }

        public abstract SerializationType Type { get; }

        protected abstract JsonWriter CreateWriter(Stream stream);
        protected abstract JsonReader CreateReader(Stream stream);

        public byte[] SerializeEnvelope(string domain, EnvelopeData data)
        {
            if (data.RawPayload == null) return Array.Empty<byte>();

            var payloadType = data.RawPayload.GetType();
            var attribute = payloadType.GetCustomAttribute<DataVersionAttribute>();
            if (attribute == null || attribute.Domain != domain || attribute.Version != data.Version)
            {
                throw new ArgumentException(
                    $"Payload of type '{payloadType}' does not match domain '{domain}' version {data.Version}",
                    nameof(data.RawPayload));
            }

            using var stream = new MemoryStream();
            using (var writer = CreateWriter(stream))
                _serializer.Serialize(writer, data.RawPayload);
            return stream.ToArray();
        }

        public EnvelopeData DeserializeEnvelope(byte[] data, Type payloadType)
        {
            VersionedTypeRegistry.ThrowIfNoDomain(payloadType);

            using var peekStream = new MemoryStream(data);
            using var peekReader = CreateReader(peekStream);
            var jObject = JObject.Load(peekReader);

            var versionToken = jObject[Names.Version];
            if (versionToken == null)
            {
                throw new JsonSerializationException($"Missing '{Names.Version}' property for type '{payloadType}'");
            }

            var version = versionToken.ToObject<Version>(_serializer);

            // JObject.CreateReader() returns a format-agnostic JTokenReader over the already-parsed
            // tree, so this replay works the same regardless of whether jObject came from JSON or BSON.
            // VersionedEnvelopeConverter.ReadJson resolves the concrete historical type for this
            // version and upgrades it to the domain's current shape in one step, so what comes back
            // here is already current-shape, not the pre-migration snapshot - "raw" refers to it being
            // handed back untyped (object), matching the interface's format-agnostic contract.
            using var valueReader = jObject.CreateReader();
            var rawPayload = _serializer.Deserialize(valueReader, payloadType);

            return new EnvelopeData(version, rawPayload);
        }
    }
}