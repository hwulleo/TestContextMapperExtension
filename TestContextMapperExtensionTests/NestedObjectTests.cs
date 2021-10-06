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
            Assert.AreEqual("Nested First Prop Value", nestedObject.NestedObject.FirstProperty);
            Assert.AreEqual("Nested Second Prop Value", nestedObject.NestedObject.SecondProperty);
            Assert.AreEqual("Double Nested First Prop Value", nestedObject.NestedObject.NestedObject.FirstProperty);
            Assert.AreEqual("Double Nested Second Prop Value", nestedObject.NestedObject.NestedObject.SecondProperty);
        }

        [Test]
        public void Make_A_My_Own_TestContext()
        {
            TestExecutionContext testExecutionContext = new TestExecutionContext();
            
            TestContext testContext = new TestContext(testExecutionContext);
        }

        [Test]
        public void Get_Some_Nullable_Info()
        {
            FlatObject flatObject = new FlatObject();
            flatObject.NullableBoolProperty = true;
            var nullableBoolType = typeof(bool?);
            var genericTypeDefinition = nullableBoolType.GetGenericTypeDefinition();
            var genericTypeArguments = genericTypeDefinition.GetGenericArguments();
        }
    }

    public class NestedObjectBase
    {
        public string FirstProperty { get; set; }
        public string SecondProperty { get; set; }
        public NestedObjectLayerTwo NestedObject { get; set; }// = new NestedObjectLayerTwo();
    }

    public class NestedObjectLayerTwo
    {
        public string FirstProperty { get; set; }
        public string SecondProperty { get; set; }
        public NestedObjectLayerThree NestedObject { get; set; }// = new NestedObjectLayerThree();
    }

    public class NestedObjectLayerThree
    {
        public string FirstProperty { get; set; }
        public string SecondProperty { get; set; }
    }
}