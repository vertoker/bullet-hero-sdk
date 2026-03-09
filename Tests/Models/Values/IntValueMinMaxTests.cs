using BHSDK.Models.Enum.Values;
using BHSDK.Models.V1.Values;
using NUnit.Framework;
using UnityEngine;

namespace BulletHeroSDK.Tests.Models.Values
{
    public class IntValueMinMaxTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1()
        {
            var intValueMinMaxV1 = new IntMinMaxV1();
            
            Assert.True(intValueMinMaxV1.Min == 0);
            Assert.True(intValueMinMaxV1.Max == 1);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_2()
        {
            const int min = 2;
            const int max = 5;
            
            var intValueMinMaxV1 = new IntMinMaxV1(min, max);
            
            Assert.True(intValueMinMaxV1.Min == min);
            Assert.True(intValueMinMaxV1.Max == max);
        }
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TypeV1()
        {
            var intValueMinMaxV1 = new IntMinMaxV1();
            
            Assert.True(intValueMinMaxV1.Type == IntType.RandomMinMax);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void GetV1()
        {
            const int min = 2;
            const int max = 5;
            
            var intValueMinMaxV1 = new IntMinMaxV1(min, max);

            var value = intValueMinMaxV1.Get();
            Assert.True(min <= value);
            Assert.True(value <= max);
        }
        
        // other versions add here
    }
}