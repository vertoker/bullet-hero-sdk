using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BH.SDK.Rules.Attributes
{
    // For the handful of strings that are keys rather than prose: a language tag looked up against
    // the player's locale, a URI scheme dispatched on. A malformed one does not throw - it simply
    // never matches, so the localized text silently falls back or the resource silently fails to
    // load. A length cap alone cannot catch that.
    //
    // The regex is compiled once per attribute instance and attribute instances are cached by the
    // analyzer, so the cost is paid once per property, not per value.

    /// <summary> A string field must match the given regular expression. </summary>
    [AttributeUsage(PropertyTarget)]
    public class RuleStringPatternAttribute : BasePropertyRuleAttribute
    {
        public string Pattern { get; }
        public string DefaultValue { get; set; }

        private readonly Regex _regex;

        public RuleStringPatternAttribute(string pattern)
        {
            Pattern = pattern;
            _regex = new Regex(pattern, RegexOptions.CultureInvariant);
        }

        public RuleStringPatternAttribute(string pattern, string defaultValue) : this(pattern)
        {
            DefaultValue = defaultValue;
        }

        protected override bool IsValidTypeInternal(PropertyInfo property)
            => property.PropertyType == typeof(string);

        protected override bool IsValidInternal(object value, RuleContext context)
            => value is string text && _regex.IsMatch(text);

        // Only repairable when a valid replacement was declared. There is no way to "nearly" match a
        // pattern, and guessing - trimming, lower-casing, dropping the offending characters - would
        // turn a wrong key into a different wrong key that no longer looks wrong.
        protected override void FixInternal(object target, PropertyInfo property, RuleContext context)
        {
            if (DefaultValue == null || !_regex.IsMatch(DefaultValue)) return;
            if (property.GetValue(target) is string current && _regex.IsMatch(current)) return;

            property.SetValue(target, DefaultValue);
        }
    }
}
