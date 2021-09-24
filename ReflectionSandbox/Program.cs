using System;
using BenchmarkDotNet.Running;

namespace ReflectionSandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            /*var benchmark = new ReflectionBenchmark();
            Console.WriteLine("------- by reflection -------");
            foreach (var model in benchmark.GenerateModelsByReflection())
                Console.WriteLine(model);

            Console.WriteLine("------- by hand -------");
            foreach (var model in benchmark.GenerateModelsByHand())
                Console.WriteLine(model);*/

            BenchmarkRunner.Run<ReflectionBenchmark>();
        }
    }
}
