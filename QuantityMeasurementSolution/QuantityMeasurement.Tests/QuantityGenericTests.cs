using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurement.Domain.Enums;
using QuantityMeasurement.Domain.ValueObjects;

namespace QuantityMeasurement.Tests
{
    [TestClass]
    public class QuantityGenericTests
    {
        private const double Epsilon = 1e-6;

        [TestMethod]
        public void LengthEquality_Works()
        {
            var a = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(12.0, LengthUnit.Inches);

            Assert.IsTrue(a.Equals(b));
        }

        [TestMethod]
        public void WeightEquality_Works()
        {
            var a = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);
            var b = new Quantity<WeightUnit>(1000.0, WeightUnit.Gram);

            Assert.IsTrue(a.Equals(b));
        }

        [TestMethod]
        public void LengthAddition_Works()
        {
            var a = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(12.0, LengthUnit.Inches);

            var result = a.Add(b, LengthUnit.Feet);

            Assert.AreEqual(2.0, result.Value, Epsilon);
        }

        [TestMethod]
        public void VolumeEquality_LitreToMillilitre()
        {
            var a = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var b = new Quantity<VolumeUnit>(1000.0, VolumeUnit.Millilitre);
        
            Assert.IsTrue(a.Equals(b));
        }
        
        [TestMethod]
        public void VolumeConversion_LitreToGallon()
        {
            var a = new Quantity<VolumeUnit>(3.78541, VolumeUnit.Litre);
        
            var result = a.ConvertTo(VolumeUnit.Gallon);
        
            Assert.AreEqual(1.0, result.Value, 1e-3);
        }
    }
}