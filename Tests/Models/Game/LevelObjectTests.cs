using System.Collections.Generic;
using BHSDK.Models.Interfaces.Components;
using BHSDK.Models.Interfaces.Game;
using BHSDK.Models.V1.Game;
using NUnit.Framework;

namespace BulletHeroSDK.Tests.Models.Game
{
    public class LevelObjectTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1()
        {
            var levelObjectV1 = new LevelObjectV1();
            
            Assert.True(levelObjectV1.ObjectId == 0);
            Assert.True(levelObjectV1.ParentObjectId == 0);
            Assert.True(levelObjectV1.IsActive);
            Assert.True(levelObjectV1.HasCollider);
            Assert.True(levelObjectV1.SpriteIndex == 0);
            Assert.True(levelObjectV1.VisibleLayer == 0);
            
            Assert.True(levelObjectV1.Pos != null);
            Assert.True(levelObjectV1.Rot != null);
            Assert.True(levelObjectV1.Sca != null);
            Assert.True(levelObjectV1.Clr != null);
            Assert.True(levelObjectV1.StartFrame == 0);
            Assert.True(levelObjectV1.EndFrame == 0);
            Assert.True(levelObjectV1.HeightIndex == 0);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_2()
        {
            const int objectId = 23;
            const int parentObjectId = 22;
            const bool isActive = true;
            const bool hasCollider = false;
            const int spriteIndex = 20;
            const int visibleLayer = 777;
            
            var pos = new List<IPos>();
            var rot = new List<IRot>();
            var sca = new List<ISca>();
            var clr = new List<IClr>();
            const int startFrame = 123;
            const int endFrame = 321;
            const int heightIndex = 1;
            
            var levelObjectV1 = new LevelObjectV1(objectId, parentObjectId, isActive, hasCollider, 
                spriteIndex, visibleLayer, pos, rot, sca, clr, startFrame, endFrame, heightIndex);
            
            Assert.True(levelObjectV1.ObjectId == objectId);
            Assert.True(levelObjectV1.ParentObjectId == parentObjectId);
            Assert.True(levelObjectV1.IsActive == isActive);
            Assert.True(levelObjectV1.HasCollider == hasCollider);
            Assert.True(levelObjectV1.SpriteIndex == spriteIndex);
            Assert.True(levelObjectV1.VisibleLayer == visibleLayer);
            
            Assert.True(levelObjectV1.Pos == pos);
            Assert.True(levelObjectV1.Rot == rot);
            Assert.True(levelObjectV1.Sca == sca);
            Assert.True(levelObjectV1.Clr == clr);
            Assert.True(levelObjectV1.StartFrame == startFrame);
            Assert.True(levelObjectV1.EndFrame == endFrame);
            Assert.True(levelObjectV1.HeightIndex == heightIndex);
        }
        
        // other versions add here
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructCurrent()
        {
            var currentType = typeof(LevelObjectV1);
            Assert.True(ILevelObject.CreateNew().GetType() == currentType);
            Assert.True(ILevelObject.GetModelType == currentType);
        }
    }
}