using System;
using System.IO;
using System.Reflection;
using System.Text;
using BH.SDK.Models;
using BH.SDK.Versions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BH.SDK.Serialization
{
    // JSON implementation of IDataSerializer, backed by the same JsonSerializer (and therefore the
    // same VersionedEnvelopeConverter) that SerializationService.SerializeData/DeserializeData use -
    // no envelope-parsing logic is duplicated here. Works uniformly for every [DataVersion] domain,
    // top-level (Level, Theme, UserSettings, ...) or nested (LevelSettings, GameLevel, ...) alike,
    // since VersionedEnvelopeConverter.CanConvert is gated purely on the attribute being present, not
    // on a fixed type list.
    public class JsonDataSerializer : IDataSerializer
    {
        private readonly JsonSerializer _serializer;

        public JsonDataSerializer(JsonSerializer serializer)
        {
            _serializer = serializer;
        }

        public byte[] SerializeEnvelope(string domain, Version version, object payload)
        {
            if (payload == null) return Array.Empty<byte>();

            var payloadType = payload.GetType();
            var attribute = payloadType.GetCustomAttribute<DataVersionAttribute>();
            if (attribute == null || attribute.Domain != domain || attribute.Version != version)
            {
                throw new ArgumentException(
                    $"Payload of type '{payloadType}' does not match domain '{domain}' version {version}",
                    nameof(payload));
            }

            using var textWriter = new StringWriter();
            _serializer.Serialize(textWriter, payload);
            return Encoding.UTF8.GetBytes(textWriter.ToString());
        }

        public (Version version, object rawPayload) DeserializeEnvelope(byte[] data, Type payloadType)
        {
            // Throws if payloadType isn't a real aggregate root before touching the data.
            VersionedTypeRegistry.GetDomain(payloadType);

            var json = Encoding.UTF8.GetString(data);
            var jObject = JObject.Parse(json);

            var versionToken = jObject[Names.Version];
            if (versionToken == null)
            {
                throw new JsonSerializationException($"Missing '{Names.Version}' property for type '{payloadType}'");
            }
            var version = versionToken.ToObject<Version>(_serializer);

            // VersionedEnvelopeConverter.ReadJson resolves the concrete historical type for this
            // version and upgrades it to the domain's current shape in one step, so what comes back
            // here is already current-shape, not the pre-migration snapshot - "raw" refers to it
            // being handed back untyped (object), matching the interface's format-agnostic contract.
            using var jsonReader = jObject.CreateReader();
            var rawPayload = _serializer.Deserialize(jsonReader, payloadType);

            return (version, rawPayload);
        }
    }
}
