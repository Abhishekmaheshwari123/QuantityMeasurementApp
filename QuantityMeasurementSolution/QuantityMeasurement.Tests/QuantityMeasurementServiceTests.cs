using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurement.Domain.DTOs;
using QuantityMeasurement.Domain.Entities;
using QuantityMeasurement.Domain.Enums;
using QuantityMeasurement.Domain.Exceptions;
using QuantityMeasurement.Domain.Interfaces;
using QuantityMeasurement.Domain.Services;

namespace QuantityMeasurement.Tests
{
    [TestClass]
    public class QuantityMeasurementServiceTests
    {
        private const double Epsilon = 1e-6;

        [TestMethod]
        public void UC15_Service_Compare_LengthCrossUnit_Works()
        {
            var service = CreateService();

            var first = new QuantityDto(12.0, LengthUnit.Inches.ToString(), "Length");
            var second = new QuantityDto(1.0, LengthUnit.Feet.ToString(), "Length");

            bool result = service.Compare(first, second);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void UC15_Service_Convert_Weight_Works()
        {
            var service = CreateService();

            var source = new QuantityDto(1000.0, WeightUnit.Gram.ToString(), "Weight");

            var result = service.Convert(source, WeightUnit.Kilogram.ToString());

            Assert.AreEqual(1.0, result.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Kilogram.ToString(), result.Unit);
            Assert.AreEqual("Weight", result.MeasurementType);
        }

        [TestMethod]
        public void UC15_Service_Add_VolumeWithTargetUnit_Works()
        {
            var service = CreateService();

            var first = new QuantityDto(1.0, VolumeUnit.Litre.ToString(), "Volume");
            var second = new QuantityDto(500.0, VolumeUnit.Millilitre.ToString(), "Volume");

            var result = service.Add(first, second, VolumeUnit.Millilitre.ToString());

            Assert.AreEqual(1500.0, result.Value, Epsilon);
            Assert.AreEqual(VolumeUnit.Millilitre.ToString(), result.Unit);
        }

        [TestMethod]
        public void UC15_Service_Subtract_Length_Works()
        {
            var service = CreateService();

            var first = new QuantityDto(10.0, LengthUnit.Feet.ToString(), "Length");
            var second = new QuantityDto(6.0, LengthUnit.Inches.ToString(), "Length");

            var result = service.Subtract(first, second);

            Assert.AreEqual(9.50, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Feet.ToString(), result.Unit);
        }

        [TestMethod]
        public void UC15_Service_Divide_Weight_Works()
        {
            var service = CreateService();

            var first = new QuantityDto(2000.0, WeightUnit.Gram.ToString(), "Weight");
            var second = new QuantityDto(1.0, WeightUnit.Kilogram.ToString(), "Weight");

            double result = service.Divide(first, second);

            Assert.AreEqual(2.0, result, Epsilon);
        }

        [TestMethod]
        public void UC15_Service_Temperature_Addition_Throws()
        {
            var service = CreateService();

            var first = new QuantityDto(10.0, TemperatureUnit.Celsius.ToString(), "Temperature");
            var second = new QuantityDto(20.0, TemperatureUnit.Celsius.ToString(), "Temperature");

            try
            {
                service.Add(first, second);
                Assert.Fail("Expected QuantityMeasurementException to be thrown");
            }
            catch (QuantityMeasurementException)
            {
                // Pass
            }
        }

        private static IQuantityMeasurementService CreateService()
        {
            return new QuantityMeasurementService(new InMemoryRepository());
        }

        private sealed class InMemoryRepository : IQuantityMeasurementRepository
        {
            private readonly List<QuantityMeasurementEntity> _items = new();

            public void Save(QuantityMeasurementEntity entity)
            {
                _items.Add(entity);
            }

            public IReadOnlyList<QuantityMeasurementEntity> GetAllMeasurements()
            {
                return _items.AsReadOnly();
            }
        }
    }
}