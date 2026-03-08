using BHSDK.Models.V1.Values;
using NUnit.Framework;
using UnityEngine;

namespace BulletHeroSDK.Tests.Models.Values
{
    public class FloatValueMinMaxStepTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1()
        {
            var floatValueMinMaxStepV1 = new FloatValueMinMaxStepV1();
            
            Assert.True(floatValueMinMaxStepV1.Min == 0f);
            Assert.True(Mathf.Approximately(floatValueMinMaxStepV1.Max, 1f));
            Assert.True(Mathf.Approximately(floatValueMinMaxStepV1.Step, 1f));
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_2()
        {
            const float min = 2f;
            const float max = 5f;
            const float step = 1f;
            
            var floatValueMinMaxStepV1 = new FloatValueMinMaxStepV1(min, max, step);
            
            Assert.True(Mathf.Approximately(floatValueMinMaxStepV1.Min, min));
            Assert.True(Mathf.Approximately(floatValueMinMaxStepV1.Max, max));
            Assert.True(Mathf.Approximately(floatValueMinMaxStepV1.Step, step));
        }
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void GetV1()
        {
            const float min = 2f;
            const float max = 2f;
            const float step = 1f;
            
            var floatValueMinMaxStepV1 = new FloatValueMinMaxStepV1(min, max, step);

            var value = floatValueMinMaxStepV1.Get();
            Assert.True(Mathf.Approximately(min, value));
            Assert.True(Mathf.Approximately(value, max));
        }
        
        // other versions add here
    }
}