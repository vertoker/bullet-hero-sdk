using System;
using BH.SDK.Rules;

namespace BH.SDK.Validations
{
    [Serializable]
    public class RuleAnalyzerSettings
    {
        /// <summary> When analyzer find first invalid rule in property - it stop inspecting other rules in property.
        /// If you enable it - analyzer will check all rules in property no matter what</summary>
        public bool analyzeAllPropertyRules = false;

        /// <summary> When recursive check of property find invalid rule - it don't inspect deep into property value.
        /// If you enable it - analyzer will check all rules in property value no matter what</summary>
        public bool analyzeAllRecursiveRules = false;

        /// <summary> Weakest severity still reported. Error only reports what makes a level
        /// unplayable; Advice (the default) reports everything. Ordered by decreasing severity in
        /// RuleGroup itself, so this is an upper bound on the enum value, not a lower one. </summary>
        public RuleGroup weakestGroup = RuleGroup.Advice;

        public RuleAnalyzerSettings() { }

        public RuleAnalyzerSettings(bool analyzeAllPropertyRules, bool analyzeAllRecursiveRules)
        {
            this.analyzeAllPropertyRules = analyzeAllPropertyRules;
            this.analyzeAllRecursiveRules = analyzeAllRecursiveRules;
        }

        public RuleAnalyzerSettings(bool analyzeAllPropertyRules, bool analyzeAllRecursiveRules,
            RuleGroup weakestGroup)
        {
            this.analyzeAllPropertyRules = analyzeAllPropertyRules;
            this.analyzeAllRecursiveRules = analyzeAllRecursiveRules;
            this.weakestGroup = weakestGroup;
        }

        /// <summary> Whether a rule of this severity is worth reporting under these settings. None
        /// means "unclassified" and is always reported - silently dropping an issue because its
        /// author forgot to set a group would be worse than a false positive. </summary>
        public bool Reports(RuleGroup group)
            => group == RuleGroup.None || group <= weakestGroup;
    }
}
