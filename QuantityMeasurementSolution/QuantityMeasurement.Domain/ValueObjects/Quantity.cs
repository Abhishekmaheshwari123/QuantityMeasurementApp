using System;
using QuantityMeasurement.Domain.Enums;
using QuantityMeasurement.Domain.Services;

namespace QuantityMeasurement.Domain.ValueObjects
{
    public sealed class Quantity<TUnit> where TUnit : struct, Enum
    {
        private const double Epsilon = 1e-6;

        public double Value { get; }
        public TUnit Unit { get; }

        public Quantity(double value, TUnit unit)
        {
            if (!double.IsFinite(value))
                throw new ArgumentException("Value must be finite.", nameof(value));

            if (!Enum.IsDefined(typeof(TUnit), unit))
                throw new ArgumentException("Invalid unit.", nameof(unit));

            Value = value;
            Unit = unit;
        }

        private static bool IsTemperatureUnit()
        {
            return typeof(TUnit) == typeof(TemperatureUnit);
        }

        private static void ValidateOperationSupport(string operation)
        {
            if (IsTemperatureUnit())
            {
                throw new NotSupportedException(
                    $"Temperature does not support {operation} operation on absolute values.");
            }
        }

        private double ToBaseUnit()
        {
            if (typeof(TUnit) == typeof(LengthUnit))
            {
                return UnitConversionService.ConvertLengthToBase(
                    Value, (LengthUnit)(object)Unit);
            }

            if (typeof(TUnit) == typeof(WeightUnit))
            {
                return UnitConversionService.ConvertWeightToBase(
                    Value, (WeightUnit)(object)Unit);
            }

            if (typeof(TUnit) == typeof(VolumeUnit))
            {
                var unit = (VolumeUnit)(object)Unit;

                return unit switch
                {
                    VolumeUnit.Litre => Value,
                    VolumeUnit.Millilitre => Value * 0.001,
                    VolumeUnit.Gallon => Value * 3.78541,
                    _ => throw new ArgumentException("Invalid VolumeUnit.")
                };
            }

            if (typeof(TUnit) == typeof(TemperatureUnit))
            {
                var unit = (TemperatureUnit)(object)Unit;

                // Base unit: Celsius
                return unit switch
                {
                    TemperatureUnit.Celsius => Value,
                    TemperatureUnit.Fahrenheit => (Value - 32.0) * 5.0 / 9.0,
                    TemperatureUnit.Kelvin => Value - 273.15,
                    _ => throw new ArgumentException("Invalid TemperatureUnit.")
                };
            }

            throw new InvalidOperationException("Unsupported unit type.");
        }

        private static double FromBaseUnit(double baseValue, TUnit targetUnit)
        {
            if (typeof(TUnit) == typeof(LengthUnit))
            {
                var unit = (LengthUnit)(object)targetUnit;

                return unit switch
                {
                    LengthUnit.Feet => baseValue,
                    LengthUnit.Inches => baseValue * 12.0,
                    LengthUnit.Yards => baseValue / 3.0,
                    LengthUnit.Centimeters => baseValue / 0.0328084,
                    _ => throw new ArgumentException("Invalid LengthUnit.")
                };
            }

            if (typeof(TUnit) == typeof(WeightUnit))
            {
                var unit = (WeightUnit)(object)targetUnit;

                return unit switch
                {
                    WeightUnit.Gram => baseValue,
                    WeightUnit.Kilogram => baseValue / 1000.0,
                    WeightUnit.Pound => baseValue / 453.592,
                    WeightUnit.Tonne => baseValue / 1_000_000.0,
                    _ => throw new ArgumentException("Invalid WeightUnit.")
                };
            }

            if (typeof(TUnit) == typeof(VolumeUnit))
            {
                var unit = (VolumeUnit)(object)targetUnit;

                return unit switch
                {
                    VolumeUnit.Litre => baseValue,
                    VolumeUnit.Millilitre => baseValue / 0.001,
                    VolumeUnit.Gallon => baseValue / 3.78541,
                    _ => throw new ArgumentException("Invalid VolumeUnit.")
                };
            }

            if (typeof(TUnit) == typeof(TemperatureUnit))
            {
                var unit = (TemperatureUnit)(object)targetUnit;

                // Base unit: Celsius
                return unit switch
                {
                    TemperatureUnit.Celsius => baseValue,
                    TemperatureUnit.Fahrenheit => (baseValue * 9.0 / 5.0) + 32.0,
                    TemperatureUnit.Kelvin => baseValue + 273.15,
                    _ => throw new ArgumentException("Invalid TemperatureUnit.")
                };
            }

            throw new InvalidOperationException("Unsupported unit type.");
        }

        private static double RoundToTwoDecimals(double value)
        {
            return Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }

        public Quantity<TUnit> ConvertTo(TUnit targetUnit)
        {
            if (!Enum.IsDefined(typeof(TUnit), targetUnit))
                throw new ArgumentException("Invalid target unit.", nameof(targetUnit));

            double baseValue = ToBaseUnit();
            double result = FromBaseUnit(baseValue, targetUnit);

            // UC14 expects temperature conversion output to be rounded consistently.
            if (IsTemperatureUnit())
                result = RoundToTwoDecimals(result);

            return new Quantity<TUnit>(result, targetUnit);
        }

        public Quantity<TUnit> Add(Quantity<TUnit> other)
        {
            return Add(other, Unit);
        }

        public Quantity<TUnit> Add(Quantity<TUnit> other, TUnit targetUnit)
        {
            ValidateOperationSupport("addition");

            if (other == null)
                throw new ArgumentNullException(nameof(other));

            double baseSum = ToBaseUnit() + other.ToBaseUnit();
            double result = FromBaseUnit(baseSum, targetUnit);

            return new Quantity<TUnit>(result, targetUnit);
        }

        public Quantity<TUnit> Subtract(Quantity<TUnit> other)
        {
            return Subtract(other, Unit);
        }

        public Quantity<TUnit> Subtract(Quantity<TUnit> other, TUnit targetUnit)
        {
            ValidateOperationSupport("subtraction");

            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (!double.IsFinite(other.Value))
                throw new ArgumentException("Other value must be finite.", nameof(other));

            double diffBase = ToBaseUnit() - other.ToBaseUnit();
            double converted = FromBaseUnit(diffBase, targetUnit);
            double rounded = RoundToTwoDecimals(converted);

            return new Quantity<TUnit>(rounded, targetUnit);
        }

        public double Divide(Quantity<TUnit> other)
        {
            ValidateOperationSupport("division");

            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (!double.IsFinite(other.Value))
                throw new ArgumentException("Other value must be finite.", nameof(other));

            double denominatorBase = other.ToBaseUnit();

            if (Math.Abs(denominatorBase) < Epsilon)
                throw new DivideByZeroException("Cannot divide by a zero quantity.");

            double numeratorBase = ToBaseUnit();
            return numeratorBase / denominatorBase;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Quantity<TUnit> other)
                return false;

            return Math.Abs(ToBaseUnit() - other.ToBaseUnit()) < Epsilon;
        }

        public override int GetHashCode()
        {
            return ToBaseUnit().GetHashCode();
        }

        public override string ToString()
        {
            return $"{Value} {Unit}";
        }
    }
}