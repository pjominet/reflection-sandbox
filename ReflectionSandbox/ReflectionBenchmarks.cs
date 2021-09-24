using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using ReflectionSandbox.Models;

namespace ReflectionSandbox
{
    [MemoryDiagnoser]
    [RankColumn]
    public class ReflectionBenchmarks
    {
        private IEnumerable<Dictionary<string, string>> _mappingInfos;

        private static IEnumerable<Dictionary<string, string>> GeneratedMappingInfos(int iterations = 50000)
        {
            var currentDateString = DateTime.Now.ToString("d");

            for (var i = 1; i <= iterations; i++)
            {
                yield return new Dictionary<string, string>
                {
                    { "CustomIdentifier", $"{i}" },
                    { "Label", "Foo" },
                    { "Value", $"{777d / i}" },
                    { "CreatedOn", currentDateString },
                    { "IsDeleted", "false" },
                    { "Foo", "" },
                    { "Bar", "" },
                    { "Number", "1337" },
                    { "Decimal", null },
                    { "AnotherDate", null }
                };
            }
        }

        public ReflectionBenchmarks()
        {
            _mappingInfos = GeneratedMappingInfos();
        }

        [GlobalSetup]
        public void Setup()
        {
            _mappingInfos = GeneratedMappingInfos();
        }

        [Benchmark] public List<Model> GenerateModelsByHand() => ModelGenerators.GenerateModelsByHand(_mappingInfos);

        [Benchmark] public List<Model> GenerateModelsByReflectionWithAttributes() => ModelGenerators.GenerateModelsByReflectionWithAttributes(_mappingInfos);

        [Benchmark] public List<Model> GenerateModelsByReflectionWithoutAttributes() => ModelGenerators.GenerateModelsByReflectionWithoutAttributes(_mappingInfos);

        //[Benchmark] public List<ModelForDelegates> GenerateModelsByReflectionWithDelegates() => ModelGenerators.GenerateModelsByReflectionWithDelegates(_mappingInfos);
    }
}
