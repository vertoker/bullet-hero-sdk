using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.V1;
using BHSDK.Models.V1.Values;
using NUnit.Framework;
using UnityEngine;

namespace BulletHeroSDK.Tests.Models.Values
{
    public class ColorTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1()
        {
            var colorV1 = new ColorV1();
            
            Assert.True(colorV1.R != null);
            Assert.True(colorV1.G != null);
            Assert.True(colorV1.B != null);
            Assert.True(colorV1.A != null);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_2()
        {
            var r = new FloatValueV1(1f);
            var g = new FloatValueMinMaxV1(0f, 1f);
            var b = new FloatValueMinMaxStepV1(0f, 1f, 1f);
            var a = new FloatValueV1(1f);
            
            var colorV1 = new ColorV1(r, g, b, a);
            
            Assert.True(colorV1.R == r);
            Assert.True(colorV1.G == g);
            Assert.True(colorV1.B == b);
            Assert.True(colorV1.A == a);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_3()
        {
            const float r = 1f;
            const float g = 0.5f;
            const float b = 0.3f;
            const float a = 0.7f;
            
            var colorV1 = new ColorV1(r, g, b, a);
            
            Assert.True(Mathf.Approximately(colorV1.R.Get(), r));
            Assert.True(Mathf.Approximately(colorV1.G.Get(), g));
            Assert.True(Mathf.Approximately(colorV1.B.Get(), b));
            Assert.True(Mathf.Approximately(colorV1.A.Get(), a));
        }
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void GetV1()
        {
            const float r = 1f;
            const float g = 0.5f;
            const float b = 0.3f;
            const float a = 0.7f;
            
            var colorV1 = new ColorV1(r, g, b, a);
            var value = colorV1.Get();
            
            Assert.True(Mathf.Approximately(value.r, r));
            Assert.True(Mathf.Approximately(value.g, g));
            Assert.True(Mathf.Approximately(value.b, b));
            Assert.True(Mathf.Approximately(value.a, a));
        }
        
        // other versions add here
    }
}