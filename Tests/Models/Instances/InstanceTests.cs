using System.Collections.Generic;
using BHSDK.Models.Interfaces.Components;
using BHSDK.Models.Interfaces.Instances;
using BHSDK.Models.V1.Instances;
using NUnit.Framework;

namespace BulletHeroSDK.Tests.Models.Instances
{
    public class InstanceTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1()
        {
            var instanceV1 = new InstanceV1();
            
            Assert.True(instanceV1.InstanceId == 0);
            Assert.True(instanceV1.ParentInstanceId == 0);
            Assert.True(instanceV1.IsActive);
            Assert.True(instanceV1.HasCollider);
            Assert.True(instanceV1.SpriteIndex == 0);
            Assert.True(instanceV1.SpriteLayer == 0);
            Assert.True(instanceV1.SublingIndex == 0);
            
            Assert.True(instanceV1.Pos != null);
            Assert.True(instanceV1.Rot != null);
            Assert.True(instanceV1.Sca != null);
            Assert.True(instanceV1.Clr != null);
            Assert.True(instanceV1.StartFrame == 0);
            Assert.True(instanceV1.EndFrame == 0);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_2()
        {
            const int instanceId = 23;
            const int parentInstanceId = 22;
            const bool isActive = true;
            const bool hasCollider = false;
            const int spriteIndex = 20;
            const int spriteLayer = 777;
            const int sublingIndex = 1;
            
            var pos = new List<IPos>();
            var rot = new List<IRot>();
            var sca = new List<ISca>();
            var clr = new List<IClr>();
            const int startFrame = 123;
            const int endFrame = 321;
            
            var levelObjectV1 = new InstanceV1(instanceId, parentInstanceId, isActive, hasCollider, 
                spriteIndex, spriteLayer, sublingIndex, pos, rot, sca, clr, startFrame, endFrame);
            
            Assert.True(levelObjectV1.InstanceId == instanceId);
            Assert.True(levelObjectV1.ParentInstanceId == parentInstanceId);
            Assert.True(levelObjectV1.IsActive == isActive);
            Assert.True(levelObjectV1.HasCollider == hasCollider);
            Assert.True(levelObjectV1.SpriteIndex == spriteIndex);
            Assert.True(levelObjectV1.SpriteLayer == spriteLayer);
            Assert.True(levelObjectV1.SublingIndex == sublingIndex);
            
            Assert.True(levelObjectV1.Pos == pos);
            Assert.True(levelObjectV1.Rot == rot);
            Assert.True(levelObjectV1.Sca == sca);
            Assert.True(levelObjectV1.Clr == clr);
            Assert.True(levelObjectV1.StartFrame == startFrame);
            Assert.True(levelObjectV1.EndFrame == endFrame);
        }
        
        // other versions add here
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructCurrent()
        {
            var currentType = typeof(InstanceV1);
            Assert.True(IInstance.CreateNew().GetType() == currentType);
            Assert.True(IInstance.GetModelType == currentType);
        }
    }
}