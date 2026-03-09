using BHSDK.Models.Interfaces.Game;
using BHSDK.Models.V1.Game;
using BHSDK.Models.V1.Values;
using NUnit.Framework;

namespace BulletHeroSDK.Tests.Models.Game
{
    public class CheckpointTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1()
        {
            var checkpointV1 = new CheckpointV1();
            
            Assert.True(checkpointV1.Name != null);
            Assert.True(checkpointV1.Active);
            Assert.True(checkpointV1.Frame == 0);
            Assert.True(checkpointV1.Color != null);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_2()
        {
            const string name = "Test Checkpoint";
            const bool active = false;
            const int frame = 123;
            var color = new ColorValueV1(1f, 1f, 1f, 1f);
            
            var checkpointV1 = new CheckpointV1(name, active, frame, color);
            
            Assert.True(checkpointV1.Name == name);
            Assert.True(checkpointV1.Active == active);
            Assert.True(checkpointV1.Frame == frame);
            Assert.True(checkpointV1.Color == color);
        }
        
        // other versions add here
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructCurrent()
        {
            var currentType = typeof(CheckpointV1);
            Assert.True(ICheckpoint.CreateNew().GetType() == currentType);
            Assert.True(ICheckpoint.GetModelType == currentType);
        }
    }
}