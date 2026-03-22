using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurement.Domain.Enums;
using QuantityMeasurement.Domain.ValueObjects;

namespace QuantityMeasurement.Tests
{
    [TestClass]
    public class QuantityAdvancedOperationsTests
    {
        private const double Epsilon = 1e-6;

        // =========================
        // UC12 - SUBTRACTION TESTS
        // =========================

        [TestMethod]
        public void UC12_Subtraction_SameUnit_FeetMinusFeet_Works()
        {
            var a = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(5.0, LengthUnit.Feet);

            var result = a.Subtract(b);

            Assert.AreEqual(5.00, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Feet, result.Unit);
        }

        [TestMethod]
        public void UC12_Subtraction_CrossUnit_FeetMinusInches_Works()
        {
            var a = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(6.0, LengthUnit.Inches);

            var result = a.Subtract(b);

            Assert.AreEqual(9.50, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Feet, result.Unit);
        }

        [TestMethod]
        public void UC12_Subtraction_ExplicitTargetUnit_Inches_Works()
        {
            var a = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(6.0, LengthUnit.Inches);

            var result = a.Subtract(b, LengthUnit.Inches);

            Assert.AreEqual(114.00, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Inches, result.Unit);
        }

        [TestMethod]
        public void UC12_Subtraction_ResultingInNegative_Works()
        {
            var a = new Quantity<WeightUnit>(2.0, WeightUnit.Kilogram);
            var b = new Quantity<WeightUnit>(5.0, WeightUnit.Kilogram);

            var result = a.Subtract(b);

            Assert.AreEqual(-3.00, result.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Kilogram, result.Unit);
        }

        [TestMethod]
        public void UC12_Subtraction_ResultingInZero_Works()
        {
            var a = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var b = new Quantity<VolumeUnit>(1000.0, VolumeUnit.Millilitre);

            var result = a.Subtract(b);

            Assert.AreEqual(0.00, result.Value, Epsilon);
            Assert.AreEqual(VolumeUnit.Litre, result.Unit);
        }

        [TestMethod]
        public void UC12_Subtraction_NonCommutative_Works()
        {
            var a = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(5.0, LengthUnit.Feet);

            var first = a.Subtract(b);
            var second = b.Subtract(a);

            Assert.AreEqual(5.00, first.Value, Epsilon);
            Assert.AreEqual(-5.00, second.Value, Epsilon);
        }

        [TestMethod]
        public void UC12_Subtraction_WithZeroOperand_Works()
        {
            var a = new Quantity<LengthUnit>(5.0, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(0.0, LengthUnit.Inches);

            var result = a.Subtract(b);

            Assert.AreEqual(5.00, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Feet, result.Unit);
        }

        [TestMethod]
        public void UC12_Subtraction_WithNegativeValues_Works()
        {
            var a = new Quantity<LengthUnit>(5.0, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(-2.0, LengthUnit.Feet);

            var result = a.Subtract(b);

            Assert.AreEqual(7.00, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Feet, result.Unit);
        }

        [TestMethod]
        public void UC12_Subtraction_Immutability_Preserved()
        {
            var a = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(2.0, LengthUnit.Feet);

            _ = a.Subtract(b);

            Assert.AreEqual(10.0, a.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Feet, a.Unit);
            Assert.AreEqual(2.0, b.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Feet, b.Unit);
        }

        // ======================
        // UC12 - DIVISION TESTS
        // ======================

        [TestMethod]
        public void UC12_Division_SameUnit_FeetByFeet_Works()
        {
            var a = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(2.0, LengthUnit.Feet);

            double result = a.Divide(b);

            Assert.AreEqual(5.0, result, Epsilon);
        }

        [TestMethod]
        public void UC12_Division_CrossUnit_InchesByFeet_Works()
        {
            var a = new Quantity<LengthUnit>(24.0, LengthUnit.Inches);
            var b = new Quantity<LengthUnit>(2.0, LengthUnit.Feet);

            double result = a.Divide(b);

            Assert.AreEqual(1.0, result, Epsilon);
        }

        [TestMethod]
        public void UC12_Division_CrossUnit_GramByKilogram_Works()
        {
            var a = new Quantity<WeightUnit>(2000.0, WeightUnit.Gram);
            var b = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);

            double result = a.Divide(b);

            Assert.AreEqual(2.0, result, Epsilon);
        }

        [TestMethod]
        public void UC12_Division_RatioLessThanOne_Works()
        {
            var a = new Quantity<VolumeUnit>(5.0, VolumeUnit.Litre);
            var b = new Quantity<VolumeUnit>(10.0, VolumeUnit.Litre);

            double result = a.Divide(b);

            Assert.AreEqual(0.5, result, Epsilon);
        }

        [TestMethod]
        public void UC12_Division_NonCommutative_Works()
        {
            var a = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(5.0, LengthUnit.Feet);

            double first = a.Divide(b);
            double second = b.Divide(a);

            Assert.AreEqual(2.0, first, Epsilon);
            Assert.AreEqual(0.5, second, Epsilon);
        }

        [TestMethod]
        public void UC12_Division_ByZero_Throws()
        {
            var a = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(0.0, LengthUnit.Feet);

            try
            {
                a.Divide(b);
                Assert.Fail("Expected DivideByZeroException was not thrown.");
            }
            catch (DivideByZeroException)
            {
            }
        }

        [TestMethod]
        public void UC12_Division_Immutability_Preserved()
        {
            var a = new Quantity<WeightUnit>(10.0, WeightUnit.Kilogram);
            var b = new Quantity<WeightUnit>(5.0, WeightUnit.Kilogram);

            _ = a.Divide(b);

            Assert.AreEqual(10.0, a.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Kilogram, a.Unit);
            Assert.AreEqual(5.0, b.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Kilogram, b.Unit);
        }

        // ==================================
        // UC13 - CENTRALIZED LOGIC VALIDATION
        // ==================================

        [TestMethod]
        public void UC13_Add_Subtract_Divide_Still_Work_After_Refactor()
        {
            var a = new Quantity<LengthUnit>(12.0, LengthUnit.Inches);
            var b = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);

            var added = a.Add(b);
            var subtracted = a.Subtract(b);
            var divided = a.Divide(b);

            Assert.AreEqual(24.00, added.Value, Epsilon);
            Assert.AreEqual(0.00, subtracted.Value, Epsilon);
            Assert.AreEqual(1.0, divided, Epsilon);
        }

        [TestMethod]
        public void UC13_BackwardCompatibility_UC12Behavior_Preserved()
        {
            var a = new Quantity<VolumeUnit>(5.0, VolumeUnit.Litre);
            var b = new Quantity<VolumeUnit>(500.0, VolumeUnit.Millilitre);

            var subtraction = a.Subtract(b);
            var addition = a.Add(b);
            var division = a.Divide(new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre));

            Assert.AreEqual(4.50, subtraction.Value, Epsilon);
            Assert.AreEqual(5.50, addition.Value, Epsilon);
            Assert.AreEqual(5.0, division, Epsilon);
        }

        // ====================================
        // UC14 - TEMPERATURE SUPPORT TESTS
        // ====================================

        [TestMethod]
        public void UC14_Temperature_Equality_CelsiusToFahrenheit_Works()
        {
            var celsius = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.Celsius);
            var fahrenheit = new Quantity<TemperatureUnit>(32.0, TemperatureUnit.Fahrenheit);

            Assert.IsTrue(celsius.Equals(fahrenheit));
        }

        [TestMethod]
        public void UC14_Temperature_Equality_CelsiusToKelvin_Works()
        {
            var celsius = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.Celsius);
            var kelvin = new Quantity<TemperatureUnit>(373.15, TemperatureUnit.Kelvin);

            Assert.IsTrue(celsius.Equals(kelvin));
        }

        [TestMethod]
        public void UC14_Temperature_Conversion_CelsiusToFahrenheit_Works()
        {
            var celsius = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.Celsius);

            var result = celsius.ConvertTo(TemperatureUnit.Fahrenheit);

            Assert.AreEqual(212.00, result.Value, Epsilon);
            Assert.AreEqual(TemperatureUnit.Fahrenheit, result.Unit);
        }

        [TestMethod]
        public void UC14_Temperature_Conversion_FahrenheitToKelvin_Works()
        {
            var fahrenheit = new Quantity<TemperatureUnit>(32.0, TemperatureUnit.Fahrenheit);

            var result = fahrenheit.ConvertTo(TemperatureUnit.Kelvin);

            Assert.AreEqual(273.15, result.Value, Epsilon);
            Assert.AreEqual(TemperatureUnit.Kelvin, result.Unit);
        }

        [TestMethod]
        public void UC14_Temperature_Addition_IsRejected()
        {
            var a = new Quantity<TemperatureUnit>(10.0, TemperatureUnit.Celsius);
            var b = new Quantity<TemperatureUnit>(20.0, TemperatureUnit.Celsius);

            try
            {
                a.Add(b);
                Assert.Fail("Expected NotSupportedException was not thrown.");
            }
            catch (NotSupportedException)
            {
            }
        }

        [TestMethod]
        public void UC14_Temperature_Subtraction_IsRejected()
        {
            var a = new Quantity<TemperatureUnit>(30.0, TemperatureUnit.Celsius);
            var b = new Quantity<TemperatureUnit>(10.0, TemperatureUnit.Celsius);

            try
            {
                a.Subtract(b);
                Assert.Fail("Expected NotSupportedException was not thrown.");
            }
            catch (NotSupportedException)
            {
            }
        }

        [TestMethod]
        public void UC14_Temperature_Division_IsRejected()
        {
            var a = new Quantity<TemperatureUnit>(30.0, TemperatureUnit.Celsius);
            var b = new Quantity<TemperatureUnit>(10.0, TemperatureUnit.Celsius);

            try
            {
                a.Divide(b);
                Assert.Fail("Expected NotSupportedException was not thrown.");
            }
            catch (NotSupportedException)
            {
            }
        }

        [TestMethod]
        public void UC14_Temperature_Immutability_Preserved_After_Convert()
        {
            var original = new Quantity<TemperatureUnit>(25.0, TemperatureUnit.Celsius);

            var converted = original.ConvertTo(TemperatureUnit.Fahrenheit);

            Assert.AreEqual(25.0, original.Value, Epsilon);
            Assert.AreEqual(TemperatureUnit.Celsius, original.Unit);

            Assert.AreEqual(77.0, converted.Value, Epsilon);
            Assert.AreEqual(TemperatureUnit.Fahrenheit, converted.Unit);
        }
    }
}