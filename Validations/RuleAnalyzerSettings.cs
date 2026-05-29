using System;

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

        public RuleAnalyzerSettings() { }

        public RuleAnalyzerSettings(bool analyzeAllPropertyRules, bool analyzeAllRecursiveRules)
        {
            this.analyzeAllPropertyRules = analyzeAllPropertyRules;
            this.analyzeAllRecursiveRules = analyzeAllRecursiveRules;
        }
    }
}