using BHSDK.Models.Interfaces;
using BHSDK.Models.V1;
using BHSDK.Models.V1.Game;
using NUnit.Framework;

namespace BulletHeroSDK.Tests.Models
{
    public class LevelTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1()
        {
            var levelV1 = new LevelV1();
            
            Assert.True(levelV1.Meta != null);
            Assert.True(levelV1.Track != null);
            Assert.True(levelV1.Rules != null);
            Assert.True(levelV1.Game != null);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_2()
        {
            var meta = new LevelMetaV1();
            var track = new LevelTrackV1();
            var gameplay = new LevelRulesV1();
            var game = new GameLevelV1();
            
            var levelV1 = new LevelV1(meta, track, gameplay, game);
            
            Assert.True(levelV1.Meta == meta);
            Assert.True(levelV1.Track == track);
            Assert.True(levelV1.Rules == gameplay);
            Assert.True(levelV1.Game == game);
        }
        
        // other versions add here
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructCurrent()
        {
            var currentType = typeof(LevelV1);
            Assert.True(ILevel.CreateNew().GetType() == currentType);
            Assert.True(ILevel.GetModelType == currentType);
        }
    }
}