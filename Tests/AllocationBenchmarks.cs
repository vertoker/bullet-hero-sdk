using System;
using NUnit.Framework;
using Unity.PerformanceTesting;
using UnityEngine.Profiling;

namespace BHSDK.Tests
{
    public class AllocationBenchmarks
    {
        // In tests, Unity allocate memory by block. Size is 4 kb
        // If you try to allocate 4096 + 1 byte - Unity allocate for your array 2 blocks
        // And of course Unity doesn't report about new allocations, it just happened inside Unity itself
        // (also can run it in runtime and build, use runtime tests)
        
        [Test, Performance]
        [Author(Metadata.Author.Vertoker)]
        [TestCase(2)]
        [TestCase(4)]
        [TestCase(10)]
        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(10000)]
        public void ByteArrayProfiler(int arraySize)
        {
            var bytesGroup = new SampleGroup("GC", SampleUnit.Byte);

            for (var i = 0; i < 100; i++)
            {
                var beforeBytes = Profiler.GetMonoUsedSizeLong();
                var array = new byte[arraySize];
                var afterBytes = Profiler.GetMonoUsedSizeLong();
                Measure.Custom(bytesGroup, afterBytes - beforeBytes);
            }
        }
        
        [Test, Performance]
        [Author(Metadata.Author.Vertoker)]
        [TestCase(2)]
        [TestCase(4)]
        [TestCase(10)]
        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(10000)]
        public void ByteArrayGC(int arraySize)
        {
            var bytesGroup = new SampleGroup("GC", SampleUnit.Byte);

            for (var i = 0; i < 100; i++)
            {
                var beforeBytes = GC.GetTotalMemory(false);
                var array = new byte[arraySize];
                var afterBytes = GC.GetTotalMemory(false);
                Measure.Custom(bytesGroup, afterBytes - beforeBytes);
            }
        }
    }
}