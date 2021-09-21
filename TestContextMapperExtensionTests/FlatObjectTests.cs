using NUnit.Framework;
using TestContextMapperExtension;

namespace TestContextMapperExtensionTests
{
    public class FlatObjectTests
    {
        class FlatObject
        {
            public string FirstProperty { get; set; }
            public string SecondProperty { get; set; }
        }

        [Test]
        public void GivenFlatObject_AndValidPropertyInRunsettings_MapPropertyShouldSetValue()
        {
            var flatObject = new FlatObject();
            TestContext.CurrentContext.MapProperty(ref flatObject, "FlatObject.FirstProperty");
            Assert.AreEqual("First Prop Value", flatObject.FirstProperty);
        }

        [Test]
        public void GivenFlatObject_AndValidPropertiesInRunsettings_MapPropertiesShouldSetValues()
        {
            var flatObject = new FlatObject();
            TestContext.CurrentContext.MapProperties(ref flatObject);
            Assert.AreEqual("First Prop Value", flatObject.FirstProperty);
            Assert.AreEqual("Second Prop Value", flatObject.SecondProperty);
        }
    }
}