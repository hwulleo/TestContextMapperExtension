using FsCheck;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using TestContextMapperExtension;
using TestContextMapperExtensionTests.TestObjects;

namespace TestContextMapperExtensionTests
{
    public class NestedObjectTests
    {
        [Test]
        public void GivenNestedObject_AndValidPropertiesInRunsettings_MapPropertiesShouldSetValues()
        {
            var nestedObject = new NestedObjectBase();
            TestContext.CurrentContext.MapProperties(ref nestedObject);
            Assert.AreEqual("First Prop Value", nestedObject.FirstProperty);
            Assert.AreEqual("Second Prop Value", nestedObject.SecondProperty);
            Assert.AreEqual("Nested1Object1 First Prop Value", nestedObject.Nested1Object1.FirstProperty);
            Assert.AreEqual("Nested1Object2 Second Prop Value", nestedObject.Nested1Object2.SecondProperty);
            Assert.AreEqual("Double Nested First Prop Value", nestedObject.Nested1Object1.Nested2Object1.FirstProperty);
            Assert.AreEqual("Double Nested Second Prop Value", nestedObject.Nested1Object1.Nested2Object1.SecondProperty);
        }

        [Test]
        public void GivenNestedObject_WithListOfStringValue_MapPropertiesShouldSetIEnumerable()
        {
            var nestedObject = new NestedObjectBase();
            TestContext.CurrentContext.MapProperties(ref nestedObject);
            Assert.AreEqual("firstString", nestedObject.ListOfStringProperty[0]);
            Assert.AreEqual("secondString", nestedObject.ListOfStringProperty[1]);
            Assert.AreEqual("firstString", nestedObject.Nested1Object1.ListOfStringProperty[0]);
            Assert.AreEqual("secondString", nestedObject.Nested1Object1.ListOfStringProperty[1]);
        }

        [Test]
        public void GivenNestedObject_WithIEnumerableOfStringValue_MapPropertiesShouldSetIEnumerable()
        {
            var nestedObject = new NestedObjectBase();
            TestContext.CurrentContext.MapProperties(ref nestedObject);
            Assert.AreEqual("firstString", nestedObject.IEnumerableOfStringProperty.First());
            Assert.AreEqual("secondString", nestedObject.IEnumerableOfStringProperty.Skip(1).First());
            Assert.AreEqual("firstString", nestedObject.Nested1Object1.IEnumerableOfStringProperty.First());
            Assert.AreEqual("secondString", nestedObject.Nested1Object1.IEnumerableOfStringProperty.Skip(1).First());
        }

        [Test]
        public void GivenNestedObject_WithNullableIEnumerableOfStringValue_MapPropertiesShouldSetNullableIEnumerable()
        {
            var nestedObject = new NestedObjectBase();
            TestContext.CurrentContext.MapProperties(ref nestedObject);
            Assert.AreEqual("firstString", nestedObject.NullableIEnumerableOfStringProperty.First());
            Assert.AreEqual("secondString", nestedObject.NullableIEnumerableOfStringProperty.Skip(1).First());
        }

        [Test]
        public void GivenNestedObject_WithIDictionaryValue_MapPropertiesShouldSetIDictionary()
        {
            
            var nestedObject = new NestedObjectBase();
            TestContext.CurrentContext.MapProperties(ref nestedObject);
            Assert.AreEqual("dictValue", nestedObject.DictionaryOfStringStringProperty["dictKey"]);
            Assert.AreEqual("dictValue2", nestedObject.DictionaryOfStringStringProperty["dictKey2"]);
            Assert.AreEqual("nestedDictValue", nestedObject.Nested1Object1.DictionaryOfStringStringProperty["nestedDictKey"]);
            Assert.AreEqual("nestedDictValue2", nestedObject.Nested1Object1.DictionaryOfStringStringProperty["nestedDictKey2"]);
        }

        [Test]
        public void Get_Some_Nullable_Info()
        {
            FlatObject flatObject = new FlatObject();
            var nullableBoolType = typeof(EnumValues?);
            var genericTypeDefinition = nullableBoolType.GetGenericTypeDefinition();
            var genericTypeArguments = genericTypeDefinition.GetGenericArguments()[0];
        }

        [Test]
        public void Get_Some_IEnumerable_Info()
        {
            NestedObjectBase nestedObject = new NestedObjectBase();
            nestedObject.ListOfStringProperty = new List<string>();
            var propertyType = nestedObject.ListOfStringProperty.GetType();
            var isGeneric = propertyType.IsGenericType;
            var genericTypeDefinition = propertyType.GetGenericTypeDefinition();
            var interfaces = genericTypeDefinition.GetInterfaces();
            var maybeIEnumerable = genericTypeDefinition.GetInterfaces().Any(i => i.Name.Contains("IEnumerable"));
            var isIEnumerable = genericTypeDefinition == typeof(IEnumerable<>);
            var hasGenericArguments = propertyType.GetGenericArguments();
            var areSimple = hasGenericArguments.All(t => IsSimpleProperty(t));
        }

        [Test]
        public void Get_Some_Nullable_IEnumerable_Info()
        {
            NestedObjectBase nestedObject = new NestedObjectBase();
            var propertyType = nestedObject.GetType().GetProperty("NullableIEnumerableOfStringProperty").PropertyType;
            var isGeneric = propertyType.IsGenericType;

            var genericTypeDefinition = propertyType.GetGenericTypeDefinition();
            var interfaces = genericTypeDefinition.GetInterfaces();
            var genericArguments = propertyType.GetGenericArguments();
            var nestedGenericArguments = genericArguments.First().GetGenericArguments();
            var areSimple = nestedGenericArguments.All(t => IsSimpleProperty(t));
        }

        [Test]
        public void Get_Some_IDictionary_Info()
        {
            NestedObjectBase nestedObject = new NestedObjectBase();
            nestedObject.DictionaryOfStringStringProperty = new();
            var propertyType = nestedObject.DictionaryOfStringStringProperty.GetType();
            var isGeneric = propertyType.IsGenericType;
            var genericTypeDefinition = propertyType.GetGenericTypeDefinition();
            var interfaces = genericTypeDefinition.GetInterfaces();
            var maybeIDict = genericTypeDefinition.GetInterfaces().Any(i => i.Name.Contains("IDictionary"));
            var hasGenericArguments = propertyType.GetGenericArguments();
            var areSimple = hasGenericArguments.All(t => IsSimpleProperty(t));
        }

        [Test]
        public void FsCheck_Example()
        {
            var nestedObjectGen = Arb.Generate<NestedObjectBase>();
            var nestedObjectSamples = nestedObjectGen.Sample(1000, 2);
            var nestedObjectJsonString = JsonConvert.SerializeObject(nestedObjectSamples, Formatting.Indented);
            TestContext.WriteLine(nestedObjectJsonString);

        }

        internal static bool IsSimpleProperty(Type propertyType)
        {
            if ((propertyType.IsPrimitive && propertyType.IsValueType)
                || propertyType == typeof(string)
                || propertyType == typeof(decimal))
            {
                return true;
            }

            return false;
        }
    }

    
}