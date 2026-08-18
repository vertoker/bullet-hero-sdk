using System;
using System.Collections.Generic;
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
    // nothing else. Unknown members are ignored on read because AfterBeatNode's extension data
    // catches them instead.

    /// <summary> Reading and writing the Afterbeat documents, with their own serializer. </summary>
    public static class AfterBeatSerialization
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
        public static T Deserialize<T>(string json) where T : AfterBeatNode
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentException("Afterbeat document is empty.", nameof(json));

            return JsonConvert.DeserializeObject<T>(json, Settings);
        }

        /// <summary> Reads one document without throwing. </summary>
        public static bool TryDeserialize<T>(string json, out T value, out string error)
            where T : AfterBeatNode
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
        public static string Serialize<T>(T value, bool indented = true) where T : AfterBeatNode
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
    }
}
