using BHSDK.Models.Interfaces;
using BHSDK.Models.V1;
using NUnit.Framework;

namespace BulletHeroSDK.Tests.Models
{
    public class LevelRulesTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1()
        {
            var levelRulesV1 = new LevelRulesV1();
            
            Assert.True(levelRulesV1 != null);
        }
        
        // other versions add here
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructCurrent()
        {
            var currentType = typeof(LevelRulesV1);
            Assert.True(ILevelRules.CreateNew().GetType() == currentType);
            Assert.True(ILevelRules.GetModelType == currentType);
        }
    }
}