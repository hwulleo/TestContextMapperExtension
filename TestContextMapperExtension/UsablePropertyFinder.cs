using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestContextMapperExtension
{
    class UsablePropertyFinder
    {
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
            if(objectToMap is null)
            {
                objectToMap = Activator.CreateInstance(objectToMapType);
            }

            foreach (PropertyInfo propInfo in objectToMapType.GetProperties().Where(p => p.CanWrite))
            {
                Type propertyType = propInfo.PropertyType;
                if (IsUsableProperty(propertyType))
                {
                    UsableProperty usableProperty = new UsableProperty
                    {
                        Name = prefix + propInfo.Name,
                        ParentObject = objectToMap,
                        PropertyInfo = propInfo
                    };
                    usableProperties.Add(usableProperty);
                }
                else if(propInfo.PropertyType.GetProperties().Any(t => IsUsableProperty(t.PropertyType)))
                {
                    if (TestContext.Parameters.Names.Any(n => n.Contains(prefix + propInfo.Name)))
                    {
                        var propInstance = Activator.CreateInstance(propertyType);
                        propInfo.SetValue(objectToMap, propInstance);
                        GetUsableProperties(prefix, propInstance, propInfo.PropertyType);
                    }
                    
                }
            }
        }

        static bool IsUsableProperty(Type propertyType)
        {
            //basic "simple" properties that are primitives or strings are easy
            if(propertyType.IsPublic && IsSimpleProperty(propertyType))
            {
                return true;
            }

            //Nullable versions of primitives and strings are easy to convert
            if (propertyType.IsGenericType
                && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>)
                && propertyType.GetGenericArguments()
                .Any(t => IsSimpleProperty(t)))
            {
                return true;
            }
            
            //IEnumerable of simple types can be converted
            if (propertyType.IsGenericType
            && propertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>)
            && propertyType.GetGenericTypeDefinition().GetGenericArguments()
            .All(t => IsSimpleProperty(t)))
            {
                return true;
            }
                
            return false;
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
    }
}
