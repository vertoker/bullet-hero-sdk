using BHSDK.Models.Interfaces;
using BHSDK.Models.V1;
using BHSDK.Models.V1.Game;
using NUnit.Framework;

namespace BulletHeroSDK.Tests.Models
{
    public class GameplayTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1()
        {
            var levelGameplayV1 = new LevelGameplayV1();
            
            Assert.True(levelGameplayV1 != null);
        }
        
        // other versions add here
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructCurrent()
        {
            var currentType = typeof(LevelGameplayV1);
            Assert.True(ILevelGameplay.CreateNew().GetType() == currentType);
            Assert.True(ILevelGameplay.GetModelType == currentType);
        }
    }
}