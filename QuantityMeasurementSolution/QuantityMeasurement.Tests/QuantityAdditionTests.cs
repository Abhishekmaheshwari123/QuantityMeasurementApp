using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurement.Domain.Enums;
using QuantityMeasurement.Domain.ValueObjects;

namespace QuantityMeasurement.Tests
{
    [TestClass]
    public class QuantityAdditionTests
    {
        private const double Epsilon = 1e-6;

        [TestMethod]
        public void testAddition_SameUnit_FeetPlusFeet()
        {
            var a = new QuantityLength(1.0, LengthUnit.Feet);
            var b = new QuantityLength(2.0, LengthUnit.Feet);

            var result = a.Add(b);

            Assert.AreEqual(3.0, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Feet, result.Unit);
        }

        [TestMethod]
        public void testAddition_CrossUnit_FeetPlusInches()
        {
            var feet = new QuantityLength(1.0, LengthUnit.Feet);
            var inches = new QuantityLength(12.0, LengthUnit.Inches);

            var result = feet.Add(inches);

            Assert.AreEqual(2.0, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Feet, result.Unit);
        }

        [TestMethod]
        public void testAddition_CrossUnit_InchPlusFeet()
        {
            var inches = new QuantityLength(12.0, LengthUnit.Inches);
            var feet = new QuantityLength(1.0, LengthUnit.Feet);

            var result = inches.Add(feet);

            Assert.AreEqual(24.0, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Inches, result.Unit);
        }

        [TestMethod]
        public void testAddition_WithZero()
        {
            var a = new QuantityLength(5.0, LengthUnit.Feet);
            var b = new QuantityLength(0.0, LengthUnit.Inches);

            var result = a.Add(b);

            Assert.AreEqual(5.0, result.Value, Epsilon);
        }

        [TestMethod]
        public void testAddition_NegativeValues()
        {
            var a = new QuantityLength(5.0, LengthUnit.Feet);
            var b = new QuantityLength(-2.0, LengthUnit.Feet);

            var result = a.Add(b);

            Assert.AreEqual(3.0, result.Value, Epsilon);
        }

        [TestMethod]
        public void testAddition_Commutativity()
        {
            var a = new QuantityLength(1.0, LengthUnit.Feet);
            var b = new QuantityLength(12.0, LengthUnit.Inches);

            var result1 = a.Add(b);
            var result2 = b.Add(a);

            Assert.IsTrue(result1.Equals(result2));
        }
    }
}