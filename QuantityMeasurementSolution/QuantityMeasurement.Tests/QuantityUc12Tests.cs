using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurement.Domain.Enums;
using QuantityMeasurement.Domain.ValueObjects;
using System;

namespace QuantityMeasurement.Tests
{
    [TestClass]
    public class QuantityUc12Tests
    {
        private const double Eps = 1e-6;

        [TestMethod]
        public void Subtraction_SameUnit_ResultInFirstUnit()
        {
            var a = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(2.0, LengthUnit.Feet);

            var result = a.Subtract(b);

            Assert.AreEqual(8.00, result.Value, Eps);
            Assert.AreEqual(LengthUnit.Feet, result.Unit);
        }

        [TestMethod]
        public void Subtraction_CrossUnit_ImplicitTarget()
        {
            // 10 ft - 6 in = 9.5 ft
            var a = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(6.0, LengthUnit.Inches);

            var result = a.Subtract(b);

            Assert.AreEqual(9.50, result.Value, Eps);
            Assert.AreEqual(LengthUnit.Feet, result.Unit);
        }

        [TestMethod]
        public void Subtraction_ExplicitTargetUnit_Works()
        {
            // 1 ft - 6 in = 6 in
            var a = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(6.0, LengthUnit.Inches);

            var result = a.Subtract(b, LengthUnit.Inches);

            Assert.AreEqual(6.00, result.Value, Eps);
            Assert.AreEqual(LengthUnit.Inches, result.Unit);
        }

        [TestMethod]
        public void Subtraction_NonCommutative()
        {
            var a = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(6.0, LengthUnit.Inches);

            var r1 = a.Subtract(b);
            var r2 = b.Subtract(a);

            Assert.AreNotEqual(r1.Value, r2.Value, Eps);
        }

        [TestMethod]
        public void Subtraction_RoundsToTwoDecimals()
        {
            // Force a value that produces >2 decimals in some unit conversions
            var a = new Quantity<LengthUnit>(1.0, LengthUnit.Yards);   // 3 feet
            var b = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);    // 1 foot

            // 3 - 1 = 2 feet => in yards = 0.666666...
            var result = a.Subtract(b, LengthUnit.Yards);

            Assert.AreEqual(0.67, result.Value, Eps);
        }

        [TestMethod]
        public void Division_SameUnit_ReturnsScalar()
        {
            var a = new Quantity<WeightUnit>(10.0, WeightUnit.Kilogram);
            var b = new Quantity<WeightUnit>(5.0, WeightUnit.Kilogram);

            double ratio = a.Divide(b);

            Assert.AreEqual(2.0, ratio, Eps);
        }

        [TestMethod]
        public void Division_CrossUnit_ReturnsScalar()
        {
            // 1000 g / 1 kg = 1.0
            var a = new Quantity<WeightUnit>(1000.0, WeightUnit.Gram);
            var b = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);

            double ratio = a.Divide(b);

            Assert.AreEqual(1.0, ratio, Eps);
        }

        [TestMethod]
        public void Division_ByZero_Throws()
        {
            var a = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var b = new Quantity<VolumeUnit>(0.0, VolumeUnit.Millilitre);

            try
            {
                a.Divide(b);
                Assert.Fail("DivideByZeroException");
            }
            catch (DivideByZeroException)
            {
                // Pass
            }
        }

        [TestMethod]
        public void Division_WorksAcrossAllCategories()
        {
            var len = new Quantity<LengthUnit>(12.0, LengthUnit.Inches).Divide(new Quantity<LengthUnit>(1.0, LengthUnit.Feet));
            var wt = new Quantity<WeightUnit>(1000.0, WeightUnit.Gram).Divide(new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram));
            var vol = new Quantity<VolumeUnit>(1000.0, VolumeUnit.Millilitre).Divide(new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre));

            Assert.AreEqual(1.0, len, Eps);
            Assert.AreEqual(1.0, wt, Eps);
            Assert.AreEqual(1.0, vol, Eps);
        }
    }
}