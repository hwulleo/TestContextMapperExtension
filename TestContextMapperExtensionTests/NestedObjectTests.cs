using NUnit.Framework;
using NUnit.Framework.Internal;
using TestContextMapperExtension;

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
        public void Get_Some_Nullable_Info()
        {
            FlatObject flatObject = new FlatObject();
            var nullableBoolType = typeof(EnumValues?);
            var genericTypeDefinition = nullableBoolType.GetGenericTypeDefinition();
            var genericTypeArguments = genericTypeDefinition.GetGenericArguments()[0];
        }
    }

    public class NestedObjectBase
    {
        public string FirstProperty { get; set; }
        public string SecondProperty { get; set; }
        public NestedObjectLayerTwo Nested1Object1 { get; set; }
        public NestedObjectLayerTwo Nested1Object2 { get; set; }
    }

    public class NestedObjectLayerTwo
    {
        public string FirstProperty { get; set; }
        public string SecondProperty { get; set; }
        public NestedObjectLayerThree Nested2Object1 { get; set; }
        public NestedObjectLayerThree Nested2Object2 { get; set; }
    }

    public class NestedObjectLayerThree
    {
        public string FirstProperty { get; set; }
        public string SecondProperty { get; set; }
    }
}