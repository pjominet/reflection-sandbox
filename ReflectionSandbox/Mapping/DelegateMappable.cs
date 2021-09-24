using System;
using System.Collections.Generic;
using System.Linq;

namespace ReflectionSandbox.Mapping
{
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

    public class DelegateMappable<T> where T : class, new()
    {
        private static readonly IEnumerable<PropertyHelper<T>> CachedPropertyHelpers = PropertyHelper<T>.GetProperties();

        public static T MapWithDelegate(Dictionary<string, string> mappingInfo)
        {
            var t = new T();
            foreach (var (target, value) in mappingInfo)
            {
                var prop = CachedPropertyHelpers.FirstOrDefault(p => p.Name == target);
                if (prop == null)
                    throw new ApplicationException($"{target} does not exist in {nameof(t)}");

                if (string.IsNullOrWhiteSpace(value))
                {
                    if (!Helpers.IsStringType(prop.Type) && !Helpers.IsOfNullableType(prop.Type))
                        throw new ApplicationException($"{target} is required, but no value was provided");
                    continue;
                }

                var typeCode = Helpers.IsOfNullableType(prop.Type)
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
    }
}
