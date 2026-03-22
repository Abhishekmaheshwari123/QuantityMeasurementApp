using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurement.Domain.Enums;
using QuantityMeasurement.Domain.ValueObjects;

namespace QuantityMeasurement.Tests
{
    [TestClass]
    public class QuantityAdditionWithTargetUnitTests
    {
        private const double Epsilon = 1e-6;
    
        [TestMethod]
        public void testAddition_ExplicitTargetUnit_Feet()
        {
            var a = new QuantityLength(1.0, LengthUnit.Feet);
            var b = new QuantityLength(12.0, LengthUnit.Inches);
    
            var result = QuantityLength.Add(a, b, LengthUnit.Feet);
    
            Assert.AreEqual(2.0, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Feet, result.Unit);
        }
    
        [TestMethod]
        public void testAddition_ExplicitTargetUnit_Inches()
        {
            var a = new QuantityLength(1.0, LengthUnit.Feet);
            var b = new QuantityLength(12.0, LengthUnit.Inches);
    
            var result = QuantityLength.Add(a, b, LengthUnit.Inches);
    
            Assert.AreEqual(24.0, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Inches, result.Unit);
        }
    
        [TestMethod]
        public void testAddition_ExplicitTargetUnit_Yards()
        {
            var a = new QuantityLength(1.0, LengthUnit.Feet);
            var b = new QuantityLength(12.0, LengthUnit.Inches);
    
            var result = QuantityLength.Add(a, b, LengthUnit.Yards);
    
            Assert.AreEqual(0.666666, result.Value, 1e-3);
            Assert.AreEqual(LengthUnit.Yards, result.Unit);
        }
    
        [TestMethod]
        public void testAddition_ExplicitTargetUnit_Commutativity()
        {
            var a = new QuantityLength(1.0, LengthUnit.Feet);
            var b = new QuantityLength(12.0, LengthUnit.Inches);
    
            var result1 = QuantityLength.Add(a, b, LengthUnit.Yards);
            var result2 = QuantityLength.Add(b, a, LengthUnit.Yards);
    
            Assert.IsTrue(result1.Equals(result2));
        }
    
        [TestMethod]
        public void testAddition_ExplicitTargetUnit_InvalidTargetUnit_Throws()
        {
            var a = new QuantityLength(1.0, LengthUnit.Feet);
            var b = new QuantityLength(12.0, LengthUnit.Inches);

            try
            {
                QuantityLength.Add(a, b, (LengthUnit)999);
                Assert.Fail("Expected an ArgumentException for invalid target unit, but no exception was thrown.");
            }
            catch (ArgumentException)
            {
                // Pass
            }
        }
    }
}