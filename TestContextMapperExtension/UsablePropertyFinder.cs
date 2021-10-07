using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TestContextMapperExtension.Converters;
using static TestContextMapperExtension.UsableProperty;

namespace TestContextMapperExtension
{
    class UsablePropertyFinder
    {
        /// <summary>
        /// Make a list of all the properties and nested properties on the object that 
        /// match those in the .runsettings file can be mapped.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToMap"></param>
        /// <returns></returns>
        public static List<UsableProperty> MakeUseablePropertyList<T>(T objectToMap)
        {
            if(objectToMap is null)
            {
                objectToMap = (T)Activator.CreateInstance(typeof(T));
            }
            GetUsableProperties(string.Empty, objectToMap, typeof(T));
            return usableProperties;
        }

        static List<UsableProperty> usableProperties = new List<UsableProperty>();
        static void GetUsableProperties(string prefix, object objectToMap, Type objectToMapType)
        {
            if(objectToMapType is null)
            {
                TestContext.Progress.WriteLine($"Unknown type for property in class {prefix}");
                return;
            }

            if (!string.IsNullOrEmpty(prefix) && !prefix.EndsWith(".")) 
            {
                prefix += ".";
            }  
            prefix += objectToMapType.Name + ".";

            //some configuration objects cause problems if instantiated with no properties assigned
            //better to return early than instantiate one
            if(!TestContext.Parameters.Names.Any(n => n.Contains(prefix)))
            {
                return;
            }
            if(objectToMap is null && CanCreateInstance(objectToMapType))
            {
                objectToMap = Activator.CreateInstance(objectToMapType);
            }

            foreach (PropertyInfo propInfo in objectToMapType.GetProperties().Where(p => p.CanWrite))
            {
                if(propInfo.PropertyType.IsClass && propInfo.PropertyType.GetProperties().Any())
                {
                    if (TestContext.Parameters.Names.Any(n => n.Contains(prefix + propInfo.Name)))
                    {
                        object propInstance = null;
                        if (CanCreateInstance(propInfo.PropertyType))
                        {
                            propInstance = Activator.CreateInstance(propInfo.PropertyType);
                            propInfo.SetValue(objectToMap, propInstance);
                        }
                        GetUsableProperties(prefix, propInstance, propInfo.PropertyType);
                    }
                }

                var usablePropertyType = GetUsablePropertyType(propInfo.PropertyType);
                if (usablePropertyType != UsablePropertyType.UnusableProperty)
                {
                    UsableProperty usableProperty = new UsableProperty
                    {
                        Name = prefix + propInfo.Name,
                        ParentObject = objectToMap,
                        PropertyInfo = propInfo,
                        Conversion = GetConversionFunction(propInfo.PropertyType)
                    };
                    usableProperties.Add(usableProperty);
                }
            }
        }

        static UsablePropertyType GetUsablePropertyType(Type propertyType)
        {
            //only public properties matter for this use case
            if (!propertyType.IsPublic)
            {
                return UsablePropertyType.UnusableProperty;
            }

            //basic "simple" properties that are primitives or strings are easy
            if (IsSimpleProperty(propertyType))
            {
                return UsablePropertyType.Simple;
            }

            if (propertyType.IsEnum)
            {
                return UsablePropertyType.Enum;
            }

            //Nullable versions of primitives and strings are easy to convert
            if (propertyType.IsGenericType
                && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>)
                && propertyType.GetGenericArguments()
                .Any(t => IsSimpleProperty(t)))
            {
                return UsablePropertyType.NullableSimple;
            }

            //IEnumerable of simple types can be converted
            if (propertyType.IsGenericType
            && propertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>)
            && propertyType.GetGenericTypeDefinition().GetGenericArguments()
            .All(t => IsSimpleProperty(t)))
            {
                return UsablePropertyType.IEnumerable;
            }

            return UsablePropertyType.UnusableProperty;
        }

        /// <summary>
        /// Returns true if property is a primitive or a string (calling that a "simple" property)
        /// </summary>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        internal static bool IsSimpleProperty(Type propertyType)
        {
            if ((propertyType.IsPrimitive && propertyType.IsValueType) ||
                propertyType == typeof(string))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// check to see if type has a public parameterless (or only optional parameter) constructor
        /// </summary>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        internal static bool CanCreateInstance(Type propertyType)
        {
            return propertyType.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                .Any(x => x.GetParameters().All(x => x.IsOptional));
        }

        public static ConversionFunction GetConversionFunction(Type type)
        {
            if(type.IsEnum)
            {
                return (t, v) =>
                {
                    var enumValue = Enum.Parse(t, v?.ToString());
                    return enumValue;
                };
            }
            else
            {
                return TConverter.ChangeType;
            }
        }

         
    }
}
