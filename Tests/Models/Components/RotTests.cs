using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Components;
using BHSDK.Models.V1.Components;
using BHSDK.Models.V1.Values;
using NUnit.Framework;

namespace BulletHeroSDK.Tests.Models.Components
{
    public class RotTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1()
        {
            var rotV1 = new RotV1();
            
            Assert.True(rotV1.Frame == 0);
            Assert.True(rotV1.Angle != null);
            Assert.True(rotV1.Easing == EasingType.Linear);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_2()
        {
            const int frame = 1435;
            const EasingType easing = EasingType.InCirc;
            var angle = new FloatValueV1(5f);

            var rotV1 = new RotV1(frame, angle, easing);
            
            Assert.True(rotV1.Frame == frame);
            Assert.True(rotV1.Angle == angle);
            Assert.True(rotV1.Easing == easing);
        }
        
        // other versions add here
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructCurrent()
        {
            var currentType = typeof(RotV1);
            Assert.True(IRot.CreateNew().GetType() == currentType);
            Assert.True(IRot.GetModelType == currentType);
        }
    }
}