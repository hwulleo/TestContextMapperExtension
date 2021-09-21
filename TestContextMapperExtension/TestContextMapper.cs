using NUnit.Framework;
using System;
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
                var propName = MakePropertyNameWithDotNotation(property);
                if(propName.Equals(propertyName, StringComparison.InvariantCulture))
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
            
            var objectProperties = objectToMap.GetType().GetProperties();
            foreach (var property in objectProperties)
            {
                var propName = MakePropertyNameWithDotNotation(property);
                var testContextPropertyValue = TestContext.Parameters[propName];
                if (testContextPropertyValue != null)
                {
                    property.SetValue(objectToMap, testContextPropertyValue);
                }
            }
            return objectToMap;
        }

        static string MakePropertyNameWithDotNotation(PropertyInfo propertyInfo)
        {
            return $"{propertyInfo.DeclaringType.Name}.{propertyInfo.Name}";
        }
    }
}
