using BHSDK.Models.Enum.Values;
using BHSDK.Models.V1.Values;
using NUnit.Framework;
using UnityEngine;

namespace BulletHeroSDK.Tests.Models.Values
{
    public class VectorRectTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1()
        {
            var vectorRectV1 = new VectorRectV1();
            
            Assert.True(vectorRectV1.MinX == 0f);
            Assert.True(vectorRectV1.MinY == 0f);
            Assert.True(Mathf.Approximately(vectorRectV1.MaxX, 1f));
            Assert.True(Mathf.Approximately(vectorRectV1.MaxY, 1f));
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_2()
        {
            var minX = new FloatValueV1(2f);
            var minY = new FloatValueV1(3f);
            var maxX = new FloatValueV1(4f);
            var maxY = new FloatValueV1(5f);
            
            var vectorRectV1 = new VectorRectV1(minX, minY, maxX, maxY);
            
            Assert.True(Mathf.Approximately(vectorRectV1.MinX, minX.Get()));
            Assert.True(Mathf.Approximately(vectorRectV1.MinY, minY.Get()));
            Assert.True(Mathf.Approximately(vectorRectV1.MaxX, maxX.Get()));
            Assert.True(Mathf.Approximately(vectorRectV1.MaxY, maxY.Get()));
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_3()
        {
            const float minX = 3f;
            const float minY = 4f;
            const float maxX = 5f;
            const float maxY = 6f;
            
            var vectorRectV1 = new VectorRectV1(minX, minY, maxX, maxY);
            
            Assert.True(Mathf.Approximately(vectorRectV1.MinX, minX));
            Assert.True(Mathf.Approximately(vectorRectV1.MinY, minY));
            Assert.True(Mathf.Approximately(vectorRectV1.MaxX, maxX));
            Assert.True(Mathf.Approximately(vectorRectV1.MaxY, maxY));
        }
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TypeV1()
        {
            var vectorRectV1 = new VectorRectV1();
            
            Assert.True(vectorRectV1.Type == VectorType.RandomRect);
        }
        // other versions add here
    }
}