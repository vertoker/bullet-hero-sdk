using System.Collections.Generic;
using BHSDK.Models.Interfaces.Game;
using BHSDK.Models.V1.Game;
using NUnit.Framework;

namespace BulletHeroSDK.Tests.Models.Game
{
    public class GameLevelTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1()
        {
            var gameLevelV1 = new GameLevelV1();
            
            Assert.True(gameLevelV1.Seed == 0);
            Assert.True(gameLevelV1.Framerate == 0);
            Assert.True(gameLevelV1.Markers != null);
            Assert.True(gameLevelV1.Checkpoints != null);
            Assert.True(gameLevelV1.Objects != null);
            Assert.True(gameLevelV1.CameraData != null);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_2()
        {
            const int seed = 1337;
            const int framerate = 60;
            var markers = new List<IMarker>();
            var checkpoints = new List<ICheckpoint>();
            var objects = new List<ILevelObject>();
            var cameraData = new CameraDataV1();
            
            var gameLevelV1 = new GameLevelV1(seed, framerate, markers, checkpoints, objects, cameraData);
            
            Assert.True(gameLevelV1.Seed == seed);
            Assert.True(gameLevelV1.Framerate == framerate);
            Assert.True(gameLevelV1.Markers == markers);
            Assert.True(gameLevelV1.Checkpoints == checkpoints);
            Assert.True(gameLevelV1.Objects == objects);
            Assert.True(gameLevelV1.CameraData == cameraData);
        }
        
        // other versions add here
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructCurrent()
        {
            var currentType = typeof(GameLevelV1);
            Assert.True(IGameLevel.CreateNew().GetType() == currentType);
            Assert.True(IGameLevel.GetModelType == currentType);
        }
    }
}