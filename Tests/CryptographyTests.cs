using BH.SDK.Services;
using NUnit.Framework;

namespace BH.SDK.Tests
{
    public class CryptographyTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Hard)]
        public void CryptographyTest()
        {
            var cryptography = new CryptographyService();

            const string text = "secret text";
            const string password = "cool password";
            
            var cryptedText = cryptography.Encrypt(text, password);
            var result = cryptography.Decrypt(cryptedText, password);
            
            Assert.AreEqual(text, result);
            Cat.Meow($"{result} - {cryptedText}");
        }
    }
}