using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurement.Domain.Enums;
using QuantityMeasurement.Domain.ValueObjects;

namespace QuantityMeasurement.Tests
{
    [TestClass]
    public class QuantityWeightTests
    {
        private const double Epsilon = 1e-6;

        // ---------------- Equality ----------------

        [TestMethod]
        public void Equality_KilogramToGram_EquivalentValue_ReturnsTrue()
        {
            var kg = new QuantityWeight(1.0, WeightUnit.Kilogram);
            var g = new QuantityWeight(1000.0, WeightUnit.Gram);

            Assert.IsTrue(kg.Equals(g));
        }

        [TestMethod]
        public void Equality_PoundToKilogram_EquivalentValue_ReturnsTrue()
        {
            var lb = new QuantityWeight(1.0, WeightUnit.Pound);
            var kg = new QuantityWeight(0.453592, WeightUnit.Kilogram);

            Assert.IsTrue(lb.Equals(kg));
        }

        [TestMethod]
        public void Equality_DifferentValues_ReturnsFalse()
        {
            var a = new QuantityWeight(1.0, WeightUnit.Kilogram);
            var b = new QuantityWeight(2.0, WeightUnit.Kilogram);

            Assert.IsFalse(a.Equals(b));
        }

        [TestMethod]
        public void Equality_NullComparison_ReturnsFalse()
        {
            var a = new QuantityWeight(1.0, WeightUnit.Kilogram);

            Assert.IsFalse(a.Equals(null));
        }

        // ---------------- Conversion ----------------

        [TestMethod]
        public void Conversion_KilogramToGram_ReturnsExpected()
        {
            var kg = new QuantityWeight(1.0, WeightUnit.Kilogram);
            var g = kg.ConvertTo(WeightUnit.Gram);

            Assert.AreEqual(1000.0, g.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Gram, g.Unit);
        }

        [TestMethod]
        public void Conversion_GramToKilogram_ReturnsExpected()
        {
            var g = new QuantityWeight(2500.0, WeightUnit.Gram);
            var kg = g.ConvertTo(WeightUnit.Kilogram);

            Assert.AreEqual(2.5, kg.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Kilogram, kg.Unit);
        }

        [TestMethod]
        public void Conversion_RoundTrip_PreservesValueWithinTolerance()
        {
            var original = new QuantityWeight(2.75, WeightUnit.Kilogram);

            var grams = original.ConvertTo(WeightUnit.Gram);
            var backToKg = grams.ConvertTo(WeightUnit.Kilogram);

            Assert.IsTrue(original.Equals(backToKg));
        }

        // ---------------- Addition (UC6-style) ----------------

        [TestMethod]
        public void Addition_SameUnit_KilogramPlusKilogram_ReturnsKilogram()
        {
            var a = new QuantityWeight(1.0, WeightUnit.Kilogram);
            var b = new QuantityWeight(2.0, WeightUnit.Kilogram);

            var result = a.Add(b);

            Assert.AreEqual(3.0, result.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Kilogram, result.Unit);
        }

        [TestMethod]
        public void Addition_CrossUnit_KilogramPlusGram_ResultInFirstUnit()
        {
            var kg = new QuantityWeight(1.0, WeightUnit.Kilogram);
            var g = new QuantityWeight(500.0, WeightUnit.Gram);

            var result = kg.Add(g);

            Assert.AreEqual(1.5, result.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Kilogram, result.Unit);
        }

        [TestMethod]
        public void Addition_CrossUnit_GramPlusKilogram_ResultInFirstUnit()
        {
            var g = new QuantityWeight(500.0, WeightUnit.Gram);
            var kg = new QuantityWeight(1.0, WeightUnit.Kilogram);

            var result = g.Add(kg);

            Assert.AreEqual(1500.0, result.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Gram, result.Unit);
        }

        // ---------------- Guard checks ----------------

        [TestMethod]
        public void Constructor_NonFiniteValue_Throws()
        {
            try
            {
                _ = new QuantityWeight(double.NaN, WeightUnit.Kilogram);
                Assert.Fail("Expected exception for non-finite value.");
            }
            catch (ArgumentException)
            {
                // pass
            }
        }

        [TestMethod]
        public void ConvertTo_InvalidTargetUnit_Throws()
        {
            var a = new QuantityWeight(1.0, WeightUnit.Kilogram);

            try
            {
                _ = a.ConvertTo((WeightUnit)999);
                Assert.Fail("Expected exception for invalid target unit.");
            }
            catch (ArgumentException)
            {
                // pass
            }
        }
    }
}