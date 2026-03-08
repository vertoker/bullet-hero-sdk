using System.Collections.Generic;
using BHSDK.Models.Interfaces;
using BHSDK.Models.V1;
using NUnit.Framework;

namespace BulletHeroSDK.Tests.Models
{
    public class LevelTrackTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1()
        {
            var levelTrackV1 = new LevelTrackV1();
            
            Assert.True(levelTrackV1.Title != null);
            Assert.True(levelTrackV1.Author != null);
            Assert.True(levelTrackV1.Sources != null);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_2()
        {
            const string title = "Life Forever";
            const string author = "Oasis";
            var sources = new List<ILevelTrackSource>();
            
            var levelTrackV1 = new LevelTrackV1(title, author, sources);
            
            Assert.True(levelTrackV1.Title == title);
            Assert.True(levelTrackV1.Author == author);
            Assert.True(levelTrackV1.Sources == sources);
        }
        
        // other versions add here
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructCurrent()
        {
            var currentType = typeof(LevelTrackV1);
            Assert.True(ILevelTrack.CreateNew().GetType() == currentType);
            Assert.True(ILevelTrack.GetModelType == currentType);
        }
    }
}