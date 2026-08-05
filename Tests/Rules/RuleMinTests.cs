using BH.SDK.Rules.Attributes;
using NUnit.Framework;

namespace BH.SDK.Tests.Rules
{
    // decimal is deliberately untested: RuleMinAttribute has a decimal ctor, but decimal is not a
    // legal attribute-argument type in C#, so that overload can never be reached from a [RuleMin]
    // declaration. Same holds for RuleMax/RuleInRange.

    /// <summary>
    /// RuleMin: value must be >= Min. Fix clamps to DefaultValue when set, to Min otherwise.
    /// </summary>
    public class RuleMinTests : BaseRuleTests
    {
        [RuleContainer]
        private class IntModel
        {
            [RuleMin(10)]
            public int Value { get; set; } = 10;
        }

        [RuleContainer]
        private class IntDefaultModel
        {
            [RuleMin(10, 42)]
            public int Value { get; set; } = 42;
        }

        [RuleContainer]
        private class FloatModel
        {
            [RuleMin(1.5f)]
            public float Value { get; set; } = 1.5f;
        }

        [RuleContainer]
        private class ByteModel
        {
            [RuleMin((byte)5)]
            public byte Value { get; set; } = 5;
        }

        [RuleContainer]
        private class UIntModel
        {
            [RuleMin(5u)]
            public uint Value { get; set; } = 5u;
        }

        [RuleContainer]
        private class LongModel
        {
            [RuleMin(5L)]
            public long Value { get; set; } = 5L;
        }

        [RuleContainer]
        private class DoubleModel
        {
            [RuleMin(1.5d)]
            public double Value { get; set; } = 1.5d;
        }

        [RuleContainer]
        private class WrongTypeModel
        {
            [RuleMin(1)]
            public string Value { get; set; } = "text";
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestValid()
        {
            AssertValid(new IntModel { Value = 100 });
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestBoundary()
        {
            AssertValid(new IntModel { Value = 10 });
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestJustUnder()
        {
            AssertInvalid<RuleMinAttribute>(new IntModel { Value = 9 });
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestFixClampsToMin()
        {
            var model = new IntModel { Value = -5 };
            AssertFixedTo(model, () => model.Value, 10);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestFixUsesDefaultValue()
        {
            var model = new IntDefaultModel { Value = -5 };
            AssertFixedTo(model, () => model.Value, 42);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestByte()
        {
            AssertValid(new ByteModel { Value = 5 });
            AssertInvalid<RuleMinAttribute>(new ByteModel { Value = 4 });
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestUInt()
        {
            AssertValid(new UIntModel { Value = 5u });
            AssertInvalid<RuleMinAttribute>(new UIntModel { Value = 4u });
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestLong()
        {
            AssertValid(new LongModel { Value = 5L });
            AssertInvalid<RuleMinAttribute>(new LongModel { Value = 4L });
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestFloat()
        {
            AssertValid(new FloatModel { Value = 1.5f });
            AssertInvalid<RuleMinAttribute>(new FloatModel { Value = 1.49f });
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestDouble()
        {
            AssertValid(new DoubleModel { Value = 1.5d });
            AssertInvalid<RuleMinAttribute>(new DoubleModel { Value = 1.49d });
        }

        // NaN compares as less than everything, so it lands on the "below Min" branch. That is the
        // behaviour the format relies on to keep a hostile file's NaN out of the runtime - assert it
        // explicitly so a future refactor of the comparison can't silently start accepting NaN.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestNaNIsInvalid()
        {
            AssertInvalid<RuleMinAttribute>(new FloatModel { Value = float.NaN });
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestNegativeInfinityIsInvalid()
        {
            AssertInvalid<RuleMinAttribute>(new FloatModel { Value = float.NegativeInfinity });
        }

        // Positive infinity satisfies a lower bound - RuleMin alone cannot keep it out. Any property
        // that must stay finite needs an upper bound too (RuleInRange), not just RuleMin.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestPositiveInfinityIsValid()
        {
            AssertValid(new FloatModel { Value = float.PositiveInfinity });
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestWrongType()
        {
            AssertWrongType(new WrongTypeModel());
        }
    }
}
