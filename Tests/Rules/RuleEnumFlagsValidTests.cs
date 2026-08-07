using System;
using BH.SDK.Models.Enum.Meta;
using BH.SDK.Rules.Attributes;
using NUnit.Framework;

namespace BH.SDK.Tests.Rules
{
    /// <summary>
    /// RuleEnumFlagsValid: the [Flags] counterpart of RuleEnumValid - combinations are legal, unknown
    /// bits are not, and a fix keeps everything it recognized.
    /// </summary>
    public class RuleEnumFlagsValidTests : BaseRuleTests
    {
        [Flags]
        private enum Sparse : ushort
        {
            None = 0,
            First = 1 << 0,
            Third = 1 << 2,
        }

        // A signed underlying type with the sign bit declared: the case where reading the value as an
        // unsigned number without reinterpreting the bit pattern would overflow instead of comparing.
        [Flags]
        private enum Signed : int
        {
            None = 0,
            Low = 1 << 0,
            Sign = 1 << 31,
        }

        [RuleContainer]
        private class DescriptorModel
        {
            [RuleEnumFlagsValid]
            public ContentDescriptor Value { get; set; } = ContentDescriptor.None;
        }

        [RuleContainer]
        private class SparseModel
        {
            [RuleEnumFlagsValid]
            public Sparse Value { get; set; } = Sparse.None;
        }

        [RuleContainer]
        private class SignedModel
        {
            [RuleEnumFlagsValid]
            public Signed Value { get; set; } = Signed.None;
        }

        [RuleContainer]
        private class NotFlagsModel
        {
            [RuleEnumFlagsValid]
            public AgeRating Value { get; set; } = AgeRating.Unrated;
        }

        [RuleContainer]
        private class WrongTypeModel
        {
            [RuleEnumFlagsValid]
            public int Value { get; set; }
        }

        // The whole reason this rule exists: RuleEnumValid would reject this, since a combination is
        // not a declared member.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void TestCombinationIsValid()
        {
            AssertValid(new DescriptorModel
            {
                Value = ContentDescriptor.Violence | ContentDescriptor.FlashingLights,
            });
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void TestNoneIsValid()
        {
            AssertValid(new DescriptorModel { Value = ContentDescriptor.None });
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void TestUndeclaredBitIsInvalid()
        {
            AssertInvalid<RuleEnumFlagsValidAttribute>(new SparseModel { Value = (Sparse)0b010 });
        }

        // The gap in a sparse flags enum is what a hand-edited or newer file most plausibly sets.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void TestFixKeepsRecognizedBits()
        {
            var model = new SparseModel { Value = (Sparse)0b111 };
            AssertFixedTo(model, () => model.Value, Sparse.First | Sparse.Third);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void TestFixClearsEverythingWhenNothingRecognized()
        {
            var model = new SparseModel { Value = (Sparse)0b010 };
            AssertFixedTo(model, () => model.Value, Sparse.None);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void TestSignBitIsValidWhenDeclared()
        {
            AssertValid(new SignedModel { Value = Signed.Sign | Signed.Low });
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void TestSignedFixKeepsRecognizedBits()
        {
            var model = new SignedModel { Value = (Signed)unchecked((int)0x80000003) };
            AssertFixedTo(model, () => model.Value, Signed.Sign | Signed.Low);
        }

        // A plain enum is RuleEnumValid's job - this rule must refuse it rather than pass every value
        // whose bits happen to overlap the declared set.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void TestNonFlagsEnumIsWrongType()
        {
            AssertWrongType(new NotFlagsModel());
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
