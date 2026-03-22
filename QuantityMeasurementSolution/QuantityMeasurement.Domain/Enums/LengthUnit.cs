using System;

namespace QuantityMeasurement.Domain.Enums
{
    /// <summary>
    /// Represents supported length measurement units.
    /// Responsible for conversion to and from base unit (Feet).
    /// </summary>
    public enum LengthUnit
    {
        Feet,
        Inches,
        Yards,
        Centimeters
    }

    // Extension methods for LengthUnit to handle conversions, adhering to the Open/Closed principle.
    public static class LengthUnitExtensions
    {
        private const double InchesPerFoot = 12.0; 
        private const double FeetPerYard = 3.0;
        private const double CmPerFoot = 30.48;

        /// <summary>
        /// Converts value in current unit to base unit (Feet).
        /// </summary>
        public static double ConvertToBaseUnit(this LengthUnit unit, double value)
        {
            return unit switch
            {
                LengthUnit.Feet => value,                                          // Base unit
                LengthUnit.Inches => value / InchesPerFoot,           // Convert inches to feet
                LengthUnit.Yards => value * FeetPerYard,               // Convert yards to feet
                LengthUnit.Centimeters => value / CmPerFoot,    // Convert centimeters to feet
                _ => throw new ArgumentException("Invalid LengthUnit.")
            };
        }

        /// <summary>
        /// Converts value from base unit (Feet) to target unit.
        /// </summary>
        public static double ConvertFromBaseUnit(this LengthUnit unit, double baseValue)
        {
            return unit switch
            {
                LengthUnit.Feet => baseValue,                                              // Base unit
                LengthUnit.Inches => baseValue * InchesPerFoot,               // Convert feet to inches
                LengthUnit.Yards => baseValue / FeetPerYard,                   // Convert feet to yards
                LengthUnit.Centimeters => baseValue * CmPerFoot,        // Convert feet to centimeters
                _ => throw new ArgumentException("Invalid LengthUnit.")
            };
        }
    }
}