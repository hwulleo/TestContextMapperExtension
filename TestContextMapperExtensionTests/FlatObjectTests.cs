using NUnit.Framework;
using System;
using TestContextMapperExtension;

namespace TestContextMapperExtensionTests
{
    public class FlatObjectTests
    {
        [Test]
        public void GivenFlatObject_AndValidPropertiesInRunsettings_MapPropertiesShouldSetValues()
        {
            var flatObject = new FlatObject();
            
            TestContext.CurrentContext.MapProperties(ref flatObject);
            
            Assert.AreEqual("First Prop Value", flatObject.FirstProperty);
            Assert.AreEqual("Second Prop Value", flatObject.SecondProperty);
            Assert.AreEqual(true, flatObject.BoolProperty);
            Assert.AreEqual(1, flatObject.IntProperty);
            Assert.AreEqual(2.0, flatObject.DoubleProperty);
            Assert.AreEqual(3.0M, flatObject.DecimalProperty);

            Assert.AreEqual("Nullable string value", flatObject.NullableStringProperty);
            Assert.AreEqual(true, flatObject.NullableBoolProperty);
            Assert.AreEqual(2, flatObject.NullableIntProperty);
            Assert.AreEqual(2.33, flatObject.NullableDoubleProperty);
            Assert.AreEqual(3.33333m, flatObject.NullableDecimalProperty);

        }

        [Test]
        public void EnumPropertyInFlatObject_AndValidPropertyInRunsettings_MapPropertiesShouldSetEnumValue()
        {
            var flatObject = new FlatObject();
            
            TestContext.CurrentContext.MapProperties(ref flatObject);

            Assert.AreEqual(EnumValues.FourthEnum, flatObject.RegularEnum);
        }

        [Test]
        public void NullableEnumPropertyInFlatObject_AndValidPropertyInRunsettings_MapPropertiesShouldSetNullableEnumValue()
        {
            var flatObject = new FlatObject();
            
            TestContext.CurrentContext.MapProperties(ref flatObject);

            Assert.AreEqual(EnumValues.SecondEnum, flatObject.NullableEnum);
        }
    }

    public class FlatObject
    {
        public string FirstProperty { get; set; }
        public string SecondProperty { get; set; }
        public bool BoolProperty { get; set; }
        public int IntProperty { get; set; }
        public double DoubleProperty { get; set; }
        public decimal DecimalProperty { get; set; }
        public string? NullableStringProperty { get; set; }
        public bool? NullableBoolProperty { get; set; }
        public int? NullableIntProperty { get; set; }
        public double? NullableDoubleProperty { get; set; }
        public decimal? NullableDecimalProperty { get; set; }
        public EnumValues RegularEnum { get; set; }
        public EnumValues? NullableEnum { get; set; }

    }

    public enum EnumValues
    {
        FirstEnum,
        SecondEnum,
        ThirdEnum,
        FourthEnum = 50
    }
}