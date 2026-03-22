using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurement.Domain.Enums;
using QuantityMeasurement.Domain.ValueObjects;

namespace QuantityMeasurement.Tests
{
    [TestClass]
    public class QuantityConversionTests
    {
        private const double Epsilon = 1e-6;
    
        [TestMethod]
        public void testConversion_FeetToInches()
        {
            double result = QuantityLength.Convert(1.0, LengthUnit.Feet, LengthUnit.Inches);
            Assert.AreEqual(12.0, result, Epsilon);
        }
    
        [TestMethod]
        public void testConversion_YardsToInches()
        {
            double result = QuantityLength.Convert(1.0, LengthUnit.Yards, LengthUnit.Inches);
            Assert.AreEqual(36.0, result, Epsilon);
        }
    
        [TestMethod]
        public void testConversion_CentimetersToInches()
        {
            double result = QuantityLength.Convert(2.54, LengthUnit.Centimeters, LengthUnit.Inches);
            Assert.AreEqual(1.0, result, 1e-4);
        }
    
        [TestMethod]
        public void testConversion_ZeroValue()
        {
            double result = QuantityLength.Convert(0.0, LengthUnit.Feet, LengthUnit.Inches);
            Assert.AreEqual(0.0, result);
        }
    
        [TestMethod]
        public void testConversion_NegativeValue()
        {
            double result = QuantityLength.Convert(-1.0, LengthUnit.Feet, LengthUnit.Inches);
            Assert.AreEqual(-12.0, result);
        }
    }
}