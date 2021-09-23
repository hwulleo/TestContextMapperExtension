using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TestContextMapperExtension
{
    public static class TestContextMapper
    {
        public static T MapProperty<T>(this TestContext testContext, ref T objectToMap, string propertyName)
        {
            if (objectToMap is null)
            {
                TestContext.Progress.WriteLine($"objectToMap provided to {nameof(MapProperty)} " +
                    $"is null. Exiting with no map.");
                return objectToMap;
            }
            var testContextPropertyValue = TestContext.Parameters[propertyName];

            var objectProperties = objectToMap.GetType().GetProperties();
            foreach (var property in objectProperties)
            {
                var propName = $"{property.DeclaringType.Name}.{property.Name}";
                if (propName.Equals(propertyName, StringComparison.InvariantCulture))
                {
                    property.SetValue(objectToMap, testContextPropertyValue);
                }
            }

            return objectToMap;
        }

        public static T MapProperties<T>(this TestContext testContext, ref T objectToMap)
        {
            if (objectToMap is null)
            {
                objectToMap = (T)Activator.CreateInstance(typeof(T));
            }

            MapPropertyInfosToPropertyName(string.Empty, objectToMap.GetType());
            foreach (var property in propertyMap)
            {
                var testContextPropertyValue = TestContext.Parameters[property.Key];
                if (testContextPropertyValue != null)
                {
                    property.Value.SetValue(objectToMap, testContextPropertyValue);
                }
            }
            return objectToMap;
        }

        static Dictionary<string, PropertyInfo> propertyMap = new Dictionary<string, PropertyInfo>();

        static void MapPropertyInfosToPropertyName(string prefix, Type t)
        {
            if (!string.IsNullOrEmpty(prefix) && !prefix.EndsWith(".")) prefix += ".";
            prefix += t.Name + ".";

            foreach (PropertyInfo propInfo in t.GetProperties().Where(p => p.CanWrite))
            {
                Type propertyType = propInfo.PropertyType;
                if (propertyType.IsPrimitive || propertyType == typeof(string))
                {
                    propertyMap.Add(prefix + propInfo.Name, propInfo);
                }
                else
                {
                    MapPropertyInfosToPropertyName(prefix, propertyType);
                }
            }
        }
    }
}
