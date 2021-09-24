using System;
using BenchmarkDotNet.Running;

namespace ReflectionSandbox
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            var benchmark = new ReflectionBenchmark();
            foreach (var model in benchmark.GenerateModelsByReflectionWithoutAttributes())
                Console.WriteLine(model);
#endif

#if RELEASE
            BenchmarkRunner.Run<ReflectionBenchmark>();
#endif
        }
    }
}
