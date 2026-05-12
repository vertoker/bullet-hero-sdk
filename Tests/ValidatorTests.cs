using BHSDK.Models.Values;
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
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestCopy()
        {
            var validator = new LevelAnalyzer();
            var level = SerializationTests.CreateTestLevel();
            var copyLevel = level.Copy();
            
            var issues = validator.Analyze(copyLevel, new LevelAnalyzerSettings());
            Assert.IsEmpty(issues);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestCopyEquals()
        {
            var level = SerializationTests.CreateTestLevel();
            var copyLevel = level.Copy();
            Assert.IsTrue(level.Equals(copyLevel));
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestCopyNotEquals()
        {
            var level = SerializationTests.CreateTestLevel();
            var copyLevel = level.Copy();
            copyLevel.Game.Objects[0].Positions[0].Anchor.Value = Alignment.BottomLeftValue;
            Assert.IsFalse(level.Equals(copyLevel));
        }
        // TODO add generators tests
    }
}