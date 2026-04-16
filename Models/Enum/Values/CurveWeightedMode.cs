namespace BHSDK.Models.Enum.Values
{
    public enum CurveWeightedMode : byte
    {
        None = 0,
        In = 1 << 0,
        Out = 1 << 1,
        Both = In | Out
    }
}