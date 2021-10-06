using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TestContextMapperExtension
{
    public static class TestContextMapper
    {
        public static T MapProperties<T>(this TestContext testContext, ref T objectToMap) where T : class, new()
        {
            var usableProperties = UsablePropertyFinder.MakeUseablePropertyList(objectToMap);
            foreach (var usableProperty in usableProperties)
            {
                var testContextPropertyValue = TestContext.Parameters[usableProperty.Name];
                if (testContextPropertyValue != null)
                {
                    var convertedValue = TConverter.ChangeType(usableProperty.PropertyInfo.PropertyType, testContextPropertyValue);
                    usableProperty.PropertyInfo.SetValue(usableProperty.ParentObject, convertedValue);
                }
            }
            return objectToMap;
        }
    }
}
