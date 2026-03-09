using BHSDK.Models.Enum.Values;
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
            var vectorV1 = new VectorValueV1();
            
            Assert.True(vectorV1.X == 0f);
            Assert.True(vectorV1.Y == 0f);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_2()
        {
            var x = new FloatValueV1(2f);
            var y = new FloatValueV1(5f);
            
            var vectorV1 = new VectorValueV1(x, y);
            
            Assert.True(Mathf.Approximately(vectorV1.X, x.Get()));
            Assert.True(Mathf.Approximately(vectorV1.Y, y.Get()));
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_3()
        {
            const float x = 4f;
            const float y = 6f;
            
            var vectorV1 = new VectorValueV1(x, y);
            
            Assert.True(Mathf.Approximately(vectorV1.X, x));
            Assert.True(Mathf.Approximately(vectorV1.Y, y));
        }
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void GetV1()
        {
            const float x = 4f;
            const float y = 6f;
            
            var vectorV1 = new VectorValueV1(x, y);
            var value = vectorV1.Get();
            
            Assert.True(Mathf.Approximately(value.x, x));
            Assert.True(Mathf.Approximately(value.y, y));
        }
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TypeV1()
        {
            var vectorV1 = new VectorValueV1();
            
            Assert.True(vectorV1.Type == VectorType.Value);
        }
        // other versions add here
    }
}