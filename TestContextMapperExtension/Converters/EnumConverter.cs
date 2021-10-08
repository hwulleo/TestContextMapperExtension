using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestContextMapperExtension.Converters
{
    public static class EnumConverter
    {
        public static object ConvertToEnum(Type type, object value)
        {
            var enumValue = Enum.Parse(type, value?.ToString());
            return enumValue;
        }

        public static object ConvertToNullableEnum(Type type, object value)
        {
            Type? nullableType = Nullable.GetUnderlyingType(type);
            //if ( /*not a nullable type*/nullableType is null)
              //  throw new ArgumentException($"Provided type {typeof(TEnum).Name} must be either an enum or a nullable enum");

            return Enum.Parse(nullableType, value?.ToString());
        }
    }
}
