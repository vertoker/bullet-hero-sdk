using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Components;
using BHSDK.Models.V1.Components;
using BHSDK.Models.V1.Values;
using NUnit.Framework;

namespace BulletHeroSDK.Tests.Models.Components
{
    public class ZoomTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1()
        {
            var zoomV1 = new ZoomV1();
            
            Assert.True(zoomV1.Frame == 0);
            Assert.True(zoomV1.Size != null);
            Assert.True(zoomV1.Easing == EasingType.Linear);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_2()
        {
            const int frame = 1437;
            const EasingType easing = EasingType.InCubic;
            var size = new FloatValueV1(59f);

            var zoomV1 = new ZoomV1(frame, size, easing);
            
            Assert.True(zoomV1.Frame == frame);
            Assert.True(zoomV1.Size == size);
            Assert.True(zoomV1.Easing == easing);
        }
        
        // other versions add here
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructCurrent()
        {
            var currentType = typeof(ZoomV1);
            Assert.True(IZoom.CreateNew().GetType() == currentType);
            Assert.True(IZoom.GetModelType == currentType);
        }
    }
}