namespace BH.SDK.Models.Enum.Meta
{
    // How far a rights holder's permission reaches. The whole point of storing this rather than a
    // bare "permission granted" bool is that the three cases fail differently: a ThisLevel grant is
    // void the moment the resource is copied into another level, while an AnyLevel grant survives
    // every reuse. A moderator reading one record has to be able to tell which of those they hold.
    //
    // Undefined is 0 and means "the author wrote nothing", never "unlimited" - same reasoning as
    // AgeRating.Unrated, and the reason a publish profile can refuse it outright.

    /// <summary> What a permission from a rights holder covers. </summary>
    public enum PermissionScope : byte
    {
        /// <summary> Nothing was declared. Not a claim that anything is permitted. </summary>
        Undefined = 0,

        /// <summary> One named level only - re-using the resource elsewhere needs a new permission. </summary>
        ThisLevel = 1,

        /// <summary> Every level by the same level author. </summary>
        AuthorLevels = 2,

        /// <summary> Any Bullet Hero level by anyone - the rights holder released it to the game. </summary>
        AnyLevel = 3,
    }
}
