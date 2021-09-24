using System;

namespace ReflectionSandbox.Mapping
{
    public static class Helpers
    {
        public static bool IsOfNullableType(Type t)
        {
            return Nullable.GetUnderlyingType(t) != null;
        }

        public static bool IsStringType(Type t)
        {
            return Type.GetTypeCode(t) == TypeCode.String;
        }
    }
}
