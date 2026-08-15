namespace BH.SDK.Serialization.Serializers
{
    // Identifies which IDataSerializer wire format a byte[] envelope was written with - see
    // VERSION-UPDATE.md, "Format-agnosticism". Byte-backed so it's cheap to persist alongside the
    // envelope bytes themselves (e.g. a leading tag byte) when the format needs to be recovered at
    // read time instead of being known ahead of time by the caller.
    //
    // JsonPretty is a WRITE-ONLY distinction: it produces the same document as Json with indentation
    // added, shares its ".json" extension, and is read back by the very same reader. Nothing can
    // recover it from a file, which is why TryFromFileExtension resolves ".json" to Json alone - the
    // choice belongs to whoever is saving, not to the file. Adding a member here is also why every
    // consumer must go through SerializationTypeExtensions instead of testing == Bson: a two-branch
    // ternary silently reads a third member as Json.
    public enum SerializationType : byte
    {
        Json = 0,
        Bson = 1,
        JsonPretty = 2,
        // TODO add format for optimal AI generation
    }
}
