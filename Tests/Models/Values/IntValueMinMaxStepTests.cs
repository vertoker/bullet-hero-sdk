using BHSDK.Models.V1.Values;
using NUnit.Framework;
using UnityEngine;

namespace BulletHeroSDK.Tests.Models.Values
{
    public class IntValueMinMaxStepTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1()
        {
            var intValueMinMaxStepV1 = new IntValueMinMaxStepV1();
            
            Assert.True(intValueMinMaxStepV1.Min == 0);
            Assert.True(intValueMinMaxStepV1.Max == 1);
            Assert.True(intValueMinMaxStepV1.Step == 1);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_2()
        {
            const int min = 2;
            const int max = 5;
            const int step = 1;
            
            var intValueMinMaxStepV1 = new IntValueMinMaxStepV1(min, max, step);
            
            Assert.True(intValueMinMaxStepV1.Min == min);
            Assert.True(intValueMinMaxStepV1.Max == max);
            Assert.True(intValueMinMaxStepV1.Step == step);
        }
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void GetV1()
        {
            const int min = 2;
            const int max = 5;
            const int step = 1;
            
            var intValueMinMaxStepV1 = new IntValueMinMaxStepV1(min, max, step);

            var value = intValueMinMaxStepV1.Get();
            Assert.True(min <= value);
            Assert.True(value <= max);
        }
        
        // other versions add here
    }
}