using BHSDK.Models.V1.Values;
using NUnit.Framework;
using UnityEngine;

namespace BulletHeroSDK.Tests.Models.Values
{
    public class VectorTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1()
        {
            var vectorV1 = new VectorV1();
            
            Assert.True(vectorV1.X != null);
            Assert.True(vectorV1.Y != null);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_2()
        {
            var x = new FloatValueV1(2f);
            var y = new FloatValueMinMaxV1(3f, 5f);
            
            var vectorV1 = new VectorV1(x, y);
            
            Assert.True(vectorV1.X == x);
            Assert.True(vectorV1.Y == y);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_3()
        {
            const float x = 4f;
            const float y = 6f;
            
            var vectorV1 = new VectorV1(x, y);
            
            Assert.True(Mathf.Approximately(vectorV1.X.Get(), x));
            Assert.True(Mathf.Approximately(vectorV1.Y.Get(), y));
        }
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void GetV1()
        {
            const float x = 4f;
            const float y = 6f;
            
            var vectorV1 = new VectorV1(x, y);
            var value = vectorV1.Get();
            
            Assert.True(Mathf.Approximately(value.x, x));
            Assert.True(Mathf.Approximately(value.y, y));
        }
        
        // other versions add here
    }
}