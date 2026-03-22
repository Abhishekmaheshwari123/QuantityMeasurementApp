using System;
using QuantityMeasurement.Domain.Interfaces;

namespace QuantityMeasurement.Domain.Enums
{
    /// <summary>
    /// Represents supported weight units.
    /// Each unit stores conversion factor relative to base unit (Kilogram).
    /// </summary>
    public enum WeightUnit
    {
        Gram,
        Kilogram,
        Pound,
        Tonne
    }

    public static class WeightUnitExtensions
    {
        // Returns the conversion factor to the base unit (Kilogram) for each weight unit, enabling consistent conversion logic across different units.
        public static double GetConversionFactor(this WeightUnit unit)
        {
            return unit switch
            {
                WeightUnit.Gram => 1.0,  // Base unit
                WeightUnit.Kilogram => 1000.0,      // 1 kilogram = 1000 grams
                WeightUnit.Pound => 453.592,    // 1 pound = 453.592 grams
                WeightUnit.Tonne => 1_000_000.0,  // 1 tonne = 1,000,000 grams
                _ => throw new ArgumentException("Invalid Weight Unit")
            };
        }

        /// <summary>
        /// Converts a value to the base unit (Gram) using the conversion factor, facilitating accurate comparisons and operations between different weight units.
        /// </summary>
        /// <param name="unit">The weight unit to convert from.</param>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value in the base unit (Gram).</returns>

        public static double ConvertToBaseUnit(this WeightUnit unit, double value)
            => value * unit.GetConversionFactor();

        
        /// <summary>
        /// Converts a value from the base unit (Gram) to the target unit using the conversion factor, enabling seamless conversion between different weight units for display or further calculations.
        /// </summary>
        /// <param name="unit">The weight unit to convert to.</param>
        /// <param name="baseValue">The value in the base unit (Gram) to convert.</param>
        /// <returns>The converted value in the target weight unit.</returns>

        public static double ConvertFromBaseUnit(this WeightUnit unit, double baseValue)
            => baseValue / unit.GetConversionFactor();

        /// <summary>
        /// Returns the name of the weight unit as a string, useful for display purposes and ensuring
        /// consistent unit naming across the application.
        /// </summary>
        /// <param name="unit">The weight unit for which to get the name.</param>
        /// <returns>The name of the weight unit as a string.</returns>
        
        public static string GetUnitName(this WeightUnit unit)
            => unit.ToString();
    }
}