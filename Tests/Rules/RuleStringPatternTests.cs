using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using NUnit.Framework;

namespace BH.SDK.Tests.Rules
{
    /// <summary>
    /// RuleStringPattern: for strings that are lookup keys rather than prose, where being malformed
    /// means silently never matching instead of failing loudly.
    /// </summary>
    public class RuleStringPatternTests : BaseRuleTests
    {
        [RuleContainer]
        private class LanguageModel
        {
            [RuleStringPattern(ValueRules.LanguageCodePattern)]
            public string Value { get; set; } = ValueRules.DefaultLanguageCode;
        }

        [RuleContainer]
        private class DefaultModel
        {
            [RuleStringPattern(ValueRules.LanguageCodePattern, ValueRules.DefaultLanguageCode)]
            public string Value { get; set; } = ValueRules.DefaultLanguageCode;
        }

        [RuleContainer]
        private class WrongTypeModel
        {
            [RuleStringPattern("^a$")]
            public int Value { get; set; }
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestPlainTag()
        {
            AssertValid(new LanguageModel { Value = "en" });
            AssertValid(new LanguageModel { Value = "ru" });
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestRegionalAndScriptTags()
        {
            AssertValid(new LanguageModel { Value = "pt-BR" });
            AssertValid(new LanguageModel { Value = "zh-Hans-CN" });
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestMalformedTags()
        {
            AssertInvalid<RuleStringPatternAttribute>(new LanguageModel { Value = "e" });
            AssertInvalid<RuleStringPatternAttribute>(new LanguageModel { Value = "en_US" });
            AssertInvalid<RuleStringPatternAttribute>(new LanguageModel { Value = "en-" });
            AssertInvalid<RuleStringPatternAttribute>(new LanguageModel { Value = "русский" });
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestEmptyIsInvalid()
        {
            AssertInvalid<RuleStringPatternAttribute>(new LanguageModel { Value = string.Empty });
        }

        // Without a declared replacement there is nothing to repair to: any guess would turn a
        // visibly wrong key into an invisibly wrong one.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestNoDefaultStaysBroken()
        {
            var model = new LanguageModel { Value = "en_US" };
            Fix(model);

            Assert.AreEqual("en_US", model.Value);
            AssertInvalid<RuleStringPatternAttribute>(model);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestFixUsesDeclaredDefault()
        {
            var model = new DefaultModel { Value = "en_US" };
            AssertFixedTo(model, () => model.Value, ValueRules.DefaultLanguageCode);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestWrongType()
        {
            AssertWrongType(new WrongTypeModel());
        }
    }
}
