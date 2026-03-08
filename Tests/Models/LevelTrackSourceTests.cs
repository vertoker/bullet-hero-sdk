using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces;
using BHSDK.Models.V1;
using NUnit.Framework;
using UnityEngine;

namespace BulletHeroSDK.Tests.Models
{
    public class LevelTrackSourceTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1()
        {
            var levelTrackSourceV1 = new LevelTrackSourceV1();
            
            Assert.True(levelTrackSourceV1.Link != null);
            Assert.True(levelTrackSourceV1.LinkType == AudioLinkType.Undefined);
            Assert.True(levelTrackSourceV1.StartTime == 0f);
            Assert.True(levelTrackSourceV1.EndTime == 0f);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_2()
        {
            const string link = "./music.mp3";
            const AudioLinkType linkType = AudioLinkType.FilePath;
            const float startTime = 2f;
            const float endTime = 5f;
            
            var levelTrackSourceV1 = new LevelTrackSourceV1(link, linkType, startTime, endTime);
            
            Assert.True(levelTrackSourceV1.Link == link);
            Assert.True(levelTrackSourceV1.LinkType == linkType);
            Assert.True(Mathf.Approximately(levelTrackSourceV1.StartTime, startTime));
            Assert.True(Mathf.Approximately(levelTrackSourceV1.EndTime, endTime));
        }
        
        // other versions add here
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructCurrent()
        {
            var currentType = typeof(LevelTrackSourceV1);
            Assert.True(ILevelTrackSource.CreateNew().GetType() == currentType);
            Assert.True(ILevelTrackSource.GetModelType == currentType);
        }
    }
}