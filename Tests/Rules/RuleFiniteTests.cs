using BH.SDK.Rules.Attributes;
using NUnit.Framework;

namespace BH.SDK.Tests.Rules
{
    /// <summary>
    /// RuleFinite: rejects NaN and the infinities on the floats no two-sided range covers.
    /// </summary>
    public class RuleFiniteTests : BaseRuleTests
    {
        [RuleContainer]
        private class FloatModel
        {
            [RuleFinite]
            public float Value { get; set; }
        }

        [RuleContainer]
        private class DoubleModel
        {
            [RuleFinite]
            public double Value { get; set; }
        }

        [RuleContainer]
        private class DefaultModel
        {
            [RuleFinite(1.5f)]
            public float Value { get; set; }
        }

        [RuleContainer]
        private class WrongTypeModel
        {
            [RuleFinite]
            public int Value { get; set; }
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void TestOrdinaryValues()
        {
            AssertValid(new FloatModel { Value = 0f });
            AssertValid(new FloatModel { Value = -12345.678f });
            AssertValid(new FloatModel { Value = float.MaxValue });
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void TestNaN()
        {
            AssertInvalid<RuleFiniteAttribute>(new FloatModel { Value = float.NaN });
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void TestInfinities()
        {
            AssertInvalid<RuleFiniteAttribute>(new FloatModel { Value = float.PositiveInfinity });
            AssertInvalid<RuleFiniteAttribute>(new FloatModel { Value = float.NegativeInfinity });
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void TestDouble()
        {
            AssertValid(new DoubleModel { Value = 1d });
            AssertInvalid<RuleFiniteAttribute>(new DoubleModel { Value = double.NaN });
            AssertInvalid<RuleFiniteAttribute>(new DoubleModel { Value = double.NegativeInfinity });
        }

        // Zero rather than a bound: the rule exists precisely where there is no bound to clamp to.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void TestFixToZero()
        {
            var model = new FloatModel { Value = float.NaN };
            AssertFixedTo(model, () => model.Value, 0f);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void TestFixToDeclaredDefault()
        {
            var model = new DefaultModel { Value = float.PositiveInfinity };
            AssertFixedTo(model, () => model.Value, 1.5f);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void TestWrongType()
        {
            AssertWrongType(new WrongTypeModel());
        }
    }
}
