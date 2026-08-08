namespace BH.SDK.Rules
{
    public static class ResourceRules
    {
        // Two different things used to share the name "sources", which read as one limit contradicting
        // itself (4 vs 16):
        // - Resource.MaxSourcesCount (4) - fallback URIs for ONE asset, tried in order until one
        //   loads. Small on purpose: each is a load attempt the player waits through.
        // - MaxMetaSources (16) - attribution entries in ResourceMeta, i.e. where the asset came
        //   from, for licensing. Never fetched, so a generous count costs nothing.
        public const int MaxMetaSources = 16;

        public const int MaxAuthors = 16;

        // Content hashes of the actual bytes behind ONE resource, and the permissions covering it.
        // Both are plural for the same reason Resource.Sources is: one resource can be several files
        // (a track re-encoded per platform), and one file can need permission from more than one
        // rights holder (composer and performer). Small caps - past a handful of either, the record
        // has stopped describing one resource.
        public const int MaxHashes = 8;
        public const int MaxPermissions = 8;

        // "sha256:" plus 64 hex characters is 71; the rest is headroom for a longer digest later.
        public const int MaxHashLength = 128;

        // A quoted permission is a mail or a DM, not a contract - long enough to hold the exchange,
        // short enough that nobody pastes a whole thread into every level file.
        public const int MaxProofText = 8192;

        // Per-category caps on a level's own resource dictionaries. These bound what a level may
        // REFERENCE, not what it may ship: every entry is a user-defined (negative-id) resource the
        // loader has to resolve before playback starts, so the count is load time, not frame time.
        // Sized by what authoring plausibly needs - a level with 256 distinct textures is already
        // extreme, one with 32 fonts is unheard of.
        public const int MaxTextures = 256;
        public const int MaxFonts = 32;
        public const int MaxAudios = 64;
        public const int MaxCompositeShapes = 256;
        public const int MaxThemes = 128;
        public const int MaxEffects = 256;

        // One source entry is a path, a URL or an addressable key.
        public const int MaxUriLength = ValueRules.MaxUrl;
    }
}
