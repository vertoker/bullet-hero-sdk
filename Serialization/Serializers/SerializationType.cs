namespace BH.SDK.Serialization.Serializers
{
    // Identifies which IDataSerializer wire format a byte[] envelope was written with - see
    // VERSION-UPDATE.md, "Format-agnosticism". Byte-backed so it's cheap to persist alongside the
    // envelope bytes themselves (e.g. a leading tag byte) when the format needs to be recovered at
    // read time instead of being known ahead of time by the caller.
    public enum SerializationType : byte
    {
        Json = 0,
        Bson = 1,
        // TODO add format for optimal AI generation
    }
}
