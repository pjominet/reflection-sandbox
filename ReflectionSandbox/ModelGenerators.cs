using System;
using System.Collections.Generic;

namespace ReflectionSandbox
{
    public static class ModelGenerators
    {

        public static List<Model> GenerateModelsByHand(IEnumerable<Dictionary<string, string>> mappingInfos)
        {
            var models = new List<Model>();
            foreach (var mappingInfo in mappingInfos)
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

        public static List<Model> GenerateModelsByReflectionWithAttributes(IEnumerable<Dictionary<string, string>> mappingInfos)
        {
            var models = new List<Model>();
            foreach (var mappingInfo in mappingInfos)
            {
                try
                {
                    models.Add(Model.MapByReflectionWithAttributes(mappingInfo));
                }
                catch (ApplicationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return models;
        }

        public static List<Model> GenerateModelsByReflectionWithoutAttributes(IEnumerable<Dictionary<string, string>> mappingInfos)
        {
            var models = new List<Model>();
            foreach (var mappingInfo in mappingInfos)
            {
                try
                {
                    models.Add(Model.MapByReflectionWithoutAttributes(mappingInfo));
                }
                catch (ApplicationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return models;
        }

        public static List<Model> GenerateModelsByReflectionWithDelegateWithoutAttributes(IEnumerable<Dictionary<string, string>> mappingInfos)
        {
            var models = new List<Model>();
            foreach (var mappingInfo in mappingInfos)
            {
                try
                {
                    models.Add(Model.MapByReflectionWithDelegateWithoutAttributes(mappingInfo));
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
