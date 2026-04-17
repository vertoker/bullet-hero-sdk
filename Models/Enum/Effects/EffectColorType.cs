namespace BHSDK.Models.Enum.Effects
{
    public enum EffectColorType : byte
    {
        Value = 0,
        GradientOverLife = 1,
        GradientBySpeed = 2,
        RandomUniform = 3,
        RandomPerComponent = 4,
        GradientRandom = 5,
        
        // TODO implement sound-dependent algo (changed with music, discretization as interpolation)
        // GradientCustom = 6, // not implemented
    }
}