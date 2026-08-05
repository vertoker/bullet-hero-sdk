using System;
using System.Reflection;

namespace BH.SDK.Rules.Attributes
{
    // C# enums accept any value of their underlying type, so `"ease": 200` deserializes without a
    // murmur and only surfaces much later, as a switch falling through to a default or throwing
    // deep inside a job. This is the rule that turns that into a reported, fixable issue.
    //
    // Note what it does NOT cover: the enums that discriminate polymorphism (ObjectType, ColorType,
    // VectorType, ScreenLimitType, LicenseType, Color4X4KeyType, the EffectShape* family). Those are
    // never stored as a property - they are the first element of a two-element JSON array, resolved
    // by a converter that throws on an unknown value. The two are different failure classes on
    // purpose: an unknown discriminator means the file cannot be read at all, an unknown enum FIELD
    // means one value is wrong and the rest of the level is fine.
    //
    // [Flags] enums are not supported: Enum.IsDefined rejects legitimate combinations. The format
    // has none today; introducing one means giving it a different rule, not loosening this one.

    /// <summary>
    /// An enum field must hold a value the enum actually declares. Fix falls back to DefaultValue,
    /// or to the enum's own zero value when that is declared, or to its first declared value.
    /// </summary>
    [AttributeUsage(PropertyTarget)]
    public class RuleEnumValidAttribute : BasePropertyRuleAttribute
    {
        public object DefaultValue { get; set; }

        public RuleEnumValidAttribute() { }

        public RuleEnumValidAttribute(object defaultValue)
        {
            DefaultValue = defaultValue;
        }

        protected override bool IsValidTypeInternal(PropertyInfo property)
            => property.PropertyType.IsEnum;

        protected override bool IsValidInternal(object value, RuleContext context)
        {
            var type = value.GetType();
            return type.IsEnum && Enum.IsDefined(type, value);
        }

        protected override void FixInternal(object target, PropertyInfo property, RuleContext context)
        {
            var type = property.PropertyType;
            if (!type.IsEnum) return;

            var value = property.GetValue(target);
            if (value != null && Enum.IsDefined(type, value)) return;

            property.SetValue(target, ResolveFallback(type));
        }

        private object ResolveFallback(Type type)
        {
            if (DefaultValue != null && DefaultValue.GetType() == type && Enum.IsDefined(type, DefaultValue))
                return DefaultValue;

            // Zero is the conventional "unset/first" value and is what a freshly-constructed model
            // holds, so prefer it when the enum declares it.
            var zero = Activator.CreateInstance(type);
            if (Enum.IsDefined(type, zero)) return zero;

            var declared = Enum.GetValues(type);
            return declared.Length > 0 ? declared.GetValue(0) : zero;
        }
    }
}
