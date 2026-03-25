using BHSDK.Services;
using NUnit.Framework;
using UnityEngine;

namespace BHSDK.Tests
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
            Debug.Log(service.Process("Hello <color=green>{username}</color>!"));
        }
    }
}