using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Components;
using BHSDK.Models.V1.Components;
using BHSDK.Models.V1.Values;
using NUnit.Framework;

namespace BulletHeroSDK.Tests.Models.Components
{
    public class ClrTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1()
        {
            var clrV1 = new ClrV1();
            
            Assert.True(clrV1.Frame == 0);
            Assert.True(clrV1.Color != null);
            Assert.True(clrV1.Easing == EasingType.Linear);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_2()
        {
            const int frame = 1235;
            const EasingType easing = EasingType.InOutElastic;
            var color = new ColorV1(1f, 1f, 1f, 1f);
            
            var clrV1 = new ClrV1(frame, color, easing);
            
            Assert.True(clrV1.Frame == frame);
            Assert.True(clrV1.Color == color);
            Assert.True(clrV1.Easing == easing);
        }
        
        // other versions add here
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructCurrent()
        {
            var currentType = typeof(ClrV1);
            Assert.True(IClr.CreateNew().GetType() == currentType);
            Assert.True(IClr.GetModelType == currentType);
        }
    }
}