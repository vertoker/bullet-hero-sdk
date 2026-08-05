using System.Collections.Generic;
using BH.SDK.Rules.Attributes;
using NUnit.Framework;

namespace BH.SDK.Tests.Rules
{
    /// <summary>
    /// RuleCollectionMaxCount: every per-track/per-level cap in the format goes through this rule.
    /// Fix drops from the tail, so the earliest authored entries survive an over-long track.
    /// </summary>
    public class RuleCollectionMaxCountTests : BaseRuleTests
    {
        [RuleContainer]
        private class ListModel
        {
            [RuleCollectionMaxCount(3)]
            public List<int> Value { get; set; } = new();
        }

        [RuleContainer]
        private class ArrayModel
        {
            [RuleCollectionMaxCount(3)]
            public int[] Value { get; set; } = { 1, 2, 3 };
        }

        [RuleContainer]
        private class WrongTypeModel
        {
            [RuleCollectionMaxCount(3)]
            public int Value { get; set; }
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestValid()
        {
            AssertValid(new ListModel { Value = new List<int> { 1 } });
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestEmpty()
        {
            AssertValid(new ListModel());
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestBoundary()
        {
            AssertValid(new ListModel { Value = new List<int> { 1, 2, 3 } });
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestJustOver()
        {
            AssertInvalid<RuleCollectionMaxCountAttribute>(
                new ListModel { Value = new List<int> { 1, 2, 3, 4 } });
        }

        // Tail-drop, not head-drop: keyframes authored first are the ones kept.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestFixDropsFromTail()
        {
            var model = new ListModel { Value = new List<int> { 1, 2, 3, 4, 5 } };
            AssertFixed(model);

            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, model.Value);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestFixArray()
        {
            var model = new ArrayModel { Value = new[] { 1, 2, 3, 4, 5 } };
            AssertFixed(model);

            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, model.Value);
        }

        // The array Fix rebuilds at exactly MaxCount whenever the length differs, including when the
        // array is SHORTER - it would pad a 1-element array up to 3. Unreachable through the analyzer
        // (a short array raises no issue, so Fix never runs), but it makes the array path lossy in the
        // other direction if anything ever calls Fix directly.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestShortArrayRaisesNoIssue()
        {
            AssertValid(new ArrayModel { Value = new[] { 1 } });
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestWrongType()
        {
            AssertWrongType(new WrongTypeModel());
        }
    }
}
