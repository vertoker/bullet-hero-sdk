namespace BH.SDK.Models.Interfaces
{
    // "A place objects live, with a timeline of its own" - the pair a frame or an object id has to
    // be judged against. Prefab satisfies it on one class; a level does NOT, because its two halves
    // are deliberately split (GameLevel owns Objects, LevelSettings owns FrameDuration - see
    // IObjectScope/IObjectIdCounter). RuleContext is what pairs those two back up for a level, and
    // this interface is what lets it recognise every other scope generically.
    //
    // Validation is the reason this composite exists: without it a rule walking into a Prefab has no
    // way to notice it left the level's timeline behind, and measures the template's frames against
    // the level's length.

    /// <summary> An object scope that also carries its own timeline length. </summary>
    public interface IFrameScope : IObjectScope, IFrameDuration
    {
    }
}
