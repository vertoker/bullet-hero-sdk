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
