using BHSDK.Models.Enum.Values;
using BHSDK.Models.V1.Values;
using NUnit.Framework;

namespace BulletHeroSDK.Tests.Models.Values
{
    public class IntValueTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1()
        {
            var intValueV1 = new IntValueV1();
            
            Assert.True(intValueV1.Value == 0);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_2()
        {
            const int value = 2;
            
            var intValueV1 = new IntValueV1(value);
            
            Assert.True(intValueV1.Value == value);
        }
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TypeV1()
        {
            var intValueV1 = new IntValueV1();
            
            Assert.True(intValueV1.Type == IntType.Value);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void GetV1()
        {
            const int value = 2;
            
            var intValueV1 = new IntValueV1(value);
            
            Assert.True(intValueV1.Get() == value);
        }
        
        // other versions add here
    }
}