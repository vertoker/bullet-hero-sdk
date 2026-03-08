using BHSDK.Models.V1.Values;
using NUnit.Framework;
using UnityEngine;

namespace BulletHeroSDK.Tests.Models.Values
{
    public class FloatValueMinMaxTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1()
        {
            var floatValueMinMaxV1 = new FloatValueMinMaxV1();
            
            Assert.True(floatValueMinMaxV1.Min == 0f);
            Assert.True(Mathf.Approximately(floatValueMinMaxV1.Max, 1f));
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_2()
        {
            const float min = 2f;
            const float max = 5f;
            
            var floatValueMinMaxV1 = new FloatValueMinMaxV1(min, max);
            
            Assert.True(Mathf.Approximately(floatValueMinMaxV1.Min, min));
            Assert.True(Mathf.Approximately(floatValueMinMaxV1.Max, max));
        }
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void GetV1()
        {
            const float min = 2f;
            const float max = 5f;
            
            var floatValueMinMaxV1 = new FloatValueMinMaxV1(min, max);

            var value = floatValueMinMaxV1.Get();
            Assert.True(min <= value);
            Assert.True(value <= max);
        }
        
        // other versions add here
    }
}