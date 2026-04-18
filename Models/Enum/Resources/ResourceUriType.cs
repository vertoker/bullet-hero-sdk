namespace BHSDK.Models.Enum.Resources
{
    public enum ResourceUriType : byte
    {
        Undefined = 0,
        RelativePath = 1, // to level directory
        AbsolutePath = 2,
        DirectUrl = 3,
        AddressableKey = 4, // for build-in levels
        // Youtube = 3,
    }
}