using BH.SDK.Services;
using NUnit.Framework;

namespace BH.SDK.Tests
{
    public class TextFormatTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestTextFormat()
        {
            var service = new TextFormatService();
            service.AddVariable("username", () => Metadata.Author.Vertoker);
            
            Assert.AreEqual(service.Process("{username}"), Metadata.Author.Vertoker);
            Cat.Meow(service.Process("Hello <color=green>{username}</color>!"));
        }
    }
}