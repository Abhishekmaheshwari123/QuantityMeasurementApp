using System;
using QuantityMeasurement.Domain.Enums;

namespace QuantityMeasurement.Domain.Services
{
    public static class UnitConversionService
    {
        public static double ConvertLengthToBase(double value, LengthUnit unit)
        {
            return unit switch
            {
                LengthUnit.Feet => value,
                LengthUnit.Inches => value / 12.0,
                LengthUnit.Yards => value * 3.0,
                LengthUnit.Centimeters => value * 0.0328084,
                _ => throw new ArgumentException("Invalid Length Unit")
            };
        }

        public static double ConvertWeightToBase(double value, WeightUnit unit)
        {
            return unit switch
            {
                WeightUnit.Gram => value,
                WeightUnit.Kilogram => value * 1000.0,
                WeightUnit.Pound => value * 453.592,
                WeightUnit.Tonne => value * 1_000_000.0,
                _ => throw new ArgumentException("Invalid Weight Unit")
            };
        }
    }
}