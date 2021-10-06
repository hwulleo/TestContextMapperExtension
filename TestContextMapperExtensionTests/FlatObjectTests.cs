using NUnit.Framework;
using System;
using TestContextMapperExtension;

namespace TestContextMapperExtensionTests
{
    public class FlatObject
    {
        public string FirstProperty { get; set; }
        public string SecondProperty { get; set; }
        public int IntProperty { get; set; }
        public double DoubleProperty { get; set; }
        public bool BoolProperty { get; set; }
        public bool? NullableBoolProperty { get; set; }
    }

    public class FlatObjectTests
    {
        [Test]
        public void GivenFlatObject_AndValidPropertiesInRunsettings_MapPropertiesShouldSetValues()
        {
            var flatObject = new FlatObject();
            TestContext.CurrentContext.MapProperties(ref flatObject);
            Assert.AreEqual("First Prop Value", flatObject.FirstProperty);
            Assert.AreEqual("Second Prop Value", flatObject.SecondProperty);
            Assert.AreEqual(1, flatObject.IntProperty);
            Assert.AreEqual(2.0, flatObject.DoubleProperty);
            Assert.AreEqual(true, flatObject.BoolProperty);
            Assert.AreEqual(true, flatObject.NullableBoolProperty);
        }
    }
}