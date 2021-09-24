using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ReflectionSandbox
{
    public interface IMappable
    {
        public void MapProperty(string target, string value);
    }

    public class PropertyHelper<T> where T : class
    {
        public string Name { get; private init; }
        public Type Type { get; private init; }
        public Func<object, object> GetValue { get; private init; }
        public Action<object, object> SetValue { get; private init; }

        private static readonly List<PropertyHelper<T>> Cache = typeof(T).GetProperties()
            .Select(property =>
                {
                    var getter = property.GetMethod;
                    var setter = property.SetMethod;

                    // BUG: Fails due to TypeInitializationException, don't know why yet
                    return new PropertyHelper<T>
                    {
                        Name = property.Name,
                        Type = property.PropertyType,
                        GetValue = getter is not null ? (Func<object, object>)Delegate.CreateDelegate(typeof(Func<object, object>), getter) : null,
                        SetValue = setter is not null ? (Action<object, object>)Delegate.CreateDelegate(typeof(Action<object, object>), setter) : null
                    };
                }
            ).ToList();

        public static List<PropertyHelper<T>> GetProperties() => Cache;
    }

    public class Mappable<T> where T : class, new()
    {
        private static readonly IEnumerable<PropertyInfo> CachedProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        private static readonly IEnumerable<PropertyHelper<T>> CachedPropertyHelpers = PropertyHelper<T>.GetProperties();

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
                    if (!IsStringType(prop.PropertyType) && !IsOfNullableType(prop.PropertyType))
                        throw new ApplicationException($"{target} is required, but no value was provided");
                    continue;
                }

                var typeCode = IsOfNullableType(prop.PropertyType)
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

        public static T MapWithDelegateWithoutAttributes(Dictionary<string, string> mappingInfo)
        {
            var t = new T();
            foreach (var (target, value) in mappingInfo)
            {
                var prop = CachedPropertyHelpers.FirstOrDefault(p => p.Name == target);
                if (prop == null)
                    throw new ApplicationException($"{target} does not exist in {nameof(t)}");

                if (string.IsNullOrWhiteSpace(value))
                {
                    if (!IsStringType(prop.Type) && !IsOfNullableType(prop.Type))
                        throw new ApplicationException($"{target} is required, but no value was provided");
                    continue;
                }

                var typeCode = IsOfNullableType(prop.Type)
                    ? Type.GetTypeCode(Nullable.GetUnderlyingType(prop.Type))
                    : Type.GetTypeCode(prop.Type);
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

        private static bool IsOfNullableType(Type t)
        {
            return Nullable.GetUnderlyingType(t) != null;
        }

        private static bool IsStringType(Type t)
        {
            return Type.GetTypeCode(t) == TypeCode.String;
        }
    }
}
