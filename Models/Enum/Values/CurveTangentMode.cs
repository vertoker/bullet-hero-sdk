namespace BHSDK.Models.Enum.Values
{
    public enum CurveTangentMode : byte
    {
        Free = 0,
        Auto = 1,
        Linear = 2,
        Constant = 3,
        ClampedAuto = 4,
        Broken = 5, // TODO made my fast realization for Evaluate, this may be junk
    }
}