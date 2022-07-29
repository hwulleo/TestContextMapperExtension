using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestContextMapperExtension
{
    public class UsableProperty
    {
        public string Name { get; set; }

        public object ParentObject { get; set; }

        public PropertyInfo PropertyInfo { get; set; }

        public UsablePropertyType UsablePropertyType { get; set; }

        public delegate object ConversionFunction(Type type, object value);

        public ConversionFunction Conversion { get; set; }
    }

    public enum UsablePropertyType
    {
        Simple,
        NullableSimple,
        Enum,
        NullableEnum,
        IList,
        IEnumerable,
        NullableIEnumerable,
        IDictionary,
        UnusableProperty
    }
}
