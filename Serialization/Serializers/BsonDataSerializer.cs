using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace BH.SDK.Serialization.Serializers
{
    // BSON implementation of IDataSerializer - see NewtonsoftDataSerializer for the shared envelope
    // logic. Reuses the exact same JsonSerializer/converter chain as JsonDataSerializer; only the
    // raw reader/writer over the byte stream differs. See VERSION-UPDATE.md, "Format-agnosticism".
    public class BsonDataSerializer : BaseNewtonsoftDataSerializer
    {
        public BsonDataSerializer(JsonSerializer serializer) : base(serializer)
        {
        }

        public override SerializationType Type => SerializationType.Bson;

        protected override JsonWriter CreateWriter(Stream stream) =>
            new BsonDataWriter(stream) { CloseOutput = false };

        protected override JsonReader CreateReader(Stream stream) =>
            new BsonDataReader(stream) { CloseInput = false };
    }
}
