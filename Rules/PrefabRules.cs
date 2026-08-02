namespace BH.SDK.Rules
{
    public static class PrefabRules
    {
        // A Prefab template has no Framerate of its own (unlike LevelSettings) to scale a "10
        // seconds" default by, so this is a flat frame count instead - matches LevelSettings'
        // own default (60fps * 10s) at a nominal 60fps.
        public const int DefaultFrameLength = 600;
    }
}
