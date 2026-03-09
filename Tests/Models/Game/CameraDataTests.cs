using System.Collections.Generic;
using BHSDK.Models.Interfaces.Components;
using BHSDK.Models.Interfaces.Game;
using BHSDK.Models.V1.Game;
using NUnit.Framework;

namespace BulletHeroSDK.Tests.Models.Game
{
    public class CameraDataTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1()
        {
            var cameraDataV1 = new CameraDataV1();
            
            Assert.True(cameraDataV1.Pos != null);
            Assert.True(cameraDataV1.Rot != null);
            Assert.True(cameraDataV1.Zoom != null);
            Assert.True(cameraDataV1.Clr != null);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_2()
        {
            var pos = new List<IPos>();
            var rot = new List<IRot>();
            var zoom = new List<IZoom>();
            var clr = new List<IClr>();
            
            var cameraDataV1 = new CameraDataV1(pos, rot, zoom, clr);
            
            Assert.True(cameraDataV1.Pos == pos);
            Assert.True(cameraDataV1.Rot == rot);
            Assert.True(cameraDataV1.Zoom == zoom);
            Assert.True(cameraDataV1.Clr == clr);
        }
        
        // other versions add here
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructCurrent()
        {
            var currentType = typeof(CameraDataV1);
            Assert.True(ICameraData.CreateNew().GetType() == currentType);
            Assert.True(ICameraData.GetModelType == currentType);
        }
    }
}