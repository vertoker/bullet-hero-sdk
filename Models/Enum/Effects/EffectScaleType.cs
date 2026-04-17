namespace BHSDK.Models.Enum.Effects
{
    public enum EffectScaleType : byte
    {
        Value = 0,
        CurvesOverLife = 1,
        CurvesBySpeed = 2,
        RandomUniform = 3,
        RandomPerComponent = 4,
        
        // TODO implement sound-dependent algo (changed with music, discretization as interpolation)
        // Custom = 5, // not implemented
    }
}