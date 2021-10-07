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
    }
}
