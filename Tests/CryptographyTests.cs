using BHSDK.Services;
using NUnit.Framework;
using UnityEngine;

namespace BHSDK.Tests
{
    public class CryptographyTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void CryptographyTest()
        {
            var cryptography = new CryptographyService();

            const string text = "secret text";
            const string password = "cool password";
            
            var cryptedText = cryptography.Encrypt(text, password);
            var result = cryptography.Decrypt(cryptedText, password);
            
            Assert.AreEqual(text, result);
            Debug.Log($"{result} - {cryptedText}");
        }
    }
}