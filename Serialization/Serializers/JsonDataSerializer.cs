using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace BH.SDK.Serialization.Serializers
{
    // JSON implementation of IDataSerializer - see NewtonsoftDataSerializer for the shared envelope
    // logic. Works uniformly for every [DataVersion] domain, top-level (Level, Theme, UserSettings,
    // ...) or nested (LevelSettings, GameLevel, ...) alike, since VersionedEnvelopeConverter.CanConvert
    // is gated purely on the attribute being present, not on a fixed type list.
    //
    // Serves both Json and JsonPretty: the formatting is set on the writer per instance rather than
    // on the shared JsonSerializer, so the two modes can coexist without one save's indentation
    // leaking into another's. Reading ignores it entirely.
    public class JsonDataSerializer : BaseNewtonsoftDataSerializer
    {
        private readonly SerializationType _type;

        public JsonDataSerializer(JsonSerializer serializer, SerializationType type = SerializationType.Json)
            : base(serializer)
        {
            _type = type;
        }

        public override SerializationType Type => _type;

        protected override JsonWriter CreateWriter(Stream stream) =>
            new JsonTextWriter(new StreamWriter(stream, Encoding.UTF8, 1024, true))
                { Formatting = _type.ToFormatting() };

        protected override JsonReader CreateReader(Stream stream) =>
            new JsonTextReader(new StreamReader(stream, Encoding.UTF8, false, 1024, true));
    }
}
