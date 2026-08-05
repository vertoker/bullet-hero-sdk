using System;
using System.Collections.Generic;
using System.Linq;
using BH.SDK.Rules.Attributes;
using NUnit.Framework;

namespace BH.SDK.Tests.Rules
{
    // The safety net for this whole folder. Adding a rule attribute without a test file is easy and
    // invisible; this makes it a compile-and-run failure instead. The registry is deliberately a
    // hand-written list rather than a naming convention, because several rules are covered by a
    // shared file (the IVector3/IVector4 families, the IPrimitiveInt trio) and a convention would
    // either forbid that or be too loose to catch anything.

    /// <summary>
    /// Every concrete BaseRuleAttribute in the SDK has a test file, and every entry in the registry
    /// still refers to a rule that exists.
    /// </summary>
    public class RuleCoverageTests
    {
        private static readonly Dictionary<Type, string> Covered = new()
        {
            { typeof(RuleMinAttribute), nameof(RuleMinTests) },
            { typeof(RuleMaxAttribute), nameof(RuleMaxTests) },
            { typeof(RuleInRangeAttribute), nameof(RuleInRangeTests) },
            { typeof(RuleNotNullAttribute), nameof(RuleNotNullTests) },
            { typeof(RuleStringMaxAttribute), nameof(RuleStringMaxTests) },

            { typeof(RuleCollectionCountAttribute), nameof(RuleCollectionCountTests) },
            { typeof(RuleCollectionMaxCountAttribute), nameof(RuleCollectionMaxCountTests) },
            { typeof(RuleCollectionUniqueAttribute), nameof(RuleCollectionUniqueTests) },

            { typeof(RuleIFloatInRangeAttribute), nameof(RuleIFloatInRangeTests) },
            { typeof(RuleIFloatMinAttribute), nameof(RuleIFloatMinTests) },
            { typeof(RuleIFloatMaxAttribute), nameof(RuleIFloatMaxTests) },
            { typeof(RuleIIntInRangeAttribute), nameof(RuleIIntInRangeTests) },
            { typeof(RuleIStringMaxAttribute), nameof(RuleIStringMaxTests) },

            { typeof(RuleIVector2InRangeAttribute), nameof(RuleIVector2InRangeTests) },
            { typeof(RuleIVector2MinAttribute), nameof(RuleIVector2MinTests) },
            { typeof(RuleIVector2MaxAttribute), nameof(RuleIVector2MaxTests) },
            { typeof(RuleIVector3InRangeAttribute), nameof(RuleIVector3RangeTests) },
            { typeof(RuleIVector3MinAttribute), nameof(RuleIVector3RangeTests) },
            { typeof(RuleIVector3MaxAttribute), nameof(RuleIVector3RangeTests) },
            { typeof(RuleIVector4InRangeAttribute), nameof(RuleIVector4RangeTests) },
            { typeof(RuleIVector4MinAttribute), nameof(RuleIVector4RangeTests) },
            { typeof(RuleIVector4MaxAttribute), nameof(RuleIVector4RangeTests) },

            { typeof(RuleIPrimitiveIntNotNullAttribute), nameof(RuleIPrimitiveIntTests) },
            { typeof(RuleIPrimitiveIntMinAttribute), nameof(RuleIPrimitiveIntTests) },
            { typeof(RuleIPrimitiveIntMaxAttribute), nameof(RuleIPrimitiveIntTests) },
            { typeof(RuleIPrimitiveGuidNotNullAttribute), nameof(RuleIPrimitiveGuidNotNullTests) },

            { typeof(RuleLevelFrameAttribute), nameof(RuleLevelFrameTests) },
            { typeof(RuleObjectIdValidAttribute), nameof(RuleObjectIdValidTests) },
            { typeof(RuleParentObjectIdValidAttribute), nameof(RuleParentObjectIdValidTests) },

            { typeof(RuleEnumValidAttribute), nameof(RuleEnumValidTests) },
            { typeof(RuleFiniteAttribute), nameof(RuleFiniteTests) },
            { typeof(RuleIVector2OrderedAttribute), nameof(RuleIVector2OrderedTests) },
            { typeof(RuleCollectionMinCountAttribute), nameof(RuleCollectionMinCountTests) },
            { typeof(RuleCollectionNoNullItemsAttribute), nameof(RuleCollectionNoNullItemsTests) },
            { typeof(RuleCollectionSortedAttribute), nameof(RuleCollectionSortedTests) },
            { typeof(RuleStringPatternAttribute), nameof(RuleStringPatternTests) },
            { typeof(RuleDictionaryKeyMatchesAttribute), nameof(RuleDictionaryKeyMatchesTests) },
            { typeof(RuleModificationKeyValidAttribute), nameof(RuleModificationKeyValidTests) },
            { typeof(RuleReferenceExistsAttribute), nameof(RuleReferenceExistsTests) },

            { typeof(RulePropertyOrderAttribute), nameof(RulePropertyOrderTests) },
        };

        private static IEnumerable<Type> ConcreteRules => typeof(BaseRuleAttribute).Assembly.GetTypes()
            .Where(type => !type.IsAbstract && typeof(BaseRuleAttribute).IsAssignableFrom(type));

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Hard)]
        public void TestEveryRuleHasTests()
        {
            var missing = ConcreteRules.Where(type => !Covered.ContainsKey(type))
                .Select(type => type.Name).ToList();

            CollectionAssert.IsEmpty(missing,
                "Rule attributes without a test file - add one under Tests/Rules/ and register it here: "
                + string.Join(", ", missing));
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Hard)]
        public void TestRegistryHasNoStaleEntries()
        {
            var live = new HashSet<Type>(ConcreteRules);
            var stale = Covered.Keys.Where(type => !live.Contains(type))
                .Select(type => type.Name).ToList();

            CollectionAssert.IsEmpty(stale,
                "Registry names rules that no longer exist: " + string.Join(", ", stale));
        }
    }
}
