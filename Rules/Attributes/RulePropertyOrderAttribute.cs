using System;
using System.Reflection;

namespace BH.SDK.Rules.Attributes
{
    // Covers every "these two fields are a range" invariant in the format, of which there were a
    // surprising number and not one of them checked: Min/Max on all four MinMax value types, the
    // per-component pairs of every Rect, ScreenLimitBounds' aspect pair, StartFrame/EndFrame on
    // objects and audio tracks, the editor's camera size pair.
    //
    // One attribute rather than the two the design sketched (a "min/max" one and a "this before
    // that" one) - they turned out to be the same check with different field names, and a second
    // attribute would only have invited guessing which one applies.
    //
    // AllowMultiple, because a type can hold several independent pairs: Color4MinMax has four.

    /// <summary>
    /// Two properties of the same object form an ordered pair: the low one must not exceed the high
    /// one. Fix swaps them.
    /// </summary>
    [AttributeUsage(ClassTarget, AllowMultiple = true)]
    public class RulePropertyOrderAttribute : BaseObjectRuleAttribute
    {
        public override string RuleNameKey => "rule_property_order";

        public string LowPropertyName { get; }
        public string HighPropertyName { get; }

        public RulePropertyOrderAttribute(string lowPropertyName, string highPropertyName)
        {
            LowPropertyName = lowPropertyName;
            HighPropertyName = highPropertyName;
        }

        protected override bool IsValidTypeInternal(Type type)
        {
            var low = Find(type, LowPropertyName);
            var high = Find(type, HighPropertyName);

            return low != null && high != null && low.PropertyType == high.PropertyType
                   && typeof(IComparable).IsAssignableFrom(low.PropertyType);
        }

        protected override bool IsValidInternal(object target, RuleContext context)
        {
            var type = target.GetType();
            var low = Find(type, LowPropertyName)?.GetValue(target);
            var high = Find(type, HighPropertyName)?.GetValue(target);

            if (low is not IComparable comparableLow || high == null) return false;

            return comparableLow.CompareTo(high) <= 0;
        }

        // Swap, not clamp. An inverted pair is nearly always the same two authored numbers entered
        // the wrong way round, and swapping preserves both; clamping one onto the other would
        // silently collapse the range to a point and lose whichever value was "wrong".
        protected override void FixInternal(object target, RuleContext context)
        {
            var type = target.GetType();
            var low = Find(type, LowPropertyName);
            var high = Find(type, HighPropertyName);
            if (low == null || high == null || !low.CanWrite || !high.CanWrite) return;

            var lowValue = low.GetValue(target);
            var highValue = high.GetValue(target);
            if (lowValue is not IComparable comparableLow || highValue == null) return;
            if (comparableLow.CompareTo(highValue) <= 0) return;

            low.SetValue(target, highValue);
            high.SetValue(target, lowValue);
        }

        private static PropertyInfo Find(Type type, string name)
            => type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
    }
}
