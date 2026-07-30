using System;

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
                SerializationType.Bson => ".bson",
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

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
