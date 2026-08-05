using BH.SDK.Serialization.Serializers;
using NUnit.Framework;

namespace BH.SDK.Tests
{
    public class SerializationTypeExtensionsTests
    {
        [TestCase(SerializationType.Json, ".json")]
        [TestCase(SerializationType.Bson, ".bson")]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void ToFileExtension_ReturnsExpectedExtension(SerializationType type, string expected)
        {
            Assert.AreEqual(expected, type.ToFileExtension());
        }

        [TestCase(".json", SerializationType.Json)]
        [TestCase(".JSON", SerializationType.Json)]
        [TestCase(".bson", SerializationType.Bson)]
        [TestCase(".BSON", SerializationType.Bson)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void TryFromFileExtension_ParsesKnownExtensions(string extension, SerializationType expected)
        {
            var result = SerializationTypeExtensions.TryFromFileExtension(extension, out var type);
            Assert.IsTrue(result);
            Assert.AreEqual(expected, type);
        }

        [TestCase(".txt")]
        [TestCase("")]
        [TestCase(".jsonx")]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void TryFromFileExtension_RejectsUnknownExtensions(string extension)
        {
            var result = SerializationTypeExtensions.TryFromFileExtension(extension, out _);
            Assert.IsFalse(result);
        }
    }
}
