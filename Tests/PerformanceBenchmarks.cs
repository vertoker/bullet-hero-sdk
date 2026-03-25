using System.Linq;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace BHSDK.Tests
{
    public class PerformanceBenchmarks
    {
        [Test, Performance]
        [Author(Metadata.Author.Vertoker)]
        [TestCase(10_000)]
        [TestCase(100_000)]
        [TestCase(1_000_000)]
        public void MeasureForLoop(int collectionSize)
        {
            var array = Enumerable.Range(0, collectionSize).ToArray();

            Measure.Method(() =>
                {
                    var sum = 0;
                    for (var i = 0; i < collectionSize; i++)
                    {
                        sum += array[i];
                    }
                })
                .SampleGroup(new SampleGroup($"for, CollectionSize = {collectionSize:N0}", SampleUnit.Microsecond))
                // .GC()
                .WarmupCount(10)
                .MeasurementCount(100)
                .IterationsPerMeasurement(1)
                .Run();
        }
        
        [Test, Performance]
        [Author(Metadata.Author.Vertoker)]
        [TestCase(10_000)]
        [TestCase(100_000)]
        [TestCase(1_000_000)]
        public void MeasureForeachLoop(int collectionSize)
        {
            var array = Enumerable.Range(0, collectionSize).ToArray();

            Measure.Method(() =>
                {
                    var sum = 0;
                    foreach (var i in array)
                    {
                        sum += i;
                    }
                })
                .SampleGroup(new SampleGroup($"foreach, CollectionSize = {collectionSize:N0}", SampleUnit.Microsecond))
                // .GC()
                .WarmupCount(10)
                .MeasurementCount(100)
                .IterationsPerMeasurement(1)
                .Run();
        }
    }
}