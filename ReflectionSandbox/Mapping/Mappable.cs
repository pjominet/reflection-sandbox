using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ReflectionSandbox.Mapping
{
    public interface IMappable
    {
        public void MapProperty(string target, string value);
    }

    public class Mappable<T> where T : class, new()
    {
        private static readonly IEnumerable<PropertyInfo> CachedProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        public static T MapWithAttributes(Dictionary<string, string> mappingInfo)
        {
            var t = new T();
            foreach (var (target, value) in mappingInfo)
            {
                var prop = CachedProperties.FirstOrDefault(p => p.Name == target);
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

        public static T MapWithoutAttributes(Dictionary<string, string> mappingInfo)
        {
            var t = new T();
            foreach (var (target, value) in mappingInfo)
            {
                var prop = CachedProperties.FirstOrDefault(p => p.Name == target);
                if (prop == null)
                    throw new ApplicationException($"{target} does not exist in {nameof(t)}");

                if (string.IsNullOrWhiteSpace(value))
                {
                    if (!Helpers.IsStringType(prop.PropertyType) && !Helpers.IsOfNullableType(prop.PropertyType))
                        throw new ApplicationException($"{target} is required, but no value was provided");
                    continue;
                }

                var typeCode = Helpers.IsOfNullableType(prop.PropertyType)
                    ? Type.GetTypeCode(Nullable.GetUnderlyingType(prop.PropertyType))
                    : Type.GetTypeCode(prop.PropertyType);
                switch (typeCode)
                {
                    case TypeCode.Int32:
                        if (int.TryParse(value, out var intResult))
                            prop.SetValue(t, intResult);
                        else throw new ApplicationException($"\"{value}\" is no int32");
                        break;
                    case TypeCode.Decimal:
                        if (decimal.TryParse(value, out var decimalResult))
                            prop.SetValue(t, decimalResult);
                        else throw new ApplicationException($"\"{value}\" is no decimal");
                        break;
                    case TypeCode.Boolean:
                        if (bool.TryParse(value, out var boolResult))
                            prop.SetValue(t, boolResult);
                        else throw new ApplicationException($"\"{value}\" is no boolean");
                        break;
                    case TypeCode.DateTime:
                        if (DateTime.TryParse(value, out var dateTimeResult))
                            prop.SetValue(t, dateTimeResult);
                        else throw new ApplicationException($"\"{value}\" is no DateTime");
                        break;
                    case TypeCode.String:
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
