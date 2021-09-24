using System;
using System.Diagnostics;
using BenchmarkDotNet.Running;

namespace ReflectionSandbox
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            var stopWatch = new Stopwatch();
            var benchmarks = new ReflectionBenchmarks();

            GC.Collect();

            stopWatch.Start();
            benchmarks.GenerateModelsByHand();
            stopWatch.Stop();
            var alloc = GC.GetTotalAllocatedBytes(true) / 1000000;
            Console.WriteLine($"{nameof(benchmarks.GenerateModelsByHand)} finished in {stopWatch.ElapsedMilliseconds}ms, mem-alloc: {alloc}mb");

            stopWatch.Reset();
            GC.Collect();

            stopWatch.Start();
            benchmarks.GenerateModelsByReflectionWithAttributes();
            stopWatch.Stop();
            alloc = GC.GetTotalAllocatedBytes(true) / 1000000 - alloc;
            Console.WriteLine($"{nameof(benchmarks.GenerateModelsByReflectionWithAttributes)} finished in {stopWatch.ElapsedMilliseconds}ms, mem-alloc: {alloc}mb");

            stopWatch.Reset();
            GC.Collect();

            stopWatch.Start();
            benchmarks.GenerateModelsByReflectionWithoutAttributes();
            stopWatch.Stop();
            alloc = GC.GetTotalAllocatedBytes(true) / 1000000 - alloc;
            Console.WriteLine($"{nameof(benchmarks.GenerateModelsByReflectionWithoutAttributes)} finished in {stopWatch.ElapsedMilliseconds}ms, mem-alloc: {alloc}mb");
#endif

#if RELEASE
            BenchmarkRunner.Run<ReflectionBenchmarks>();
#endif
        }
    }
}
