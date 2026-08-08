namespace BH.SDK.Publishing
{
    // Where a resource came from, graded. A licence field says what the author CLAIMS; this says how
    // much that claim is worth given the site it came from. The two together are what makes
    // moderation cheap: a CC0 claim on a Kenney URL needs nobody to look at it, the same claim on a
    // random blog needs a human, and a claim on a site that licenses nothing needs no human at all
    // because the answer is already no.
    //
    // The grades are not a severity ladder to compare with >=. Each maps to one outcome - publish,
    // publish and queue for review, refuse - and PublishReadinessAnalyzer is the only place that
    // mapping lives.

    /// <summary> How far a known resource site can be taken at its word. </summary>
    public enum SourceTrust : byte
    {
        /// <summary> No record covers this site. What that means is the profile's decision
        /// (PublishProfile.UnknownSourceTrust), not this enum's. </summary>
        Unknown = 0,

        /// <summary> Everything on the site is publishable, no per-resource check needed
        /// (Kenney, ccMixter, Incompetech). </summary>
        Approved = 1,

        /// <summary> Publishable, but the site hosts more than one kind of terms, so the record is
        /// worth a glance (Pixabay, SoundImage). </summary>
        PartiallyApproved = 2,

        /// <summary> The site carries every license there is and states each per upload - the
        /// declared license has to be read against the actual page (OpenGameArt, FMA, Wikimedia). </summary>
        RequiresLicenseCheck = 3,

        /// <summary> Terms depend on the individual work or on a plan the uploader chose, so the
        /// resource itself has to be identified before anything can be said (Rawpixel, itch.io). </summary>
        RequiresResourceCheck = 4,

        /// <summary> Nothing from here may be published, whatever the record claims
        /// (streaming rips, commercial libraries). </summary>
        NotAllowed = 5,
    }
}
