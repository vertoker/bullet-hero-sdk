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
            var validator = new LevelValidator();
            var level = SerializationTests.CreateTestLevel();
            var issues = validator.Validate(level, new LevelValidatorSettings());
            
            Assert.That(issues, Is.Empty);
        }
    }
}