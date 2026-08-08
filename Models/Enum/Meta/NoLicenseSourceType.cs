namespace BH.SDK.Models.Enum.Meta
{
    // Where a work with no stated terms was taken from. These used to sit in TypicalLicenseType as
    // NoLicense_SourcedFrom_* values, which was wrong in three ways: a platform is not a license, so
    // "terms unknown" became expressible twice and the two spellings graded differently; the origin
    // duplicated what ResourceUrl already says, free to disagree with it; and every new site would
    // have been a new number in a wire format that keeps its numbers forever.
    //
    // It stays an enum rather than becoming a TrustedSource key because it answers a different
    // question. The roster grades a site ("can anything from here be published"), is operator-owned
    // and changes as sites change their terms. This is the author's own statement of where the file
    // came from, stored in the level, and the handful of platforms that issue no license at all is a
    // short and slow-moving list. A site whose terms merely need reading belongs in the roster and
    // not here.

    /// <summary> Which platform an unlicensed work was taken from. </summary>
    public enum NoLicenseSourceType : byte
    {
        /// <summary> Origin not stated. </summary>
        Undefined = 0,

        YouTube = 1,
        SoundCloud = 2,
        Spotify = 3,
    }
}
