using System;
using System.Linq;
using BH.SDK.Validations.Graph;
using NUnit.Framework;

namespace BH.SDK.Tests.Rules
{
    // Same safety net RuleCoverageTests provides for declarative rules, for the graph half: a new
    // GraphRule value that nobody named would otherwise only fail once a real level triggered it.

    /// <summary>
    /// GraphRuleKeys: every GraphRule has a unique key, and an unnamed value throws.
    /// </summary>
    public class GraphRuleKeysTests
    {
        private static GraphRule[] AllRules => Enum.GetValues(typeof(GraphRule)).Cast<GraphRule>().ToArray();

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void TestEveryGraphRuleHasKey()
        {
            var unnamed = AllRules
                .Where(rule => !rule.GetKey().StartsWith("graph_", StringComparison.Ordinal))
                .Select(rule => rule.ToString()).ToList();

            CollectionAssert.IsEmpty(unnamed,
                "Graph rules without a \"graph_\" key: " + string.Join(", ", unnamed));
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void TestKeysAreUnique()
        {
            var duplicates = AllRules.GroupBy(rule => rule.GetKey())
                .Where(group => group.Count() > 1)
                .Select(group => group.Key).ToList();

            CollectionAssert.IsEmpty(duplicates,
                "Graph rules sharing one key: " + string.Join(", ", duplicates));
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void TestUnknownValueThrows()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ((GraphRule)200).GetKey());
        }
    }
}
