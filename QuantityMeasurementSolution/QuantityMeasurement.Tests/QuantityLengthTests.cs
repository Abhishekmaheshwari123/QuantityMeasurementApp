using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurement.Domain.Enums;
using QuantityMeasurement.Domain.ValueObjects;

namespace QuantityMeasurement.Tests
{
    [TestClass]
    public class QuantityLengthTests
    {
        [TestMethod]
        public void testEquality_FeetToFeet_SameValue()
        {
            var a = new QuantityLength(1.0, LengthUnit.Feet);
            var b = new QuantityLength(1.0, LengthUnit.Feet);

            Assert.IsTrue(a.Equals(b));
        }

        [TestMethod]
        public void testEquality_InchToFeet_EquivalentValue()
        {
            var feet = new QuantityLength(1.0, LengthUnit.Feet);
            var inches = new QuantityLength(12.0, LengthUnit.Inches);

            Assert.IsTrue(feet.Equals(inches));
        }

        [TestMethod]
        public void testEquality_FeetToFeet_DifferentValue()
        {
            var a = new QuantityLength(1.0, LengthUnit.Feet);
            var b = new QuantityLength(2.0, LengthUnit.Feet);

            Assert.IsFalse(a.Equals(b));
        }

        [TestMethod]
        public void testEquality_NullComparison()
        {
            var a = new QuantityLength(1.0, LengthUnit.Feet);

            Assert.IsFalse(a.Equals(null));
        }

        [TestMethod]
        public void testEquality_SameReference()
        {
            var a = new QuantityLength(1.0, LengthUnit.Feet);

            Assert.IsTrue(a.Equals(a));
        }
    }
}