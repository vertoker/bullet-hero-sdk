using BHSDK.Models.Interfaces;
using BHSDK.Models.V1;
using NUnit.Framework;

namespace BulletHeroSDK.Tests.Models
{
    public class AuthorTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1()
        {
            var authorV1 = new AuthorV1();
            
            Assert.True(authorV1.Name != null);
            Assert.True(authorV1.Order == 0);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_2()
        {
            const string author = "vertoker";
            const int order = 1;
            
            var authorV1 = new AuthorV1(author, order);
            
            Assert.True(authorV1.Name == author);
            Assert.True(authorV1.Order == order);
        }
        
        // other versions add here
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructCurrent()
        {
            var currentType = typeof(AuthorV1);
            Assert.True(IAuthor.CreateNew().GetType() == currentType);
            Assert.True(IAuthor.GetModelType == currentType);
        }
    }
}