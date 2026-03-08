using BHSDK.Models.V1.Values;
using NUnit.Framework;
using UnityEngine;

namespace BulletHeroSDK.Tests.Models.Values
{
    public class FloatValueTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1()
        {
            var floatValueV1 = new FloatValueV1();
            
            Assert.True(floatValueV1.Value == 0f);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_2()
        {
            const float value = 2f;
            
            var floatValueV1 = new FloatValueV1(value);
            
            Assert.True(Mathf.Approximately(floatValueV1.Value, value));
        }
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void GetV1()
        {
            const float value = 2f;
            
            var floatValueV1 = new FloatValueV1(value);
            
            Assert.True(Mathf.Approximately(floatValueV1.Get(), value));
        }
        
        
        // other versions add here
    }
}