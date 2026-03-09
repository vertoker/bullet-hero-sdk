using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Components;
using BHSDK.Models.V1.Components;
using BHSDK.Models.V1.Values;
using NUnit.Framework;

namespace BulletHeroSDK.Tests.Models.Components
{
    public class ScaTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1()
        {
            var scaV1 = new ScaV1();
            
            Assert.True(scaV1.Frame == 0);
            Assert.True(scaV1.Vector != null);
            Assert.True(scaV1.Ease == EaseType.Linear);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_2()
        {
            const int frame = 1235;
            const EaseType ease = EaseType.InOutBack;
            var vector = new VectorValueV1(1f, 1f);

            var scaV1 = new ScaV1(frame, vector, ease);
            
            Assert.True(scaV1.Frame == frame);
            Assert.True(scaV1.Vector == vector);
            Assert.True(scaV1.Ease == ease);
        }
        
        // other versions add here
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructCurrent()
        {
            var currentType = typeof(ScaV1);
            Assert.True(ISca.CreateNew().GetType() == currentType);
            Assert.True(ISca.GetModelType == currentType);
        }
    }
}