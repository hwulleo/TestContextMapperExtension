using NUnit.Framework;
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
    }

    public class NestedObjectBase
    {
        public string FirstProperty { get; set; }
        public string SecondProperty { get; set; }
        public NestedObjectLayerTwo NestedObject { get; set; } = new NestedObjectLayerTwo();
    }

    public class NestedObjectLayerTwo
    {
        public string FirstProperty { get; set; }
        public string SecondProperty { get; set; }
        public NestedObjectLayerThree NestedObject { get; set; } = new NestedObjectLayerThree();
    }

    public class NestedObjectLayerThree
    {
        public string FirstProperty { get; set; }
        public string SecondProperty { get; set; }
    }
}