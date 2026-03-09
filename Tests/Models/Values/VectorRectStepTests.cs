using BHSDK.Models.Enum.Values;
using BHSDK.Models.V1.Values;
using NUnit.Framework;
using UnityEngine;

namespace BulletHeroSDK.Tests.Models.Values
{
    public class VectorRectStepTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1()
        {
            var vectorRectStepV1 = new VectorRectStepV1();
            
            Assert.True(vectorRectStepV1.MinX == 0f);
            Assert.True(vectorRectStepV1.MinY == 0f);
            Assert.True(Mathf.Approximately(vectorRectStepV1.MaxX, 1f));
            Assert.True(Mathf.Approximately(vectorRectStepV1.MaxY, 1f));
            Assert.True(Mathf.Approximately(vectorRectStepV1.Step, 1f));
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_2()
        {
            var minX = new FloatValueV1(2f);
            var minY = new FloatValueV1(3f);
            var maxX = new FloatValueV1(4f);
            var maxY = new FloatValueV1(5f);
            var step = new FloatValueV1(6f);
            
            var vectorRectStepV1 = new VectorRectStepV1(minX, minY, maxX, maxY, step);
            
            Assert.True(Mathf.Approximately(vectorRectStepV1.MinX, minX.Get()));
            Assert.True(Mathf.Approximately(vectorRectStepV1.MinY, minY.Get()));
            Assert.True(Mathf.Approximately(vectorRectStepV1.MaxX, maxX.Get()));
            Assert.True(Mathf.Approximately(vectorRectStepV1.MaxY, maxY.Get()));
            Assert.True(Mathf.Approximately(vectorRectStepV1.Step, step.Get()));
        }
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void ConstructV1_3()
        {
            const float minX = 3f;
            const float minY = 4f;
            const float maxX = 5f;
            const float maxY = 6f;
            const float step = 7f;
            
            var vectorRectStepV1 = new VectorRectStepV1(minX, minY, maxX, maxY, step);
            
            Assert.True(Mathf.Approximately(vectorRectStepV1.MinX, minX));
            Assert.True(Mathf.Approximately(vectorRectStepV1.MinY, minY));
            Assert.True(Mathf.Approximately(vectorRectStepV1.MaxX, maxX));
            Assert.True(Mathf.Approximately(vectorRectStepV1.MaxY, maxY));
            Assert.True(Mathf.Approximately(vectorRectStepV1.Step, step));
        }
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TypeV1()
        {
            var vectorRectStepV1 = new VectorRectStepV1();
            
            Assert.True(vectorRectStepV1.Type == VectorType.RandomRectStep);
        }
        // other versions add here
    }
}