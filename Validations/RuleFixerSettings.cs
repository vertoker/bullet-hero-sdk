namespace BH.SDK.Validations
{
    public class RuleFixerSettings
    {
        // A repair can legitimately create a new violation elsewhere: assigning an id to an unset
        // resource makes it disagree with the dictionary key it is filed under, padding a collection
        // to its minimum introduces entries with their own rules. Reverse-order fixing handles the
        // nested case, but not this one - the second issue does not exist yet when the first is
        // fixed. So a full repair re-analyzes and goes again, a few times at most: anything still
        // reported after that is either unfixable by design (a dangling reference) or a genuine
        // repair loop, and both deserve to be reported rather than ground against forever.
        public int maxPasses = 4;

        public RuleFixerSettings() { }

        public RuleFixerSettings(int maxPasses)
        {
            this.maxPasses = maxPasses;
        }
    }
}
