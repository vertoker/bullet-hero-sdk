namespace BH.SDK.Rules
{
    public static class LevelRules
    {
        public const int MaxMarkerEvents = 1024;
        public const int MaxCheckpointEvents = 128;
        public const int MaxBackgroundEvents = 128;
        public const int MaxThemeEvents = 128;
        public const int MaxScreenLimitEvents = 128;

        public const int MaxPlayerKeys = 512;
        public const int MaxCameraKeys = 512;
        public const int MaxPostProcessingKeys = 512;
        public const int MaxObjectKeys = 32;
        public const int MaxAudioKeys = 32;

        // Was deliberately uncapped for a long time, on the grounds that peak simultaneous objects
        // (LevelLimitHints) is what actually costs anything at runtime. It is capped now because a
        // total count is what a LOADER pays for - every object is deserialized, id-mapped and
        // parent-linked before playback ever decides it is off-screen - so an unbounded count is an
        // unbounded load, not an unbounded frame. 2^18 sits far above any authored level and far
        // below what would exhaust a phone.
        public const int MaxObjects = 262_144;

        // Longest parent chain AUTHORED CONTENT may have. Depth is walked per object per frame (a
        // child's transform and layer are the sum up its chain), so the real ceiling is the
        // consumer's: the Unity player walks it into a fixed stackalloc of
        // LevelPlayerSettings.MaxChildInherit (16) and, past that, composes an object against a
        // mid-chain ancestor instead of its root.
        //
        // This is that ceiling MINUS ONE. The editor parents its own overlays (the selection
        // outline's marching-ants segments, the gizmo handles) one level under the selected object,
        // so a level authored right at the runtime cap would push its own overlay past it - the
        // object would render correctly and its selection border would not. Cycles are a graph
        // invariant and checked separately.
        public const int MaxObjectDepth = 15;

        public const int MaxAudioTracks = 512;
        public const int MaxResourcesMeta = 512;

        public const int MaxPrefabs = 64;

        // Bounds of LevelLimitHints - purely a format-level sanity clamp, so a corrupted or
        // hostile file can't ask a player's device to preallocate gigabytes before the runtime even
        // looks at the number. The real ceiling is per-device and applied at runtime; the hint
        // itself is advisory and never trusted on its own.
        public const int MinCapacityHint = 0;
        public const int MaxCapacityHint = 1_048_576; // 2^20

        // Zero is not "seed number zero", it is the absence of a seed - the same convention every
        // tier of seed resolution follows (per-launch override, then LevelSettings.Seed, then a
        // freshly generated one), so a consumer only ever has to ask IsValidSeed instead of
        // spelling out != 0 at each of the three steps. Shaped like AudioRules.IsActiveMixLevel: a
        // constant plus the one predicate that reads it, rather than a rule attribute, because 0 is
        // perfectly VALID authored data - it is what an unpinned level stores.
        public const int NullSeed = 0;
        public const int MinSeed = 0;

        public static bool IsValidSeed(int seed) => seed != NullSeed;
    }
}
