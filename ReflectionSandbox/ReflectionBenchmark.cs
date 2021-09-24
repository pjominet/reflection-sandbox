using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace ReflectionSandbox
{
    [MemoryDiagnoser]
    public class ReflectionBenchmark
    {
        private readonly string _currentDateString = DateTime.Now.ToString("d");
        private readonly IEnumerable<Dictionary<string, string>> _mappingInfos;

        public ReflectionBenchmark(int iterations = 10000)
        {
            _mappingInfos = GeneratedMappingInfos(iterations);
        }

        [GlobalSetup]
        private IEnumerable<Dictionary<string, string>> GeneratedMappingInfos(int iterations)
        {
            for (var i = 1; i <= iterations; i++)
            {
                yield return new Dictionary<string, string>
                {
                    { "CustomIdentifier", $"{i}" },
                    { "Label", "Foo" },
                    { "Value", $"{7d / i}" },
                    { "CreatedOn", _currentDateString },
                    { "IsDeleted", "false" },
                    { "Foo", "" },
                    { "Bar", "" },
                    { "Number", "1337" },
                    { "Decimal", null },
                    { "AnotherDate", null }
                };
            }
        }

        [Benchmark]
        public List<Model> GenerateModelsByHand()
        {
            var models = new List<Model>();
            foreach (var mappingInfo in _mappingInfos)
            {
                try
                {
                    var model = new Model();
                    foreach (var (target, value) in mappingInfo)
                        model.MapProperty(target, value);
                    models.Add(model);
                }
                catch (ApplicationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return models;
        }

        [Benchmark]
        public List<Model> GenerateModelsByReflection()
        {
            var models = new List<Model>();
            foreach (var mappingInfo in _mappingInfos)
            {
                try
                {
                    models.Add(Model.MapPropertiesByReflection(mappingInfo));
                }
                catch (ApplicationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return models;
        }

        [Benchmark]
        public List<Model> GenerateModelsByReflectionHelper()
        {
            var models = new List<Model>();
            foreach (var mappingInfo in _mappingInfos)
            {
                try
                {
                    models.Add(ReflectionHelper<Model>.MapPropertiesByReflectionHelper(mappingInfo));
                }
                catch (ApplicationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return models;
        }
    }
}
