using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ReflectionSandbox
{
    public interface IMappable
    {
        public void MapProperty(string target, string value);
    }

    public abstract class Mappable<T> where T : class, new()
    {
        public static T MapPropertiesByReflection(Dictionary<string, string> mappingInfo)
        {
            var t = new T();
            var properties = t.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var (target, value) in mappingInfo)
            {
                var prop = properties.FirstOrDefault(p => p.Name == target);
                if (prop == null)
                    throw new ApplicationException($"{target} does not exist in {nameof(t)}");

                var attribute = (MappableAttribute)prop.GetCustomAttribute(typeof(MappableAttribute));
                if (attribute is null)
                    continue;

                if (string.IsNullOrWhiteSpace(value))
                {
                    if (attribute.IsRequired)
                        throw new ApplicationException($"{target} is required, but no value was provided");
                    continue;
                }

                switch (attribute.OfType)
                {
                    case PropertyType.Integer:
                        if (int.TryParse(value, out var intResult))
                            prop.SetValue(t, intResult);
                        else throw new ApplicationException($"\"{value}\" is no int32");
                        break;
                    case PropertyType.Decimal:
                        if (decimal.TryParse(value, out var decimalResult))
                            prop.SetValue(t, decimalResult);
                        else throw new ApplicationException($"\"{value}\" is no decimal");
                        break;
                    case PropertyType.Bool:
                        if (bool.TryParse(value, out var boolResult))
                            prop.SetValue(t, boolResult);
                        else throw new ApplicationException($"\"{value}\" is no boolean");
                        break;
                    case PropertyType.DateTime:
                        if (DateTime.TryParse(value, out var dateTimeResult))
                            prop.SetValue(t, dateTimeResult);
                        else throw new ApplicationException($"\"{value}\" is no DateTime");
                        break;
                    case PropertyType.String:
                        prop.SetValue(t, value);
                        break;
                    default:
                        throw new ApplicationException("Unwanted type");
                }
            }

            return t;
        }
    }
}
