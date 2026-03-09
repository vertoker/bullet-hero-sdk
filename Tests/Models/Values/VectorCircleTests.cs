using BHSDK.Models.Enum.Values;
using BHSDK.Models.V1.Values;
using NUnit.Framework;
using UnityEngine;

namespace BulletHeroSDK.Tests.Models.Values
{
    public class VectorCircleTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1()
        {
            var vectorCircleV1 = new VectorCircleV1();
            
            Assert.True(vectorCircleV1.X == 0f);
            Assert.True(vectorCircleV1.Y == 0f);
            Assert.True(Mathf.Approximately(vectorCircleV1.Radius, 1f));
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_2()
        {
            var x = new FloatValueV1(2f);
            var y = new FloatValueV1(3f);
            var radius = new FloatValueV1(2f);
            
            var vectorCircleV1 = new VectorCircleV1(x, y, radius);
            
            Assert.True(Mathf.Approximately(vectorCircleV1.X, x.Get()));
            Assert.True(Mathf.Approximately(vectorCircleV1.Y, y.Get()));
            Assert.True(Mathf.Approximately(vectorCircleV1.Radius, radius.Get()));
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_3()
        {
            const float x = 4f;
            const float y = 6f;
            const float radius = 2f;
            
            var vectorCircleV1 = new VectorCircleV1(x, y, radius);
            
            Assert.True(Mathf.Approximately(vectorCircleV1.X, x));
            Assert.True(Mathf.Approximately(vectorCircleV1.Y, y));
            Assert.True(Mathf.Approximately(vectorCircleV1.Radius, radius));
        }
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TypeV1()
        {
            var vectorCircleV1 = new VectorCircleV1();
            
            Assert.True(vectorCircleV1.Type == VectorType.RandomCircle);
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void GetV1()
        {
            const float x = 4f;
            const float y = 6f;
            const float radius = 2f;
            
            var vectorCircleV1 = new VectorCircleV1(x, y, radius);
            var value = vectorCircleV1.Get();
            
            Assert.True(Vector2.Distance(value, new Vector2(x, y)) <= radius);
        }
        
        // other versions add here
    }
}