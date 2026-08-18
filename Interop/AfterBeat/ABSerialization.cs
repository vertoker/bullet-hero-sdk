using System;
using System.Collections.Generic;
using System.Globalization;
using BH.SDK.Interop.AfterBeat.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BH.SDK.Interop.AfterBeat
{
    // These documents deliberately do NOT go through SerializationService, and that is the point of
    // the whole Interop/ folder rather than an optimisation. That service exists to read and write
    // THIS project's format: it wraps every [DataVersion] aggregate in a {"version", "value"}
    // envelope, and it installs two dozen converters implementing this format's own polymorphism.
    // Both would corrupt a foreign document - an envelope Afterbeat cannot read, and a value
    // converter looking for a [typeEnum, payload] pair in a file that has never heard of one.
    //
    // So: a bare serializer, OptIn contracts (every property here is explicitly named), and
    // nothing else. Unknown members are ignored on read because ABNode's extension data
    // catches them instead.

    /// <summary> Reading and writing the Afterbeat documents, with their own serializer. </summary>
    public static class ABSerialization
    {
        private static readonly JsonSerializerSettings Settings = new()
        {
            ContractResolver = new OptInResolver(),
            MissingMemberHandling = MissingMemberHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,

            // Same reason SerializationService needs it: without Replace, a property already
            // holding an instance from its own constructor is POPULATED rather than replaced, and
            // a list authored in the file is appended to the default one instead of taking its
            // place.
            ObjectCreationHandling = ObjectCreationHandling.Replace,

            // A level folder is authored on somebody else's machine in somebody else's locale, and
            // a float is a float everywhere.
            Culture = System.Globalization.CultureInfo.InvariantCulture,

            // These documents carry no dates, and letting Newtonsoft guess one out of a string that
            // merely looks like one is how a level name becomes a timestamp.
            DateParseHandling = DateParseHandling.None,

            Converters = { new LenientIntConverter() },
        };

        private static readonly JsonSerializer Serializer = JsonSerializer.Create(Settings);

        /// <summary> The serializer these documents are read and written with. Exposed so a caller
        /// working on a JToken tree uses the same contracts. </summary>
        public static JsonSerializer GetSerializer() => Serializer;

        public static VgdLevel DeserializeLevel(string json) => Deserialize<VgdLevel>(json);
        public static VgmMeta DeserializeMeta(string json) => Deserialize<VgmMeta>(json);
        public static VgtTheme DeserializeTheme(string json) => Deserialize<VgtTheme>(json);
        public static VgpPrefab DeserializePrefab(string json) => Deserialize<VgpPrefab>(json);

        /// <summary> Reads one document. Throws <see cref="JsonException"/> on malformed JSON - a
        /// file that is not JSON at all is not something a converter can report its way around. </summary>
        public static T Deserialize<T>(string json) where T : ABNode
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentException("Afterbeat document is empty.", nameof(json));

            return JsonConvert.DeserializeObject<T>(json, Settings);
        }

        /// <summary> Reads one document without throwing. </summary>
        public static bool TryDeserialize<T>(string json, out T value, out string error)
            where T : ABNode
        {
            try
            {
                value = Deserialize<T>(json);
                error = null;
                return value != null;
            }
            catch (Exception exception)
            {
                value = null;
                error = exception.Message;
                return false;
            }
        }

        /// <summary> Writes one document. Indented by default: a level folder is a thing people
        /// open, and Afterbeat writes its own files readably. </summary>
        public static string Serialize<T>(T value, bool indented = true) where T : ABNode
            => JsonConvert.SerializeObject(value, indented ? Formatting.Indented : Formatting.None, Settings);

        // OptIn rather than the default, matching SerializationService's own choice for the same
        // reason: a property becomes part of the wire format by being named, never by existing.
        //
        // Forced in CreateProperties rather than by assigning contract.MemberSerialization after
        // base.CreateObjectContract has run - by then the property list has already been collected
        // under the old mode, so the assignment reads as if it worked and filters nothing. The
        // extension-data member is wired up separately by the base contract and is unaffected.
        private class OptInResolver : DefaultContractResolver
        {
            protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
                => base.CreateProperties(type, MemberSerialization.OptIn);
        }

        // Afterbeat writes whole numbers as FLOATS wherever its own editor happened to hold one in a
        // float field: an object's depth arrives as 59.0, a gradient rotation as 0.0, a parallax
        // animation length likewise. Newtonsoft refuses that for an int property and throws, which
        // takes the whole document with it - two of the three levels in the author's own corpus
        // failed to open at all, on their very first object, and the error named a type mismatch
        // rather than anything an author could act on.
        //
        // Retyping the fields would be the wrong fix twice over: they ARE integers (a depth of 59.5
        // means nothing), and the next float-shaped int in a document nobody has read yet would fail
        // exactly the same way. Rounding on the way in is what the source game itself does.
        private class LenientIntConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
                => objectType == typeof(int) || objectType == typeof(int?);

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                JsonSerializer serializer)
            {
                switch (reader.TokenType)
                {
                    case JsonToken.Null:
                        return objectType == typeof(int?) ? null : 0;

                    case JsonToken.Integer:
                        return Convert.ToInt32(reader.Value, CultureInfo.InvariantCulture);

                    case JsonToken.Float:
                        return (int)Math.Round(Convert.ToDouble(reader.Value, CultureInfo.InvariantCulture),
                            MidpointRounding.AwayFromZero);

                    case JsonToken.String:
                    {
                        var text = (string)reader.Value;
                        if (double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed))
                            return (int)Math.Round(parsed, MidpointRounding.AwayFromZero);
                        break;
                    }
                }

                throw new JsonSerializationException(
                    $"Expected a number for {objectType.Name}, found {reader.TokenType} at {reader.Path}.");
            }

            // Written back as a plain integer: this is a lenient READER, and writing 59.0 because
            // that is how the file happened to say 59 would spread the problem rather than absorb it.
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                if (value == null) writer.WriteNull();
                else writer.WriteValue(Convert.ToInt32(value, CultureInfo.InvariantCulture));
            }
        }
    }
}
