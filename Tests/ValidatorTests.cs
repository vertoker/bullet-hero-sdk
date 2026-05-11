using BHSDK.Validations;
using NUnit.Framework;

namespace BHSDK.Tests
{
    public class ValidatorTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestValidator()
        {
            var validator = new LevelAnalyzer();
            var level = SerializationTests.CreateTestLevel();
            var issues = validator.Analyze(level, new LevelAnalyzerSettings());
            
            Assert.IsEmpty(issues);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestFixer()
        {
            var fixer = new LevelFixer();
            var validator = new LevelAnalyzer();
            var level = SerializationTests.CreateInvalidTestLevel();
            
            var issues = validator.Analyze(level, new LevelAnalyzerSettings());
            Assert.IsNotEmpty(issues);
            
            fixer.Fix(issues, level, new LevelFixerSettings());
            issues = validator.Analyze(level, new LevelAnalyzerSettings());
            Assert.IsEmpty(issues);
        }
    }
}