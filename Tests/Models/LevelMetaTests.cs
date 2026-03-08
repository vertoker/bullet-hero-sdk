using System;
using System.Collections.Generic;
using BHSDK.Models.Interfaces;
using BHSDK.Models.V1;
using NUnit.Framework;

namespace BulletHeroSDK.Tests.Models
{
    public class LevelMetaTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1()
        {
            var levelMetaV1 = new LevelMetaV1();
            
            Assert.True(levelMetaV1.LevelName != null);
            Assert.True(levelMetaV1.LevelVersion != null);
            Assert.True(levelMetaV1.Authors != null);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_2()
        {
            const string levelName = "Cool level name";
            var levelVersion = new Version(1, 2, 3, 4);
            var authors = new List<IAuthor>();
            
            var levelMetaV1 = new LevelMetaV1(levelName, levelVersion, authors);
            
            Assert.True(levelMetaV1.LevelName == levelName);
            Assert.True(levelMetaV1.LevelVersion == levelVersion);
            Assert.True(levelMetaV1.Authors == authors);
        }
        
        // other versions add here
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructCurrent()
        {
            var currentType = typeof(LevelMetaV1);
            Assert.True(ILevelMeta.CreateNew().GetType() == currentType);
            Assert.True(ILevelMeta.GetModelType == currentType);
        }
    }
}