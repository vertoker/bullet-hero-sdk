using System;

namespace BH.SDK.Serialization
{
    // Format-agnostic envelope contract (requirement 3, see VERSION-UPDATE.md "Format-agnosticism").
    // Only the envelope read/write step is format-specific - resolving a version tag to a concrete
    // type and walking the migration chain to a domain's current shape both live in
    // BH.SDK.Versions.VersionedTypeRegistry and are shared by every implementation of this
    // interface. JsonDataSerializer is the only implementation today; a future BSON/XML one would
    // implement the same contract without touching VersionedTypeRegistry at all.
    public interface IDataSerializer
    {
        byte[] SerializeEnvelope(string domain, Version version, object payload);
        (Version version, object rawPayload) DeserializeEnvelope(byte[] data, Type payloadType);
    }
}
