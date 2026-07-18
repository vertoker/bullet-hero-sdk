namespace BH.SDK.Models.Enum.Resources
{
    public enum ResourceUriType : byte
    {
        Undefined = 0,
        LevelPath = 1,
        AbsolutePath = 2,
        DirectUrl = 3,
        AddressableKey = 4, // for build-in levels
        StreamingAssets = 5,
    }
}