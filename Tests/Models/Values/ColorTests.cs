using BHSDK.Models.Enum.Values;
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
            var colorV1 = new ColorValueV1();
            
            Assert.True(Mathf.Approximately(colorV1.R, 1f));
            Assert.True(Mathf.Approximately(colorV1.G, 1f));
            Assert.True(Mathf.Approximately(colorV1.B, 1f));
            Assert.True(Mathf.Approximately(colorV1.A, 1f));
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_2()
        {
            var r = new FloatValueV1(1f);
            var g = new FloatValueV1(0.3f);
            var b = new FloatValueV1(0.7f);
            var a = new FloatValueV1(1f);
            
            var colorV1 = new ColorValueV1(r, g, b, a);
            
            Assert.True(Mathf.Approximately(colorV1.R, r.Get()));
            Assert.True(Mathf.Approximately(colorV1.G, g.Get()));
            Assert.True(Mathf.Approximately(colorV1.B, b.Get()));
            Assert.True(Mathf.Approximately(colorV1.A, a.Get()));
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_3()
        {
            const float r = 1f;
            const float g = 0.5f;
            const float b = 0.3f;
            const float a = 0.7f;
            
            var colorV1 = new ColorValueV1(r, g, b, a);
            
            Assert.True(Mathf.Approximately(colorV1.R, r));
            Assert.True(Mathf.Approximately(colorV1.G, g));
            Assert.True(Mathf.Approximately(colorV1.B, b));
            Assert.True(Mathf.Approximately(colorV1.A, a));
        }
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TypeV1()
        {
            var colorV1 = new ColorValueV1();
            
            Assert.True(colorV1.Type == ColorType.Value);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void GetV1()
        {
            const float r = 1f;
            const float g = 0.5f;
            const float b = 0.3f;
            const float a = 0.7f;
            
            var colorV1 = new ColorValueV1(r, g, b, a);
            var value = colorV1.Get();
            
            Assert.True(Mathf.Approximately(value.r, r));
            Assert.True(Mathf.Approximately(value.g, g));
            Assert.True(Mathf.Approximately(value.b, b));
            Assert.True(Mathf.Approximately(value.a, a));
        }
        
        // other versions add here
    }
}