using BHSDK.Models.Enum.Values;
using BHSDK.Models.V1.Values;
using NUnit.Framework;
using UnityEngine;

namespace BulletHeroSDK.Tests.Models.Values
{
    public class ColorMinMaxTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1()
        {
            var colorMinMaxV1 = new ColorMinMaxV1();
            
            Assert.True(Mathf.Approximately(colorMinMaxV1.MinR, 0f));
            Assert.True(Mathf.Approximately(colorMinMaxV1.MinG, 0f));
            Assert.True(Mathf.Approximately(colorMinMaxV1.MinB, 0f));
            Assert.True(Mathf.Approximately(colorMinMaxV1.MinA, 0f));
            Assert.True(Mathf.Approximately(colorMinMaxV1.MaxR, 1f));
            Assert.True(Mathf.Approximately(colorMinMaxV1.MaxG, 1f));
            Assert.True(Mathf.Approximately(colorMinMaxV1.MaxB, 1f));
            Assert.True(Mathf.Approximately(colorMinMaxV1.MaxA, 1f));
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_2()
        {
            var minR = new FloatValueV1(0.1f);
            var minG = new FloatValueV1(0.2f);
            var minB = new FloatValueV1(0.3f);
            var minA = new FloatValueV1(0.4f);
            var maxR = new FloatValueV1(0.5f);
            var maxG = new FloatValueV1(0.6f);
            var maxB = new FloatValueV1(0.7f);
            var maxA = new FloatValueV1(0.8f);
            
            var colorMinMaxV1 = new ColorMinMaxV1(minR, minG, minB, minA, maxR, maxG, maxB, maxA);
            
            Assert.True(Mathf.Approximately(colorMinMaxV1.MinR, minR.Get()));
            Assert.True(Mathf.Approximately(colorMinMaxV1.MinG, minG.Get()));
            Assert.True(Mathf.Approximately(colorMinMaxV1.MinB, minB.Get()));
            Assert.True(Mathf.Approximately(colorMinMaxV1.MinA, minA.Get()));
            Assert.True(Mathf.Approximately(colorMinMaxV1.MaxR, maxR.Get()));
            Assert.True(Mathf.Approximately(colorMinMaxV1.MaxG, maxG.Get()));
            Assert.True(Mathf.Approximately(colorMinMaxV1.MaxB, maxB.Get()));
            Assert.True(Mathf.Approximately(colorMinMaxV1.MaxA, maxA.Get()));
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_3()
        {
            const float minR = 0.1f;
            const float minG = 0.2f;
            const float minB = 0.3f;
            const float minA = 0.4f;
            const float maxR = 0.5f;
            const float maxG = 0.6f;
            const float maxB = 0.7f;
            const float maxA = 0.8f;
            
            var colorMinMaxV1 = new ColorMinMaxV1(minR, minG, minB, minA, maxR, maxG, maxB, maxA);
            
            Assert.True(Mathf.Approximately(colorMinMaxV1.MinR, minR));
            Assert.True(Mathf.Approximately(colorMinMaxV1.MinG, minG));
            Assert.True(Mathf.Approximately(colorMinMaxV1.MinB, minB));
            Assert.True(Mathf.Approximately(colorMinMaxV1.MinA, minA));
            Assert.True(Mathf.Approximately(colorMinMaxV1.MaxR, maxR));
            Assert.True(Mathf.Approximately(colorMinMaxV1.MaxG, maxG));
            Assert.True(Mathf.Approximately(colorMinMaxV1.MaxB, maxB));
            Assert.True(Mathf.Approximately(colorMinMaxV1.MaxA, maxA));
        }
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TypeV1()
        {
            var colorMinMaxV1 = new ColorMinMaxV1();
            
            Assert.True(colorMinMaxV1.Type == ColorType.RandomMinMax);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void GetV1()
        {
            const float minR = 0.1f;
            const float minG = 0.2f;
            const float minB = 0.3f;
            const float minA = 0.4f;
            const float maxR = 0.5f;
            const float maxG = 0.6f;
            const float maxB = 0.7f;
            const float maxA = 0.8f;
            
            var colorMinMaxV1 = new ColorMinMaxV1(minR, minG, minB, minA, maxR, maxG, maxB, maxA);
            var value = colorMinMaxV1.Get();
            
            Assert.True(minR <= value.r);
            Assert.True(value.r <= maxR);
            Assert.True(minG <= value.g);
            Assert.True(value.g <= maxG);
            Assert.True(minB <= value.b);
            Assert.True(value.b <= maxB);
            Assert.True(minA <= value.a);
            Assert.True(value.a <= maxA);
        }
        
        // other versions add here
    }
}