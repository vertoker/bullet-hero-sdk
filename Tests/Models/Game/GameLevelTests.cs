using System.Collections.Generic;
using BHSDK.Models.Interfaces.Game;
using BHSDK.Models.Interfaces.Instances;
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
            Assert.True(gameLevelV1.CameraData != null);
            Assert.True(gameLevelV1.Instances != null);
            Assert.True(gameLevelV1.PrefabInstances != null);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_2()
        {
            const int seed = 1337;
            const int framerate = 60;
            var markers = new List<IMarker>();
            var checkpoints = new List<ICheckpoint>();
            var cameraData = new CameraDataV1();
            var instances = new List<IInstance>();
            var prefabInstances = new List<IPrefabInstance>();
            
            var gameLevelV1 = new GameLevelV1(seed, framerate, markers, checkpoints, cameraData, instances, prefabInstances);
            
            Assert.True(gameLevelV1.Seed == seed);
            Assert.True(gameLevelV1.Framerate == framerate);
            Assert.True(gameLevelV1.Markers == markers);
            Assert.True(gameLevelV1.Checkpoints == checkpoints);
            Assert.True(gameLevelV1.CameraData == cameraData);
            Assert.True(gameLevelV1.Instances == instances);
            Assert.True(gameLevelV1.PrefabInstances == prefabInstances);
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