using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Components;
using BHSDK.Models.V1.Components;
using BHSDK.Models.V1.Values;
using NUnit.Framework;

namespace BulletHeroSDK.Tests.Models.Components
{
    public class PosTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1()
        {
            var posV1 = new PosV1();
            
            Assert.True(posV1.Frame == 0);
            Assert.True(posV1.Vector != null);
            Assert.True(posV1.Anchor == Anchor.Center_Middle);
            Assert.True(posV1.Ease == EaseType.Linear);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_2()
        {
            const int frame = 1235;
            const Anchor anchor = Anchor.Center_Bottom;
            const EaseType ease = EaseType.InOutBack;
            var vector = new VectorValueV1(1f, 1f);

            var posV1 = new PosV1(frame, vector, anchor, ease);
            
            Assert.True(posV1.Frame == frame);
            Assert.True(posV1.Vector == vector);
            Assert.True(posV1.Anchor == anchor);
            Assert.True(posV1.Ease == ease);
        }
        
        // other versions add here
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructCurrent()
        {
            var currentType = typeof(PosV1);
            Assert.True(IPos.CreateNew().GetType() == currentType);
            Assert.True(IPos.GetModelType == currentType);
        }
    }
}