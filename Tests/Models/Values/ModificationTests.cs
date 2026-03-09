using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.V1.Values;
using NUnit.Framework;

namespace BulletHeroSDK.Tests.Models.Values
{
    public class ModificationTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1()
        {
            var modificationV1 = new ModificationV1();
            
            Assert.True(modificationV1.PropertyPath != null);
            Assert.True(modificationV1.Value != null);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_2()
        {
            const string propertyPath = "/123/321";
            const string value = "3.03";
            
            var modificationV1 = new ModificationV1(propertyPath, value);
            
            Assert.True(modificationV1.PropertyPath == propertyPath);
            Assert.True(modificationV1.Value == value);
        }
        
        // other versions add here
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructCurrent()
        {
            var currentType = typeof(ModificationV1);
            Assert.True(IModification.CreateNew().GetType() == currentType);
            Assert.True(IModification.GetModelType == currentType);
        }
    }
}