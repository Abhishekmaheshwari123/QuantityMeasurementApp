using System;
using QuantityMeasurement.Domain.Interfaces;

namespace QuantityMeasurement.Domain.Enums
{
    /// <summary>
    /// Represents supported length measurement units.
    /// Responsible for conversion to and from base unit (Feet).
    /// </summary>
    public enum LengthUnit : int
    {
        Feet = 1,
        Inches = 2,
        Yards = 3,
        Centimeters = 4
    }

    // Extension methods for LengthUnit to handle conversion logic, adhering to Single Responsibility Principle by keeping conversion logic separate from the enum definition.
    public static class LengthUnitExtensions
    {
        // Returns the conversion factor to the base unit (Feet) for each length unit, enabling consistent conversion logic across different units.
        public static double GetFactor(this LengthUnit unit)
        {
            return unit switch
            {
                LengthUnit.Feet => 1.0,     // Base unit
                LengthUnit.Inches => 1.0 / 12.0,        // 1 foot = 12 inches
                LengthUnit.Yards => 3.0,            // 1 yard = 3 feet
                LengthUnit.Centimeters => 0.0328084,     // 1 centimeter = 0.0328084 feet
                _ => throw new ArgumentException("Invalid Length Unit")
            };
        }

        // Converts a value to the base unit (Feet) using the conversion factor, facilitating accurate comparisons and operations between different length units.
        public static double ConvertToBaseUnit(this LengthUnit unit, double value)
            => value * unit.GetFactor();

        // Converts a value from the base unit (Feet) to the target unit using the conversion factor, enabling seamless conversion between different length units for display or further calculations.
        public static double ConvertFromBaseUnit(this LengthUnit unit, double baseValue)
            => baseValue / unit.GetFactor();
    }
}