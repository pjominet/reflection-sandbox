using System;

namespace ReflectionSandbox
{
    public enum PropertyType
    {
        Integer,
        Decimal,
        String,
        DateTime,
        Bool
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class MappableAttribute : Attribute
    {
        public PropertyType OfType { get; }
        public bool IsRequired { get; }

        public MappableAttribute(PropertyType ofType, bool isRequired = false)
        {
            OfType = ofType;
            IsRequired = isRequired;
        }
    }
}
