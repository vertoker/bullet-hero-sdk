namespace BH.SDK.Rules.Attributes
{
    /// <summary> Which of a level's resource dictionaries a reference is expected to resolve in. </summary>
    public enum ResourceReferenceKind : byte
    {
        Texture = 0,
        Font = 1,
        Audio = 2,
    }
}
