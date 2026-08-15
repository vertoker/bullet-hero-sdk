using System;
using Newtonsoft.Json;

namespace BH.SDK.Serialization.Serializers
{
    // Maps SerializationType to/from the on-disk file extension it's written/read with, so callers
    // can pick a serializer from an existing file (directory scan) or build a file name for a
    // caller-chosen format (save), without duplicating the ".json"/".bson" strings at every call site.
    public static class SerializationTypeExtensions
    {
        public static string ToFileExtension(this SerializationType type)
        {
            return type switch
            {
                SerializationType.Json => ".json",
                SerializationType.JsonPretty => ".json",
                SerializationType.Bson => ".bson",
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        // The only place a serialization mode becomes a Newtonsoft Formatting. It lives here rather
        // than on SerializationSettings because indentation is a property of the mode a caller picks
        // per save, not of the shared JsonSerializer every save goes through.
        public static Formatting ToFormatting(this SerializationType type)
        {
            return type switch
            {
                SerializationType.Json => Formatting.None,
                SerializationType.JsonPretty => Formatting.Indented,
                SerializationType.Bson => Formatting.None,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        // Deliberately never yields JsonPretty: indentation is unrecoverable from a file, and both
        // JSON modes read identically - see SerializationType's own header.
        public static bool TryFromFileExtension(string extension, out SerializationType type)
        {
            switch (extension.ToLowerInvariant())
            {
                case ".json":
                    type = SerializationType.Json;
                    return true;
                case ".bson":
                    type = SerializationType.Bson;
                    return true;
                default:
                    type = default;
                    return false;
            }
        }
    }
}
