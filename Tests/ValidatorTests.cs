using BHSDK.Models.Values;
using BHSDK.Validations;
using NUnit.Framework;

namespace BHSDK.Tests
{
    public class ValidatorTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestValidatorLevel()
        {
            var validator = new RuleAnalyzer();
            var level = SerializationTests.CreateTestLevel();
            var issues = validator.Analyze(level, new RuleAnalyzerSettings());
            
            Assert.IsEmpty(issues);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestFixerLevel()
        {
            var fixer = new RuleFixer();
            var validator = new RuleAnalyzer();
            var level = SerializationTests.CreateInvalidTestLevel();
            
            var issues = validator.Analyze(level, new RuleAnalyzerSettings());
            Assert.IsNotEmpty(issues);
            
            fixer.Fix(issues, level, new RuleFixerSettings());
            issues = validator.Analyze(level, new RuleAnalyzerSettings());
            Assert.IsEmpty(issues);
        }
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestValidatorLevelMeta()
        {
            var validator = new RuleAnalyzer();
            var level = SerializationTests.CreateTestLevelMeta();
            var issues = validator.Analyze(level, new RuleAnalyzerSettings());
            
            Assert.IsEmpty(issues);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestFixerLevelMeta()
        {
            var fixer = new RuleFixer();
            var validator = new RuleAnalyzer();
            var level = SerializationTests.CreateInvalidTestLevelMeta();
            
            var issues = validator.Analyze(level, new RuleAnalyzerSettings());
            Assert.IsNotEmpty(issues);
            
            fixer.Fix(issues, level, new RuleFixerSettings());
            issues = validator.Analyze(level, new RuleAnalyzerSettings());
            Assert.IsEmpty(issues);
        }
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestValidatorSettings()
        {
            var validator = new RuleAnalyzer();
            var settings = SerializationTests.CreateValidTestSettings();
            var issues = validator.Analyze(settings, new RuleAnalyzerSettings());
            
            Assert.IsEmpty(issues);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestFixerSettings()
        {
            var fixer = new RuleFixer();
            var validator = new RuleAnalyzer();
            var settings = SerializationTests.CreateInvalidTestSettings();
            
            var issues = validator.Analyze(settings, new RuleAnalyzerSettings());
            Assert.IsNotEmpty(issues);
            
            fixer.Fix(issues, settings, new RuleFixerSettings());
            issues = validator.Analyze(settings, new RuleAnalyzerSettings());
            Assert.IsEmpty(issues);
        }
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestCopyLevel()
        {
            var validator = new RuleAnalyzer();
            var level = SerializationTests.CreateTestLevel();
            var copyLevel = level.Copy();
            
            var issues = validator.Analyze(copyLevel, new RuleAnalyzerSettings());
            Assert.IsEmpty(issues);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestCopyEqualsLevel()
        {
            var level = SerializationTests.CreateTestLevel();
            var copyLevel = level.Copy();
            Assert.IsTrue(level.Equals(copyLevel));
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestCopyNotEqualsLevel()
        {
            var level = SerializationTests.CreateTestLevel();
            var copyLevel = level.Copy();
            copyLevel.Game.Objects[0].Positions[0].Anchor.Value = Alignment.BottomLeftValue;
            Assert.IsFalse(level.Equals(copyLevel));
        }
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestCopySettings()
        {
            var validator = new RuleAnalyzer();
            var settings = SerializationTests.CreateValidTestSettings();
            var copySettings = settings.Copy();
            
            var issues = validator.Analyze(copySettings, new RuleAnalyzerSettings());
            Assert.IsEmpty(issues);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestCopyEqualsSettings()
        {
            var settings = SerializationTests.CreateValidTestSettings();
            var copySettings = settings.Copy();
            Assert.IsTrue(settings.Equals(copySettings));
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestCopyNotEqualsSettings()
        {
            var settings = SerializationTests.CreateValidTestSettings();
            var copySettings = settings.Copy();
            copySettings.Audio.Game = 0.123f;
            Assert.IsFalse(settings.Equals(copySettings));
        }
        
        // TODO add generators tests
    }
}