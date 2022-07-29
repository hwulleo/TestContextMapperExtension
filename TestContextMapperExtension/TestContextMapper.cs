using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TestContextMapperExtension.Converters;
using static TestContextMapperExtension.UsableProperty;

namespace TestContextMapperExtension
{
    public static class TestContextMapper
    {
        public static Dictionary<UsablePropertyType, ConversionFunction> ConversionFunctionMap =
            new Dictionary<UsablePropertyType, ConversionFunction>
            { 
                { UsablePropertyType.Enum, EnumConverter.ConvertToEnum },
                { UsablePropertyType.Simple, TConverter.ChangeType },
                { UsablePropertyType.NullableSimple, TConverter.ChangeType },
                { UsablePropertyType.NullableEnum, EnumConverter.ConvertToNullableEnum },
                { UsablePropertyType.IList, IListConverter.ConvertToIList },
                { UsablePropertyType.IEnumerable, IEnumerableConverter.ConvertToIEnumerable },
                { UsablePropertyType.IDictionary, IDictionaryConverter.ConvertToIDictionary }
            };
        
        public static T MapProperties<T>(this TestContext testContext, ref T objectToMap) where T : class, new()
        {
            var usableProperties = UsablePropertyFinder.MakeUseablePropertyList(objectToMap);
            foreach (var usableProperty in usableProperties)
            {
                var testContextPropertyValue = TestContext.Parameters[usableProperty.Name];
                if (testContextPropertyValue != null)
                {
                    var convertedValue = usableProperty.Conversion
                        .Invoke(usableProperty.PropertyInfo.PropertyType, testContextPropertyValue);
                    usableProperty.PropertyInfo.SetValue(usableProperty.ParentObject, convertedValue);
                }
            }
            return objectToMap;
        }
    }
}
