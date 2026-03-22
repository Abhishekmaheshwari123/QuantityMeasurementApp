using System;

namespace QuantityMeasurement.Domain.Enums
{
    /// <summary>
    /// Represents supported weight units.
    /// Each unit stores conversion factor relative to base unit (Kilogram).
    /// </summary>
    public enum WeightUnit
    {
        Kilogram = 1,
        Gram = 2,
        Pound = 3
    }

    /// <summary>
    /// Extension methods for WeightUnit conversion responsibility.
    /// </summary>
    public static class WeightUnitExtensions
    {
        private const double PoundToKilogram = 0.453592;

        /// <summary>
        /// Returns conversion factor relative to base unit (Kilogram).
        /// </summary>
        public static double GetConversionFactor(this WeightUnit unit)
        {
            return unit switch
            {
                WeightUnit.Kilogram => 1.0,
                WeightUnit.Gram => 0.001,
                WeightUnit.Pound => PoundToKilogram,
                _ => throw new ArgumentException("Invalid weight unit.")
            };
        }

        /// <summary>
        /// Converts value to base unit (Kilogram).
        /// </summary>
        public static double ConvertToBase(this WeightUnit unit, double value)
        {
            return value * unit.GetConversionFactor();
        }

        /// <summary>
        /// Converts value from base unit (Kilogram) to target unit.
        /// </summary>
        public static double ConvertFromBase(this WeightUnit unit, double baseValue)
        {
            return baseValue / unit.GetConversionFactor();
        }
    }
}